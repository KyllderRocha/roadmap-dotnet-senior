using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class GetAllUserTasksQueryHandler : IRequestHandler<GetAllUserTasksQuery, IEnumerable<UserTask>>
{
    private readonly ITaskRepository _userTaskRepository;
    private readonly IHttpContextAccessor _httpContextAccessor; 

    public GetAllUserTasksQueryHandler(ITaskRepository userTaskRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userTaskRepository = userTaskRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<UserTask>> Handle(GetAllUserTasksQuery request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Não foi possível identificar o utilizador.");

        return await _userTaskRepository.GetAllByUserIdAsync(userId);
    }
}