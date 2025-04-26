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

    [Test]
    public async Task Test1()
    {
        var aniParser = new AniParser();
        var url       = "https://mikanani.me/RSS/Search?searchstr=%E5%8C%97%E5%AE%87%E6%B2%BB";
        url = url.Replace("mikanani.me", "mikanime.tv").Trim();
        var reader = new FeedReader();
        var items =
            await
                reader.RetrieveFeedAsync(url);
        var testList = items.Select(item => item.Title!).ToList();
        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            if (result == null)
            {
                Console.WriteLine($"Parser failed:\n\t{testStr}");
                continue;
            }

            if (result.IsMultiple)
            {
                Console.WriteLine($"Origin title : {testStr}"                                                   +
                                  $"\n\tTitle         : {result.Title}"                                         +
                                  $"\n\tStart Episode : {result.StartEpisode} to Episode : {result.EndEpisode}" +
                                  $"\n\tLanguage      : {result.Language.ToString()}"                           +
                                  $"\n\tResolution    : {result.Resolution}"                                    +
                                  $"\n\tGroup         : {result.SourceGroup}");
                continue;
            }

            Console.WriteLine($"Origin title : {testStr}"                      +
                              $"\n\tTitle      : {result.Title}"               +
                              $"\n\tEpisode    : {result.Episode}"             +
                              $"\n\tLanguage   : {result.Language.ToString()}" +
                              $"\n\tResolution : {result.Resolution}"          +
                              $"\n\tGroup      : {result.SourceGroup}");
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
        var url       = "https://www.miobt.com/rss-%E6%A8%B1%E6%A1%83%E8%8A%B1%E5%AD%97%E5%B9%95%E7%BB%84.xml";
        var reader    = new FeedReader(options);

        var items    = await reader.RetrieveFeedAsync(url);
        var testList = items.Select(item => item.Title!).ToList();
        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            if (result == null)
            {
                Console.WriteLine($"Parser failed:\n\t{testStr}");
                continue;
            }

            if (result.IsMultiple)
            {
                Console.WriteLine($"Origin title : {testStr}"                                                   +
                                  $"\n\tTitle         : {result.Title}"                                         +
                                  $"\n\tStart Episode : {result.StartEpisode} to Episode : {result.EndEpisode}" +
                                  $"\n\tLanguage      : {result.Language.ToString()}"                           +
                                  $"\n\tResolution    : {result.Resolution}"                                    +
                                  $"\n\tGroup         : {result.SourceGroup}");
                continue;
            }

            Console.WriteLine($"Origin title : {testStr}"                      +
                              $"\n\tTitle      : {result.Title}"               +
                              $"\n\tEpisode    : {result.Episode}"             +
                              $"\n\tLanguage   : {result.Language.ToString()}" +
                              $"\n\tResolution : {result.Resolution}"          +
                              $"\n\tGroup      : {result.SourceGroup}");
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
        var testStr   = " [樱桃花字幕组] Rock wa Lady no Tashinami deshite - 03（1080P） [5D1648CF].mp4(650.2MB)";
        testStr = "[樱桃花字幕组] Rock wa Lady no Tashinami deshite - 03（1080P） [5D1648CF].mp4(650.2MB)";
        var match = a.Match(testStr);
        if (match.Success)
        {
            Console.WriteLine("success");
            Console.WriteLine(match);
        }
    }
}