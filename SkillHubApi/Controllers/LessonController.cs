using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var lessons = await _lessonService.GetAllAsync(pageNumber, pageSize);
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LessonCreateDto dto)
        {
            var result = await _lessonService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LessonUpdateDto dto)
        {
            var success = await _lessonService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _lessonService.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
