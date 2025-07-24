using SkillHubApi.Models;

namespace SkillHubApi.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; }
        public string? Bio { get; set; }
        public bool IsActive { get; set; }
    }
}
