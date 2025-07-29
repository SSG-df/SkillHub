using System;
using System.Text.Json.Serialization;
using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public class ReportRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RequestedById { get; set; }  
        public virtual User? RequestedBy { get; set; }
        public string Reason { get; set; } = string.Empty; 
        public Guid? LessonId { get; set; }          
        public Guid? UserId { get; set; }             
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime RequestedAt { get; set; }
    }
}