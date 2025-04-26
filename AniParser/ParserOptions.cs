using Banned.AniParser.Core;

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
    public List<BaseParser> CustomParsers { get; } = new();

    /// <summary>
    /// 是否启用文件名预处理（如去除特殊符号）
    /// </summary>
    public bool EnablePreprocessing { get; set; } = true;
}