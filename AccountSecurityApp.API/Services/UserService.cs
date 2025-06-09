using AccountSecurityApp.API.Data;
using AccountSecurityApp.API.Models;
using AccountSecurityApp.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AccountSecurityApp.API.DTOs;
using AccountSecurityApp.API.Services.Interfaces;

namespace AccountSecurityApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> RegisterAsync(RegisterRequestDTO dto)
        {
            if (!PasswordHelper.IsPasswordValid(dto.Password, out _))
                return false;

            var existingUser = await _dbContext.Users.AnyAsync(u => u.Username == dto.Username);
            if (existingUser)
                return false;

            var user = new User
            {
                Username = dto.Username
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<string?> LoginAsync(LoginRequestDTO dto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return "Login successful";
        }
    }
}
