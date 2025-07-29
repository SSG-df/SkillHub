using System.Text.Json.Serialization;
using SkillHubApi.Models;

namespace SkillHubApi.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? Bio { get; set; }
        public bool IsActive { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
    }
}