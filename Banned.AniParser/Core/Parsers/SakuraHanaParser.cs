using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class SakuraHanaParser : BaseParser
{
    public override string        GroupName => "樱桃花字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?樱桃花字幕组(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)-\s?(?<episode>\d+)\s?(?:v(?<version>\d+))?\s*[\[\(（]?(?<resolution>\d+p)[\]\)）]?\s*(?:\[[^\[\]]+\]\s*)?\[(?<lang>.+?)](?:\s*\[(?<source>[^\[\]]+)])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?樱桃花字幕组(?:&[^\[\]]+)?)](\[)?(?<title>[^\[\]]+?)(])?\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)(]\[)?\s?(?<source>[a-z]+Rip)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?樱桃花字幕组(?:&[^\[\]]+)?)]](?<title>[^\[\]]+?)(])?\[(?<start>\d+)-(?<end>\d+)]\[(?<resolution>\d+p)(]\[)?\s?[^\[\]]+]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public SakuraHanaParser()
    {
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2()];
        MultipleEpisodePatterns = [MultiplePattern()];
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
