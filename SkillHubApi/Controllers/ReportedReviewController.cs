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
        public async Task<IActionResult> GetAll()
        {
            var result = await _reportedReviewService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reportedReviewService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReportedReviewCreateDto dto)
        {
            var result = await _reportedReviewService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReportedReviewUpdateDto dto)
        {
            var success = await _reportedReviewService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _reportedReviewService.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
