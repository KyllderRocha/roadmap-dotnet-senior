using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class GetUserTaskByIdQueryHandler : IRequestHandler<GetUserTaskByIdQuery, UserTask>
{
    private readonly ITaskRepository _userTaskRepository;

    public GetUserTaskByIdQueryHandler(ITaskRepository userTaskRepository)
    {
        _userTaskRepository = userTaskRepository;
    }

    public async Task<UserTask> Handle(GetUserTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userTaskRepository.GetByIdAsync(request.TaskId);
    }
}