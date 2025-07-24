using System;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public class ReportedReview
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ReviewId { get; set; }
        public Guid ReporterId { get; set; }
        public Review? Review { get; set; }
        public string? Reason { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime ReportedAt { get; set; }
    }
}
