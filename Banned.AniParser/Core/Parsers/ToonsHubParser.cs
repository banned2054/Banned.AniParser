using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class ToonsHubParser : BaseTransferParser
{
    public override string GroupName => "ToonsHub";

    // [ToonsHub] 转生就是剑 第二季 / Reincarnated as a Sword Season 2 - 09 [ADN][1080P][CHS&CHT&EN][MKV]
    // [ToonsHub] 淫狱圣地 / Ingoku Danchi - 03 [UNCENSORED ADN][1080P][DUAL][JP&EN][MKV]
    // [ToonsHub] 百花录 英语版（日配版） / Fairies Album Season 3 (Japanese Dub) - 08 [AMZN][1080P][RAW][MKV]
    [GeneratedRegex(@"\[ToonsHub](?<title>.+?)\s+-\s+(?<episode>\d+)(?:v(?<version>\d+))?\s+\[(?:(?:UNCENSORED)\s+)?(?<websource>CR|ADN|AMZN|ABEMA|IQIYI|NF|DSNP|BAHA)\]\s*\[(?<resolution>\d+p)\]\s*(?:\[(?<flag>DUAL|RAW)\]\s*)?(?:\[(?<lang>CHS&CHT&EN|CHT&EN|JP&EN|EN)\]\s*)?\[(?<extension>MKV|MP4)]$",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    public ToonsHubParser()
    {
        LanguageMap["CHS&CHT&EN"] = EnumLanguage.EngScTc;
        LanguageMap["CHT&EN"]     = EnumLanguage.EngTc;
        LanguageMap["JP&EN"]      = EnumLanguage.JpEng;
        LanguageMap["EN"]         = EnumLanguage.Eng;

        SubtitleTypeMap["CHS&CHT&EN"] = EnumSubtitleType.Muxed;
        SubtitleTypeMap["CHT&EN"]     = EnumSubtitleType.Muxed;
        SubtitleTypeMap["JP&EN"]      = EnumSubtitleType.Muxed;
        SubtitleTypeMap["EN"]         = EnumSubtitleType.Muxed;

        SingleEpisodePatterns = [SinglePattern()];
        InitMap();
    }
}
