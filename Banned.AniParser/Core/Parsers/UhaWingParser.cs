using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class UhaWingParser : BaseParser
{
    public override string        GroupName => "悠哈璃羽字幕社";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public UhaWingParser()
    {
        LanguageMap["CHS_JP"] = EnumLanguage.JpSc;
        LanguageMap["CHT_JP"] = EnumLanguage.JpTc;

        SubtitleTypeMap["CHS"]    = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHT"]    = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHS_JP"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHT_JP"] = EnumSubtitleType.Embedded;

        SingleEpisodePatterns =
        [
            new(
                @"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)\s?(?<codeV>HEVC-?[a-z0-9]*|x264|x265)?\s?(?<codeA>FLAC|AAC)?]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<codeV>HEVC-?[a-z0-9]*|x264|x265)?\s?(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<media_type>Movie)]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<codeV>HEVC-?[a-z0-9]*|x264|x265)\s(?<codeA>FLAC|AAC)]\[(?<extension>MKV|MP4)?\s?(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)](?:\[(?:v(?<version>\d+))])?\[(?<media_type>Movie)]\[(?<codeV>x264|x265)\s(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"[\[【](?<group>(?:[^\[\]]+&)?(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)(?:&[^\[\]]+)?)[】\]]\s?\[(?<title>[^\[\]]+?)]\[(?<lang>.+?)]\[(?<media_type>Movie)]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s?(?<codeV>HEVC-?[a-z0-9]*|x264|x265)?\s?(?<codeA>FLAC|AAC)?]",
                RegexOptions.IgnoreCase),
        ];
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

        return new ParseResult
        {
            Title        = title,
            Episode      = ParseIntGroup(match, "episode"),
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Language     = lang,
            MediaType    = mediaType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            SubtitleType = subType,
        };
    }
}
