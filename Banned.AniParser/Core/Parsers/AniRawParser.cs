using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class AniRawParser : BaseTransferParser
{
    public override string GroupName => "ANi";

    public AniRawParser()
    {
        LanguageMap["CHT CHS"]     = EnumLanguage.ScTc;
        SubtitleTypeMap["CHT CHS"] = EnumSubtitleType.Muxed;
        SubtitleTypeMap["CHT"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHS"]     = EnumSubtitleType.Embedded;
        SingleEpisodePatterns =
        [
            new(
                @"\[ANi](?<title>.+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<resolution>\d+p)]\[(?<websource>Baha)]\[(?<source>WEB-DL)]\[(?<codeA>AAC)\s(?<codeV>AVC)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }

    protected override (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var lowerLang    = lang.ToLower().Trim();
        var language     = EnumLanguage.None;
        var subtitleType = EnumSubtitleType.Embedded;
        foreach (var (k, v) in LanguageMap.OrderByDescending(kvp => kvp.Key.Length))
        {
            if (!lowerLang.Contains(k.ToLower())) continue;
            language = v;
            break;
        }

        return (language, subtitleType);
    }
}
