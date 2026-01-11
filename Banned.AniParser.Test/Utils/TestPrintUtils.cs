using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text;

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

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"Origin title   : {testStr}");
        stringBuilder.AppendLine($"\tTitle           : #{result.Title}#");

        switch (result.MediaType)
        {
            case EnumMediaType.MultipleEpisode :
                stringBuilder.AppendLine($"\tStart Episode   : {result.StartEpisode} to Episode : {result.EndEpisode}");
                break;
            case EnumMediaType.SingleEpisode :
                stringBuilder.AppendLine($"\tEpisode         : {result.Episode}");
                break;
            case EnumMediaType.Movie :
                stringBuilder.AppendLine("\tType            : Movie");
                break;
            case EnumMediaType.Ova :
                stringBuilder.AppendLine("\tType            : OVA");
                if (result.Episode != null && result.Episode != 0)
                    stringBuilder.AppendLine($"\tEpisode         : {result.Episode}");
                break;
        }

        if (result.Version > 1)
        {
            stringBuilder.AppendLine($"\tVersion         : {result.Version}");
        }

        if (result.GroupType == EnumGroupType.Transfer)
        {
            stringBuilder.AppendLine($"\tWeb Source      : {result.WebSource}");
        }

        stringBuilder.AppendLine($"\tLanguage        : {result.Language.ToString()}");
        stringBuilder.AppendLine($"\tResolution      : {result.Resolution}");
        stringBuilder.AppendLine($"\tGroup           : {result.Group}");
        if (!string.IsNullOrWhiteSpace(result.VideoCodec))
        {
            stringBuilder.AppendLine($"\tVideo Codec     : {result.VideoCodec}");
        }

        if (!string.IsNullOrWhiteSpace(result.AudioCodec))
        {
            stringBuilder.AppendLine($"\tAudio Codec     : {result.AudioCodec}");
        }

        if (result.ColorBitDepth > 1)
        {
            stringBuilder.AppendLine($"\tColor Bit Depth : {result.ColorBitDepth}bit");
        }

        Console.WriteLine(stringBuilder.ToString());
    }
}
