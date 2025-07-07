using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class KitaujiSubParser : BaseParser
{
    public override string        GroupName => "北宇治字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public KitaujiSubParser()
    {
        LanguageMap["CHT_JP"] = EnumLanguage.JpTc;
        LanguageMap["CHS_JP"] = EnumLanguage.JpSc;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[(北宇治字幕组|KitaujiSub)\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<source>[a-zA-Z]+[Rr]ip)\]\[[^\[\]]+\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };

        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[(北宇治字幕组|KitaujiSub)\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?[^\[\]]*\]\[(?<source>[a-zA-Z]+[Rr]ip)\]\[[^\[\]]+\]\[(?<lang>.+?)\]",
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

        return new ParseResult
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
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
            resolution = match.Groups["resolution"].Value;
        }

        return new ParseResult
        {
            IsMultiple   = true,
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
