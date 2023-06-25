using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Avalon_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Avalon_API.DAL
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private UserContext context;

        public UserRepository(UserContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            return await context.Users.Select(x => ItemToDTO(x)).ToListAsync();
        }

        public async Task<User> GetUserByIDAsync(long id)
        {
            var item = await context.Users.FindAsync(id);
            if (item == null)
            {
                throw new Exception();
            }

            return item;
        }

        public void InsertUser(User item)
        {
            context.Users.Add(item);
        }

        public void DeleteUser(User item)
        {
            context.Users.Remove(item);
        }

        public void UpdateUser(User item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static UserDTO ItemToDTO(User user) =>
        new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            ProfilePhoto = user.ProfilePhoto,
            Package = user.Package,
            RemainingPhoto = user.RemainingPhoto,
            Password = user.Password
        };

        public bool UserExists(long id)
        {
            return context.Users.Any(e => e.Id == id);
        }

        public async Task<User> Login(Auth auth)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == auth.Name);
            if (user == null)
            {
                throw new Exception();
            }

            return user;
        }
    }
}