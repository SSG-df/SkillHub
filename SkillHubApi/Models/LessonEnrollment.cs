using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SkillHubApi.Models
{
    public class LessonEnrollment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
        [Required]
        [ForeignKey("Lesson")]
        public Guid LessonId { get; set; }
        [JsonIgnore]
        public virtual Lesson? Lesson { get; set; }

        [Required]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
    }
}