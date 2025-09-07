using MediatR;
using TaskManager.Domain.Entities;

public record GetAllUserTasksQuery() : IRequest<IEnumerable<UserTask>>;