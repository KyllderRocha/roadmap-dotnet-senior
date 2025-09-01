using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(ITaskRepository userRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserTask> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);

        if (task == null)
            return null;

        task.SetTitle(request.Title);
        task.SetIsDone(request.IsDone);
        task.SetUserId(request.UserId);

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}