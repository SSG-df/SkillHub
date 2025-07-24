using System;

namespace SkillHubApi.Dtos
{
    public class LessonTagCreateDto
    {
        public Guid LessonId { get; set; }
        public string? TagName { get; set; }
    }
}
