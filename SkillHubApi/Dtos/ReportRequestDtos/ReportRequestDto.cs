using System;
using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class ReportRequestDto
    {
        public Guid Id { get; set; }
        public Guid RequestedById { get; set; }
        public string Reason { get; set; } = string.Empty;
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime RequestedAt { get; set; }
        public Guid? UserId { get; set; }
    }
}
