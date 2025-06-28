
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        var result = await _context.Users.AddAsync(user);
        return result.Entity;
    }

    public void Delete(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        _context.Users.Remove(user);
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return _context.Users.ToListAsync().ContinueWith(task => task.Result.AsEnumerable());
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("ID cannot be empty.", nameof(id));

        return _context.Users.FindAsync(id).AsTask();
    }

    public void Update(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        _context.Users.Update(user);
    }
}