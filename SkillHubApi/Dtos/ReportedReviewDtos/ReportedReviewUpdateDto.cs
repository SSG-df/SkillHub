namespace SkillHubApi.Dtos
{
    public class ReportedReviewUpdateDto
    {
        public Guid Id { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
