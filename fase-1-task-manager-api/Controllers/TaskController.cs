using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{

    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("GetUserTasks")]
    public async Task<IActionResult> GetUserTasks(string userId)
    {
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
        {
            return BadRequest("Invalid user ID.");
        }

        var tasks = await _taskService.GetAllByUserIdAsync(parsedUserId);

        if (tasks == null || !tasks.Any())
        {
            return NotFound("No tasks found for the user.");
        }
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid task ID.");
        }

        var task = await _taskService.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound("Task not found.");
        }

        return Ok(task);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] UserTask task)
    {
        if (task == null)
        {
            return BadRequest("Task cannot be null.");
        }

        var createdTask = await _taskService.AddAsync(task);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(Guid id, [FromBody] UserTask task)
    {
        if (id == Guid.Empty || task == null || task.Id != id)
        {
            return BadRequest("Invalid task data.");
        }

        _taskService.Update(task);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid task ID.");
        }

        var task = await _taskService.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound("Task not found.");
        }

        _taskService.Delete(task);
        return NoContent();
    }
        
}