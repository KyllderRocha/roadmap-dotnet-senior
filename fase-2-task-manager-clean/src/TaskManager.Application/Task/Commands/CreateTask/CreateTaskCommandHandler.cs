using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, UserTask>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(ITaskRepository userRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserTask> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new UserTask(request.title, request.userId);

        await _taskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task;
    }
}