using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class FlSnowParser : BaseParser
{
    public override string GroupName => "雪飘工作室";

    public FlSnowParser()
    {
        LanguageMap["繁"] = EnumLanguage.Tc;

        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[雪飘工作室\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[雪飘工作室\]\[(?<title>[^\[\]]+?)\]\[(?<resolution>\d+[pP])\]\[S(?<season>\d+)E(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[雪飘工作室\]\[(?<title>[^\[\]]+?)\]\[(?<resolution>\d+[pP])\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[FLsnow\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]\.(?<lang>chs|cht)",
                RegexOptions.IgnoreCase),
            new(
                @"\[FLsnow\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]",
                RegexOptions.IgnoreCase),
        };

        MultipleEpisodePatterns = new List<Regex>()
        {
        };
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
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

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            Resolution   = resolution,
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParserInfo CreateParsedResultMultiple(Match match)
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
            resolution = match.Groups["resolution"].Value.Trim();
        }

        return new ParserInfo
        {
            IsMultiple   = true,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = GroupName,
            Resolution   = resolution,
            Language     = lang,
            SubtitleType = subType
        };
    }
}