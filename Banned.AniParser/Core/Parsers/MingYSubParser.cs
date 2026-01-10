using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class MingYSubParser : BaseParser
{
    public override string        GroupName => "MingYSub";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?MingY(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?MingY(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[MingY](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern1();

    [GeneratedRegex(@"\[MingY](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern2();

    public MingYSubParser()
    {
        SubtitleTypeMap["JpSc"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["JpTc"] = EnumSubtitleType.Embedded;
        LanguageMap["JpCn"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["JpCn"] = EnumSubtitleType.Muxed;
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2()];
        MultipleEpisodePatterns = [MultiplePattern1(), MultiplePattern2()];
        InitMap();
    }
}
