namespace SkillHubApi.Dtos
{
    public class ReviewCreateDto
    {
        public Guid LessonId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
