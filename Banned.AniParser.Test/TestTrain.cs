using Banned.AniParser.Core.Parsers;
using Banned.AniParser.Test.Models;
using Banned.AniParser.Test.Utils;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Test;

internal class TestTrain
{
    [Test]
    public async Task MergeData()
    {
        var dirPath      = "result";
        var filePathList = Directory.GetFiles(dirPath);
        var data         = new TrainTitle();
        foreach (var filePath in filePathList)
        {
            var nowStr = await File.ReadAllTextAsync(filePath);
            var now    = JsonConvert.DeserializeObject<TrainTitle>(nowStr);
            if (now == null)
            {
                Console.WriteLine($"Error, file path: {filePath}");
                continue;
            }

            data.TitleList.AddRange(now.TitleList);
            data.UrlList.AddRange(now.UrlList);
        }

        data.TitleList = data.TitleList.Distinct().ToList();
        data.UrlList   = data.UrlList.Distinct().ToList();
        Console.WriteLine(data.TitleList.Count);
        Console.WriteLine(data.UrlList.Count);
        var file = $"result/{Guid.NewGuid().ToString()}.json";
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(data));
        Console.WriteLine(file);
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
            var len = data.TitleList.Count;
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
            var total = frequencyList.Sum(kv => kv.Value);
            Console.WriteLine($"Total {len}, failed {total}, success rate={(1 - total * 1m / len) * 100}%");
            foreach (var pair in frequencyList)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }
    }

    [Test]
    public async Task ParserOneTrainData()
    {
        var parser = new SakuraHanaParser();

        var file    = "result/61019a31-316e-4ef9-9199-05e191846f2f.json";
        var dataStr = await File.ReadAllTextAsync(file);
        var data    = JsonConvert.DeserializeObject<TrainTitle>(dataStr);
        if (data == null) return;
        var frequencyList = data.TitleList
                                .Where(e => e.StartsWith("[樱桃花字幕组]"))
                                .Select(e => (result : parser.TryMatch(e), title : e))
                                .Select(e => (result : e.result.Info, e.title))
                                .ToList();
        foreach (var tuple in frequencyList)
        {
            TestPrintUtils.PrintParserInfo(tuple.result, tuple.title);
        }
    }

    [Test]
    public async Task GetTrainData()
    {
        var random = new Random();
        var url    = "https://mikanani.me/RSS/Classic";
        var data   = new TrainTitle();
        for (var i = 1; i < 20; i++)
        {
            var rssString = await TestNetUtils.Fetch($"{url}/{i}");
            var testList  = TestRssUtils.GetTitlesAndTorrentUrls(rssString);
            if (testList.Count == 0) break;
            data.TitleList.AddRange(testList.Select(e => e.Title));
            data.UrlList.AddRange(testList.Select(e => e.TorrentUrl));

            await Task.Delay(TimeSpan.FromSeconds(random.Next(100)));
        }

        data.UrlList   = data.UrlList.Distinct().ToList();
        data.TitleList = data.TitleList.Distinct().ToList();

        Console.WriteLine(data.TitleList.Count);
        if (!Directory.Exists("result"))
        {
            Directory.CreateDirectory("result");
        }

        var filePath = $"result/{Guid.NewGuid().ToString()}.json";
        await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(data));
        Console.WriteLine(filePath);
    }
}
