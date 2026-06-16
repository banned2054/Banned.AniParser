using Banned.AniParser.Models.Enums;
using Banned.AniParser.Test.Utils;
using System.Diagnostics;
using System.Text.Json;

namespace Banned.AniParser.Test;

public class TestParser
{
    [Test]
    public async Task TestParserWithUrl()
    {
        var aniParser = new AniParser();
        var url       = "https://mikanani.me/RSS/Classic";
        var rssString = await TestNetUtils.Fetch(url);

        var testList = TestRssUtils.GetAllTitle(rssString);

        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            TestPrintUtils.PrintParserInfo(result, testStr);
        }
    }

    [Test]
    public void TestParserWithStringList()
    {
        var testStr = new List<string>
        {
            "[OguriClub&S1YURICON] Umamusume Cinderella Gray[23][1080p][WebRip][HEVC_EAC3][CHS_JP].mkv"
        };
        var aniParser = new AniParser();
        foreach (var str in testStr)
        {
            var result = aniParser.Parse(str);
            TestPrintUtils.PrintParserInfo(result, str);
        }
    }

    [Test]
    public void TestParserUsesMatchedSource()
    {
        var aniParser = new AniParser();

        var translationResult = aniParser.Parse("[KitaujiSub] Test Title[01][BDRip][HEVC_AAC][CHS].mkv");
        Assert.That(translationResult, Is.Not.Null);
        Assert.That(translationResult!.Source, Is.EqualTo(EnumSource.BDRip));

        var transferResult =
            aniParser.Parse("[Prejudice-Studio] Test Title - 01 [Bilibili WEB-DL 1080p AVC 10bit AAC MP4][CHS].mkv");
        Assert.That(transferResult, Is.Not.Null);
        Assert.That(transferResult!.Source, Is.EqualTo(EnumSource.WEB_DL));
        Assert.That(transferResult.WebSource, Is.EqualTo("Bilibili"));
    }

    [Test]
    public void TestSourceStringNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(JsonSerializer.Serialize(EnumSource.WEB_DL), Is.EqualTo("\"WEB-DL\""));
            Assert.That(JsonSerializer.Deserialize<EnumSource>("\"WEB-DL\""), Is.EqualTo(EnumSource.WEB_DL));
            Assert.That(JsonSerializer.Serialize(EnumSource.WEBRip), Is.EqualTo("\"WEBRip\""));
            Assert.That(JsonSerializer.Serialize(EnumSource.BDRip), Is.EqualTo("\"BDRip\""));
            Assert.That(JsonSerializer.Serialize(EnumSource.TVRip), Is.EqualTo("\"TVRip\""));
            Assert.That(JsonSerializer.Serialize(EnumSource.DVDRip), Is.EqualTo("\"DVDRip\""));
        });
    }

    [Test]
    public void TestGetAllParserName()
    {
        var aniParser = new AniParser();

        var translationParserList = aniParser.GetTranslationParserList();
        Console.WriteLine("翻译组");
        foreach (var translationParser in translationParserList)
        {
            Console.WriteLine($"- {translationParser}");
        }

        var transferParserList = aniParser.GetTransferParserList();
        Console.WriteLine("搬运组");
        foreach (var transferParser in transferParserList)
        {
            Console.WriteLine($"- {transferParser}");
        }

        var compressionParserList = aniParser.GetCompressionParserList();
        Console.WriteLine("压制组");
        foreach (var compressionParser in compressionParserList)
        {
            Console.WriteLine($"- {compressionParser}");
        }
    }

    [Test]
    public async Task TestRunTime()
    {
        var aniParser = new AniParser();

        var dataStr = await File.ReadAllTextAsync("Data/data.json");

        var testList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(dataStr) ?? [];

        // 创建并启动 Stopwatch
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var results = aniParser.ParseBatch(testList);

        // 停止计时并获取运行时间
        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;

        // 输出运行时间
        Console.WriteLine($"函数运行时间：{elapsed.TotalMilliseconds} 毫秒");

        Console.WriteLine($"测试样例数量:{testList.Count}\n匹配结果:{results.Count()}");
    }
}
