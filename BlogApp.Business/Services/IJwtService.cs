using BlogApp.Data.Entities;
using System.Collections.Generic;

namespace BlogApp.Business.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user, List<string> roles);
    }
}