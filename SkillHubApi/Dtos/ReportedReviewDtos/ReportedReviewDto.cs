using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class ReportedReviewDto
    {
        public Guid Id { get; set; }
        public Guid ReviewId { get; set; }
        public Guid ReporterId { get; set; }
        public string Reason { get; set; } = string.Empty;
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime ReportedAt { get; set; }
    }
}
