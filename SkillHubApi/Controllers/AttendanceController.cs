using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using SkillHubApi.Interfaces;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ILessonEnrollmentService _enrollmentService;
        private readonly ILessonService _lessonService;

        public AttendanceController(
            IAttendanceService attendanceService,
            ILessonEnrollmentService enrollmentService,
            ILessonService lessonService)
        {
            _attendanceService = attendanceService;
            _enrollmentService = enrollmentService;
            _lessonService = lessonService;
        }

        [Authorize(Roles = "Admin,Mentor")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null) return NotFound();
            
            if (!await HasAccessToAttendance(attendance))
                return Forbid();
                
            return Ok(attendance);
        }

       [Authorize(Roles = "Admin,Mentor")]
       [HttpPost]
        public async Task<IActionResult> Create([FromBody] AttendanceCreateDto dto)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
            {
                var result = await _attendanceService.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AttendanceUpdateDto dto)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null) return NotFound();

            if (!await HasAccessToAttendance(attendance))
                return Forbid();

            var result = await _attendanceService.UpdateAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null) return NotFound();

            if (!await HasAccessToAttendance(attendance))
                return Forbid();

            var result = await _attendanceService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        private async Task<bool> HasAccessToAttendance(AttendanceDto attendance)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == "Admin") return true;
            if (currentUserRole == "Mentor" && attendance.LessonId.HasValue) 
                return await IsLessonMentor(attendance.LessonId.Value, currentUserId);
            if (currentUserRole == "Learner") 
                return attendance.UserId == currentUserId;
            
            return false;
        }

        private async Task<bool> HasAccessToCreateAttendance(Guid enrollmentId)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == "Admin") return true;
            
            var enrollment = await _enrollmentService.GetByIdAsync(enrollmentId);
            if (enrollment == null || !enrollment.LessonId.HasValue) return false;
            
            return currentUserRole == "Mentor" && await IsLessonMentor(enrollment.LessonId.Value, currentUserId);
        }

        private async Task<bool> IsLessonMentor(Guid lessonId, Guid userId)
        {
            var lesson = await _lessonService.GetByIdAsync(lessonId);
            return lesson?.MentorId == userId;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        }
    }
}