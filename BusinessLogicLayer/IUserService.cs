using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetById(Guid id);

        User GetUserByEmail(UserLoginRequestDto user);
        Task<bool> CreateUser(User user);
    }
}
