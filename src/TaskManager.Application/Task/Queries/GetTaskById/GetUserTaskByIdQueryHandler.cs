using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class GetUserTaskByIdQueryHandler : IRequestHandler<GetUserTaskByIdQuery, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUserTaskByIdQueryHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UserTask> Handle(GetUserTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (!userId.HasValue)
            throw new UnauthorizedAccessException("Não foi possível identificar o utilizador.");

        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        if (task == null || task.UserId != userId.Value)
            throw new UnauthorizedAccessException("Não tem permissão para aceder a esta tarefa.");

        return task;
    }
}