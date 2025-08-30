using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common.DTOs.Student;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using SchoolManagementSystem.Application.Common.Interfaces;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUniqueIdGenerator _uniqueIdGenerator;

        public StudentsController(
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IUniqueIdGenerator uniqueIdGenerator)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _uniqueIdGenerator = uniqueIdGenerator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            var students = await _studentRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<StudentDto>>(students));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
        {
            var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var student = await _studentRepository.GetStudentWithDetailsAsync(id);
            if (student == null) return NotFound();

            if (currentUserRole == "Student" && student.UserId != currentUserId)
                return Forbid();

            return Ok(_mapper.Map<StudentDto>(student));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto dto)
        {
            if (await _userRepository.EmailExistAsync(dto.Email))
                return BadRequest("Email already exists");

            // Create user and hash password
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Role = "Student",
                IsActive = true
            };
            _passwordHasher.CreatePassword(dto.Password, out var hash, out var salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _userRepository.AddAsync(user);

            // Create student with unique ID
            var student = new Student
            {
                StudentId = await _uniqueIdGenerator.GenerateStudentIdAsync(),
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                EnrollmentDate = dto.EnrollmentDate,
                UserId = user.Id
            };

            await _studentRepository.AddAsync(student);

            var studentDto = _mapper.Map<StudentDto>(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, StudentDto studentDto)
        {
            var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound();

            if (currentUserRole == "Student" && student.UserId != currentUserId)
                return Forbid();

            _mapper.Map(studentDto, student);
            await _studentRepository.UpdateAsync(student);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound();

            await _studentRepository.DeleteAsync(student);
            return NoContent();
        }

        [HttpPost("{id}/suspend")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> SuspendStudent(Guid id, [FromBody] SuspendStudentRequest request)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound();

            student.IsSuspended = true;
            student.SuspensionEndDate = request.EndDate;
            student.SuspensionReason = request.Reason;

            await _studentRepository.UpdateAsync(student);
            return NoContent();
        }

        [HttpPost("{id}/unsuspend")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnsuspendStudent(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound();

            student.IsSuspended = false;
            student.SuspensionEndDate = DateTime.MinValue;
            student.SuspensionReason = string.Empty;

            await _studentRepository.UpdateAsync(student);
            return NoContent();
        }

        [HttpGet("suspended")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetSuspendedStudents()
        {
            var students = await _studentRepository.GetSuspendedStudentsAsync();
            return Ok(_mapper.Map<IEnumerable<StudentDto>>(students));
        }
    }

    public class SuspendStudentRequest
    {
        public DateTime EndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
