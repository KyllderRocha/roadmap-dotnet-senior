using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using Microsoft.AspNetCore.Http; 
using System.Security.Claims;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateTaskCommandHandler(ITaskRepository userRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _taskRepository = userRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserTask> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("Não foi possível identificar o utilizador.");
        }

        var task = new UserTask(request.title, userId);

        await _taskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}