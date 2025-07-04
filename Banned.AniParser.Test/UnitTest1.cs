﻿using Banned.AniParser.Test.Utils;
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
            "[Moozzi2] Qualidea Code - 01 (BD 1920x1080 x.264 FLACx3).mkv",
            "[Moozzi2] Adachi to Shimamura - 12 END (BD 1920x1080 x265-10Bit Flac).mkv",
            "[Moozzi2] Adachi to Shimamura - 01 (BD 1920x1080 x265-10Bit Flac).mkv",
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