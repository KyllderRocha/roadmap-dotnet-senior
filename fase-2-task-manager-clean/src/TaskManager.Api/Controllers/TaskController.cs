using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly ITaskService _taskService;

    public TaskController(IMediator mediator, ITaskService taskService)
    {
        _mediator = mediator;
        _taskService = taskService;
    }

    [HttpGet("GetUserTasks/{UserId}")]
    public async Task<IActionResult> GetUserTasks([FromRoute] GetAllUserTasksQuery query)
    {
        if (query == null || query.UserId == Guid.Empty)
        {
            return BadRequest("Invalid user ID.");
        }

        var tasks = await _mediator.Send(query);


        if (tasks == null || !tasks.Any())
        {
            return NotFound("No tasks found for the user.");
        }
        return Ok(tasks);
    }

    [HttpGet("{TaskId}")]
    public async Task<IActionResult> GetTaskById([FromRoute] GetUserTaskByIdQuery query)
    {
        if (query == null || query.TaskId == Guid.Empty)
        {
            return BadRequest("Invalid task ID.");
        }

        var task = await _mediator.Send(query);
        
        if (task == null)
        {
            return NotFound("Task not found.");
        }

        return Ok(task);
    }
     
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        var createdTask = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Task ID mismatch.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(DeleteTaskCommand command)
    {
        var result = await _mediator.Send(command);
        if (result == null)
            return NotFound("Task not found.");

        return NoContent();
    }
        
}