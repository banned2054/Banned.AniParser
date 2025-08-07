using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class NekoMoeParser : BaseParser
{
    public override string        GroupName => "喵萌奶茶屋";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public NekoMoeParser()
    {
        SingleEpisodePatterns =
        [
            new(
                @"【(?:喵萌奶茶屋|喵萌Production)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【(?<group>喵萌奶茶屋&[^\[\]]+)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Nekomoe kissaten\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>[^\[\]]+&Nekomoe kissaten)\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\](?:\[(?:v(?<version>\d+))?\])?",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(
                @"【(?:喵萌奶茶屋|喵萌Production)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        ];
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(match.Groups["episode"].Value);

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value.Trim();
            group = string.IsNullOrEmpty(group) ? GroupName : group;
        }

        var version = match.Groups["version"].Success
            ? int.TryParse(match.Groups["version"].Value, out _) ? int.Parse(match.Groups["version"].Value) : 1
            : 1;

        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Version      = version,
            Group        = group,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
