using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class JsumParser : BaseParser
{
    public override string        GroupName => "jsum";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    [GeneratedRegex(@"\[(?<title>[^\[\]]+?)]\[(?<episode>\d+(?:\.\d+)?)]\[BDRIP]\[(?<resolution>\d+p)]\[(?<vCodec>H264|H265)_(?<aCodec>FLAC)(?:x2)?]\.mkv",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<title>[^\[\]]+?)]\[BDRIP]\[(?<resolution>\d+p)]\[(?<vCodec>H264|H265)_(?<aCodec>FLAC)(?:x2)?]\.mkv",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    public JsumParser()
    {
        SingleEpisodePatterns = [SinglePattern1(), SinglePattern2()];
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
        InitMap();
    }


    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = ParseIntGroup(match, "episode"),
            Group        = GroupName,
            GroupType    = this.GroupType,
            Language     = lang,
            MediaType    = EnumMediaType.SingleEpisode,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Source       = "BDRip",
            SubtitleType = subType,
        };
    }
}
