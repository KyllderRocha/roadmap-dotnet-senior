using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using Microsoft.AspNetCore.Http; 
using System.Security.Claims;
using TaskManager.Application.Common.Interfaces;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<UserTask> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException("Não foi possível identificar o utilizador.");
        }

        var task = new UserTask(request.title, userId.Value);

        await _taskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}