using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class BillionMetaLabParser : BaseParser
{
    public override string GroupName => "亿次研同好会";

    public BillionMetaLabParser()
    {
        LanguageMap["CHS&JPN"]         = EnumLanguage.JpSc;
        SubtitleTypeMap["CHS&JPN"]     = EnumSubtitleType.Embedded;
        LanguageMap["CHT&JPN"]         = EnumLanguage.JpTc;
        SubtitleTypeMap["CHT&JPN"]     = EnumSubtitleType.Embedded;
        LanguageMap["CHS&CHT&JPN"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["CHS&CHT&JPN"] = EnumSubtitleType.Muxed;
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