using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class UhaWingParser : BaseParser
{
    public override string        GroupName => "悠哈璃羽字幕社";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)\s?(?<vCodec>HEVC|x264|x265)?(?:-?(?<rate>\d+)bit)?\s?(?<aCodec>FLAC|AAC)?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<vCodec>HEVC|x264|x265)?(?:-?(?<rate>\d+)bit)?\s?(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<mediaType>Movie)]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<vCodec>HEVC|x264|x265)(?:-?(?<rate>\d+)bit)?\s(?<aCodec>FLAC|AAC)]\[(?<extension>MKV|MP4)?\s?(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)](?:\[(?:v(?<version>\d+))])?\[(?<mediaType>Movie)]\[(?<vCodec>x264|x265)\s(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern4();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<lang>.+?)]\[(?<mediaType>Movie)]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s?(?<vCodec>HEVC|x264|x265)?(?:-?(?<rate>\d+)bit)?\s?(?<aCodec>FLAC|AAC)?]",
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

        return new ParseResult
        {
            Title         = GetGroupOrDefault(match, "title", string.Empty),
            Episode       = ParseIntGroup(match, "episode"),
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Language      = lang,
            MediaType     = ParseSingleMediaType(match),
            Resolution    = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            SubtitleType  = subType,
            Version       = ParseVersion(match),
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
        };
    }
}
