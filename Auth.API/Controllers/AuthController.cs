using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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
    }
}
