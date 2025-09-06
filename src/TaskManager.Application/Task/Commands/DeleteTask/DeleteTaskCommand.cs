using MediatR;
using TaskManager.Domain.Entities;

public record DeleteTaskCommand(Guid id) : IRequest<UserTask>;