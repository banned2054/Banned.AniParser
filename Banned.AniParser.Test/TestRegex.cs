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
                Regex(@"\[(?<group>(?:[^\[\]]+&)?Nekomoe kissaten(?:&[^\[\]]+)?)]\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                      RegexOptions.IgnoreCase);
        parser =
            new
                Regex(@"\[(?<group>(?:[^\[\]]+&)?Nekomoe kissaten(?:&[^\[\]]+)?)]\[(?<title>[^\[\]]+?)]\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>[a-z]+Rip)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                      RegexOptions.IgnoreCase);
        var testStrList = new List<string>
        {
            "[Nekomoe kissaten][Watashi wo Tabetai, Hitodenashi][11][1080p][JPSC].mp4",
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
        var dirPath   = @"D:\Downloads";
        var fileList  = Directory.GetFiles(dirPath).Where(e => e.EndsWith(".torrent"));
        foreach (var file in fileList)
        {
            Console.WriteLine(file);
            var torrent =
                await Torrent.LoadAsync(file);

            if (torrent.Files.Count > 0)
            {
                foreach (var fileI in torrent.Files)
                {
                    var result = aniParser.Parse(fileI.Path);
                    TestPrintUtils.PrintParserInfo(result, fileI.Path);
                }
            }
            else
            {
                var result = aniParser.Parse(torrent.Name);
                TestPrintUtils.PrintParserInfo(result, torrent.Name);
            }
        }
    }
}
