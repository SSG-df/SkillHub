using System;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public enum ReportType
    {
        UserSummary,
        LessonSummary,
        MentorLessons
    }
    public class ReportRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RequestedById { get; set; }
        public User? RequestedBy { get; set; }
        public string Reason { get; set; } = string.Empty;
        public ReportType Type { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime RequestedAt { get; set; }
        public string Format { get; set; } = "CSV";
    }
}
