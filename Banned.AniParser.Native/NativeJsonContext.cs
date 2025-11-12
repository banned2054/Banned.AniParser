using Banned.AniParser.Models;
using System.Text.Json.Serialization;

namespace Banned.AniParser.Native;

[JsonSourceGenerationOptions(WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ParseResult))]
[JsonSerializable(typeof(ParseResult[]))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(ErrorDto))]
internal partial class NativeJsonContext : JsonSerializerContext
{
}

internal sealed class ErrorDto
{
    public string? error { get; set; }
}
