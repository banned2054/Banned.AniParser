using Banned.AniParser.Models;
using System.Text.RegularExpressions;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Core.Parsers;

public class JsumParser : BaseParser
{
    public override string GroupName { get; } = "jsum";

    public JsumParser()
    {
        //  [Dan Da Dan][01][BDRIP][1080P][H264_FLACx2].mkv
        SingleEpisodePatterns = new List<Regex>
        {
            new(@"\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+(?:\.\d+)?)\]\[BDRIP\]\[(?<resolution>\d+[pP])\]\[(?<vcodec>H264|H265)_(?<acodec>FLAC(?:x2)?)\]\.mkv",
                RegexOptions.IgnoreCase),
            new(@"\[(?<title>[^\[\]]+?)\]\[BDRIP\]\[(?<resolution>\d+[pP])\]\[(?<vcodec>H264|H265)_(?<acodec>FLAC(?:x2)?)\]\.mkv",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
        };
        //[流云字幕组&VCB-S&ANK-Raws] 双斩少女 / KILL la KILL / キルラキル 10-bit 1080p AVC BDRip [Reseed Fin]
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

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
            SubtitleType = subType,
            Source       = "BDRip",
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
            SubtitleType = EnumSubtitleType.None,
            Source       = "BDRip",
        };
    }
}