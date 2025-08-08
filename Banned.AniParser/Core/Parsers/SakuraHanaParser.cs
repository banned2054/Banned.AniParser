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
            new(
                @"\[樱桃花字幕组](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\[(?<resolution>\d+p)]\[[^\[\]]+]\[(?<lang>.+?)]\[(?<source>[a-z]+Rip)]",
                RegexOptions.IgnoreCase),
            new(
                @"\[樱桃花字幕组](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+p)[\)）]?\s*\[(?<lang>.+?)]\s?\[[a-z0-9]+]",
                RegexOptions.IgnoreCase),
            new(
                @"\[樱桃花字幕组](?<title>[^\[\]]+?)(])?\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)(]\[)?\s?[^\[\]]+]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"\[樱桃花字幕组](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+p)[\)）]?\s?\[[a-z0-9]+]",
                RegexOptions.IgnoreCase),
            new(
                @"\[樱桃花字幕组](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(
                @"\[樱桃花字幕组](?<title>[^\[\]]+?)(])?\[(?<start>\d+)-(?<end>\d+)]\[(?<resolution>\d+p)(]\[)?\s?[^\[\]]+]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
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
