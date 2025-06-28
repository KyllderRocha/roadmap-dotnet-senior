using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string name, string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}