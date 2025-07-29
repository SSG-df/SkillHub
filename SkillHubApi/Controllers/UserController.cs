using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var users = await _userService.GetAllAsync(pageNumber, pageSize);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid();
            }

            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<UserDto>> GetByUsername(string username)
        {
            var currentUserRole = GetCurrentUserRole();
            var currentUserId = GetCurrentUserId();

            if (currentUserRole == "Learner")
            {
                var currentUser = await _userService.GetByIdAsync(currentUserId);
                if (currentUser?.Username != username)
                {
                    return Forbid();
                }
            }
            else if (currentUserRole == "Mentor")
            {
                var requestedUser = await _userService.GetByUsernameAsync(username);
                if (requestedUser?.Role.ToString() == "Admin")
                {
                    return Forbid();
                }
            }

            var user = await _userService.GetByUsernameAsync(username);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<ActionResult<UserDto>> CreateAdminUser([FromBody] UserCreateDto dto)
        {
            var result = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole != "Admin" && currentUserId != dto.Id)
            {
                return Forbid();
            }

            var success = await _userService.UpdateAsync(dto);
            return success ? Ok() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _userService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var success = await _userService.ActivateAsync(id);
            return success ? Ok() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var success = await _userService.DeactivateAsync(id);
            return success ? Ok() : NotFound();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var success = await _userService.ChangePasswordAsync(currentUserId, dto);
            return success ? Ok() : BadRequest("Password change failed");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);
            return result.Success ? Ok(result) : Unauthorized(result.ErrorMessage);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var currentUserId = GetCurrentUserId();
            var user = await _userService.GetByIdAsync(currentUserId);
            return user == null ? NotFound() : Ok(user);
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