using Auth.Application.Interfaces;
using Auth.Contracts.Requests;
using Auth.Domain;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;
    private readonly IRefreshTokenRepository _refreshTokens;

    public AuthController(
        IUserRepository users,
        IPasswordHasher hasher,
        ITokenService tokens,
        IRefreshTokenRepository refreshTokens)
    {
        _users = users;
        _hasher = hasher;
        _tokens = tokens;
        _refreshTokens = refreshTokens;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequests request)
    {
        var existing = await _users.GetByEmailAsync(request.Email);
        if (existing != null)
            return BadRequest("User already exists");

        var hash = _hasher.Hash(request.Password);
        var user = Auth.Domain.Entities.User.Create(request.Email, hash);

        await _users.AddAsync(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _users.GetByEmailAsync(request.Email);
        if (user == null || !_hasher.Verify(request.Password, user.PasswordHash))
            return Unauthorized();

        var authResult = _tokens.GenerateTokens(user);

        var refreshToken = RefreshToken.Create(
            user.Id,
            authResult.RefreshToken,
            DateTime.UtcNow.AddDays(7)
        );

        await _refreshTokens.AddAsync(refreshToken);

        return Ok(authResult);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var existing = await _refreshTokens.GetAsync(request.RefreshToken);

        if (existing == null || existing.IsRevoked || existing.ExpiresAt < DateTime.UtcNow)
            return Unauthorized();

        existing.Revoke();
        await _refreshTokens.UpdateAsync(existing);

        var user = await _users.GetByIdAsync(existing.UserId);
        if (user == null)
            return Unauthorized();

        var authResult = _tokens.GenerateTokens(user);

        await _refreshTokens.AddAsync(
            RefreshToken.Create(user.Id, authResult.RefreshToken, DateTime.UtcNow.AddDays(7))
        );

        return Ok(authResult);
    }
}
