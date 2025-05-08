using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class UhaWingParser : BaseParser
{
    public override string GroupName { get; } = "悠哈璃羽字幕社";

    public UhaWingParser()
    {
        SubtitleTypeMap["CHT"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHS"] = EnumSubtitleType.Embedded;
        //x264 1080p][CHT]
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"【悠哈璃羽字幕社】\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?:x264\s)?(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[UHA-Wing\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
        };
    }
}