using Banned.AniParser.Models.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.AniParser.Utils;

public sealed class EnumSourceJsonConverter : JsonConverter<EnumSource>
{
    public override EnumSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => StringUtils.SourceStr2Enum(reader.GetString() ?? string.Empty);

    public override void Write(Utf8JsonWriter writer, EnumSource value, JsonSerializerOptions options)
        => writer.WriteStringValue(StringUtils.SourceEnum2Str(value));
}
