
using Microsoft.AspNetCore.Mvc;
namespace Tests.Controllers;

public class UserControllerTests
{
    private readonly MockUserService _mockUserService;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _mockUserService = new MockUserService();
        _userController = new UserController(_mockUserService);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnOk_WhenUsersExist()
    {
        _mockUserService.CreateUserAsync("Test User 1", "test1@test.com").Wait();
        _mockUserService.CreateUserAsync("Test User 2", "test2@test.com").Wait();

        // Act
        var result = await _userController.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count);
        Assert.Equal("Test User 1", returnedUsers[0].Name);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnNoContent_WhenNoUsersExist()
    {
        // Act
        var result = await _userController.GetAllUsers();
        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public void CreateUser_ShouldReturnOk_WhenUserIsCreated()
    {
        // Arrange
        var newUser = new User("New User", "newuser@test.com");

        // Act
        var result = _userController.CreateUser(newUser);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var createdUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal("New User", createdUser.Name);
    }

    [Fact]
    public void CreateUser_ShouldReturnBadRequest_WhenUserIsNull()
    {
        // Arrange
        User nullUser = null;
        // Act
        var result = _userController.CreateUser(nullUser);
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Equal("User cannot be null.", badRequestResult.Value);
    }

}
