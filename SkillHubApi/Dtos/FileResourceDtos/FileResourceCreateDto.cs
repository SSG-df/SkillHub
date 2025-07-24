namespace SkillHubApi.Dtos
{
    public class FileResourceCreateDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public Guid LessonId { get; set; }
    }
}
