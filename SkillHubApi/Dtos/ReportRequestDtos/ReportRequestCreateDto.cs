namespace SkillHubApi.Dtos
{
    public class ReportRequestCreateDto
    {
        public Guid RequestedById { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
