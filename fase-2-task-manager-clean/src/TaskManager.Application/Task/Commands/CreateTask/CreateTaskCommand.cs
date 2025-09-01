using MediatR;
using TaskManager.Domain.Entities;

public record CreateTaskCommand(string title, Guid userId) : IRequest<UserTask>;