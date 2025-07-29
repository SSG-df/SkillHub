using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;
using System.Security.Claims;

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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reviewService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reviewService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("lesson/{lessonId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByLessonId(Guid lessonId)
        {
            var result = await _reviewService.GetByLessonIdAsync(lessonId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Learner,Admin")]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
        {
            try
            {
                var result = await _reviewService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReviewUpdateDto dto)
        {
            var success = await _reviewService.UpdateAsync(id, dto);
            
            if (!success)
            {
                var review = await _reviewService.GetByIdAsync(id);
                return review == null ? NotFound() : Forbid();
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _reviewService.DeleteAsync(id);
            
            if (!success)
            {
                var review = await _reviewService.GetByIdAsync(id);
                return review == null ? NotFound() : Forbid();
            }
            
            return NoContent();
        }
    }
}