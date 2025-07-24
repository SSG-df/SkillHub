using System;
using System.Text.Json.Serialization;

namespace SkillHubApi.Dtos
{
    public class AttendanceUpdateDto
    {
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
