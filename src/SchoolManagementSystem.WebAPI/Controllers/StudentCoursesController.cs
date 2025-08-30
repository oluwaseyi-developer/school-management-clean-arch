using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common.DTOs;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentCoursesController : ControllerBase
    {
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public StudentCoursesController(
            IStudentCourseRepository studentCourseRepository,
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
        {
            _studentCourseRepository = studentCourseRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<StudentCourseDto>> EnrollStudent(EnrollStudentDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(dto.StudentId);
            if (student == null) return BadRequest("Student not found");

            var course = await _courseRepository.GetByIdAsync(dto.CourseId);
            if (course == null) return BadRequest("Course not found");

            var existingEnrollment = await _studentCourseRepository.GetByStudentAndCourseAsync(dto.StudentId, dto.CourseId);
            if (existingEnrollment != null) return BadRequest("Student already enrolled");

            var enrollment = new StudentCourse
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                EnrollmentDate = DateTime.UtcNow
            };

            await _studentCourseRepository.AddAsync(enrollment);
            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, _mapper.Map<StudentCourseDto>(enrollment));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentCourseDto>> GetEnrollment(Guid id)
        {
            var enrollment = await _studentCourseRepository.GetByIdAsync(id);
            if (enrollment == null) return NotFound();

            return Ok(_mapper.Map<StudentCourseDto>(enrollment));
        }

        [HttpPut("{id}/grade")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateGrade(Guid id, [FromBody] UpdateGradeDto dto)
        {
            var enrollment = await _studentCourseRepository.GetByIdAsync(id);
            if (enrollment == null) return NotFound();

            enrollment.Grade = dto.Grade;
            await _studentCourseRepository.UpdateAsync(enrollment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UnenrollStudent(Guid id)
        {
            var enrollment = await _studentCourseRepository.GetByIdAsync(id);
            if (enrollment == null) return NotFound();

            await _studentCourseRepository.DeleteAsync(enrollment);
            return NoContent();
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<StudentCourseDto>>> GetStudentEnrollments(Guid studentId)
        {
            var enrollments = await _studentCourseRepository.GetByStudentIdAsync(studentId);
            return Ok(_mapper.Map<IEnumerable<StudentCourseDto>>(enrollments));
        }

        [HttpGet("course/{courseId}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentCourseDto>>> GetCourseEnrollments(Guid courseId)
        {
            var enrollments = await _studentCourseRepository.GetByCourseIdAsync(courseId);
            return Ok(_mapper.Map<IEnumerable<StudentCourseDto>>(enrollments));
        }

        [HttpGet("grades/{min}/{max}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentCourseDto>>> GetEnrollmentsByGradeRange(decimal min, decimal max)
        {
            var enrollments = await _studentCourseRepository.GetByGradeRangeAsync(min, max);
            return Ok(_mapper.Map<IEnumerable<StudentCourseDto>>(enrollments));
        }
    }

    public class UpdateGradeDto
    {
        public decimal Grade { get; set; }
    }
}
