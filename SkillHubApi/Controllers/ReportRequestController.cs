using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _reportRequestService.GetAllAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var report = await _reportRequestService.GetByIdAsync(id);
            if (report == null) return NotFound();

            if (!HasAccessToReport(report))
                return Forbid();

            return Ok(report);
        }

        [HttpPost]
        [Authorize(Roles = "Learner,Mentor,Admin")]
        public async Task<IActionResult> Create([FromBody] ReportRequestCreateDto dto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == Guid.Empty)
                    return Unauthorized();

                var result = await _reportRequestService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReportRequestUpdateDto dto)
        {
            if (!await HasPermissionToModify(id))
                return Forbid();

            var success = await _reportRequestService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await HasPermissionToModify(id))
                return Forbid();

            var success = await _reportRequestService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Guid.Empty;
            return userId;
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        private bool HasAccessToReport(ReportRequestDto report)
        {
            return IsAdmin() || report.RequestedById == GetCurrentUserId();
        }

        private async Task<bool> HasPermissionToModify(Guid reportId)
        {
            if (IsAdmin()) return true;
            
            var report = await _reportRequestService.GetByIdAsync(reportId);
            return report?.RequestedById == GetCurrentUserId();
        }
    }
}