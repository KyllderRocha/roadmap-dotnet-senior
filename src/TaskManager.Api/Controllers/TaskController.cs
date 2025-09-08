using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class TaskController : ControllerBase
{

    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetUserTasks")]
    public async Task<IActionResult> GetUserTasks([FromQuery] GetAllUserTasksQuery query)
    {
        var tasks = await _mediator.Send(query);
        return Ok(tasks);
    }

    [HttpGet("{TaskId}")]
    public async Task<IActionResult> GetTaskById([FromRoute] GetUserTaskByIdQuery query)
    {
        var task = await _mediator.Send(query);
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        var createdTask = await _mediator.Send(command);        
        return CreatedAtAction(nameof(GetTaskById), new { TaskId = createdTask.Id }, createdTask);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask([FromRoute] DeleteTaskCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
        
}