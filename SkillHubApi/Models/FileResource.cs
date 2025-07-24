using SkillHubApi.Dtos;

namespace SkillHubApi.Models
{
    public class FileResource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public Guid LessonId { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
