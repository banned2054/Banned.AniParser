using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class NekoMoeParser : BaseParser
{
    public override string GroupName => "喵萌奶茶屋";

    public NekoMoeParser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"【喵萌奶茶屋】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),

            new(
                @"【喵萌Production】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Nekomoe kissaten\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>()
        {
            new(
                @"【喵萌奶茶屋】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"【喵萌Production】(?:★\d+月新番★)?\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase)
        };
    }
}