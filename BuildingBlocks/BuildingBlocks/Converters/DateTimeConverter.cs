using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BuildingBlocks.Converters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse the string into DateTime (assumes input is in "yyyy-MM-dd" format)
        _ = DateTime.TryParseExact(reader.GetString(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value);
        return value;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Write the DateTime as a string in "yyyy-MM-dd" format
        writer.WriteStringValue(value.ToString(DateFormat));
    }
}
