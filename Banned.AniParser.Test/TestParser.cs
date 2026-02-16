using Banned.AniParser.Test.Utils;
using System.Diagnostics;

namespace Banned.AniParser.Test;

public class TestParser
{
    [Test]
    public async Task TestParserWithUrl()
    {
        var aniParser = new AniParser();
        var url =
            "https://mikanani.me/RSS/Search?searchstr=%E7%8C%8E%E6%88%B7%E5%8E%8B%E5%88%B6%E9%83%A8";
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
            "[Prejudice-Studio] 我们不可能成为恋人！绝对不行。〜再次闪耀！〜 Watanare 〜Next Sunshine〜 - OVA05 [WebRip 1080P AVC 8bit AAC MP4][繁体内嵌][V2] [409.7 MB] [复制磁连]"
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

        var testList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(dataStr);

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
