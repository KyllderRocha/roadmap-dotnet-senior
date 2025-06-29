
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;
    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserTask> AddAsync(UserTask task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be null.");
        }

        var result = await _context.Tasks.AddAsync(task);
        return result.Entity;
    }

    public void Delete(UserTask task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be null.");
        }

        _context.Tasks.Remove(task);
    }

    public async Task<IEnumerable<UserTask>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

        return tasks.AsEnumerable();
    }

    public async Task<UserTask?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty.", nameof(id));
        }

        return await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public void Update(UserTask task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be null.");
        }

        _context.Tasks.Update(task);
    }
}