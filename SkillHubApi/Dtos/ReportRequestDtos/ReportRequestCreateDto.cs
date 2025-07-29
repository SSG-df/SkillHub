public class ReportRequestCreateDto
{
    public required string? Reason { get; set; }

    private string? _lessonId;
    public string? LessonId
    {
        get => _lessonId;
        set => _lessonId = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private string? _userId;
    public string? UserId
    {
        get => _userId;
        set => _userId = string.IsNullOrWhiteSpace(value) ? null : value;
    }
}