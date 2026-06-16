using Banned.AniParser.Utils;
using System.Text.Json.Serialization;

namespace Banned.AniParser.Models.Enums;

[JsonConverter(typeof(EnumSourceJsonConverter))]
public enum EnumSource
{
    WEB_DL,
    WEBRip,
    BDRip,
    TVRip,
    DVDRip,
    Unknown
}
