namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public ICollection<UserTask> Tasks { get; private set; } = new List<UserTask>();

    private User() { }

    public static User Create(string name, string email)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = string.Empty
        };
    }

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}