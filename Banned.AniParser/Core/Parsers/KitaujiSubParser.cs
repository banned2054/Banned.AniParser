using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class KitaujiSubParser : BaseParser
{
    public override string        GroupName => "北宇治字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[(北宇治字幕组|KitaujiSub)](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<source>[a-z]+Rip)]\[(?<codeV>HEVC)_(?<codeA>AAC(?:x2)?)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    [GeneratedRegex(@"\[(北宇治字幕组|KitaujiSub)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)[^\[\]]*]\[(?<source>[a-z]+Rip)]\[(?<codeV>HEVC)_(?<codeA>AAC)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public KitaujiSubParser()
    {
        LanguageMap["CHT_JP"]   = EnumLanguage.JpTc;
        LanguageMap["CHS_JP"]   = EnumLanguage.JpSc;
        SingleEpisodePatterns   = [SinglePattern()];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
