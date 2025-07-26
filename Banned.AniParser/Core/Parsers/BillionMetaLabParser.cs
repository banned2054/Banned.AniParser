using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class BillionMetaLabParser : BaseParser
{
    public override string        GroupName => "亿次研同好会";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public BillionMetaLabParser()
    {
        SubtitleTypeMap["Chs&Jpn"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["Cht&Jpn"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["Chs&Cht&Jpn"] = EnumSubtitleType.Muxed;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[Billion\sMeta\sLab\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+p)\]\[[^\[\]]+\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }
}
