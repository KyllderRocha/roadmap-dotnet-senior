using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using BCrypt.Net;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Name, request.Email);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.SetPassword(passwordHash);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}