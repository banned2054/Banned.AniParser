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
            new(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\s(?<episode>\d+(\.\d+)?)\s\((?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<vcodec>HEVC-YUV420P10)\s(?<acodec>FLAC)\sDTS-HDMA\)\.mkv",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\((?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<vcodec>HEVC-YUV420P10)\s(?<acodec>FLAC)\sDTS-HDMA\)",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);
        var title = match.Groups["title"].Value.Trim();

        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = title,
            Episode      = ParseDecimalGroup(match, "episode"),
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = EnumLanguage.None,
            SubtitleType = EnumSubtitleType.None
        };
    }
}
