using MediatR;
using TaskManager.Domain.Entities;

public record GetAllUsersQuery() : IRequest<IEnumerable<User>>;