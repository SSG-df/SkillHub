using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Interfaces;

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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var users = await _userService.GetAllAsync(pageNumber, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            var success = await _userService.CreateAsync(dto);
            if (!success) return BadRequest("Failed to create user.");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var success = await _userService.UpdateAsync(dto);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var success = await _userService.ActivateAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var success = await _userService.DeactivateAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPost("{id}/check-password")]
        public async Task<IActionResult> CheckPassword(Guid id, [FromBody] string password)
        {
            var isCorrect = await _userService.CheckPasswordAsync(id, password);
            return Ok(isCorrect);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }
    }
}
