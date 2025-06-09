using AccountSecurityApp.API.DTOs;

namespace AccountSecurityApp.API.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterRequestDTO dto);
        Task<string?> LoginAsync(LoginRequestDTO dto);
    }
}
