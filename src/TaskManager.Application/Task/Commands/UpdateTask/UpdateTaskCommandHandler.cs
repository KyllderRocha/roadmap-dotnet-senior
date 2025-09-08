using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<UserTask> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);

        if (task == null)
            throw new NotFoundException(nameof(UserTask), request.Id);

        var userId = _currentUserService.UserId;

        if (!userId.HasValue || task.UserId != userId.Value)
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