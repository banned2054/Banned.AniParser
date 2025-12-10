using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class BeanSubParser : BaseParser
{
    public override string        GroupName => "豌豆字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public BeanSubParser()
    {
        SingleEpisodePatterns =
        [
            new(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】(?:★\d+月新番)?\[(?<title>[^\[\]]+?)\]\[(?:\d+\()?(?<episode>\d+)(?:\))?]\[v(?<version>\d+)]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)](?:\sv(?<version>\d+))?",
                RegexOptions.IgnoreCase),
            new(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】(?<media_type>★剧场版)\[(?<title>[^\[\]]+?)\]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)](?:\sv(?<version>\d+))?",
                RegexOptions.IgnoreCase),
            new(@"\[(?<group>(?:[^\[\]]+&)?BeanSub(?:&[^\[\]]+)?)]\[(?<title>[^\[\]]+?)\]\[(?<media_type>movie)\]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(?<codeV>x264)_?(?<codeA>aac)?](?:\sv(?<version>\d+))?",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)-(?<end>\d+)(?:[\u4e00-\u9fff\s]+)(?:v(?<version>\d+))?](?:\[合集])?\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)]",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
