namespace SkillHubApi.Dtos
{
    public class ReportedReviewCreateDto
    {
        public Guid ReviewId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
