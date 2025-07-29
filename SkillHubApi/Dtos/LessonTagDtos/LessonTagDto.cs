using System;
using System.Text.Json.Serialization;   

namespace SkillHubApi.Dtos
{
    public class LessonTagDto
    {
        public Guid LessonId { get; set; }
        public Guid TagId { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public TagDto? Tag { get; set; }
    }
}
