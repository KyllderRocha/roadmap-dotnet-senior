using Domain.Entities;
using Domain.Interfaces;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    public TaskService(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<UserTask> AddAsync(UserTask task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be null.");
        }

        var result = await _taskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public void Delete(UserTask task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be null.");
        }

        _taskRepository.Delete(task);
        _unitOfWork.SaveChangesAsync();
    }

    public Task<IEnumerable<UserTask>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        return _taskRepository.GetAllByUserIdAsync(userId);
    }

    public Task<UserTask?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty.", nameof(id));
        }

        return _taskRepository.GetByIdAsync(id);
    }

    public void Update(UserTask task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be null.");
        }

        _taskRepository.Update(task);
        _unitOfWork.SaveChangesAsync();
    }
}