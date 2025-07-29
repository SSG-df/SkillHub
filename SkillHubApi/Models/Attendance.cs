using System;
using System.Text.Json.Serialization;

namespace SkillHubApi.Models
{
    public class Attendance
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EnrollmentId { get; set; }
        public LessonEnrollment? Enrollment { get; set; }
        public DateTime AttendedAt { get; set; } = DateTime.UtcNow;
        public bool IsPresent { get; set; } = true;
    }
}