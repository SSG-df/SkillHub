using SkillHubApi.Models;

namespace SkillHubApi.Dtos
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? Bio { get; set; }
    }
}
