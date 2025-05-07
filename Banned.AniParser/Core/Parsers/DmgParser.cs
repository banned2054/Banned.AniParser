using System.Text.RegularExpressions;
using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Core.Parsers;

public class DmgParser : BaseParser
{
    public override string GroupName { get; } = "动漫国字幕组";

    public DmgParser()
    {
        SubtitleTypeMap["简体"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["繁体"] = EnumSubtitleType.Embedded;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"【动漫国字幕组】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?(?:\s?END)?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【(?<group>动漫国字幕组&[^\[\]]+)】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?(?:\s?END)?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"【动漫国字幕组】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:\s?END)?(?:\(全集\))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
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

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = group,
            Resolution   = match.Groups["resolution"].Value,
            Language     = lang,
            SubtitleType = subType
        };
    }
}