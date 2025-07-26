using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class MingYSubParser : BaseParser
{
    public override string        GroupName => "MingYSub";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public MingYSubParser()
    {
        SubtitleTypeMap["JpSc"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["JpTc"] = EnumSubtitleType.Embedded;
        LanguageMap["JpCn"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["JpCn"] = EnumSubtitleType.Muxed;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>MingY&[^\[\]]+)\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>MingY&[^\[\]]+)\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-z]+Rip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value;
        }

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
            Resolution   = StringUtils.ResolutionStr2Enum(resolution),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
