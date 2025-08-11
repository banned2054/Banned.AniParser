using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class VcbStudioParser : BaseParser
{
    public override string        GroupName => "Vcb-Studio";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    public VcbStudioParser()
    {
        SingleEpisodePatterns =
        [
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<episode>\d+(?:\.\d+)?)?(?:\(?(?<special_season>OVA|OAD)(?<special_episode>\d+)?\)?)?]\[(?<codec>Ma10p|Ma444-10p|Hi444pp|Hi10p)?_?(?<resolution>\d+p)(?:_HDR)?]\[[^\[\]]+](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)(?<rate>\d+-bit)?\s?(?<resolution>\d+p)\s?(?<codec>HEVC|AVC)?\s?(?<source>[a-z]+Rip)\s\[(?<media_type>MOVIE)\s?(:?Fin|Reseed)?]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)]\[(?<codec>Ma10p|Ma444-10p|Hi444pp|Hi10p)?_?(?<resolution>\d+p)(?:_HDR)?]\[[^\[\]]+](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)(?<rate>\d+-bit)?\s?(?<resolution>\d+p)\s?(?<codec>HEVC|AVC)?\s?(?<source>[a-z]+Rip)\s\[(?<season>(?!(?:movie|fin|reseed)(?:\b|[\s\]])).+?)?\s?(?:Fin)?]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var mediaType = EnumMediaType.SingleEpisode;
        var episode   = ParseDecimalGroup(match, "episode");
        if (match.Groups["special_episode"].Success)
        {
            episode   = int.Parse(match.Groups["special_episode"].Value);
            mediaType = EnumMediaType.Ova;
        }

        if (match.Groups["media_type"].Success)
        {
            if (match.Groups["media_type"].Value.ToLower() == "movie")
            {
                mediaType = EnumMediaType.Movie;
            }
        }

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var title = match.Groups["title"].Value.Trim();
        if (match.Groups["special_season"].Success)
        {
            title = $"{title} {match.Groups["special_season"].Value}";
        }

        return new ParseResult
        {
            Title        = title,
            Episode      = episode,
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Language     = lang,
            MediaType    = mediaType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Source       = "BDRip",
            SubtitleType = subType,
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        var mediaType = EnumMediaType.MultipleEpisode;
        var title     = match.Groups["title"].Value.Trim();

        if (title.Contains("剧场版"))
        {
            mediaType = EnumMediaType.Movie;
        }

        return new ParseResult
        {
            Title        = title,
            StartEpisode = ParseIntGroup(match, "start"),
            EndEpisode   = ParseIntGroup(match, "end"),
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Language     = EnumLanguage.None,
            MediaType    = mediaType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Source       = "BDRip",
            SubtitleType = EnumSubtitleType.None,
        };
    }
}
