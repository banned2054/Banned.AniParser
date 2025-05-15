using Banned.AniParser.Core;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser;

public class ParserOptions
{
    /// <summary>
    /// 是否启用内置解析器（默认 true）
    /// </summary>
    public bool UseDefaultParsers { get; set; } = true;

    /// <summary>
    /// 自定义解析器列表
    /// </summary>
    public List<BaseParser> CustomParsers { get; set; } = new();

    /// <summary>
    /// 是否全都强制转换成繁体/简体中文,仅对标题
    /// </summary>
    public EnumChineseGlobalization Globalization { get; set; } = EnumChineseGlobalization.NotChange;
}