using System.Text.Json;
using Banned.AniParser.Models;

namespace Banned.Aniparser.AI;

internal class Program
{
    private static async Task Main()
    {
        var inputFilePath = "Data/data.json";
        var dataJson      = await File.ReadAllTextAsync(inputFilePath);
        var originTitles  = JsonSerializer.Deserialize<List<string>>(dataJson);
        if (originTitles is not { Count: > 0 }) return;
        var parser        = new AniParser.AniParser();
        var failedTitles  = new List<string>();
        var successTitles = new List<(string title, ParseResult result)>();
        foreach (var originTitle in originTitles)
        {
            var result = parser.Parse(originTitle);
            if (result == null)
            {
                failedTitles.Add(originTitle);
                continue;
            }

            successTitles.Add((originTitle, result));
        }
    }
}
