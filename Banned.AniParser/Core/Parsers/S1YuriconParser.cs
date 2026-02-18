using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class S1YuriconParser : BaseParser
{
    public override string        GroupName => "S1百综字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?S1(?:YURICON|百综字幕组)(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)(?:\[v(?<version>\d+)])?\[(?<episode>\d+)](?:\[(?<resolution>\d+p)])?\[(?<source>[a-z]+Rip)](?:\[(?<vCodec>HEVC|AVC)_(?<aCodec>EAC3|AAC)])?\[(:?ch[st]\]\[)?(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?S1(?:YURICON|百综字幕组)(?:&[^\[\]]+)?)](?<title>[^\[\]]+?)(?:\[v(?<version>\d+)])?\[(?<start>\d+)-(?<end>\d+)](?:\[(?<resolution>\d+p)])?\[(?<source>[a-z]+Rip)](?:\[(?<vCodec>HEVC|AVC)_(?<aCodec>EAC3|AAC)])?\[(:?ch[st]\]\[)?(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public S1YuriconParser()
    {
        SingleEpisodePatterns   = [SinglePattern1(),];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
