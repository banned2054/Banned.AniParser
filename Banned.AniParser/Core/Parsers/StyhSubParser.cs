using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class StyhSubParser : BaseParser
{
    public override string        GroupName => "霜庭云花";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[霜庭云花Sub]\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:V(?<version>\d+))?(\sEND)?]\[(?<resolution>\d+p)]\[(?<vCodec>HEVC|AVC)\s?(?<aCodec>AAC)?]\[(?<lang>.+?)]\[(?<source>[a-z]+Rip)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[霜庭云花Sub](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:V(?<version>\d+))?\s?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s(?<vCodec>HEVC|AVC)\s?(?<aCodec>AAC)?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[STYHSub]\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:V(?<version>\d+))?(\sEND)?]\[(?<resolution>\d+[pP])]\[(?<source>[a-z]+Rip)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    [GeneratedRegex(@"\[STYHSub](?<title>[^\[\]]+?)-\s?S(?<season>\d+)E(?<episode>\d+)(?:v(?<version>\d+))?\s?-\s?\[(?<source>[a-z])\s(?<vCodec>HEVC|AVC)\s?(?<aCodec>AAC)?\s(?<lang>.+?)\s?(V(?<version1>\d+))?]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern4();

    [GeneratedRegex(@"\[霜庭云花Sub]\[(?<title>[^\[\]]+?)]\[(?<start>\d+)-(?<end>\d+)\s?(?<OAD>[a-z\u4e00-\u9fff]+)?]\[(?<resolution>\d+p)]\[(?<vCodec>HEVC|AVC)\s?(?<aCodec>AAC)?]\[(?<lang>.+?)]\[(?<source>[a-z]+Rip)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public StyhSubParser()
    {
        LanguageMap["JpSc_JpTc"] = EnumLanguage.JpScTc;
        SingleEpisodePatterns    = [SinglePattern1(), SinglePattern2(), SinglePattern3(), SinglePattern4()];
        MultipleEpisodePatterns  = [MultiplePattern()];
        InitMap();
    }
}
