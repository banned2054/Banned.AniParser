using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class SweetSubParser : BaseParser
{
    public override string GroupName => "SweetSub";

    public SweetSubParser()
    {
        //[SweetSub][机动战士高达 GQuuuuuuX][Mobile Suit Gundam GQuuuuuuX][04][WebRip][1080P][AVC 8bit][简日双语] [626.94 MB]
        SingleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[SweetSub\]\[(?<title>[^\[\]]+?)\]\[(?<engTitle>[^\[\]]+?)\]\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<source>[a-zA-Z]+[Rr]ip)\]\[(?<resolution>\d+[pP])\]\[[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        //[SweetSub][大欺詐師 razbliuto][GREAT PRETENDER razbliuto][01-04 精校合集][WebRip][1080P][AVC 8bit][繁日雙語][v2]
        MultipleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[SweetSub\]\[(?<title>[^\[\]]+?)\]\[(?<engTitle>[^\[\]]+?)\]\[(?<start>\d+)-(?<end>\d+)\s?(?<OAD>[a-zA-Z\u4e00-\u9fff]+)?\]\[(?<source>[a-zA-Z]+[Rr]ip)\]\[(?<resolution>\d+[pP])\]\[[^\[\]]*\]\[(?<lang>.+?)\](\[(?:v(?<version1>\d+))\])?",
                RegexOptions.IgnoreCase),
        };
    }
}