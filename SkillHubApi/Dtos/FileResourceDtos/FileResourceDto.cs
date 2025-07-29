namespace SkillHubApi.Dtos
{
    public class FileResourceDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public Guid LessonId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? AuthorName { get; set; }
    }

    public class FileResourceDetailsDto : FileResourceDto
    {
        public string StoragePath { get; set; } = string.Empty;
    }
}