using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;
public interface ITokenService
{
    AuthResult GenerateTokens(User user);
}
