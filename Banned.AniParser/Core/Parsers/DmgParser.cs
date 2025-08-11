using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class DmgParser : BaseParser
{
    public override string        GroupName => "动漫国字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public DmgParser()
    {
        LanguageMap["CHT_JPN"] = EnumLanguage.JpTc;
        LanguageMap["CHS_JPN"] = EnumLanguage.JpTc;

        SingleEpisodePatterns =
        [
            new(
                @"[【\[](?<group>(?:[^\[\]]+&)?[動动]漫[國国](?:字幕[組组])?(?:&[^\[\]]+)?)[】\]](?:★\d+月新番)?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?(?:\s?END)?]\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(
                @"[【\[](?<group>(?:[^\[\]]+&)?[動动]漫[國国](?:字幕[組组])?(?:&[^\[\]]+)?)[】\]](?:★\d+月新番)?\[(?<title>[^\[\]]+?)]\[(?<start>\d+)-(?<end>\d+)(?:\s?END)?(?:\(全集\))?](?:\[(?<source>[a-z]+Rip)])?(?:\[(?<codeV>AVC)_(?<codeA>AAC)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
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
