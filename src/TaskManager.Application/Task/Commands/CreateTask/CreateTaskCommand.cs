using MediatR;
using TaskManager.Domain.Entities;

public record CreateTaskCommand(string title) : IRequest<UserTask>;