using BlogApp.Data.DTOs;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<bool> RegisterUserAsync(RegisterRequest request);
    }
}