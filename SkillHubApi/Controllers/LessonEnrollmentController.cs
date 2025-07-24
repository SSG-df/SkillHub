using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonEnrollmentController : ControllerBase
    {
        private readonly ILessonEnrollmentService _lessonEnrollmentService;

        public LessonEnrollmentController(ILessonEnrollmentService lessonEnrollmentService)
        {
            _lessonEnrollmentService = lessonEnrollmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _lessonEnrollmentService.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var enrollment = await _lessonEnrollmentService.GetByIdAsync(id);
            if (enrollment == null) return NotFound();
            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LessonEnrollmentCreateDto dto)
        {
            var result = await _lessonEnrollmentService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _lessonEnrollmentService.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
