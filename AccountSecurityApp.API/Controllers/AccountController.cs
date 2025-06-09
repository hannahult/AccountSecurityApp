using AccountSecurityApp.API.Data;
using AccountSecurityApp.API.DTOs;
using AccountSecurityApp.API.Helpers;
using AccountSecurityApp.API.Models;
using AccountSecurityApp.API.Services.Interfaces;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountSecurityApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var success = await _userService.RegisterAsync(request);
            if (!success)
                return BadRequest("Invalid registration data");

            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _userService.LoginAsync(request);
            if (result == null)
                return Unauthorized("Invalid username or password.");

            return Ok(result);
        }
    }
}
