namespace Tests.Services;

public class UserServiceTests
{
    private readonly MockUserRepository _mockUserRepository;
    private readonly MockUnitOfWork _mockUnitOfWork;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new MockUserRepository();
        _mockUnitOfWork = new MockUnitOfWork();
        _userService = new UserService(_mockUserRepository, _mockUnitOfWork);
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
        await _mockUserRepository.AddAsync(existingUser);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _userService.CreateUserAsync("Another User", "exists@email.com"));
        
        Assert.Equal("Email already exists.", exception.Message);
    }
}