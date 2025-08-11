using System.Text.RegularExpressions;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Core.Parsers;

public class StudioGreenTeaParser : BaseParser
{
    public override string        GroupName => "绿茶字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public StudioGreenTeaParser()
    {
        LanguageMap["ASSx2"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["ASSx2"] = EnumSubtitleType.Muxed;
        SingleEpisodePatterns =
        [
            new(@"\[绿茶字幕组](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>WebRip)])?\[(?<codeV>HEVC)_?(?<codeA>AAC)]\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(@"\[Studio\sGreenTea](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<source>WebRip)]\[(?<codeV>HEVC)-?(?<rate>\d+bit)?\s?(?<resolution>\d+p)\s?(?<codeA>AAC)(:?]\[|\s)(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
