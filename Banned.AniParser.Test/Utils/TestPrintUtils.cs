using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Test.Utils;

internal class TestPrintUtils
{
    public static void PrintParserInfo(ParseResult? result, string testStr)
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
                switch (result.MediaType)
                {
                    case EnumMediaType.MultipleEpisode :
                        Console.WriteLine($"Origin title : {testStr}" +
                                          $"\n\tTitle         : #{result.Title}#" +
                                          $"\n\tStart Episode : {result.StartEpisode} to Episode : {result.EndEpisode}" +
                                          $"\n\tLanguage      : {result.Language.ToString()}" +
                                          $"\n\tResolution    : {result.Resolution}" +
                                          $"\n\tGroup         : {result.Group}");
                        return;
                    case EnumMediaType.SingleEpisode :
                        Console.WriteLine($"Origin title : {testStr}"                      +
                                          $"\n\tTitle      : #{result.Title}#"             +
                                          $"\n\tEpisode    : {result.Episode}"             +
                                          $"\n\tLanguage   : {result.Language.ToString()}" +
                                          $"\n\tResolution : {result.Resolution}"          +
                                          $"\n\tGroup      : {result.Group}");
                        return;
                    case EnumMediaType.Movie :
                        Console.WriteLine($"Origin title : {testStr}"                      +
                                          $"\n\tType       : #Movie#"                      +
                                          $"\n\tTitle      : #{result.Title}#"             +
                                          $"\n\tLanguage   : {result.Language.ToString()}" +
                                          $"\n\tWeb Source : {result.WebSource}"           +
                                          $"\n\tResolution : {result.Resolution}"          +
                                          $"\n\tGroup      : {result.Group}");
                        return;
                }

                return;
            }
            case EnumGroupType.Transfer :
            {
                switch (result.MediaType)
                {
                    case EnumMediaType.MultipleEpisode :
                        Console.WriteLine($"Origin title : {testStr}" +
                                          $"\n\tTitle         : #{result.Title}#" +
                                          $"\n\tStart Episode : {result.StartEpisode} to Episode : {result.EndEpisode}" +
                                          $"\n\tLanguage      : {result.Language.ToString()}" +
                                          $"\n\tWeb Source    : {result.WebSource}" +
                                          $"\n\tResolution    : {result.Resolution}" +
                                          $"\n\tGroup         : {result.Group}");
                        return;
                    case EnumMediaType.SingleEpisode :
                        Console.WriteLine($"Origin title : {testStr}"                      +
                                          $"\n\tTitle      : #{result.Title}#"             +
                                          $"\n\tEpisode    : {result.Episode}"             +
                                          $"\n\tLanguage   : {result.Language.ToString()}" +
                                          $"\n\tWeb Source : {result.WebSource}"           +
                                          $"\n\tResolution : {result.Resolution}"          +
                                          $"\n\tGroup      : {result.Group}");
                        return;
                    case EnumMediaType.Movie :
                        Console.WriteLine($"Origin title : {testStr}"                      +
                                          $"\n\tType       : #Movie#"                      +
                                          $"\n\tTitle      : #{result.Title}#"             +
                                          $"\n\tLanguage   : {result.Language.ToString()}" +
                                          $"\n\tWeb Source : {result.WebSource}"           +
                                          $"\n\tResolution : {result.Resolution}"          +
                                          $"\n\tGroup      : {result.Group}");
                        return;
                }

                return;
            }
            case EnumGroupType.Compression :
            {
                switch (result.MediaType)
                {
                    case EnumMediaType.MultipleEpisode :
                        Console.WriteLine($"Origin title : {testStr}" +
                                          $"\n\tTitle         : #{result.Title}#" +
                                          $"\n\tStart Episode : {result.StartEpisode} to Episode : {result.EndEpisode}" +
                                          $"\n\tLanguage      : {result.Language.ToString()}" +
                                          $"\n\tResolution    : {result.Resolution}" +
                                          $"\n\tGroup         : {result.Group}");
                        return;
                    case EnumMediaType.SingleEpisode :
                        Console.WriteLine($"Origin title : {testStr}"                      +
                                          $"\n\tTitle      : #{result.Title}#"             +
                                          $"\n\tEpisode    : {result.Episode}"             +
                                          $"\n\tLanguage   : {result.Language.ToString()}" +
                                          $"\n\tResolution : {result.Resolution}"          +
                                          $"\n\tGroup      : {result.Group}");
                        return;
                    case EnumMediaType.Movie :
                        Console.WriteLine($"Origin title : {testStr}"                      +
                                          $"\n\tType       : #Movie#"                      +
                                          $"\n\tTitle      : #{result.Title}#"             +
                                          $"\n\tLanguage   : {result.Language.ToString()}" +
                                          $"\n\tWeb Source : {result.WebSource}"           +
                                          $"\n\tResolution : {result.Resolution}"          +
                                          $"\n\tGroup      : {result.Group}");
                        return;
                }

                return;
            }
        }
    }
}
