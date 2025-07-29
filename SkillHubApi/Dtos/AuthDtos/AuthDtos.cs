using System.ComponentModel.DataAnnotations;
using SkillHubApi.Models;

namespace SkillHubApi.Dtos
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required, MinLength(4)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; } = UserRole.Learner; 
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }

   public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public UserDto User { get; set; } = null!;
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required, MinLength(4)]
        public string NewPassword { get; set; } = string.Empty;
    }
}