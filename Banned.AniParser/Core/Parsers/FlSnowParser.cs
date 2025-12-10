using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class FlSnowParser : BaseParser
{
    public override string        GroupName => "雪飘工作室";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public FlSnowParser()
    {
        LanguageMap["繁"] = EnumLanguage.Tc;

        SingleEpisodePatterns =
        [
            new(@"\[雪飘工作室]\[(?<title>[^\[\]]+?)](?:\[(?<resolution>\d+p)])?\[(?:S(?<season>\d+))?(?:E)?(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(@"\[FLsnow]\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)](?:\.(?<lang>chs|cht))?",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
