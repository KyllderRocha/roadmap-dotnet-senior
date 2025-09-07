using MediatR;

namespace TaskManager.Application.Users.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<AuthenticationResult>;