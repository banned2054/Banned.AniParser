using System.Text.RegularExpressions;

namespace Banned.AniParser.Utils;

internal class StringUtils
{
    public static string ReplaceWithRegex(string input, Dictionary<Regex, string> regexReplacements)
    {
        // 遍历字典中的每个正则表达式和替换字符串
        return regexReplacements.Aggregate(input, (current, pair) => pair.Key.Replace(current, pair.Value));
    }
}