using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class VcbStudioParser : BaseParser
{
    public override string        GroupName => "Vcb-Studio";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<episode>\d+(?:\.\d+)?)?(?:\(?(?<mediaType>OVA|OAD)(?<episode>\d+)?\)?)?]\[(?<vCodec>Ma10p|Ma444-10p|Hi444pp|Hi10p|Pro10p)?_?(?<resolution>\d+p)(?:_HDR)?]\[[^\[\]]+](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)(?:(?<rate>\d+)-?bit)?\s?(?<resolution>\d+p)\s?(?<vCodec>HEVC|AVC)?\s?(?<source>[a-z]+Rip)\s\[(?<mediaType>MOVIE)\s?(:?Fin|Reseed)?]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)]\[(?<vCodec>Ma10p|Ma444-10p|Hi444pp|Hi10p|Pro10p)?_?(?<resolution>\d+p)(?:_HDR)?]\[[^\[\]]+](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\s?(?<resolution>\d+p)\s?(?<vCodec>HEVC|AVC)?\s?(?<source>[a-z]+Rip)\s\[(?<season>(?!(?:movie|fin|reseed)(?:\b|[\s\]])).+?)?\s?(?:Fin)?]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern2();

    public VcbStudioParser()
    {
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2()];
        MultipleEpisodePatterns = [MultiplePattern1(), MultiplePattern2()];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);
        var title = match.Groups["title"].Value.Trim();
        if (match.Groups["mediaType"].Success)
        {
            title = $"{title} {match.Groups["mediaType"].Value}";
        }

        return new ParseResult
        {
            Title         = title,
            Episode       = ParseDecimalGroup(match, "episode"),
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Language      = lang,
            MediaType     = ParseSingleMediaType(match),
            Resolution    = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Source        = "BDRip",
            SubtitleType  = subType,
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
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
            Title         = title,
            StartEpisode  = ParseIntGroup(match, "start"),
            EndEpisode    = ParseIntGroup(match, "end"),
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Language      = EnumLanguage.None,
            MediaType     = mediaType,
            Resolution    = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Source        = "BDRip",
            SubtitleType  = EnumSubtitleType.None,
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
        };
    }

    protected override int ParseColorBitDepth(Match match)
    {
        if (match.Groups["rate"].Success && int.TryParse(match.Groups["rate"].Value, out var rate))
        {
            return rate;
        }

        if (!match.Groups["vCodec"].Success) return 8;
        var vCodec = match.Groups["vCodec"].Value.ToUpper();
        if (vCodec.Contains("10P") || vCodec == "HI444PP")
        {
            return 10;
        }

        return 8;
    }

    protected override string ParseVideoCodec(Match match)
    {
        if (!match.Groups["vCodec"].Success) return string.Empty;
        var vCodec = match.Groups["vCodec"].Value.ToUpper();
        return vCodec switch
        {
            "HI444PP" or "HI10P"   => "AVC",
            "MA10P" or "MA444-10P" => "HEVC",
            "PRO10P"               => "AV1",
            _                      => vCodec
        };
    }
}
