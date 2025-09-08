using MediatR;
using TaskManager.Domain.Entities;

public record CreateUserCommand(string Name, string Email, string Password) : IRequest<User>;