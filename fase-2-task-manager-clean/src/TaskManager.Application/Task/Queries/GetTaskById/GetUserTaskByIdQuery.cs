using MediatR;
using TaskManager.Domain.Entities;

public record GetUserTaskByIdQuery(Guid TaskId) : IRequest<UserTask>;