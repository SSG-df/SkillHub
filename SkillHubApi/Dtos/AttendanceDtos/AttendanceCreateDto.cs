using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class AttendanceCreateDto
    {
        public Guid LessonEnrollmentId { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
