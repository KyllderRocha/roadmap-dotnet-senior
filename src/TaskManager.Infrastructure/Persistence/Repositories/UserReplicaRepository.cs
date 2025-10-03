

using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class UserReplicaRepository : IUserReplicaRepository
{
    private readonly ApplicationDbContext _context;
    public UserReplicaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserReplica> AddAsync(UserReplica user)
    {
        var result = await _context.UserReplicas.AddAsync(user);
        return result.Entity;
    }

    public void Delete(UserReplica user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        _context.UserReplicas.Remove(user);
    }

    public async Task<UserReplica?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("ID cannot be empty.", nameof(id));

        return await _context.UserReplicas.FindAsync(id);
    }

    public async Task<UserReplica?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        return await _context.UserReplicas.FirstOrDefaultAsync(u => u.Email == email);
    }

    public void Update(UserReplica user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        _context.UserReplicas.Update(user);
    }
}