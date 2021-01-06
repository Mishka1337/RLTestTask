using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RLTestTask.Exceptions;
using RLTestTask.Models;

namespace RLTestTask.Services
{
    public interface IUserService 
    {
        Task<IEnumerable<UserDto>> GetAllUsersOnPage(int page, int num);
        Task<UserDto> GetUserById(int id);
        Task<UserDto> CreateUser(UserDto userDto);
        Task<UserDto> UpdateUser(UserDto userDto);
        Task DeleteUser(int id);

        Task<IEnumerable<UserDto>> GetAllUsersByRole(int roleId, int page, int num);
    }

    public class UserService : IUserService
    {
        private ApplicationContext dbContext;
        private IPasswordHasher<User> passwordHasher;
        public UserService(ApplicationContext context, IPasswordHasher<User> passwordHasher)
        {
            this.dbContext = context;
            this.passwordHasher = passwordHasher;
        }
        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            bool isLoginAlreadyExists = dbContext
                        .Users
                        .Any(u => u.Login == userDto.Login);
            if (isLoginAlreadyExists) 
            {
                throw new UserServiceException("Login already exists");
            }
            if (String.IsNullOrWhiteSpace(userDto.Password))
            {
                throw new UserServiceException("User's password should not be empty or null");
            }
            User user = new User(userDto);
            restoreRolesOfUsers(user, userDto.Roles);
            user.Password = passwordHasher.HashPassword(user, user.Password);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return new UserDto(user);
        }

        public async Task DeleteUser(int id)
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) 
            {
                return;
            }
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersByRole(int roleId, int page, int num)
        {
            Role role = await dbContext.Roles
                .Include(r => r.Users)
                .ThenInclude(u => u.Roles)
                .FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
            {
                return null;
            }
            IEnumerable<UserDto> userDtos = role.Users.Skip(page * num).Take(num).Select(u => new UserDto(u));
            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersOnPage(int page, int num)
        {
            return await dbContext
                    .Users
                    .Skip(page * num)
                    .Take(num)
                    .Include(u => u.Roles)
                    .Select(u => new UserDto(u))
                    .ToArrayAsync();
        }

        public async Task<UserDto> GetUserById(int id)
        {
            User user = await dbContext
                .Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }
            await dbContext.Entry(user).Collection(u => u.Roles).LoadAsync();
            return new UserDto(user);
        }

        public async Task<UserDto> UpdateUser(UserDto userDto)
        {
            if (userDto.Id == default(int))
            {
                throw new UserServiceException("Expected user's id");
            }
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id);
            if (user == null)
            {
                throw new UserServiceException("Expected existing user");
            }
            if (user.Login != userDto.Login)
            {
                throw new UserServiceException("Cannot change login");
            }
            await dbContext.Entry(user).Collection(u => u.Roles).LoadAsync();
            string userPassword = user.Password;
            user.FillDataFromDto(userDto);
            if (String.IsNullOrWhiteSpace(userDto.Password))
            {
                user.Password = userPassword;
            }
            else 
            {
                user.Password = passwordHasher.HashPassword(user,user.Password);
            }
            restoreRolesOfUsers(user, userDto.Roles);
            await dbContext.SaveChangesAsync();
            return new UserDto(user);
        }

        private void restoreRolesOfUsers(User user,IEnumerable<RoleDto> roles)
        {
            List<Role> loadedRoles = new List<Role>();
            foreach(RoleDto roleDto in roles)
            {
                Role role = dbContext.Roles.FirstOrDefault(r => roleDto.Id == r.Id);
                loadedRoles.Add(role);
            }
            if (loadedRoles.Count == 0)
            {
                return;
            }
            user.Roles = loadedRoles;
            if (user.Roles.Any(r => r == null))
            {
                throw new UserServiceException("Recieved invalid roles");
            }
        }
    }
}