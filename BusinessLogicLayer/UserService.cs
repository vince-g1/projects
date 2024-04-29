using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetById(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
        public User GetUser(UserLoginRequestDto user)
        {
            return  _userRepository.GetUser(user);
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }
        public async Task<bool> CreateUser(User user)
        {
            return await _userRepository.CreateUser(user);
        }
    }
}
