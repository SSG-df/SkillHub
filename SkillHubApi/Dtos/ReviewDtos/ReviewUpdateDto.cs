namespace SkillHubApi.Dtos
{
    public class ReviewUpdateDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
