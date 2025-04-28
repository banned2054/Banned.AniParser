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
        url = "https://mikanani.me/RSS/Search?searchstr=lolihouse";
        url = "https://mikanani.me/RSS/Search?searchstr=mingy";
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
            HttpClient   = httpClient,
            ThrowOnError = true // 可选，根据需要配置
        };
        var aniParser = new AniParser();
        var url       = "https://bangumi.moe/rss/latest";
        var reader    = new FeedReader(options);

        var items    = await reader.RetrieveFeedAsync(url);
        var testList = items.Select(item => item.Title!).ToList();
        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            PrintParserInfo(result, testStr);
        }
    }

    [Test]
    public void Test3()
    {
        var a = new Regex(
                          @"^\[樱桃花字幕组\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+[pP])[\)）]?\s?\[[a-zA-Z0-9]+\]",
                          RegexOptions.IgnoreCase);
        //var a = new Regex(
        //                  @"^\[樱桃花字幕组\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+[pP])[\)）]?\s?\[[a-zA-Z0-9]+\]",
        //                  RegexOptions.IgnoreCase);

        var aniParser = new AniParser();
        var testStr = new List<string>
        {
            "[LoliHouse] Kowloon Generic Romance / 九龙大众浪漫 - 04 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕] [303.83 MB]",
            "[LoliHouse] Kowloon Generic Romance - 04 [WebRip 1080p HEVC-10bit AAC SRTx2].mkv",
            "[BeanSub&LoliHouse] Kuroshitsuji - Midori no Majo-hen - 04 [WebRip 1080p HEVC-10bit AAC ASSx2].TC.ass"
        };
        foreach (var str in testStr)
        {
            var result = aniParser.Parse(str);
            PrintParserInfo(result, str);
        }
    }

    [Test]
    public void TestShowList()
    {
        var aniParser = new AniParser();
        var result    = aniParser.GetParserList();
        foreach (var group in result)
        {
            Console.WriteLine(group);
        }
    }
}