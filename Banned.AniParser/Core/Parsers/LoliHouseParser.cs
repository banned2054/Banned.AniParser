using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

internal class LoliHouseParser : BaseParser
{
    public override string        GroupName => "LoliHouse";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public LoliHouseParser()
    {
        LanguageMap["无字幕"]     = EnumLanguage.None;
        LanguageMap["TC"]      = EnumLanguage.Tc;
        LanguageMap["SC"]      = EnumLanguage.Sc;
        LanguageMap["英语"]      = EnumLanguage.Eng;
        SubtitleTypeMap["无字幕"] = EnumSubtitleType.None;
        SingleEpisodePatterns =
        [
            new(
                @"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*(?:\([\u4e00-\u9fff]+\))?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s?(?<codeV>AVC|HEVC)(?:[-\s])?(?<rate>\d+bit)?\s?(?<codeA>AAC(?:x2)?)?]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*(?:\([\u4e00-\u9fff]+\))?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*]\.(?<lang>[^\.]*)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*(?:\([\u4e00-\u9fff]+\))?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(
                @"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)\s*[^\[\]]*]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
