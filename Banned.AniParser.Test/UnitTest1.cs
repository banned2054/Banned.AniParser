using System.Diagnostics;
using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using SimpleFeedReader;
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
        url = "https://mikanani.me/RSS/Search?searchstr=%E9%9C%9C%E5%BA%AD%E4%BA%91%E8%8A%B1&subgroupid=570&page=1";
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
        var url       = "https://bangumi.moe/rss/latest";
        var reader    = new FeedReader(options);

        var items            = await reader.RetrieveFeedAsync(url);
        var testList         = items.Select(item => item.Title!).ToList();
        var successTitleList = new List<string>();
        var failTitleList    = new List<string>();

        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            PrintParserInfo(result, testStr);
            //if (result == null) failTitleList.Add(testStr);
            //else successTitleList.Add(testStr);
        }

        //Console.WriteLine("Success:");
        //foreach (var title in successTitleList)
        //{
        //    Console.WriteLine($"\t{title}");
        //}
        //Console.WriteLine("Fail:");
        //foreach (var title in failTitleList)
        //{
        //    Console.WriteLine($"\t{title}");
        //}
    }

    [Test]
    public void Test3()
    {
        var a = new Regex(
                          @"\[黒ネズミたち\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>((HEVC|AVC|AAC)\s?)+)\s?(?<extension>[a-zA-Z\s]+)\)",
                          RegexOptions.IgnoreCase);
        //
        var aniParser = new AniParser();
        var testStr = new List<string>
        {
            "[Sakurato] Summer Pockets [04][AVC-8bit 1080p AAC][CHS].mp4",
        };
        foreach (var str in testStr)
        {
            var result = aniParser.Parse(str);
            PrintParserInfo(result, str);
            //var result = a.Match(str);
            //if (result.Success)
            //{
            //    Console.WriteLine($"\n\tTitle      : {result.Groups["title"]}"      +
            //                      $"\n\tEpisode    : {result.Groups["episode"]}"    +
            //                      $"\n\tWeb Source : {result.Groups["websource"]}"  +
            //                      $"\n\tResolution : {result.Groups["resolution"]}" +
            //                      $"\n\tCodex      : {result.Groups["codec"]}");
            //}
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