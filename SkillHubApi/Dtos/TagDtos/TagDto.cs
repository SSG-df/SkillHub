using System.Text.Json.Serialization;

public class TagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime CreatedAt { get; set; }
}
