using System.Text.Json.Serialization;
using SkillHubApi.Models;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public class Attendance
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EnrollmentId { get; set; }
        public LessonEnrollment? Enrollment { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DateTime AttendedAt { get; set; } 
        public bool IsPresent { get; set; } = true;
    }
}
