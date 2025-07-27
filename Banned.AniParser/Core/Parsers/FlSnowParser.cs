using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class FlSnowParser : BaseParser
{
    public override string        GroupName => "雪飘工作室";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public FlSnowParser()
    {
        LanguageMap["繁"] = EnumLanguage.Tc;

        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[雪飘工作室\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[雪飘工作室\]\[(?<title>[^\[\]]+?)\]\[(?<resolution>\d+p)\]\[S(?<season>\d+)E(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[雪飘工作室\]\[(?<title>[^\[\]]+?)\]\[(?<resolution>\d+p)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[FLsnow\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+p)\]\.(?<lang>chs|cht)",
                RegexOptions.IgnoreCase),
            new(
                @"\[FLsnow\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+p)\]",
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
            resolution = match.Groups["resolution"].Value.Trim();
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
            Group        = GroupName,
            Resolution   = StringUtils.ResolutionStr2Enum(resolution),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        var startEpisode = 0;
        var endEpisode   = 0;
        if (match.Groups["start"].Success)
        {
            startEpisode = int.Parse(Regex.Replace(match.Groups["start"].Value.Trim(), @"\D+", ""));
        }

        if (match.Groups["end"].Success)
        {
            endEpisode = int.Parse(Regex.Replace(match.Groups["end"].Value.Trim(), @"\D+", ""));
        }

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value.Trim().ToLower();
        }

        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = GroupName,
            Resolution   = StringUtils.ResolutionStr2Enum(resolution),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
