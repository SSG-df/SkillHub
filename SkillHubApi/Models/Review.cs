using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [MaxLength(1000)]
        public string? Comment { get; set; }
        public bool IsVisible { get; set; } = true;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DateTime CreatedAt { get; set; } 
    }
}
