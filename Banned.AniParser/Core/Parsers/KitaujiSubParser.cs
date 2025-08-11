using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class KitaujiSubParser : BaseParser
{
    public override string        GroupName => "北宇治字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public KitaujiSubParser()
    {
        LanguageMap["CHT_JP"] = EnumLanguage.JpTc;
        LanguageMap["CHS_JP"] = EnumLanguage.JpSc;
        SingleEpisodePatterns =
        [
            new(
                @"\[(北宇治字幕组|KitaujiSub)](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<source>[a-z]+Rip)]\[(?<codeV>HEVC)_(?<codeA>AAC(?:x2)?)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];

        MultipleEpisodePatterns =
        [
            new(
                @"\[(北宇治字幕组|KitaujiSub)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)[^\[\]]*]\[(?<source>[a-z]+Rip)]\[(?<codeV>HEVC)_(?<codeA>AAC)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
