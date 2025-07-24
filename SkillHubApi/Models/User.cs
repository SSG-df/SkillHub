using System.ComponentModel.DataAnnotations;
using SkillHubApi.Models;
using SkillHubApi.Dtos;


namespace SkillHubApi.Models
{
    public enum UserRole
    {
        Learner = 0,
        Mentor = 1,
        Admin = 2
    }
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; }
        [MaxLength(1000)]
        public string? Bio { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Lesson>? CreatedLessons { get; set; }
        public ICollection<LessonEnrollment>? Enrollments { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
