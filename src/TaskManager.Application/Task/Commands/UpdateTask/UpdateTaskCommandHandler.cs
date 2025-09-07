using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateTaskCommandHandler(ITaskRepository userRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _taskRepository = userRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserTask> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);

        if (task == null)
            throw new NotFoundException(nameof(UserTask), request.Id);

        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId) || task.UserId != userId)
        {
            throw new UnauthorizedAccessException("Não tem permissão para atualizar esta tarefa.");
        }

        task.SetTitle(request.Title);
        task.SetIsDone(request.IsDone);

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}