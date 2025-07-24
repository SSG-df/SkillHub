using System;

namespace SkillHubApi.Dtos
{
    public class LessonTagUpdateDto
    {
        public Guid LessonId { get; set; }
        public Guid TagId { get; set; }
    }
}
