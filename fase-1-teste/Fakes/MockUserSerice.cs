
public class MockUserService : IUserService
{
    // This is a mock implementation of IUserService for testing purposes.
    // In a real application, this would interact with a database or other data source.

    private readonly List<User> _users = new List<User>();
    public Task<User> CreateUserAsync(string name, string email)
    {
        var newUser = new User(name, email);
        _users.Add(newUser);
        return Task.FromResult(newUser);    
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return Task.FromResult<IEnumerable<User>>(_users);
    }
}