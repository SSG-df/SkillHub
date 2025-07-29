using System;
using System.Text.Json.Serialization;


namespace SkillHubApi.Dtos
{
    public class AttendanceDto
    {
        public Guid Id { get; set; }
        public Guid LessonEnrollmentId { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public Guid? LessonId { get; set; }
        public Guid? UserId { get; set; }
    }
}
