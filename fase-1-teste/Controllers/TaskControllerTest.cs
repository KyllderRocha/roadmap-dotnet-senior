using Microsoft.AspNetCore.Mvc;
namespace Tests.Controllers;

public class TaskControllerTests
{
    private readonly MockTaskService _mockTaskService;
    private readonly TaskController _taskController;

    public TaskControllerTests()
    {
        _mockTaskService = new MockTaskService();
        _taskController = new TaskController(_mockTaskService);
    }

    [Fact]
    public async Task GetAllTasks_ReturnsOkResult_WithListOfTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task1 = new UserTask ("Task 1" , userId);
        var task2 = new UserTask ("Task 2" , userId);
        await _mockTaskService.AddAsync(task1);
        await _mockTaskService.AddAsync(task2);
        // Act
        var result = await _taskController.GetUserTasks(userId.ToString());
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tasks = Assert.IsAssignableFrom<IEnumerable<UserTask>>(okResult.Value);
        Assert.Equal(2, tasks.Count());
        Assert.Contains(tasks, t => t.Id == task1.Id);
        Assert.Contains(tasks, t => t.Id == task2.Id);
    }

    [Fact]
    public async Task GetTaskById_ReturnsOkResult_WithTask()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task = new UserTask("Sample Task" , userId);
        await _mockTaskService.AddAsync(task);
        // Act
        var result = await _taskController.GetTaskById(task.Id);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTask = Assert.IsType<UserTask>(okResult.Value);
        Assert.Equal(task.Id, returnedTask.Id);
        Assert.Equal(task.UserId, returnedTask.UserId);
        Assert.Equal(task.Title, returnedTask.Title);

    }

    [Fact]
    public async Task AddTask_ReturnsCreatedAtActionResult_WithTask()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task = new UserTask("New Task", userId);
        // Act
        var result = await _taskController.CreateTask(task);
        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedTask = Assert.IsType<UserTask>(createdResult.Value);
        Assert.Equal(task.Id, returnedTask.Id);
        Assert.Equal(task.UserId, returnedTask.UserId);
        Assert.Equal(task.Title, returnedTask.Title);

    }

    [Fact]
    public async Task UpdateTask_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task = new UserTask ("Task to Update", userId);
        await _mockTaskService.AddAsync(task);
        task.Title = "Updated Task Title";
        // Act
        var result = _taskController.UpdateTask(task.Id, task);
        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedTask = await _mockTaskService.GetByIdAsync(task.Id);
        Assert.NotNull(updatedTask);
        Assert.Equal("Updated Task Title", updatedTask?.Title);
    }

    [Fact]
    public async Task DeleteTask_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task = new UserTask("Task to Delete", userId);
        await _mockTaskService.AddAsync(task);
        // Act
        var result = _taskController.DeleteTask(task.Id);
        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedTask = await _mockTaskService.GetByIdAsync(task.Id);
        Assert.Null(deletedTask);
    }

    [Fact]
    public async Task GetAllTasks_ReturnsNotFound_WhenUserHasNoTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        // Act
        var result = await _taskController.GetUserTasks(userId.ToString());
        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}