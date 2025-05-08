using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using SimpleFeedReader;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    private static void PrintParserInfo(ParserInfo? result, string testStr)
    {
        if (result == null)
        {
            Console.WriteLine($"Parser failed:\n\t{testStr}");
            return;
        }

        switch (result.GroupType)
        {
            case EnumGroupType.Translation :
            {
                if (result.IsMultiple)
                {
                    Console.WriteLine($"Origin title : {testStr}"                                                   +
                                      $"\n\tTitle         : {result.Title}"                                         +
                                      $"\n\tStart Episode : {result.StartEpisode} to Episode : {result.EndEpisode}" +
                                      $"\n\tLanguage      : {result.Language.ToString()}"                           +
                                      $"\n\tResolution    : {result.Resolution}"                                    +
                                      $"\n\tGroup         : {result.Group}");
                    return;
                }

                Console.WriteLine($"Origin title : {testStr}"                      +
                                  $"\n\tTitle      : {result.Title}"               +
                                  $"\n\tEpisode    : {result.Episode}"             +
                                  $"\n\tLanguage   : {result.Language.ToString()}" +
                                  $"\n\tResolution : {result.Resolution}"          +
                                  $"\n\tGroup      : {result.Group}");
                return;
            }
            case EnumGroupType.Transfer :
            {
                if (result.IsMultiple)
                {
                    Console.WriteLine($"Origin title : {testStr}"                                                   +
                                      $"\n\tTitle         : {result.Title}"                                         +
                                      $"\n\tStart Episode : {result.StartEpisode} to Episode : {result.EndEpisode}" +
                                      $"\n\tLanguage      : {result.Language.ToString()}"                           +
                                      $"\n\tWeb Source    : {result.WebSource}"                                     +
                                      $"\n\tResolution    : {result.Resolution}"                                    +
                                      $"\n\tGroup         : {result.Group}");
                    return;
                }

                Console.WriteLine($"Origin title : {testStr}"                      +
                                  $"\n\tTitle      : {result.Title}"               +
                                  $"\n\tEpisode    : {result.Episode}"             +
                                  $"\n\tLanguage   : {result.Language.ToString()}" +
                                  $"\n\tWeb Source : {result.WebSource}"           +
                                  $"\n\tResolution : {result.Resolution}"          +
                                  $"\n\tGroup      : {result.Group}");
                return;
            }
        }
    }

    [Test]
    public async Task Test1()
    {
        var aniParser = new AniParser();
        var url       = "https://mikanani.me/RSS/Search?searchstr=%E5%8C%97%E5%AE%87%E6%B2%BB";
        url = "https://mikanani.me/RSS/Search?searchstr=%E5%96%B5%E8%90%8CProduction";
        url =
            "https://mikanani.me/RSS/Search?searchstr=%E5%96%B5%E8%90%8C%E5%A5%B6%E8%8C%B6%E5%B1%8B%26%E5%8D%83%E5%A4%8F%E5%AD%97%E5%B9%95%E7%BB%84";
        url = url.Replace("mikanani.me", "mikanime.tv").Trim();
        var reader = new FeedReader();
        var items =
            await
                reader.RetrieveFeedAsync(url);
        var testList = items.Select(item => item.Title!).ToList();
        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            PrintParserInfo(result, testStr);
        }
    }

    [Test]
    public async Task Test2()
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
            "https://bangumi.moe/rss/latest";
        var reader = new FeedReader(options);

        var items            = await reader.RetrieveFeedAsync(url);
        var testList         = items.Select(item => item.Title!).ToList();
        var successTitleList = new List<string>();
        var failTitleList    = new List<string>();

        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            PrintParserInfo(result, testStr);
        }
    }

    [Test]
    public void Test3()
    {
        var aniParser = new AniParser();
        var testStr = new List<string>
        {
            "[喵萌Production&LoliHouse] 前桥魔女 / Maebashi Witches 02 [WebRip 1080p HEVC-10bit AAC ASSx2][简繁日内封字幕]",
        };
        var a = new Regex
            (
             @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)(?:-\s)*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]\[(?<lang>.+?)\]",
             RegexOptions.IgnoreCase);
        foreach (var str in testStr)
        {
            //var result = aniParser.Parse(str);
            var result = a.Match(str);
            if (result.Success)
            {
                foreach (Group group in result.Groups)
                {
                    Console.WriteLine($"{group.Name}:{group.Value}");
                }
            }
            //PrintParserInfo(result, str);
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