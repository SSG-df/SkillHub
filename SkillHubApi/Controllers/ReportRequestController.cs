using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportRequestController : ControllerBase
    {
        private readonly IReportRequestService _reportRequestService;

        public ReportRequestController(IReportRequestService reportRequestService)
        {
            _reportRequestService = reportRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reportRequestService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reportRequestService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReportRequestCreateDto dto)
        {
            var result = await _reportRequestService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReportRequestUpdateDto dto)
        {
            var success = await _reportRequestService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _reportRequestService.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
