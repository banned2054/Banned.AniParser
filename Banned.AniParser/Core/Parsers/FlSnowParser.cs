using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class FlSnowParser : BaseParser
{
    public override string        GroupName => "雪飘工作室";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[雪飘工作室]\[(?<title>[^\[\]]+?)](?:\[(?<resolution>\d+p)])?\[(?:S(?<season>\d+))?(?:E)?(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[FLsnow]\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)](?:\.(?<lang>chs|cht))?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    public FlSnowParser()
    {
        LanguageMap["繁"] = EnumLanguage.Tc;

        SingleEpisodePatterns = [SinglePattern1(), SinglePattern2()];
        InitMap();
    }
}
