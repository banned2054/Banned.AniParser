using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class CherryBlossomParser : BaseParser
{
    public override string GroupName => "樱桃花字幕组";

    public CherryBlossomParser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[樱桃花字幕组\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\[(?<resolution>\d+[pP])\]\[[^\[\]]+\]\[(?<lang>.+?)\]\[(?<source>[a-zA-Z]+[Rr]ip)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[樱桃花字幕组\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+[pP])[\)）]?\s*\[(?<lang>.+?)\]\s?\[[a-zA-Z0-9]+\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[樱桃花字幕组\](?<title>[^\[\]]+?)(\])?\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])(\]\[)?\s?[^\[\]]+\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[樱桃花字幕组\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+[pP])[\)）]?\s?\[[a-zA-Z0-9]+\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[樱桃花字幕组\](?<title>[^\[\]]+?)(\])?\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?\]\[(?<resolution>\d+[pP])(\]\[)?\s?[^\[\]]+\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
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