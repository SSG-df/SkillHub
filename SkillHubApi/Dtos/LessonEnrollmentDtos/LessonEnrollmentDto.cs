using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class LessonEnrollmentDto
    {
        public Guid? Id { get; set; }
        public Guid? LessonId { get; set; }
        public Guid? UserId { get; set; }
        
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime EnrolledAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}