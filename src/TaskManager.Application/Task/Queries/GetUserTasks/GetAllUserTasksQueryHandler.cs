using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

public class GetAllUserTasksQueryHandler : IRequestHandler<GetAllUserTasksQuery, IEnumerable<UserTask>>
{
    private readonly ITaskRepository _userTaskRepository;

    public GetAllUserTasksQueryHandler(ITaskRepository userTaskRepository)
    {
        _userTaskRepository = userTaskRepository;
    }

    public async Task<IEnumerable<UserTask>> Handle(GetAllUserTasksQuery request, CancellationToken cancellationToken)
    {
        return await _userTaskRepository.GetAllByUserIdAsync(request.UserId);
    }
}