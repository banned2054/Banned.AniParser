using Banned.AniParser.Test.Utils;
using MonoTorrent;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Test;

internal class TestRegex
{
    [Test]
    public void TestRegexWithOneString()
    {
        var parser =
            new
                Regex(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】(?<media_type>★剧场版)\[(?<title>[^\[\]]+?)\]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)]",
                      RegexOptions.IgnoreCase);
        parser =
            new
                Regex(@"【(?<group>(?:[^\[\]]+&)?豌豆字幕组(?:&[^\[\]]+)?)】(?<media_type>★剧场版)\[(?<title>[^\[\]]+?)\]\[(?<lang>.+?)]\[(?<resolution>\d+p)]\[(mp4|mkv)]",
                      RegexOptions.IgnoreCase);
        var testStrList = new List<string>
        {
            "【豌豆字幕组&风之圣殿字幕组】★剧场版[电锯人 / 链锯人 蕾洁篇][繁体][1080P][MP4] [复制磁连]",
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
