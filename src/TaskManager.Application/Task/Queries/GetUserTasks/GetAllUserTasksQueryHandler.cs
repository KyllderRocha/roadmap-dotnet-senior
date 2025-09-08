using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class GetAllUserTasksQueryHandler : IRequestHandler<GetAllUserTasksQuery, IEnumerable<UserTask>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllUserTasksQueryHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<UserTask>> Handle(GetAllUserTasksQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (!userId.HasValue)
            throw new UnauthorizedAccessException("Não foi possível identificar o utilizador.");

        return await _taskRepository.GetAllByUserIdAsync(userId.Value);
    }
}