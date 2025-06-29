
public class FakeTaskRepository : ITaskRepository
{
    private readonly List<UserTask> _tasks = new();

    public Task<UserTask> AddAsync(UserTask task)
    {
        _tasks.Add(task);
        return Task.FromResult(task);
    }

    public void Delete(UserTask task)
    {
        _tasks.Remove(task);
    }

    public Task<IEnumerable<UserTask>> GetAllByUserIdAsync(Guid userId)
    {
        var tasks = _tasks.Where(t => t.UserId == userId);
        return Task.FromResult(tasks);
    }

    public Task<UserTask?> GetByIdAsync(Guid id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(task);
    }

    public void Update(UserTask task)
    {
        var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existingTask != null)
        {
            existingTask.SetTitle(task.Title);
            existingTask.SetIsDone(task.IsDone);
        }
    }
}
