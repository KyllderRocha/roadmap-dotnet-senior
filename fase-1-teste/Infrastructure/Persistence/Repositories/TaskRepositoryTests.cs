
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Tests.Repositories;

public class TaskRepositoryTests
{
    public User userTest;
    public TaskRepositoryTests()
    {
        using var context = GetInMemoryDbContext();
        var userId = Guid.NewGuid();
        userTest = new User(userId, "Test User", "testuser@example.com");
        context.Users.Add(userTest);
    }

    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTask_WhenCalled()
    {
        await using var context = GetInMemoryDbContext();

        var taskRepository = new TaskRepository(context);
        var newTask = new UserTask("Teste Task", userTest.Id);

        await taskRepository.AddAsync(newTask);
        await context.SaveChangesAsync();
        var taskInDb = await context.Tasks.FirstOrDefaultAsync(t => t.Id == newTask.Id);

        Assert.NotNull(taskInDb);
        Assert.Equal("Teste Task", taskInDb.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        await using var context = GetInMemoryDbContext();
        var taskId = Guid.NewGuid();
        var newTask = new UserTask("Task Exists", userTest.Id);
        context.Tasks.Add(newTask);
        await context.SaveChangesAsync();
        var taskRepository = new TaskRepository(context);
        var result = await taskRepository.GetByIdAsync(newTask.Id);
        Assert.NotNull(result);
        Assert.Equal(newTask.Id, result.Id);
        Assert.Equal(newTask.Title, result.Title);
        Assert.Equal(newTask.UserId, result.UserId);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnTasks_WhenUserHasTasks()
    {
        await using var context = GetInMemoryDbContext();
        var taskRepository = new TaskRepository(context);
        var newTask1 = new UserTask("Task 1", userTest.Id);
        var newTask2 = new UserTask("Task 2", userTest.Id);

        context.Tasks.AddRange(newTask1, newTask2);
        await context.SaveChangesAsync();

        var tasks = await taskRepository.GetAllByUserIdAsync(userTest.Id);

        Assert.NotNull(tasks);
        Assert.Equal(2, tasks.Count());
        Assert.Contains(tasks, t => t.Title == "Task 1");
        Assert.Contains(tasks, t => t.Title == "Task 2");
    }

    [Fact]
    public async Task Update_ShouldUpdateTask_WhenCalled()
    {
        await using var context = GetInMemoryDbContext();
        var taskRepository = new TaskRepository(context);
        var task = new UserTask("Old Task", userTest.Id);
        await taskRepository.AddAsync(task);
        task.Title = "Updated Task";
        task.IsDone = true;
        task.UserId = userTest.Id;

        taskRepository.Update(task);
        await context.SaveChangesAsync();

        var updatedTask = await taskRepository.GetByIdAsync(task.Id);
        Assert.NotNull(updatedTask);
        Assert.Equal("Updated Task", updatedTask.Title);
        Assert.True(updatedTask.IsDone);
    }

    [Fact]
    public async Task Delete_ShouldRemoveTask_WhenCalled()
    {
        await using var context = GetInMemoryDbContext();
        var taskRepository = new TaskRepository(context);
        var task = new UserTask("Task to Delete", userTest.Id);
        await taskRepository.AddAsync(task);
        taskRepository.Delete(task);
        await context.SaveChangesAsync();
        var deletedTask = await taskRepository.GetByIdAsync(task.Id);
        Assert.Null(deletedTask);
    }
}