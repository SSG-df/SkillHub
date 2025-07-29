using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using SkillHubApi.Services;
using System.Security.Claims;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var currentUserRole = GetCurrentUserRole();
            var lessons = await _lessonService.GetAllAsync(pageNumber, pageSize);
            return Ok(lessons);
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm,
            [FromQuery] Guid? mentorId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] List<Guid>? tagIds,
            [FromQuery] DifficultyLevel? difficulty,
            [FromQuery] int? minCapacity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var lessons = await _lessonService.SearchAsync(
                searchTerm,
                mentorId,
                startDate,
                endDate,
                tagIds,
                difficulty,
                minCapacity,
                pageNumber,
                pageSize);

            return Ok(lessons);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LessonCreateDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == "Mentor" && dto.MentorId != currentUserId)
                return Forbid();

            try
            {
                var result = await _lessonService.CreateAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LessonUpdateDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == "Mentor")
            {
                var lesson = await _lessonService.GetByIdAsync(id);
                if (lesson?.MentorId != currentUserId)
                    return Forbid();
            }

            try
            {
                var success = await _lessonService.UpdateAsync(id, dto);
                return success ? Ok() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == "Mentor")
            {
                var lesson = await _lessonService.GetByIdAsync(id);
                if (lesson?.MentorId != currentUserId)
                    return Forbid();
            }

            var success = await _lessonService.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }

        private Guid GetCurrentUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}