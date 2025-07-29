using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;
using System.Security.Claims;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LessonTagController : ControllerBase
    {
        private readonly ILessonTagService _lessonTagService;
        private readonly ILessonService _lessonService;
        private readonly ITagService _tagService;

        public LessonTagController(
            ILessonTagService lessonTagService,
            ILessonService lessonService,
            ITagService tagService)
        {
            _lessonTagService = lessonTagService;
            _lessonService = lessonService;
            _tagService = tagService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _lessonTagService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{lessonId}/{tagId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid lessonId, Guid tagId)
        {
            var result = await _lessonTagService.GetByIdAsync(lessonId, tagId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Mentor,Admin")]
        public async Task<IActionResult> Create([FromBody] LessonTagCreateDto dto)
        {
            var currentUserId = GetCurrentUserId();
            
            var allTags = await _tagService.GetAllAsync();
            var existingTag = allTags.FirstOrDefault(t => t.Name.Equals(dto.TagName, StringComparison.OrdinalIgnoreCase));
            if (existingTag != null)
            {
                var tagDetails = await _tagService.GetByIdAsync(existingTag.Id);
                if (tagDetails?.CreatedBy != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid("You don't have permission to use this tag");
                }
            }

            if (User.IsInRole("Mentor"))
            {
                var lesson = await _lessonService.GetByIdAsync(dto.LessonId);
                if (lesson?.MentorId != currentUserId)
                {
                    return Forbid("You don't own this lesson");
                }
            }
            try
            {
                var result = await _lessonTagService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), 
                    new { lessonId = result.LessonId, tagId = result.TagId }, 
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{lessonId}/{tagId}")]
        [Authorize(Roles = "Mentor,Admin")]
        public async Task<IActionResult> Delete(Guid lessonId, Guid tagId)
        {
            var currentUserId = GetCurrentUserId();
            
            if (User.IsInRole("Mentor"))
            {
                var lesson = await _lessonService.GetByIdAsync(lessonId);
                if (lesson?.MentorId != currentUserId)
                {
                    return Forbid("You don't own this lesson");
                }
            }

            var lessonTag = await _lessonTagService.GetByIdAsync(lessonId, tagId);
            if (lessonTag != null)
            {
                var tag = await _tagService.GetByIdAsync(lessonTag.TagId);
                if (tag?.CreatedBy != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid("You don't own this tag");
                }
            }

            var success = await _lessonTagService.DeleteAsync(lessonId, tagId);
            return success ? NoContent() : NotFound();
        }

        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userId);
        }
    }
}