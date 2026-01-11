using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class LoliHouseParser : BaseParser
{
    public override string        GroupName => "LoliHouse";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*(?:\([\u4e00-\u9fff]+\))?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s?(?<vCodec>AVC|HEVC)\s?(?:-?(?<rate>\d+)bit)?\s?(?<aCodec>AAC)?(?:x2)?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*(?:\([\u4e00-\u9fff]+\))?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*]\.(?<lang>[^\.]*)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*(?:\([\u4e00-\u9fff]+\))?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?LoliHouse)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)\s*[^\[\]]*]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public LoliHouseParser()
    {
        LanguageMap["无字幕"]      = EnumLanguage.None;
        LanguageMap["TC"]       = EnumLanguage.Tc;
        LanguageMap["SC"]       = EnumLanguage.Sc;
        LanguageMap["英语"]       = EnumLanguage.Eng;
        SubtitleTypeMap["无字幕"]  = EnumSubtitleType.None;
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2(), SinglePattern3()];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
