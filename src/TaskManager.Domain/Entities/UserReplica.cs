namespace TaskManager.Domain.Entities;

public class UserReplica
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }

    private UserReplica() {}

    public static UserReplica Create(Guid id, string email)
    {
        return new UserReplica
        {
            Id = id,
            Email = email
        };
    }
}