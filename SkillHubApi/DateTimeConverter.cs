using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd HH:mm";
    private static readonly TimeZoneInfo TashkentTimeZone = 
        TimeZoneInfo.FindSystemTimeZoneById("Asia/Tashkent");

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return TimeZoneInfo.ConvertTimeToUtc(
            DateTime.ParseExact(value!, Format, null),
            TashkentTimeZone
        );
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var tashkentTime = TimeZoneInfo.ConvertTimeFromUtc(value, TashkentTimeZone);
        writer.WriteStringValue(tashkentTime.ToString(Format));
    }
}