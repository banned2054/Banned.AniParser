using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class BeanSubParser : BaseParser
{
    public override string        GroupName => "豌豆字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?:\d+\()?(?<episode>\d+)(?:\))?]\[v(?<version>\d+)]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)](?:\sv(?<version>\d+))?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】(?<media_type>★剧场版)\[(?<title>[^\[\]]+?)\]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)](?:\sv(?<version>\d+))?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?BeanSub(?:&[^\[\]]+)?)]\[(?<title>[^\[\]]+?)\]\[(?<media_type>movie)\]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(?<codeV>x264)_?(?<codeA>aac)?](?:\sv(?<version>\d+))?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    [GeneratedRegex(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)-(?<end>\d+)(?:[\u4e00-\u9fff\s]+)(?:v(?<version>\d+))?](?:\[合集])?\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public BeanSubParser()
    {
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2(), SinglePattern3(),];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
