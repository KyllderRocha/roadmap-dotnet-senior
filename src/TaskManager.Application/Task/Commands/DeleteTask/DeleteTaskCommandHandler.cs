using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserTask> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.id);
        if (task == null) return null;

        _taskRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}