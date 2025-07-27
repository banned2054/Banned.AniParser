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
    public void TestRegexWithOneString()
    {
        var parser =
            new Regex(
                      @"【悠哈璃羽字幕社】\[(?<title>[^\[\]]+?)\]\[(?<media_type>Movie)\]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+x\d+)\s(?<codeV>HEVC-?[a-z0-9]+|x264|x265)\s(?<codeA>FLAC|AAC)\]\[(?<extension>MKV|MP4)?\s?(?<lang>.+?)\]",
                      RegexOptions.IgnoreCase);
        parser =
            new Regex(
                      @"【悠哈璃羽字幕社】\[(?<title>[^\[\]]+?)\]",
                      RegexOptions.IgnoreCase);
        var testStrList = new List<string>
        {
            "【悠哈璃羽字幕组】[Bocchi the Rock Re: / Re:Re:  ][Movie][BDRIP 1920x1080 HEVC-YUV420P10 FLAC][MKV 简繁外挂]"
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
