namespace Domain.Entities;

public class UserTask
{
    public Guid Id { get; private set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; private set; } = null!; 


    public UserTask(string title, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsDone = false;
        UserId = userId;
    }

    public void MarkAsDone()
    {
        IsDone = true;
    }
}