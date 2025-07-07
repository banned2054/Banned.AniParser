using Banned.AniParser.Models;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class NekoMoeParser : BaseParser
{
    public override string GroupName => "喵萌奶茶屋";

    public NekoMoeParser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"【(?:喵萌奶茶屋|喵萌Production)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【(?<group>喵萌奶茶屋&[^\[\]]+)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Nekomoe kissaten\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>[^\[\]]+&Nekomoe kissaten)\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\](?:\[(?:v(?<version>\d+))?\])?",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"【(?:喵萌奶茶屋|喵萌Production)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
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
}
