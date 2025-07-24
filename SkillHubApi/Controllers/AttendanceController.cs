using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null) return NotFound();
            return Ok(attendance);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AttendanceCreateDto dto)
        {
            var created = await _attendanceService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AttendanceUpdateDto dto)
        {
            var result = await _attendanceService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _attendanceService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
