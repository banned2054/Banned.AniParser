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
        SingleEpisodePatterns =
        [
            new(
                @"\[(北宇治字幕组|KitaujiSub)](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<source>[a-z]+Rip)]\[(?<codeV>HEVC)_(?<codeA>AAC(?:x2)?)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];

        MultipleEpisodePatterns =
        [
            new(
                @"\[(北宇治字幕组|KitaujiSub)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)[^\[\]]*]\[(?<source>[a-z]+Rip)]\[(?<codeV>HEVC)_(?<codeA>AAC)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(match.Groups["episode"].Value);

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value;
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
            GroupType    = this.GroupType,
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
            startEpisode = int.Parse(match.Groups["start"].Value);
        }

        if (match.Groups["end"].Success)
        {
            endEpisode = int.Parse(match.Groups["end"].Value);
        }

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value;
        }

        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = GroupName,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(resolution),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
