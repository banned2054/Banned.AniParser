using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class FeiBanYaMaParser : BaseTransferParser
{
    public override string GroupName => "沸班亚马制作组";

    public FeiBanYaMaParser()
    {
        LanguageMap["Multi-Subs"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["Multi-Subs"] = EnumSubtitleType.Muxed;

        SingleEpisodePatterns =
        [
            new(
                @"\[Feibanyama](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>CR|ABEMA|AMZN|IQIYI)\s(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s(?<codeV>HEVC)-?(?<rate>\d+bit)?\s(?<codeA>AAC|OPUS|E-AC-3)\s(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"\[沸班亚马制作组](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>CR|ABEMA|AMZN|IQIYI)\s(?<source>[a-z]+Rip)\s(?:AI)?(?<resolution>\d+p)\s(?<codeV>HEVC)-?(?<rate>\d+bit)?\s(?<codeA>AAC|OPUS|E-AC-3)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
