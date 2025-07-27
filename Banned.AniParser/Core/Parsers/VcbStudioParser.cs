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
        SingleEpisodePatterns = new List<Regex>
        {
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\[(?<episode>\d+(?:\.\d+)?)?(?:\(?(?<special_season>OVA|OAD)(?<special_episode>\d+)?\)?)?\]\[(?<codec>Ma10p|Ma444-10p|Hi444pp|Hi10p)?_?(?<resolution>\d+p)(?:_HDR)?\]\[[^\[\]]+\](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)(?<rate>\d+-bit)?\s?(?<resolution>\d+p)\s?(?<codec>HEVC|AVC)?\s?(?<source>[a-z]+Rip)\s\[(?<media_type>MOVIE)\s?(:?Fin|Reseed)?\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)\]\[(?<codec>Ma10p|Ma444-10p|Hi444pp|Hi10p)?_?(?<resolution>\d+p)(?:_HDR)?\]\[[^\[\]]+\](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)(?<rate>\d+-bit)?\s?(?<resolution>\d+p)\s?(?<codec>HEVC|AVC)?\s?(?<source>[a-z]+Rip)\s\[(?<season>(?!(?:movie|fin|reseed)(?:\b|[\s\]])).+?)\s?(?:Fin)?\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var mediaType = EnumMediaType.SingleEpisode;
        var episode   = 1f;
        if (match.Groups["episode"].Success)
        {
            episode = float.Parse(match.Groups["episode"].Value);
        }
        else if (match.Groups["special_episode"].Success)
        {
            episode   = int.Parse(Regex.Replace(match.Groups["special_episode"].Value, @"\D+", ""));
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

        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value;
        }

        return new ParseResult
        {
            MediaType    = mediaType,
            Title        = title,
            Episode      = episode,
            Group        = group,
            GroupType    = GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType,
            Source       = "BDRip",
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        var mediaType    = EnumMediaType.MultipleEpisode;
        var title        = match.Groups["title"].Value.Trim();
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

        if (title.Contains("剧场版"))
        {
            mediaType = EnumMediaType.Movie;
        }

        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value;
        }

        return new ParseResult
        {
            MediaType    = mediaType,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = group,
            GroupType    = GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = EnumLanguage.None,
            SubtitleType = EnumSubtitleType.None,
            Source       = "BDRip",
        };
    }
}
