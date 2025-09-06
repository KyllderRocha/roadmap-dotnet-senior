using MediatR;
using TaskManager.Domain.Entities;

public record GetAllUserTasksQuery(Guid UserId) : IRequest<IEnumerable<UserTask>>;