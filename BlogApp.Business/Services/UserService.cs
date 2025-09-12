using BlogApp.Data.Interfaces;
using BlogApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Проверяем, существует ли пользователь с таким email
            if (await _userRepository.EmailExistsAsync(user.Email))
            {
                throw new ArgumentException("Пользователь с таким email уже существует");
            }

            // Проверяем, существует ли пользователь с таким username
            if (await _userRepository.UsernameExistsAsync(user.Username))
            {
                throw new ArgumentException("Пользователь с таким именем уже существует");
            }

            // Устанавливаем роль "Пользователь" по умолчанию
            user.Role = "Пользователь";
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            return user;
        }
        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var user = await _userRepository.GetUserWithRolesAsync(userId);
            return user?.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>();
        }

        //public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        //{
        //    // Проверяем, существует ли пользователь
        //    if (!await _userRepository.UserExistsAsync(userId))
        //        return false;

        //    // Проверяем, существует ли роль
        //    if (!await _roleRepository.RoleExistsAsync(roleId))
        //        return false;

        //    // Проверяем, не назначена ли уже эта роль пользователю
        //    if (await _userRoleRepository.UserRoleExistsAsync(userId, roleId))
        //        return false;

        //    // Назначаем роль
        //    var userRole = new UserRole { UserId = userId, RoleId = roleId };
        //    await _userRoleRepository.AddAsync(userRole);
        //    await _userRoleRepository.SaveChangesAsync();

        //    return true;
        //}

        //public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        //{
        //    // Проверяем, существует ли связь пользователь-роль
        //    var userRole = await _userRoleRepository.GetUserRoleAsync(userId, roleId);
        //    if (userRole == null)
        //        return false;

        //    // Удаляем связь
        //    _userRoleRepository.Remove(userRole);
        //    await _userRoleRepository.SaveChangesAsync();

        //    return true;
        //}

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            // Проверяем, не используется ли email другим пользователем
            if (existingUser.Email != user.Email && await _userRepository.EmailExistsAsync(user.Email))
            {
                throw new ArgumentException("Пользователь с таким email уже существует");
            }

            // Проверяем, не используется ли username другим пользователем
            if (existingUser.Username != user.Username && await _userRepository.UsernameExistsAsync(user.Username))
            {
                throw new ArgumentException("Пользователь с таким именем уже существует");
            }

            // Обновляем только разрешенные поля
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;

            // Пароль обновляем только если он предоставлен
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                existingUser.PasswordHash = user.PasswordHash;
            }

            _userRepository.Update(existingUser);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id) != null;
        }

        Task<User> IUserService.GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserService.GetUserByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            // Проверяем, существует ли пользователь
            if (!await _userRepository.UserExistsAsync(userId))
                return false;

            // Проверяем, существует ли роль
            if (!await _roleRepository.RoleExistsAsync(roleId))
                return false;

            // Проверяем, не назначена ли уже эта роль пользователю
            if (await _userRoleRepository.UserRoleExistsAsync(userId, roleId))
                return false;

            // Назначаем роль
            var userRole = new UserRole { UserId = userId, RoleId = roleId };
            await _userRoleRepository.AddAsync(userRole);
            await _userRoleRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            // Проверяем, существует ли связь пользователь-роль
            var userRole = await _userRoleRepository.GetUserRoleAsync(userId, roleId);
            if (userRole == null)
                return false;

            // Удаляем связь
            _userRoleRepository.Remove(userRole);
            await _userRoleRepository.SaveChangesAsync();

            return true;
        }
    }
}