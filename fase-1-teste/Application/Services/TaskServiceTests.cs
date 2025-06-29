namespace Tests.Services;

public class TaskServiceTests
{
    private readonly FakeTaskRepository _fakeTaskRepository;
    private readonly FakeUnitOfWork _fakeUnitOfWork;
    private readonly FakeUserRepository _fakeUserRepository;
    private readonly TaskService _taskService;
    private readonly UserService _userService;

    private User user;

    public TaskServiceTests()
    {
        _fakeTaskRepository = new FakeTaskRepository();
        _fakeUnitOfWork = new FakeUnitOfWork();
        _fakeUserRepository = new FakeUserRepository();
        _taskService = new TaskService(_fakeTaskRepository, _fakeUnitOfWork);
        _userService = new UserService(_fakeUserRepository, _fakeUnitOfWork);
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
        await _taskService.AddAsync(task1);
        await _taskService.AddAsync(task2);

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
        await _taskService.AddAsync(task);
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
        await _taskService.AddAsync(task);

        task.SetTitle("Updated Task Title");
        task.MarkAsDone();
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