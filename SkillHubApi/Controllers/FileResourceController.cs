using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;
using System.Security.Claims;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileResourceController : ControllerBase
    {
        private readonly IFileResourceService _fileResourceService;
        private readonly ILessonService _lessonService;

        public FileResourceController(IFileResourceService fileResourceService, ILessonService lessonService)
        {
            _fileResourceService = fileResourceService;
            _lessonService = lessonService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var files = await _fileResourceService.GetAllAsync();
            return Ok(files);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var file = await _fileResourceService.GetByIdAsync(id);
            if (file == null) return NotFound();

            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole != "Admin")
            {
                var lesson = await _lessonService.GetByIdAsync(file.LessonId);
                if (lesson?.MentorId != GetCurrentUserId())
                    return Forbid();
            }

            return Ok(file);
        }

        [Authorize(Roles = "Admin,Mentor")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FileResourceCreateDto dto)
        {
            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole == "Mentor")
            {
                var lesson = await _lessonService.GetByIdAsync(dto.LessonId);
                if (lesson?.MentorId != GetCurrentUserId())
                    return Forbid();
            }

            await _fileResourceService.AddAsync(dto);
            return Ok();
        }

        [Authorize(Roles = "Admin,Mentor")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FileResourceUpdateDto dto)
        {
            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole == "Mentor")
            {
                var file = await _fileResourceService.GetByIdAsync(dto.Id);
                if (file == null) return NotFound();

                var lesson = await _lessonService.GetByIdAsync(file.LessonId);
                if (lesson?.MentorId != GetCurrentUserId())
                    return Forbid();
            }

            await _fileResourceService.UpdateAsync(dto);
            return Ok();
        }

        [Authorize(Roles = "Admin,Mentor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole == "Mentor")
            {
                var file = await _fileResourceService.GetByIdAsync(id);
                if (file == null) return NotFound();

                var lesson = await _lessonService.GetByIdAsync(file.LessonId);
                if (lesson?.MentorId != GetCurrentUserId())
                    return Forbid();
            }

            await _fileResourceService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var file = await _fileResourceService.GetByIdAsync(id);
            if (file == null) return NotFound();

            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole != "Admin")
            {
                var lesson = await _lessonService.GetByIdAsync(file.LessonId);
                if (lesson?.MentorId != GetCurrentUserId())
                    return Forbid();
            }

            var fileDownload = await _fileResourceService.DownloadFileAsync(id);
            if (fileDownload?.Content == null || fileDownload.ContentType == null)
                return NotFound();

            return File(fileDownload.Content, fileDownload.ContentType, fileDownload.FileName);
        }

        [Authorize]
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetByLessonId(Guid lessonId)
        {
            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole != "Admin")
            {
                var lesson = await _lessonService.GetByIdAsync(lessonId);
                if (lesson?.MentorId != GetCurrentUserId())
                    return Forbid();
            }

            var files = await _fileResourceService.GetByLessonIdAsync(lessonId);
            return Ok(files);
        }

        private Guid GetCurrentUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }
    }
}