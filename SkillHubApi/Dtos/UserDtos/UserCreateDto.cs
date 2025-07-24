using SkillHubApi.Models;

namespace SkillHubApi.Dtos
{
    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? Bio { get; set; }
    }
}
