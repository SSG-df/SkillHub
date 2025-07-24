using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public enum DifficultyLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }

    public class Lesson
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DifficultyLevel Difficulty { get; set; }
        public int Capacity { get; set; } = 10;
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime StartTime { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime EndTime { get; set; }
        [Required]
        public Guid MentorId { get; set; }
        public User? Mentor { get; set; }
        public ICollection<LessonEnrollment>? Enrollments { get; set; }
        public ICollection<FileResource>? Resources { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<LessonTag>? LessonTags { get; set; }
    }
}
