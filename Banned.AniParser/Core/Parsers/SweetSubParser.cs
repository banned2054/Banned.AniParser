using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class SweetSubParser : BaseParser
{
    public override string GroupName => "SweetSub";

    public SweetSubParser()
    {
        SingleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[SweetSub\]\[(?<title>[^\[\]]+?)\]\[(?<engTitle>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<source>[a-zA-Z]+[Rr]ip)\]\[(?<resolution>\d+[pP])\]\[[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[SweetSub\]\[(?<title>[^\[\]]+?)\]\[(?<engTitle>[^\[\]]+?)\]\[(?<start>\d+)-(?<end>\d+)\s?(?<OAD>[a-zA-Z\u4e00-\u9fff]+)?\]\[(?<source>[a-zA-Z]+[Rr]ip)\]\[(?<resolution>\d+[pP])\]\[[^\[\]]*\]\[(?<lang>.+?)\](\[(?:v(?<version1>\d+))\])?",
                RegexOptions.IgnoreCase),
        };
    }
}