using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class SakuratoParser : BaseParser
{
    public override string        GroupName => "桜都字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[[樱桜]都字幕[組组]](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[Sakurato](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<vCodec>HEVC|AVC)(?:-?(?<rate>\d+)-?bit)?\s?(?<resolution>\d+p)\s?(?<aCodec>(AAC))?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(?:[樱桜]都字幕[組组]|Sakurato)](?<title>[^\[\]]+?)\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    [GeneratedRegex(@"\[[樱桜]都字幕[組组]](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)\s?(?:END|Fin)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?]\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public SakuratoParser()
    {
        SingleEpisodePatterns = [SinglePattern1(), SinglePattern2(), SinglePattern3()];

        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
