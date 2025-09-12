using BlogApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(BlogDbContext context)
        {
            // Применяем миграции
            await context.Database.MigrateAsync();

            // Проверяем, есть ли уже роли
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(
                    new Role { Id = 1, Name = "Administrator" },
                    new Role { Id = 2, Name = "Moderator" },
                    new Role { Id = 3, Name = "User" }
                );
                await context.SaveChangesAsync();
            }

            // Проверяем, есть ли уже пользователи
            if (!await context.Users.AnyAsync())
            {
                // Создаем тестовых пользователей
                var users = new[]
                {
                    new User { Username = "admin", Email = "admin@example.com", PasswordHash = "hashed_password" },
                    new User { Username = "moderator", Email = "moderator@example.com", PasswordHash = "hashed_password" },
                    new User { Username = "user", Email = "user@example.com", PasswordHash = "hashed_password" }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();

                // Назначаем роли пользователям
                await context.UserRoles.AddRangeAsync(
                    new UserRole { UserId = users[0].Id, RoleId = 1 }, // admin - Administrator
                    new UserRole { UserId = users[1].Id, RoleId = 2 }, // moderator - Moderator
                    new UserRole { UserId = users[2].Id, RoleId = 3 }  // user - User
                );

                await context.SaveChangesAsync();
            }
        }
    }
}