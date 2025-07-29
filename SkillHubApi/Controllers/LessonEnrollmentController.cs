using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;
using System.Security.Claims;
using SkillHubApi.Interfaces;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonEnrollmentController : ControllerBase
    {
        private readonly ILessonEnrollmentService _lessonEnrollmentService;
        private readonly ILessonService _lessonService;

        public LessonEnrollmentController(
            ILessonEnrollmentService lessonEnrollmentService,
            ILessonService lessonService)
        {
            _lessonEnrollmentService = lessonEnrollmentService;
            _lessonService = lessonService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            var enrollments = currentUserRole == "Admin"
                ? await _lessonEnrollmentService.GetAllAsync()
                : await _lessonEnrollmentService.GetByUserIdAsync(currentUserId);

            return Ok(enrollments);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var enrollment = await _lessonEnrollmentService.GetByIdAsync(id);
            if (enrollment == null) return NotFound();

            if (!await CanAccessEnrollment(enrollment))
                return Forbid();

            return Ok(enrollment);
        }

        [Authorize]
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetByLessonId(Guid lessonId)
        {
            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole == "Learner")
            {
                var isEnrolled = await _lessonEnrollmentService.IsUserEnrolledAsync(GetCurrentUserId(), lessonId);
                if (!isEnrolled) return Forbid();
            }

            var enrollments = await _lessonEnrollmentService.GetByLessonIdAsync(lessonId);
            return Ok(enrollments);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole != "Admin" && currentUserId != userId)
                return Forbid();

            var enrollments = await _lessonEnrollmentService.GetByUserIdAsync(userId);
            return Ok(enrollments);
        }

        [Authorize(Roles = "Learner,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LessonEnrollmentCreateDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == "Learner" && dto.UserId != currentUserId)
                return Forbid();

            try
            {
                var result = await _lessonEnrollmentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LessonEnrollmentUpdateDto dto)
        {
            var enrollment = await _lessonEnrollmentService.GetByIdAsync(id);
            if (enrollment == null) return NotFound();

            if (!await CanAccessEnrollment(enrollment))
                return Forbid();

            var success = await _lessonEnrollmentService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var enrollment = await _lessonEnrollmentService.GetByIdAsync(id);
            if (enrollment == null) return NotFound();

            if (!await CanAccessEnrollment(enrollment))
                return Forbid();

            var success = await _lessonEnrollmentService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpGet("check-enrollment")]
        public async Task<IActionResult> CheckEnrollment(
            [FromQuery] Guid userId,
            [FromQuery] Guid lessonId)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole != "Admin" && currentUserId != userId)
                return Forbid();

            var isEnrolled = await _lessonEnrollmentService.IsUserEnrolledAsync(userId, lessonId);
            return Ok(new { IsEnrolled = isEnrolled });
        }

        [Authorize]
        [HttpGet("lesson/{lessonId}/count")]
        public async Task<IActionResult> GetEnrollmentCount(Guid lessonId)
        {
            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole == "Learner")
            {
                var isEnrolled = await _lessonEnrollmentService.IsUserEnrolledAsync(GetCurrentUserId(), lessonId);
                if (!isEnrolled) return Forbid();
            }

            var count = await _lessonEnrollmentService.GetEnrollmentCountAsync(lessonId);
            return Ok(new { EnrollmentCount = count });
        }

        private async Task<bool> CanAccessEnrollment(LessonEnrollmentDto enrollment)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == "Admin") return true;
            if (enrollment.UserId == currentUserId) return true;
            if (enrollment.LessonId.HasValue && await IsLessonMentor(enrollment.LessonId.Value, currentUserId)) return true;
            return false;
        }

        private async Task<bool> IsLessonMentor(Guid lessonId, Guid userId)
        {
            var lesson = await _lessonService.GetByIdAsync(lessonId);
            return lesson?.MentorId == userId;
        }

        private Guid GetCurrentUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}