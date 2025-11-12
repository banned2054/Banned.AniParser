using System.ComponentModel;

namespace Banned.AniParser.Models.Enums;

public enum EnumResolution
{
    [Description("Standard Definition (480p)")]
    R480P,

    [Description("HD (720p)")]
    R720P,

    [Description("Full HD (1080p)")]
    R1080P,

    [Description("2K (2048×1080+)")]
    R2K,

    [Description("4K (2160p or above)")]
    R4K,

    [Description("Unknown / Custom")]
    Unknown
}
