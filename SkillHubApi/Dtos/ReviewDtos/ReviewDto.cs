using System;
using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
