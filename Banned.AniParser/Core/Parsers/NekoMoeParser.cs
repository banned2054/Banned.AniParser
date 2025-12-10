using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class NekoMoeParser : BaseParser
{
    public override string        GroupName => "喵萌奶茶屋";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public NekoMoeParser()
    {
        SingleEpisodePatterns =
        [
            new(@"【(?<group>(?:[^\[\]]+&)?喵萌(?:奶茶屋|Production)(?:&[^\[\]]+)?)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?Nekomoe kissaten(?:&[^\[\]]+)?)]\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]\[v(?<version>\d+)]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(@"【喵萌(?:奶茶屋|Production)】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)]\[(?<start>\d+)-(?<end>\d+)(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
