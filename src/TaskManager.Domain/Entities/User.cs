namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; private set; }
    public ICollection<UserTask> Tasks { get; private set; } = new List<UserTask>();

    private User() { }

    public User(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
    }

    public User(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

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