using SkillHubApi.Models;
using System.ComponentModel.DataAnnotations;
using SkillHubApi.Dtos;
using System.Text.Json.Serialization;
using SkillHubApi;

namespace SkillHubApi.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public ICollection<LessonTag>? LessonTags { get; set; }
        public Guid CreatedBy { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
    }
}