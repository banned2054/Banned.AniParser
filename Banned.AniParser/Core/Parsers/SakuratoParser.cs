using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class SakuratoParser : BaseParser
{
    public override string        GroupName => "桜都字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public SakuratoParser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Sakurato\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<vcodec>(HEVC|AVC|AVC-8bit|HEVC-10bit))?\s?(?<resolution>\d+p)\s?(?<acodec>(AAC))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Sakurato\](?<title>[^\[\]]+?)\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };

        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-z\u4e00-\u9fff]+))?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }
}
