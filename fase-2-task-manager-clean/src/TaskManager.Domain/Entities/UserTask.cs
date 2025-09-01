namespace TaskManager.Domain.Entities;

public class UserTask
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsDone { get; private set; }
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; } = null!;


    public UserTask(string title, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsDone = false;
        UserId = userId;
    }

    public void SetTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(newTitle));
        }
        Title = newTitle;
    }

    public void MarkAsDone()
    {
        IsDone = true;
    }

    public void SetIsDone(bool isDone)
    {
        IsDone = isDone;
    }

    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }
}