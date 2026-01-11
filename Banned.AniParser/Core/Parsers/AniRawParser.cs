using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class AniRawParser : BaseTransferParser
{
    public override string GroupName => "ANi";

    [GeneratedRegex(@"\[ANi](?<title>.+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<resolution>\d+p)]\[(?<websource>Baha)]\[(?<source>WEB-DL)]\[(?<aCodec>AAC)\s(?<vCodec>AVC)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex Pattern();

    public AniRawParser()
    {
        LanguageMap["CHT CHS"]     = EnumLanguage.ScTc;
        SubtitleTypeMap["CHT CHS"] = EnumSubtitleType.Muxed;
        SubtitleTypeMap["CHT"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHS"]     = EnumSubtitleType.Embedded;
        SingleEpisodePatterns      = [Pattern(),];
        InitMap();
    }

    protected override (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var s            = lang.AsSpan().Trim().ToString().ToLowerInvariant();
        var language     = EnumLanguage.None;
        var subtitleType = EnumSubtitleType.Embedded;
        foreach (var (k, v) in LanguageMapSorted)
        {
            if (!s.Contains(k, StringComparison.Ordinal)) continue;
            language = v;
            break;
        }

        return (language, subtitleType);
    }
}
