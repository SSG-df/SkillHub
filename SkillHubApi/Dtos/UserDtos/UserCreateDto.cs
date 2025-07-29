using System.ComponentModel.DataAnnotations;
using SkillHubApi.Models;

namespace SkillHubApi.Dtos
{
    public class UserCreateDto
    {
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(4)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; }
        [MaxLength(1000)]
        public string? Bio { get; set; }
    }
}