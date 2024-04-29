using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _dbContext;

        public UserRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Users.FindAsync(id);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get user with the given email and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User GetUser(UserLoginRequestDto user)
        {
            try
            {
                return _dbContext.Users.First(x => x.Email == user.Email && x.Password==user.Password);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                return await _dbContext.Users.ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Create new user from the registration form details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
