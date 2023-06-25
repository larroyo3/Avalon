using System;
using System.Collections.Generic;
using Avalon_API.Models;

namespace Avalon_API.DAL
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<User> GetUserByIDAsync(long id);
        Task<User> Login(Auth auth);
        void InsertUser(User item);
        void DeleteUser(User item);
        void UpdateUser(User item);
        Task SaveAsync();
        bool UserExists(long id);
    }
}