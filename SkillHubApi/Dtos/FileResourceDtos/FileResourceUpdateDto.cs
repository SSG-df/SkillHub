namespace SkillHubApi.Dtos
{
    public class FileResourceUpdateDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
    }
}
