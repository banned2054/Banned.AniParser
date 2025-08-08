using Banned.AniParser.Core.Parsers;
using Banned.AniParser.Test.Models;
using Banned.AniParser.Test.Utils;
using Newtonsoft.Json;
using SimpleFeedReader;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

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
            "https://mikanani.me/RSS/Search?searchstr=%E6%A8%B1%E6%A1%83%E8%8A%B1%E5%AD%97%E5%B9%95%E7%BB%84";
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
            "[Comicat][Shiunji-ke no Kodomotachi][12][720P][GB][MP4]"
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

    [Test]
    public async Task GetTrainData()
    {
        var random = new Random();
        var url    = "https://mikanani.me/RSS/Classic";
        url = "https://mikanani.me/RSS/Search?searchstr=%5Bsweetsub%5D";
        var data = new TrainTitle();
        for (var i = 1; i < 20; i++)
        {
            var rssString = await TestNetUtils.Fetch($"{url}/{i}");
            rssString = await TestNetUtils.Fetch($"{url}&page={i}");
            var testList = TestRssUtils.GetAllTitle(rssString);
            if (testList.Count == 0) break;
            data.TitleList.AddRange(testList);
            break;

            await Task.Delay(TimeSpan.FromSeconds(random.Next(100)));
        }

        Console.WriteLine(data.TitleList.Count);
        if (!Directory.Exists("result"))
        {
            Directory.CreateDirectory("result");
        }

        var filePath = $"result/{Guid.NewGuid().ToString()}.json";
        await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(data));
        Console.WriteLine(filePath);
    }

    [Test]
    public async Task CheckTrainData()
    {
        var fileList = Directory.GetFiles("result");
        foreach (var file in fileList)
        {
            var dataStr = await File.ReadAllTextAsync(file);
            var data    = JsonConvert.DeserializeObject<TrainTitle>(dataStr);
            if (data == null) continue;

            Console.WriteLine(data.TitleList.Count);
            data.TitleList = data.TitleList.Distinct().Order().ToList();
            Console.WriteLine(data.TitleList.Count);
            await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(data));
        }
    }

    [Test]
    public async Task ParserTrainData()
    {
        var parser = new AniParser();
        var regex = new Regex(
                              @"^[【\[](?<group>[^\[\]]+?)[\]】]",
                              RegexOptions.IgnoreCase);

        var fileList = Directory.GetFiles("result");
        foreach (var file in fileList)
        {
            var dataStr = await File.ReadAllTextAsync(file);
            var data    = JsonConvert.DeserializeObject<TrainTitle>(dataStr);
            if (data == null) continue;

            // 创建并启动 Stopwatch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var frequencyList = data.TitleList
                                    .Select(e => (result : parser.Parse(e), title : e))
                                    .Where(e => e.result == null)
                                    .Where(e => regex.IsMatch(e.title))
                                    .Select(e => regex.Match(e.title).Groups["group"].Value.Trim())
                                    .GroupBy(s => s)
                                    .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
                                    .OrderByDescending(e => e.Value)
                                    .ToList(); // 停止计时并获取运行时间
            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;
            // 输出运行时间
            Console.WriteLine($"函数运行时间：{elapsed.TotalMilliseconds} 毫秒");
            foreach (var pair in frequencyList)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }
    }
}
