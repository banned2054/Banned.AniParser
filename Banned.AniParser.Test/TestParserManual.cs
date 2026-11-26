using Banned.AniParser.Test.Utils;
using System.Diagnostics;
using System.Text.Json;

namespace Banned.AniParser.Test;

/// <summary>
/// 手动探索性测试：依赖外网 RSS、本地数据文件或仅打印解析结果，没有断言。
/// 默认不参与 dotnet test / CI；需要时显式运行：
/// dotnet test --filter "TestCategory=Manual"
/// </summary>
[Category("Manual")]
[Explicit("手动探索性测试，需显式指定才能运行")]
public class TestParserManual
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
