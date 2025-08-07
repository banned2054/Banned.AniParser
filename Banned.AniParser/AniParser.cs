using Banned.AniParser.Core;
using Banned.AniParser.Core.Parsers;
using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;

namespace Banned.AniParser;

public class AniParser
{
    private readonly List<BaseParser> _parsers = new();
    private readonly ParserOptions    _options;

    // 构造函数允许注入配置
    public AniParser(Action<ParserOptions>? configure = null)
    {
        _options = new ParserOptions();
        configure?.Invoke(_options);

        InitializeDefaultParsers();
    }

    /// <summary>
    /// 获取当前有的所有字幕组、压制组以及搬运组的列表（字典顺序）
    /// </summary>
    public List<string> GetParserList()
    {
        var result = _parsers
                    .Select(parser => parser.GroupName)
                    .OrderBy(name => name) // 按字典顺序排序
                    .ToList();
        return result;
    }


    /// <summary>
    /// 获取当前有的所有字幕组的列表（字典顺序）
    /// </summary>
    public List<string> GetTranslationParserList()
    {
        var result = _parsers
                    .Where(parser => parser.GroupType == EnumGroupType.Translation)
                    .Select(parser => parser.GroupName)
                    .OrderBy(name => name) // 按字典顺序排序
                    .ToList();
        return result;
    }


    /// <summary>
    /// 获取当前有的所有搬运组的列表（字典顺序）
    /// </summary>
    public List<string> GetTransferParserList()
    {
        var result = _parsers
                    .Where(parser => parser.GroupType == EnumGroupType.Transfer)
                    .Select(parser => parser.GroupName)
                    .OrderBy(name => name) // 按字典顺序排序
                    .ToList();
        return result;
    }

    /// <summary>
    /// 获取当前有的所有压制组的列表（字典顺序）
    /// </summary>
    public List<string> GetCompressionParserList()
    {
        var result = _parsers
                    .Where(parser => parser.GroupType == EnumGroupType.Compression)
                    .Select(parser => parser.GroupName)
                    .OrderBy(name => name) // 按字典顺序排序
                    .ToList();
        return result;
    }


    private void InitializeDefaultParsers()
    {
        if (_options.UseDefaultParsers)
        {
            _parsers.Add(new AnkRawParser());
            _parsers.Add(new AniRawParser());
            _parsers.Add(new BillionMetaLabParser());
            _parsers.Add(new ComicatParser());
            _parsers.Add(new DmgParser());
            _parsers.Add(new FeiBanYaMaParser());
            _parsers.Add(new FlSnowParser());
            _parsers.Add(new HaruhanaParser());
            _parsers.Add(new JsumParser());
            _parsers.Add(new KiraraFantasiaParser());
            _parsers.Add(new KitaujiSubParser());
            _parsers.Add(new LoliHouseParser());
            _parsers.Add(new MingYSubParser());
            _parsers.Add(new Moozzi2Parser());
            _parsers.Add(new NekoMoeParser());
            _parsers.Add(new PrejudiceStudioParser());
            _parsers.Add(new SakuraHanaParser());
            _parsers.Add(new SakuratoParser());
            _parsers.Add(new StyhSubParser());
            _parsers.Add(new SweetSubParser());
            _parsers.Add(new UhaWingParser());
            _parsers.Add(new VcbStudioParser());
        }

        _parsers.AddRange(_options.CustomParsers);
    }

    /// <summary>
    /// 核心解析方法
    /// </summary>
    public ParseResult? Parse(string filename)
    {
        foreach (var parser in _parsers)
        {
            var (success, result) = parser.TryMatch(filename);
            if (!success || result == null) continue;
            switch (_options.Globalization)
            {
                case EnumChineseGlobalization.Simplified :
                {
                    result.Title = StringUtils.ConvertToSimplified(result.Title);
                    break;
                }
                case EnumChineseGlobalization.Traditional :
                {
                    result.Title = StringUtils.ConvertToTraditional(result.Title);
                    break;
                }
                case EnumChineseGlobalization.NotChange :
                    break;
                default :
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        return null;
    }

    /// <summary>
    /// 批量解析
    /// </summary>
    public IEnumerable<ParseResult> ParseBatch(IEnumerable<string> fileNames)
    {
        return fileNames.Select(Parse).Where(result => result != null)!;
    }
}
