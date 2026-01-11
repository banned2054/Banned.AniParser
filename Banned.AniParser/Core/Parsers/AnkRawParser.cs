using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class AnkRawParser : BaseParser
{
    public override string        GroupName => "ANK-Raws";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\s(?<episode>\d+(\.\d+)?)\s\((?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<vCodec>HEVC)-?(?<pixelFmt>YUV420P)(?<rate>10)\s(?<aCodec>FLAC)\sDTS-HDMA\)\.mkv",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\((?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<vCodec>HEVC)-?(?<pixelFmt>YUV420P)(?<rate>10)\s(?<aCodec>FLAC)\sDTS-HDMA\)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public AnkRawParser()
    {
        SingleEpisodePatterns   = [SinglePattern(),];
        MultipleEpisodePatterns = [MultiplePattern(),];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            Title         = GetGroupOrDefault(match, "title", string.Empty),
            Episode       = ParseDecimalGroup(match, "episode"),
            Language      = lang,
            MediaType     = EnumMediaType.SingleEpisode,
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Resolution    = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Source        = match.Groups["source"].Value,
            SubtitleType  = subType,
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        return new ParseResult
        {
            Title         = GetGroupOrDefault(match, "title", string.Empty),
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Language      = EnumLanguage.None,
            MediaType     = EnumMediaType.MultipleEpisode,
            Resolution    = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Source        = match.Groups["source"].Value,
            SubtitleType  = EnumSubtitleType.None,
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
        };
    }
}
