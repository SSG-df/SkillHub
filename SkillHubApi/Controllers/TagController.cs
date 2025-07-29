using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;
using System.Security.Claims;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tagService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _tagService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Create([FromBody] TagCreateDto dto)
        {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var currentUserId))
            return Unauthorized();

            var tag = await _tagService.CreateAsync(dto, currentUserId);
            return CreatedAtAction(nameof(GetById), new { id = tag.Id }, tag);
        }
       

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] TagUpdateDto dto)
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (currentUserRole != "Admin" && currentUserRole != "Mentor")
            {
                return Forbid();
            }

            if (currentUserRole != "Admin")
            {
                var tag = await _tagService.GetByIdAsync(id);
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var currentUserId))
                {
                    return Forbid();
                }

                if (tag?.CreatedBy != currentUserId)
                {
                    return Forbid();
                }
            }

            var success = await _tagService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
    
            if (currentUserRole != "Admin")
            {
                var tag = await _tagService.GetByIdAsync(id);
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var currentUserId))
                {
                    return Forbid();
                }

                if (tag?.CreatedBy != currentUserId)
                {
                    return Forbid();
                }
            }

            var success = await _tagService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}