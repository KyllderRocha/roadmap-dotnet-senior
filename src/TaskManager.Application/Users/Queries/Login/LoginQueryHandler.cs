using MediatR;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Interfaces;
using BCrypt.Net;

namespace TaskManager.Application.Users.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(query.Email); 

        if (user is null || !BCrypt.Net.BCrypt.Verify(query.Password, user.PasswordHash))
        {
            throw new Exception("Credenciais inválidas."); 
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(token);
    }
}