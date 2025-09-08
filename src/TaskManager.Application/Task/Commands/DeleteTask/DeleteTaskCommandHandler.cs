using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<UserTask> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.id);
        if (task == null) 
            throw new NotFoundException("Task not found");

        var userId = _currentUserService.UserId;

        if (!userId.HasValue || task.UserId != userId.Value)
        {
            throw new UnauthorizedAccessException("Não tem permissão para eliminar esta tarefa.");
        }
        
        _taskRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}