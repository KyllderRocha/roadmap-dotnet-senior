namespace Tests.Services;

public class TaskServiceTests
{
    private readonly MockTaskRepository _mockTaskRepository;
    private readonly MockUnitOfWork _mockUnitOfWork;
    private readonly MockUserRepository _mockUserRepository;
    private readonly TaskService _taskService;
    private readonly UserService _userService;

    private User user;

    public TaskServiceTests()
    {
        _mockTaskRepository = new MockTaskRepository();
        _mockUnitOfWork = new MockUnitOfWork();
        _mockUserRepository = new MockUserRepository();
        _taskService = new TaskService(_mockTaskRepository, _mockUnitOfWork);
        _userService = new UserService(_mockUserRepository, _mockUnitOfWork);
        user = _userService.CreateUserAsync("Test User", "testuser@example.com").Result;
    }

    [Fact]
    public async Task AddTaskAsync_ShouldReturnNewTask_WhenCalled()
    {
        var title = "New Task";
        var taskEntity = new UserTask(title, user.Id);
        var task = await _taskService.AddAsync(taskEntity);

        Assert.NotNull(task);
        Assert.Equal(title, task.Title);
        Assert.Equal(user.Id, task.UserId);
        Assert.False(task.IsDone);
    }

    [Fact]
    public async Task GetTasksByUserIdAsync_ShouldReturnTasks_WhenUserHasTasks()
    {
        var task1 = new UserTask("Task 1", user.Id);
        var task2 = new UserTask("Task 2", user.Id);
        _taskService.AddAsync(task1).Wait();
        _taskService.AddAsync(task2).Wait();

        var tasks = await _taskService.GetAllByUserIdAsync(user.Id);
        Assert.NotNull(tasks);
        Assert.Contains(tasks, t => t.Title == "Task 1");
        Assert.Contains(tasks, t => t.Title == "Task 2");
        Assert.Equal(2, tasks.Count());
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        var task = new UserTask("Existing Task", user.Id);
        _taskService.AddAsync(task).Wait();
        var result = await _taskService.GetByIdAsync(task.Id);
        Assert.NotNull(result);
        Assert.Equal(task.Id, result.Id);
        Assert.Equal(task.Title, result.Title);
        Assert.Equal(task.UserId, result.UserId);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask_WhenCalled()
    {
        var task = new UserTask("Task to Update", user.Id);
        _taskService.AddAsync(task).Wait();

        task.Title = "Updated Task Title";
        task.IsDone = true;
        _taskService.Update(task);
        var updatedTask = await _taskService.GetByIdAsync(task.Id);

        Assert.NotNull(updatedTask);
        Assert.Equal("Updated Task Title", updatedTask.Title);
        Assert.True(updatedTask.IsDone);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldRemoveTask_WhenCalled()
    {
        var task = new UserTask("Task to Delete", user.Id);
        await _taskService.AddAsync(task);

        _taskService.Delete(task);
        var deletedTask = await _taskService.GetByIdAsync(task.Id);

        Assert.Null(deletedTask);
    }
}