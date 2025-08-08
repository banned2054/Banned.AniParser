using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class JsumParser : BaseParser
{
    public override string        GroupName => "jsum";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    public JsumParser()
    {
        SingleEpisodePatterns =
        [
            new(@"\[(?<title>[^\[\]]+?)]\[(?<episode>\d+(?:\.\d+)?)]\[BDRIP]\[(?<resolution>\d+p)]\[(?<vcodec>H264|H265)_(?<acodec>FLAC(?:x2)?)]\.mkv",
                RegexOptions.IgnoreCase),
            new(@"\[(?<title>[^\[\]]+?)]\[BDRIP]\[(?<resolution>\d+p)]\[(?<vcodec>H264|H265)_(?<acodec>FLAC(?:x2)?)]\.mkv",
                RegexOptions.IgnoreCase),
        ];
        FilterList =
        [
            new(@"TV-CM\s[^\[\]]+Ver\.", RegexOptions.IgnoreCase),
            new(@"Character\sPV\s[^\[\]]+Ver\.", RegexOptions.IgnoreCase),
            new(@"\[Menu]", RegexOptions.IgnoreCase),
            new(@"\[NCOP]", RegexOptions.IgnoreCase),
            new(@"\[NCED]", RegexOptions.IgnoreCase),
            new(@"\[LOGO]", RegexOptions.IgnoreCase),
            new(@"\[Producer\sLogo]", RegexOptions.IgnoreCase),
            new(@"PV\s#\d+", RegexOptions.IgnoreCase),
            new(@"Short\sAnime\s#\d+-\d+", RegexOptions.IgnoreCase),
            new(@"Blu-ray\s&\sDVD\sCM\sCollection", RegexOptions.IgnoreCase),
        ];
    }


    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(match.Groups["episode"].Value);

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType,
            Source       = "BDRip",
        };
    }
}
