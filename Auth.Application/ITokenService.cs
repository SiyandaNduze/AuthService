using Auth.Domain.Entities;

public interface ITokenService
{
    AuthResult GenerateTokens(User user);
}
