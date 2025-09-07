using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;


namespace TaskManager.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        // As "claims" são informações que queremos guardar dentro do token (ex: ID e email do utilizador)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            // Adicionaremos mais claims no futuro, como roles (cargos)
        };

        // A chave secreta que usaremos para assinar o token. Deve ser guardada de forma segura!
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // A data de expiração do token
        var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpiryHours"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}