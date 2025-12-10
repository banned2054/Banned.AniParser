using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class SakuraHanaParser : BaseParser
{
    public override string        GroupName => "樱桃花字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public SakuraHanaParser()
    {
        SingleEpisodePatterns =
        [
            new(@"\[(?<group>(?:[^\[\]]+&)?樱桃花字幕组(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)-\s?(?<episode>\d+)\s?(?:v(?<version>\d+))?\s*[\[\(（]?(?<resolution>\d+p)[\]\)）]?\s*(?:\[[^\[\]]+\]\s*)?\[(?<lang>.+?)](?:\s*\[(?<source>[^\[\]]+)])?",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?樱桃花字幕组(?:&[^\[\]]+)?)](\[)?(?<title>[^\[\]]+?)(])?\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)(]\[)?\s?(?<source>[a-z]+Rip)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(@"\[(?<group>(?:[^\[\]]+&)?樱桃花字幕组(?:&[^\[\]]+)?)]](?<title>[^\[\]]+?)(])?\[(?<start>\d+)-(?<end>\d+)]\[(?<resolution>\d+p)(]\[)?\s?[^\[\]]+]\[(?<lang>.+?)]",
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
