using System.Text.RegularExpressions;

namespace Banned.AniParser.Test;

internal class TestRegex
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var parser =
            //new
            //    Regex(@"\[(?<group>(?:[^\[\]]+&)VCB-Studio(?:&[^\[\]]+))\]\s(?<title>[^\[\]]+?)\s\[(?<episode>\d+(?:\.\d+)?)(?:\((?:OAD|OVA)?\d*\))?(?<special_episode>OVA)?\​]\[(?<codec>Ma10p_|Ma444-10p_|Hi444pp_|Hi10p_)?(?<resolution>\d+[pP])(?:_HDR)?\]\[[^\[\]]+\](?:\.(?<language>[^\[\]\.]+))",
            //          RegexOptions.IgnoreCase);
            new
                Regex(@"\[(?<group>(?:[^\[\]]+&)?VCB-Studio(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)(?:10-bit)?\s?(?<resolution>\d+[pP])\s?(?<codec>HEVC|AVC)?\s?(?:(?<source>[a-zA-Z]+[Rr]ip))\s\[(?<season>[^\[\]]+)(?:Fin)?\]",
                      RegexOptions.IgnoreCase);
        var testStrList = new List<string>
        {
            "[流云字幕组&VCB-S&ANK-Raws] 双斩少女 / KILL la KILL / キルラキル 10-bit 1080p AVC BDRip [Reseed Fin]",
            "[VCB-Studio] 向山进发 / Yama no Susume / ヤマノススメ 10-bit 1080p HEVC BDRip [S1-S3 + OVA Reseed + S4 Fin",
            "[VCB-Studio] 元气少女缘结神 / Kamisama Hajimemashita / 神様はじめました 10-bit 1080p/720p HEVC BDRip/DVDRip [S1-S2 + OAD Fin]",
        };
        foreach (var testStr in testStrList)
        {
            var match = parser.Match(testStr);
            if (!match.Success) continue;
            Console.WriteLine(testStr);
            foreach (Group group in match.Groups)
            {
                Console.WriteLine($"\t{group.Name}:{group.Value}");
            }
        }
    }
}