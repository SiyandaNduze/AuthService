using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Auth.Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokens;

        public AuthController(
            IUserRepository users,
            IPasswordHasher hasher,
            ITokenService tokens)
        {
            _users = users;
            _hasher = hasher;
            _tokens = tokens;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _users.GetByEmailAsync(request.Email);
            if (user == null || !_hasher.Verify(request.Password, user.PasswordHash))
                return Unauthorized();

            return Ok(_tokens.GenerateTokens(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequests request)
        {
            var existing = await _users.GetByEmailAsync(request.Email);
            if (existing != null)
                return BadRequest("User already exists");

            var hash = _hasher.Hash(request.Password);
            var user = User.Create(request.Email, hash);

            await _users.AddAsync(user);

            return Ok();
        }
    }
}
