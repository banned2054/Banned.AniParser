using SimpleFeedReader;

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
}