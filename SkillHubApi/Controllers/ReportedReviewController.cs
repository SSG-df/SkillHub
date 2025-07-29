using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportedReviewController : ControllerBase
    {
        private readonly IReportedReviewService _reportedReviewService;

        public ReportedReviewController(IReportedReviewService reportedReviewService)
        {
            _reportedReviewService = reportedReviewService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<ReportedReviewDto>>> GetAll()
        {
            return Ok(await _reportedReviewService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ReportedReviewDto>> GetById(Guid id)
        {
            var report = await _reportedReviewService.GetByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [HttpPost]
        [Authorize(Roles = "Learner,Mentor,Admin")]
        public async Task<ActionResult<ReportedReviewDto>> Create([FromBody] ReportedReviewCreateDto dto)
        {
            try
            {
                var result = await _reportedReviewService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReportedReviewUpdateDto dto)
        {
            var success = await _reportedReviewService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _reportedReviewService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}