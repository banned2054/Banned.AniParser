using System.Globalization;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Utils;

internal class StringUtils
{
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
}