using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserTask> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.id);
        if (task == null) 
            throw new NotFoundException("Task not found");

        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId) || task.UserId != userId)
        {
            throw new UnauthorizedAccessException("Não tem permissão para eliminar esta tarefa.");
        }
        
        _taskRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}