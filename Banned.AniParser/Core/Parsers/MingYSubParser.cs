using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class MingYSubParser : BaseParser
{
    public override string        GroupName => "MingYSub";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public MingYSubParser()
    {
        SubtitleTypeMap["JpSc"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["JpTc"] = EnumSubtitleType.Embedded;
        LanguageMap["JpCn"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["JpCn"] = EnumSubtitleType.Muxed;
        SingleEpisodePatterns =
        [
            new(
                @"\[(?<group>(?:[^\[\]]+&)?MingY(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(?:[^\[\]]+&)?MingY(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(
                @"\[MingY](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
