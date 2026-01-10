using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class UhaWingParser : BaseParser
{
    public override string        GroupName => "悠哈璃羽字幕社";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)\s?(?<codeV>HEVC|x264|x265)?(?:-?(?<rate>\d+)bit)?\s?(?<codeA>FLAC|AAC)?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<codeV>HEVC|x264|x265)?(?:-?(?<rate>\d+)bit)?\s?(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<media_type>Movie)]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<codeV>HEVC|x264|x265)(?:-?(?<rate>\d+)bit)?\s(?<codeA>FLAC|AAC)]\[(?<extension>MKV|MP4)?\s?(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)](?:\[(?:v(?<version>\d+))])?\[(?<media_type>Movie)]\[(?<codeV>x264|x265)\s(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern4();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<lang>.+?)]\[(?<media_type>Movie)]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s?(?<codeV>HEVC|x264|x265)?(?:-?(?<rate>\d+)bit)?\s?(?<codeA>FLAC|AAC)?]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern5();

    public UhaWingParser()
    {
        LanguageMap["CHS_JP"] = EnumLanguage.JpSc;
        LanguageMap["CHT_JP"] = EnumLanguage.JpTc;

        SubtitleTypeMap["CHS"]    = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHT"]    = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHS_JP"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHT_JP"] = EnumSubtitleType.Embedded;

        SingleEpisodePatterns =
            [SinglePattern1(), SinglePattern2(), SinglePattern3(), SinglePattern4(), SinglePattern5()];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);
        var title = match.Groups["title"].Value.Trim();

        var mediaType = EnumMediaType.SingleEpisode;
        if (match.Groups["media_type"].Success)
        {
            if (match.Groups["media_type"].Value.ToLower() == "movie")
                mediaType = EnumMediaType.Movie;
        }

        var videoCodec    = string.Empty;
        var audioCodec    = string.Empty;
        var colorBitDepth = "-1";
        if (match.Groups["codeV"].Success)
        {
            videoCodec = match.Groups["codeV"].Value.ToUpper()
                              .Replace("X264", "AVC")
                              .Replace("X265", "HEVC")
                              .Trim();
        }

        if (match.Groups["codeA"].Success)
        {
            audioCodec = match.Groups["codeA"].Value.ToUpper()
                              .Trim();
        }

        if (match.Groups["rate"].Success)
        {
            colorBitDepth = match.Groups["rate"].Value;
        }

        return new ParseResult
        {
            Title         = title,
            Episode       = ParseIntGroup(match, "episode"),
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Language      = lang,
            MediaType     = mediaType,
            Resolution    = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            SubtitleType  = subType,
            Version       = ParseVersion(match),
            VideoCodec    = videoCodec,
            AudioCodec    = audioCodec,
            ColorBitDepth = int.Parse(colorBitDepth)
        };
    }
}
