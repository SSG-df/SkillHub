namespace SkillHubApi.Dtos
{
    public class FileResourceDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
    }
}
