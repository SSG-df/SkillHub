using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class LessonUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Difficulty { get; set; } = string.Empty;
        public int Capacity { get; set; } = 10;
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime StartTime { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime EndTime { get; set; }
        [Required]
        public Guid MentorId { get; set; }
    }
}
