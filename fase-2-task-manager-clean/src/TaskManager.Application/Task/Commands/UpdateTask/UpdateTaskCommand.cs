using MediatR;
using TaskManager.Domain.Entities;

public record UpdateTaskCommand(
    Guid Id,             
    string Title,
    bool IsDone,
    Guid UserId
) : IRequest<UserTask>;