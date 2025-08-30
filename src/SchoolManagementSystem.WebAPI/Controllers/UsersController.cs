using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common.DTOs.User;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;


namespace SchoolManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _userRepository.GetUserWithDetailsAsync(id);
            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.IsActive = true;
            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(string role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }
    }
}
