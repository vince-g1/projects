using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid id);
        User GetUser(UserLoginRequestDto user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> CreateUser(User user);
    }
}
