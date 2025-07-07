using System.Globalization;
using System.Text.RegularExpressions;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Utils;

internal class StringUtils
{
    private static readonly Regex ResolutionWithWidthAndHeight =
        new(@"(?<width>\D+)x(?<height>\d+)", RegexOptions.IgnoreCase);

    private static readonly Regex ResolutionWithP = new(@"(?<height>\d+)[pP]", RegexOptions.IgnoreCase);

    public static string ReplaceWithRegex(string input, Dictionary<Regex, string> regexReplacements)
    {
        // 遍历字典中的每个正则表达式和替换字符串
        return regexReplacements.Aggregate(input, (current, pair) => pair.Key.Replace(current, pair.Value));
    }

    public static string ConvertToTraditional(string text)
    {
        // 使用繁体中文文化信息
        var culture = new CultureInfo("zh-TW");
        return ConvertText(text, culture);
    }

    public static string ConvertToSimplified(string text)
    {
        // 使用简体中文文化信息
        var culture = new CultureInfo("zh-CN");
        return ConvertText(text, culture);
    }

    public static string ConvertText(string text, CultureInfo culture)
    {
        // 使用TextInfo进行转换
        var textInfo = culture.TextInfo;
        return textInfo.ToTitleCase(text);
    }

    public static EnumResolution ResolutionStr2Enum(string resolution)
    {
        var match = ResolutionWithWidthAndHeight.Match(resolution);
        if (match.Success)
        {
            var height = int.Parse(match.Groups["height"].Value);
            var width  = int.Parse(match.Groups["width"].Value);
            return height switch
            {
                >= 2160                    => EnumResolution.R4K,
                >= 1080 when width >= 2048 => EnumResolution.R2K,
                >= 1080                    => EnumResolution.R1080p,
                >= 720                     => EnumResolution.R720p,
                >= 480                     => EnumResolution.R480p,
                _                          => EnumResolution.Unknown
            };
        }

        match = ResolutionWithP.Match(resolution);
        if (match.Success)
        {
            var height = int.Parse(match.Groups["height"].Value);
            return height switch
            {
                < 600  => EnumResolution.R480p,
                < 900  => EnumResolution.R720p,
                < 1260 => EnumResolution.R1080p,
                < 1800 => EnumResolution.R2K,
                _      => EnumResolution.R4K,
            };
        }

        return EnumResolution.Unknown;
    }
}
