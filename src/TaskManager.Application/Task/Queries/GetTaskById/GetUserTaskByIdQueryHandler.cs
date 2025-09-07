using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class GetUserTaskByIdQueryHandler : IRequestHandler<GetUserTaskByIdQuery, UserTask>
{
    private readonly ITaskRepository _userTaskRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetUserTaskByIdQueryHandler(ITaskRepository userTaskRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userTaskRepository = userTaskRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserTask> Handle(GetUserTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Não foi possível identificar o utilizador.");

        var task = await _userTaskRepository.GetByIdAsync(request.TaskId);

        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Não tem permissão para aceder a esta tarefa.");

        return task;
    }
}