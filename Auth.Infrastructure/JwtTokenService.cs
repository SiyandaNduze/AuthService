using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public AuthResult GenerateTokens(User user)
    {
        // Generate access + refresh tokens
        return new AuthResult("access-token", "refresh-token");
    }
}
