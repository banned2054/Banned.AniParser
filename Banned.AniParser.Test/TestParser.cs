using Banned.AniParser.Test.Utils;
using SimpleFeedReader;
using System.Diagnostics;
using System.Net;

namespace Banned.AniParser.Test;

public class TestParser
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestParserWithUrl()
    {
        var aniParser = new AniParser();
        var url =
            "https://bangumi.moe/rss/tags/575446452165b9ba0c485d13";
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
        var aniParser = new AniParser();
        var testStr = new List<string>
        {
            "【喵萌奶茶屋】★04月新番★[Silent Witch 沉默魔女的秘密 / サイレント・ウィッチ 沈黙の魔女の隠しごと / Silent Witch - Chinmoku no Majo no Kakushigoto][01][1080p][简日双语] [复制磁连]"
        };
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
        var handler = new HttpClientHandler
        {
            Proxy    = new WebProxy("http://127.0.0.1:7890"),
            UseProxy = true
        };
        var httpClient = new HttpClient(handler);
        var options = new FeedReaderOptions
        {
            HttpClient = httpClient,
        };
        var aniParser = new AniParser();
        var url =
            "https://mikanani.me/RSS/Search?searchstr=%5B%E6%B2%B8%E7%8F%AD%E4%BA%9A%E9%A9%AC%E5%88%B6%E4%BD%9C%E7%BB%84%5D&subgroupid=1231&page=1";
        var reader = new FeedReader(options);

        var items    = await reader.RetrieveFeedAsync(url);
        var testList = items.Select(item => item.Title!).ToList();

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
