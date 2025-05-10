using Banned.AniParser.Models;
using System.Text.RegularExpressions;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Core.Parsers;

public class VcbStudioParser : BaseParser
{
    public override string GroupName { get; } = "VcbStudio";

    public VcbStudioParser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\[(?<episode>\d+(?:\.\d+)?)(?:\((?:OAD|OVA)?\d*\))?(?<special_episode>OVA)?\]\[(?<codec>Ma10p|Ma444-10p|Hi444pp|Hi10p)?_?(?<resolution>\d+[pP])(?:_HDR)?\]\[[^\[\]]+\](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)\]\[(?<codec>Ma10p|Ma444-10p|Hi444pp|Hi10p)?_?(?<resolution>\d+[pP])(?:_HDR)?\]\[[^\[\]]+\](?:\.(?<language>[^\[\]\.]+))?\.?(?:mp4|mkv|ass|mka)",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)(?:10-bit)?\s?(?<resolution>\d+[pP])\s?(?<codec>HEVC|AVC)?\s?(?:(?<source>[a-zA-Z]+[Rr]ip))\s\[(?<season>[^\[\]]+)(?:Fin)?\]",
                RegexOptions.IgnoreCase),
        };
        //[流云字幕组&VCB-S&ANK-Raws] 双斩少女 / KILL la KILL / キルラキル 10-bit 1080p AVC BDRip [Reseed Fin]
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
        {
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));
        }

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);


        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            GroupType    = EnumGroupType.Compression,
            Resolution   = match.Groups["resolution"].Value,
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParserInfo CreateParsedResultMultiple(Match match)
    {
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


        return new ParserInfo
        {
            IsMultiple   = true,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = GroupName,
            GroupType    = EnumGroupType.Compression,
            Resolution   = match.Groups["resolution"].Value,
            Language     = EnumLanguage.None,
            SubtitleType = EnumSubtitleType.None
        };
    }
}