using SimpleFeedReader;
using System.Diagnostics;
using System.Net;

namespace Banned.AniParser.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test2()
    {
        var aniParser = new AniParser();
        var url =
            "https://bangumi.moe/rss/tags/5596b174a0b788232ee352cb";
        var rssString = await TestNetUtils.Fetch(url);

        var testList = TestRssUtils.GetAllTitle(rssString);

        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            TestPrintUtils.PrintParserInfo(result, testStr);
        }
    }

    [Test]
    public void Test3()
    {
        var aniParser = new AniParser();
        var testStr = new List<string>
        {
            "[VCB-Studio] Sidonia no Kishi [11(Director's Cut Ver.)][Hi10p_1080p][x264_flac].mkv",
            "[VCB-Studio] Non Non Biyori [13(OAD)][Ma10p_576p][x265_flac].mkv",
            "[VCB-Studio] High School Fleet [14(OVA02)][Ma10p_1080p][x265_flac_aac].mkv",
            "[VCB-Studio] CHAOS;CHILD [00][Ma10p_1080p][x265_flac_aac].mkv",
            "[VCB-Studio] Sangatsu no Lion [11.5][Ma10p_1080p][x265_flac].mkv",
            "[VCB-Studio&AI-Raws] slamdunk [099][Ma10p_1080p][x265_flac].mkv",
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
        var result    = aniParser.GetParserList();
        foreach (var groupName in result)
        {
            Console.WriteLine($"- {groupName}");
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
        var url       = "https://bangumi.moe/rss/latest";
        var reader    = new FeedReader(options);

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