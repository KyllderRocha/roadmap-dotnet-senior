using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using BCrypt.Net;
using TaskManager.Application.Common.Interfaces;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBusProducer _messageBus;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMessageBusProducer messageBus)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Name, request.Email);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.SetPassword(passwordHash);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userCreatedEvent = new UserCreatedEvent(user.Id, user.Email);
        await _messageBus.PublishAsync(userCreatedEvent);

        return user;
    }
}

public record UserCreatedEvent(Guid UserId, string Email);