

using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Persistence.Repositories;

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

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        return users.AsEnumerable();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("ID cannot be empty.", nameof(id));

        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public void Update(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        _context.Users.Update(user);
    }
}