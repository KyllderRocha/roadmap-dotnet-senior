using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

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
        var user = new User(request.Name, request.Email);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}