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

        // 【澄空学园&华盟字幕社&动漫国字幕组】★04月新番[Summer Pockets][05][1080P][简体][MP4]
        // 【動漫國字幕組】★04月新番[賽馬娘盧毛灰姑娘][05][1080P][繁體][MP4]
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"【[動动]漫[國国]字幕[組组]】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?(?:\s?END)?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【(?<group>(?:[動动]漫[國国]字幕[組组]&[^\[\]]+|[^\[\]]+&[動动]漫[國国]字幕[組组]))】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?(?:\s?END)?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[DMG\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?(?:\s?END)?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(?:&[^\[\]]+&)?DMG&[^\[\]]+)\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?(?:\s?END)?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"【[動动]漫[國国]字幕[組组]】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:\s?END)?(?:\(全集\))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value.Trim();
        }

        return new ParseResult
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = group,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var lowerLang = lang.ToLower().Trim();
        var language  = EnumLanguage.None;
        foreach (var (k, v) in LanguageMap.OrderByDescending(kvp => kvp.Key.Length))
        {
            if (!lowerLang.Contains(k.ToLower())) continue;
            language = v;
            break;
        }

        return (language, EnumSubtitleType.Embedded);
    }
}
