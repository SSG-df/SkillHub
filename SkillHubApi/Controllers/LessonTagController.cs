using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonTagController : ControllerBase
    {
        private readonly ILessonTagService _lessonTagService;

        public LessonTagController(ILessonTagService lessonTagService)
        {
            _lessonTagService = lessonTagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _lessonTagService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{lessonId}/{tagId}")]
        public async Task<IActionResult> GetById(Guid lessonId, Guid tagId)
        {
            var result = await _lessonTagService.GetByIdAsync(lessonId, tagId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LessonTagCreateDto dto)
        {
            var result = await _lessonTagService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{lessonId}/{tagId}")]
        public async Task<IActionResult> Delete(Guid lessonId, Guid tagId)
        {
            var success = await _lessonTagService.DeleteAsync(lessonId, tagId);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
