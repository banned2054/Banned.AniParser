using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class BillionMetaLabParser : BaseParser
{
    public override string        GroupName => "亿次研同好会";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[Billion\sMeta\sLab](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)](?:\[(?<vCodec>HEVC)\s?(?:-?(?<rate>\d+)bit)?])?\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[Billion\sMeta\sLab](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<vCodec>HEVC)\s?(?:-?(?<rate>\d+)bit)?])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    public BillionMetaLabParser()
    {
        SubtitleTypeMap["Chs&Jpn"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["Cht&Jpn"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["Chs&Cht&Jpn"] = EnumSubtitleType.Muxed;

        LanguageMap["中日双语"]   = EnumLanguage.JpScTc;
        SingleEpisodePatterns = [SinglePattern1(), SinglePattern2(),];
        InitMap();
    }
}
