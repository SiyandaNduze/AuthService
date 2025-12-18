namespace Auth.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool EmailVerified { get; private set; }
    public bool IsLocked { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            EmailVerified = false,
            IsLocked = false,
            CreatedAt = DateTime.UtcNow
        };
    }
}
