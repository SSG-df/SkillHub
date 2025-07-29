using System;
using System.Text.Json.Serialization;
using SkillHubApi.Models;

namespace SkillHubApi.Dtos
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
