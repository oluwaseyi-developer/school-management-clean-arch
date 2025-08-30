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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
        {
            var course = await _courseRepository.GetCourseWithStudentsAsync(id);
            if (course == null) return NotFound();

            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto dto)
        {
            if (await _courseRepository.CourseCodeExistsAsync(dto.CourseCode))
                return BadRequest("Course code already exists");

            var course = new Course
            {
                CourseCode = dto.CourseCode,
                CourseName = dto.CourseName,
                CourseDescription = dto.Description,
                Credits = dto.Credits
            };

            await _courseRepository.AddAsync(course);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, _mapper.Map<CourseDto>(course));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateCourse(Guid id, CourseDto dto)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return NotFound();

            _mapper.Map(dto, course);
            await _courseRepository.UpdateAsync(course);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return NotFound();

            await _courseRepository.DeleteAsync(course);
            return NoContent();
        }

        [HttpGet("credits/{min}/{max}")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByCredits(int min, int max)
        {
            var courses = await _courseRepository.GetCoursesByCreditsAsync(min, max);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(courses));
        }
    }
}
