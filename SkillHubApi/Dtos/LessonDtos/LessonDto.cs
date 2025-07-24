using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class LessonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Capacity { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime StartTime { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime EndTime { get; set; }
        public Guid MentorId { get; set; }
    }
}
