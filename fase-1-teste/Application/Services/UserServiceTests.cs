namespace Tests.Services;

public class UserServiceTests
{
    private readonly FakeUserRepository _fakeUserRepository;
    private readonly FakeUnitOfWork _fakeUnitOfWork;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _fakeUserRepository = new FakeUserRepository();
        _fakeUnitOfWork = new FakeUnitOfWork();
        _userService = new UserService(_fakeUserRepository, _fakeUnitOfWork);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnNewUser_WhenEmailIsUnique()
    {
        var name = "New User";
        var email = "new@email.com";

        var result = await _userService.CreateUserAsync(name, email);

        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var existingUser = new User("Existing User", "exists@email.com");
        await _fakeUserRepository.AddAsync(existingUser);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _userService.CreateUserAsync("Another User", "exists@email.com"));
        
        Assert.Equal("Email already exists.", exception.Message);
    }
}