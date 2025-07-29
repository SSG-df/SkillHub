using System;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public class LessonTag
    {
        public Guid LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public Guid TagId { get; set; }
        public Tag? Tag { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
