using System.ComponentModel.DataAnnotations;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using SkillHubApi.Services;

namespace SkillHubApi.Dtos
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

   public class AuthResponseDto
{
    public string? Token { get; set; }
    public UserDto? User { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public object? Data { get; set; }
}
}
