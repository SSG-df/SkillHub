using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public class LessonEnrollment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime EnrolledAt { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
