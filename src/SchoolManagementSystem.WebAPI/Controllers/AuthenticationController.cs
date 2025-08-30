using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common.DTOs.Authentication;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthController(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Invalid credentials");
            }

            if (!user.IsActive)
            {
                return Unauthorized("Account is deactivated");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60), // align with JwtSetting.ExpiryMinute if needed
                Email = user.Email,
                Role = user.Role,
                FullName = $"{user.FirstName} {user.LastName}"
            });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _userRepository.EmailExistAsync(request.Email))
            {
                return BadRequest("Email already exists");
            }

            _passwordHasher.CreatePassword(request.Password, out var hash, out var salt);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = request.Role,
                IsActive = true
            };

            await _userRepository.AddAsync(user);
            return Ok("User registered successfully");
        }
    }
}
