using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string name, string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}