using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common.DTOs;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuspensionRequestsController : ControllerBase
    {
        private readonly ISuspensionRequestRepository _suspensionRequestRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public SuspensionRequestsController(
            ISuspensionRequestRepository suspensionRequestRepository,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            IMapper mapper)
        {
            _suspensionRequestRepository = suspensionRequestRepository;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SuspensionRequestDto>>> GetAllSuspensionRequests()
        {
            var requests = await _suspensionRequestRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SuspensionRequestDto>>(requests));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuspensionRequestDto>> GetSuspensionRequest(Guid id)
        {
            var request = await _suspensionRequestRepository.GetWithDetailsAsync(id);
            if (request == null) return NotFound();

            // Teachers can only view their own requests
            var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserRole == "Teacher" && request.Teacher.UserId != currentUserId)
                return Forbid();

            return Ok(_mapper.Map<SuspensionRequestDto>(request));
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<SuspensionRequestDto>> CreateSuspensionRequest(CreateSuspensionRequestDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(dto.StudentId);
            if (student == null) return BadRequest("Student not found");

            var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var teacher = (await _teacherRepository.FindAsync(t => t.UserId == currentUserId)).FirstOrDefault();
            if (teacher == null) return Forbid();

            var request = new SuspensionRequest
            {
                Reason = dto.Reason,
                Evidence = dto.Evidence,
                RequestDate = DateTime.UtcNow,
                Status = SuspensionStatus.Pending,
                StudentId = dto.StudentId,
                TeacherId = teacher.Id
            };

            await _suspensionRequestRepository.AddAsync(request);
            return CreatedAtAction(nameof(GetSuspensionRequest), new { id = request.Id }, _mapper.Map<SuspensionRequestDto>(request));
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveSuspensionRequest(Guid id, [FromBody] ApproveSuspensionRequestDto dto)
        {
            var request = await _suspensionRequestRepository.GetWithDetailsAsync(id);
            if (request == null) return NotFound();
            if (request.Status != SuspensionStatus.Pending) return BadRequest("Request is not pending");

            request.Status = SuspensionStatus.Approved;
            request.SuspensionStartDate = DateTime.UtcNow;
            request.SuspensionEndDate = dto.EndDate;
            request.AdminComments = dto.Comments;
            request.ApprovedById = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            // Update student suspension status
            var student = await _studentRepository.GetByIdAsync(request.StudentId);
            if (student != null)
            {
                student.IsSuspended = true;
                student.SuspensionEndDate = dto.EndDate;
                student.SuspensionReason = request.Reason;
                await _studentRepository.UpdateAsync(student);
            }

            await _suspensionRequestRepository.UpdateAsync(request);
            return NoContent();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectSuspensionRequest(Guid id, [FromBody] RejectSuspensionRequestDto dto)
        {
            var request = await _suspensionRequestRepository.GetWithDetailsAsync(id);
            if (request == null) return NotFound();
            if (request.Status != SuspensionStatus.Pending) return BadRequest("Request is not pending");

            request.Status = SuspensionStatus.Rejected;
            request.AdminComments = dto.Comments;
            request.ApprovedById = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            await _suspensionRequestRepository.UpdateAsync(request);
            return NoContent();
        }

        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<SuspensionRequestDto>>> GetSuspensionRequestsByStudent(Guid studentId)
        {
            var requests = await _suspensionRequestRepository.GetByStudentIdAsync(studentId);
            return Ok(_mapper.Map<IEnumerable<SuspensionRequestDto>>(requests));
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SuspensionRequestDto>>> GetSuspensionRequestsByStatus(SuspensionStatus status)
        {
            var requests = await _suspensionRequestRepository.GetByStatusAsync(status);
            return Ok(_mapper.Map<IEnumerable<SuspensionRequestDto>>(requests));
        }
    }

    public class ApproveSuspensionRequestDto
    {
        public DateTime EndDate { get; set; }
        public string Comments { get; set; } = string.Empty;
    }

    public class RejectSuspensionRequestDto
    {
        public string Comments { get; set; } = string.Empty;
    }
}
