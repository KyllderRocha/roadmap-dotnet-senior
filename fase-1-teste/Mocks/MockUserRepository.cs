public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User> AddAsync(User user)
    {
        if (_users.Any(u => u.Email == user.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        _users.Add(user);
        return Task.FromResult(user);
    }

    public void Delete(User user)
    {
        _users.Remove(user);
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<User>>(_users);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public void Update(User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
        }
    }
}
