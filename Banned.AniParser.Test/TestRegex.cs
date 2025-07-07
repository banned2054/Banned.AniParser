using Banned.AniParser.Test.Utils;
using MonoTorrent;
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
            new
                Regex(
                      @"\[Prejudice-Studio\](?<title>[^\[\]]+?)\s?\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?\]\[(?<websource>Bilibili)\s(?<source>WEB-DL|WebRip)\s(?<resolution>\d+[pP])\s(?<codeV>AVC)\s(?<videoRate>\d+bit)\s(?<codeA>AAC)\s?(?<extension>MP4|MKV)?\]\[(?<lang>.+?)\]",
                      RegexOptions.IgnoreCase);
        parser =
            new Regex(
                      @"\[Prejudice-Studio\](?<title>[^\[\]]+?)\s?\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?\]\[(?<websource>Bilibili)\s(?<source>WEB-DL|WebRip)\s(?<resolution>\d+[pP])\s(?<codeV>AVC)\s(?<videoRate>\d+bit)\s(?<codeA>AAC)\s?(?<extension>MP4|MKV)?\]\[(?<lang>.+?)\]",
                      RegexOptions.IgnoreCase);
        var testStrList = new List<string>
        {
            "[Prejudice-Studio] 这是妳与我的最后战场，或是开创世界的圣战 第二季（仅限港澳台） Kimi to Boku no Saigo no Senjou S2 [01-12][Bilibili WEB-DL 1080P AVC 8bit AAC MKV][繁体内封]"
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
