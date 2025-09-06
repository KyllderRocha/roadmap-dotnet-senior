using MediatR;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase{
    
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var createdUser = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAllUsers), new { id = createdUser.Id }, createdUser);
    }
}