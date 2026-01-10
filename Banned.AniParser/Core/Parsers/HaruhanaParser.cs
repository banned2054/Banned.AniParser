using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class HaruhanaParser : BaseParser
{
    public override string        GroupName => "拨雪寻春";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[❀?(?<group>(?:[^\[\]]+&)?(?:拨雪寻春|Haruhana)(?:&[^\[\]]+)?)❀?](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?(\[(?<source>[a-z]+Rip)])?\[(?<codec>HEVC)-?(?<rate>\d+bit)?\s?(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    public HaruhanaParser()
    {
        LanguageMap["CHI_JPN"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["CHI_JPN"] = EnumSubtitleType.Muxed;
        LanguageMap["CHT_JPN"]     = EnumLanguage.JpTc;
        SubtitleTypeMap["CHT_JPN"] = EnumSubtitleType.Embedded;
        LanguageMap["CHS_JPN"]     = EnumLanguage.JpSc;
        SubtitleTypeMap["CHS_JPN"] = EnumSubtitleType.Embedded;
        SingleEpisodePatterns      = [SinglePattern()];
        InitMap();
    }
}
