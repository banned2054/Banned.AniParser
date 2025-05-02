using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class BillionMetaLabParser : BaseParser
{
    public override string GroupName => "亿次研同好会";

    public BillionMetaLabParser()
    {
        SubtitleTypeMap["Chs&Jpn"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["Cht&Jpn"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["Chs&Cht&Jpn"] = EnumSubtitleType.Muxed;
        //[Billion Meta Lab] 双重四亡·恶魔毁灭 Dead Dead Demons Dededede Destruction [16][1080P][HEVC 10bit][CHS&CHT&JPN]
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[Billion\sMeta\sLab\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]\[[^\[\]]+\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>()
        {
        };
    }
}