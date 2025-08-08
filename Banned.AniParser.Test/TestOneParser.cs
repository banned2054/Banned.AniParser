using Banned.AniParser.Core.Parsers;
using Banned.AniParser.Test.Models;
using Banned.AniParser.Test.Utils;
using Newtonsoft.Json;

namespace Banned.AniParser.Test;

public class TestOneParser
{
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
}
