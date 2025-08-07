using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class AnkRawParser : BaseParser
{
    public override string        GroupName => "ANK-Raws";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    public AnkRawParser()
    {
        SingleEpisodePatterns =
        [
            new(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\s(?<episode>\d+(\.\d+)?)\s\((?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<vcodec>HEVC-YUV420P10)\s(?<acodec>FLAC)\sDTS-HDMA\)\.mkv",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\((?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<vcodec>HEVC-YUV420P10)\s(?<acodec>FLAC)\sDTS-HDMA\)",
                RegexOptions.IgnoreCase),
        ];
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0m;
        if (match.Groups["episode"].Success)
        {
            episode = decimal.Parse(match.Groups["episode"].Value);
        }

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var title = match.Groups["title"].Value.Trim();

        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value.Trim();
            group = string.IsNullOrEmpty(group) ? group : GroupName;
        }

        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = title,
            Episode      = episode,
            Group        = group,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value;
            group = string.IsNullOrEmpty(group) ? group : GroupName;
        }

        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Group        = group,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = EnumLanguage.None,
            SubtitleType = EnumSubtitleType.None
        };
    }
}
