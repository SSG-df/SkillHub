using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reviewService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reviewService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetByLessonId(Guid lessonId)
        {
            var result = await _reviewService.GetByLessonIdAsync(lessonId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
        {
            var result = await _reviewService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReviewUpdateDto dto)
        {
            var success = await _reviewService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _reviewService.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
