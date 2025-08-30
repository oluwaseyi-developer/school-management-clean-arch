using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using SchoolManagementSystem.Application.Common.Interfaces;
using SchoolManagementSystem.Domain.Entities;
using AutoMapper;
using SchoolManagementSystem.Application.Common.DTOs;

namespace SchoolManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUniqueIdGenerator _uniqueIdGenerator;

        public TeachersController(
            ITeacherRepository teacherRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IUniqueIdGenerator uniqueIdGenerator)
        {
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _uniqueIdGenerator = uniqueIdGenerator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers()
        {
            var teachers = await _teacherRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TeacherDto>>(teachers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDto>> GetTeacher(Guid id)
        {
            var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var teacher = await _teacherRepository.GetTeacherWithDetailsAsync(id);
            if (teacher == null) return NotFound();

            if (currentUserRole == "Teacher" && teacher.UserId != currentUserId)
                return Forbid();

            return Ok(_mapper.Map<TeacherDto>(teacher));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TeacherDto>> CreateTeacher(CreateTeacherDto dto)
        {
            if (await _userRepository.EmailExistAsync(dto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Role = "Teacher",
                IsActive = true
            };

            _passwordHasher.CreatePassword(dto.Password, out var hash, out var salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _userRepository.AddAsync(user);

            var teacher = new Teacher
            {
                TeacherId = await _uniqueIdGenerator.GenerateTeacherIdAsync(),
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                HireDate = dto.HireDate,
                SubjectSpecialty = dto.SubjectSpecialty,
                UserId = user.Id
            };

            await _teacherRepository.AddAsync(teacher);

            var teacherDto = _mapper.Map<TeacherDto>(teacher);
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacherDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(Guid id, TeacherDto teacherDto)
        {
            var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher == null) return NotFound();

            if (currentUserRole == "Teacher" && teacher.UserId != currentUserId)
                return Forbid();

            _mapper.Map(teacherDto, teacher);
            await _teacherRepository.UpdateAsync(teacher);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher == null) return NotFound();

            await _teacherRepository.DeleteAsync(teacher);
            return NoContent();
        }

        [HttpGet("subject/{subject}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachersBySubject(string subject)
        {
            var teachers = await _teacherRepository.GetTeachersBySubjectAsync(subject);
            return Ok(_mapper.Map<IEnumerable<TeacherDto>>(teachers));
        }
    }
}
