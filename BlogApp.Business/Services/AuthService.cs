using BlogApp.Data.Interfaces;
using BlogApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApp.Data.DTOs;

namespace BlogApp.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IUserService userService,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            // Находим пользователя по имени
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);

            if (user == null)
                throw new UnauthorizedAccessException("Неверное имя пользователя или пароль");

            // Получаем роли пользователя
            var roles = await _userService.GetUserRolesAsync(user.Id);

            // Генерируем токен
            var token = _jwtService.GenerateToken(user, roles);

            // Возвращаем ответ
            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Roles = roles
            };
        }

        public async Task<bool> RegisterUserAsync(RegisterRequest request)
        {
            // Проверяем, существует ли пользователь с таким email
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new ArgumentException("Пользователь с таким email уже существует");

            // Проверяем, существует ли пользователь с таким username
            if (await _userRepository.UsernameExistsAsync(request.Username))
                throw new ArgumentException("Пользователь с таким именем уже существует");

            // Проверяем совпадение паролей
            if (request.Password != request.ConfirmPassword)
                throw new ArgumentException("Пароли не совпадают");

            // Создаем нового пользователя
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            // Сохраняем пользователя
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Назначаем роль "User" по умолчанию
            var userRoleAssigned = await _userService.AssignRoleToUserAsync(user.Id, 3);

            return userRoleAssigned;
        }

        private string HashPassword(string password)
        {
            return password;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return password == storedHash;
        }
    }
}