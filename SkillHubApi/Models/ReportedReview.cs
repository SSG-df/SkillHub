using System;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace SkillHubApi.Models
{
    public class ReportedReview
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ReviewId { get; set; }
        
        [Required]
        public Guid ReporterId { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string? Reason { get; set; }
        
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
        public Review? Review { get; set; }
        public User? Reporter { get; set; }
        public string? ModeratorComment { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? ProcessedAt { get; set; }
    }

}