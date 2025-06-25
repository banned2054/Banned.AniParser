using MonoTorrent;
using System.Text.RegularExpressions;
using Banned.AniParser.Test.Utils;

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

    [Test]
    public async Task TestByTorrentFile()
    {
        var aniParser = new AniParser();
        // 读取 .torrent 文件
        var torrent =
            await
                Torrent.LoadAsync(@"D:\Downloads\[U2].42576.torrent");

        // 多文件 torrent
        if (torrent.Files.Count > 0)
        {
            foreach (var file in torrent.Files)
            {
                var result = aniParser.Parse(file.Path);
                TestPrintUtils.PrintParserInfo(result, file.Path);
            }
        }
        else
        {
            var result = aniParser.Parse(torrent.Name);
            TestPrintUtils.PrintParserInfo(result, torrent.Name);
        }
    }
}