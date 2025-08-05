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
            new
                Regex(
                      @"\[(?<group>(?:[^\[\]]+&)?LoliHouse)\](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s?(?<codeV>AVC|HEVC)(?:[-\s])?(?<rate>\d+-bit)?\s?(?<codeA>AAC(?:x2)?)?\]\[(?<lang>.+?)\]",
                      RegexOptions.IgnoreCase);
        parser =
            new(
                @"\[(?<group>(?:[^\[\]]+&)?LoliHouse)\](?<title>[^\[\]]+?)-?\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s?(?<codeV>AVC|HEVC)(?:[-\s])?(?<rate>\d+-bit)?",
                RegexOptions.IgnoreCase);
        var testStrList = new List<string>
        {
            "[LoliHouse] 明天，美食广场见。 / Food Court de, Mata Ashita. - 05 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕]"
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
