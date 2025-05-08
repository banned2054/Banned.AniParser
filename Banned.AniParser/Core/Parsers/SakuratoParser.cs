using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class SakuratoParser : BaseParser
{
    public override string GroupName => "桜都字幕组";

    public SakuratoParser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Sakurato\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<vcodec>(HEVC|AVC|AVC-8bit|HEVC-10bit))?\s?(?<resolution>\d+[pP])\s?(?<acodec>(AAC))?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Sakurato\](?<title>[^\[\]]+?)\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };

        MultipleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }
}