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
        SubtitleTypeMap["CHT"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHS"] = EnumSubtitleType.Embedded;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"【悠哈璃羽字幕[社组]】\s?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?:x264\s)?(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【悠哈璃羽字幕[社组]】\s?\[(?<title>[^\[\]]+?)\]\[(?<media_type>Movie)\]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<codeV>HEVC-?[a-z0-9]+|x264|x265)\s(?<codeA>FLAC|AAC)\]\[(?<extension>MKV|MP4)?\s?(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【悠哈璃羽字幕[社组]】\s?\[(?<title>[^\[\]]+?)\]\[(?<media_type>Movie)\]\[(?<codeV>x264|x265)\s(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【悠哈璃羽字幕[社组]】\s?\[(?<title>[^\[\]]+?)\]\[(?<lang>.+?)\]\[(?<media_type>Movie)\]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s?(?<codeV>HEVC-?[a-z0-9]+|x264|x265)?\s?(?<codeA>FLAC|AAC)?\]",
                RegexOptions.IgnoreCase),
            //【悠哈璃羽字幕社】 [摇曳露营剧场版_Yuru Camp Movie][简日双语][MOVIE][WEBRIP 1920x1080]
            new(
                @"\[UHA-Wing\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 1;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var title     = match.Groups["title"].Value.Trim();
        var mediaType = EnumMediaType.SingleEpisode;
        if (match.Groups["media_type"].Success)
        {
            if (match.Groups["media_type"].Value.ToLower() == "movie")
                mediaType = EnumMediaType.Movie;
        }

        return new ParseResult
        {
            MediaType    = mediaType,
            Title        = title,
            Episode      = episode,
            Group        = GroupName,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
