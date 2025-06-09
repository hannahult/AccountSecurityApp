using AccountSecurityApp.API.Data;
using AccountSecurityApp.API.DTOs;
using AccountSecurityApp.API.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountSecurityApp.API.Helpers;

namespace AccountSecurityApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password needed.");

            bool userExists = await _dbContext.Users.AnyAsync(u => u.Username == request.Username);
            if (userExists)
                return Conflict("Username is taken");

            if (!PasswordHelper.IsPasswordValid(request.Password, out var passwordError))
            {
                return BadRequest(passwordError);
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Register ´succeded");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password needed.");

            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Unauthorized("Wrong username or password.");

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordCorrect)
                return Unauthorized("Wrong username or password.");

            return Ok("Login succeded");
        }
    }
}
