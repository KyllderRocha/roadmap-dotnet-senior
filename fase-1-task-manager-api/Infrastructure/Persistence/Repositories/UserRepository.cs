
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

    public void Update(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        _context.Users.Update(user);
    }
}