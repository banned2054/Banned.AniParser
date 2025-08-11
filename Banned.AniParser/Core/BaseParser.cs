using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core;

public abstract class BaseParser
{
    protected Dictionary<string, EnumLanguage> LanguageMap = new()
    {
        ["简繁日"]         = EnumLanguage.JpScTc,
        ["Chs&Cht&Jpn"] = EnumLanguage.JpScTc,
        ["简日"]          = EnumLanguage.JpSc,
        ["Chs&Jpn"]     = EnumLanguage.JpSc,
        ["JpSc"]        = EnumLanguage.JpSc,
        ["繁日"]          = EnumLanguage.JpTc,
        ["Cht&Jpn"]     = EnumLanguage.JpTc,
        ["JpTc"]        = EnumLanguage.JpTc,
        ["简繁"]          = EnumLanguage.ScTc,
        ["Chs&Cht"]     = EnumLanguage.ScTc,
        ["Cht&Chs"]     = EnumLanguage.ScTc,
        ["简体"]          = EnumLanguage.Sc,
        ["Chs"]         = EnumLanguage.Sc,
        ["繁体"]          = EnumLanguage.Tc,
        ["繁體"]          = EnumLanguage.Tc,
        ["Cht"]         = EnumLanguage.Tc,
        ["GB"]          = EnumLanguage.Sc,
        ["BIG5"]        = EnumLanguage.Sc,
    };

    protected Dictionary<string, EnumSubtitleType> SubtitleTypeMap = new()
    {
        ["内嵌"] = EnumSubtitleType.Embedded,
        ["內嵌"] = EnumSubtitleType.Embedded,
        ["內封"] = EnumSubtitleType.Embedded,
        ["内封"] = EnumSubtitleType.Muxed,
        ["外挂"] = EnumSubtitleType.External,
    };

    protected Dictionary<Regex, string> GroupNameMap = new()
    {
        { new Regex("(?:Sakurato|[樱桜]都字幕[組组])", RegexOptions.IgnoreCase), "桜都字幕组" },
        { new Regex(@"(?:喵萌Production|Nekomoe\skissaten)", RegexOptions.IgnoreCase), "喵萌奶茶屋" },
        { new Regex("STYHSub", RegexOptions.IgnoreCase), "霜庭云花" },
        { new Regex("(?:DMG|動漫國字幕組)", RegexOptions.IgnoreCase), "动漫国字幕组" },
        { new Regex("(?:动漫国&)", RegexOptions.IgnoreCase), "动漫国字幕组" },
        { new Regex("FLsnow", RegexOptions.IgnoreCase), "雪飘工作室" },
        { new Regex("Haruhana", RegexOptions.IgnoreCase), "拨雪寻春" },
        { new Regex("KitaujiSub", RegexOptions.IgnoreCase), "北宇治字幕组" },
        { new Regex("MingY&", RegexOptions.IgnoreCase), "MingYSub&" },
        { new Regex(@"Billion\sMeta\sLab", RegexOptions.IgnoreCase), "亿次研同好会" },
        { new Regex("(?:悠哈璃羽字幕组|UHA-Wing)", RegexOptions.IgnoreCase), "悠哈璃羽字幕社" },
        { new Regex("Comicat", RegexOptions.IgnoreCase), "漫猫字幕组" },
    };

    protected IReadOnlyList<(string Key, EnumLanguage Lang)>    LanguageMapSorted;
    protected IReadOnlyList<(string Key, EnumSubtitleType Sub)> SubtitleTypeMapSorted;
    protected IReadOnlyList<(Regex Key, string Value)>          GroupNameMapSorted;

    protected List<Regex> SingleEpisodePatterns   = [];
    protected List<Regex> MultipleEpisodePatterns = [];
    protected List<Regex> FilterList              = [];

    public abstract string        GroupName { get; }
    public abstract EnumGroupType GroupType { get; }

    protected BaseParser()
    {
        InitMap();
    }

    public virtual (bool Success, ParseResult? Info) TryMatch(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename)) return (false, null);
        filename = filename.Trim();

        // 1) 过滤
        if (FilterList.Any(f => f.IsMatch(filename)))
        {
            return (false, null);
        }

        // 2) 多集优先
        foreach (var result in from pattern in MultipleEpisodePatterns
                               select pattern.Match(filename)
                               into match
                               where match.Success
                               select CreateParsedResultMultiple(match))
        {
            if (!string.IsNullOrEmpty(result.Group))
                result.Group = ReplaceWithRegexList(result.Group, GroupNameMapSorted);

            return (true, result);
        }

        // 3) 单集
        foreach (var result in from pattern in SingleEpisodePatterns
                               select pattern.Match(filename)
                               into match
                               where match.Success
                               select CreateParsedResultSingle(match))
        {
            if (!string.IsNullOrEmpty(result.Group))
                result.Group = ReplaceWithRegexList(result.Group, GroupNameMapSorted);

            return (true, result);
        }

        return (false, null);
    }

    // 使用“已排序表（长键优先）”的替换
    private static string ReplaceWithRegexList(string text, IReadOnlyList<(Regex Key, string Value)> list)
    {
        var s = text;
        for (var i = 0; i < list.Count; i++)
            s = list[i].Key.Replace(s, list[i].Value);
        return s;
    }


    protected virtual ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);
        var title     = match.Groups["title"].Value.Trim();
        var mediaType = EnumMediaType.SingleEpisode;
        if (title.Contains("剧场版"))
        {
            mediaType = EnumMediaType.Movie;
        }

        return new ParseResult
        {
            MediaType    = mediaType,
            Title        = title,
            Episode      = ParseDecimalGroup(match, "episode"),
            Version      = ParseVersion(match),
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected virtual ParseResult CreateParsedResultMultiple(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = ParseIntGroup(match, "start"),
            EndEpisode   = ParseIntGroup(match, "end"),
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected virtual (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var s        = lang.AsSpan().Trim().ToString().ToLowerInvariant();
        var language = EnumLanguage.None;
        foreach (var (k, v) in LanguageMapSorted)
            if (s.Contains(k, StringComparison.Ordinal))
            {
                language = v;
                break;
            }

        var subtitleType = EnumSubtitleType.None;
        foreach (var (k, v) in SubtitleTypeMapSorted)
            if (s.Contains(k, StringComparison.Ordinal))
            {
                subtitleType = v;
                break;
            }

        return (language, subtitleType);
    }


    protected static int ParseIntGroup(Match m, string name, int @default = 0)
        => m.Groups[name].Success &&
           int.TryParse(m.Groups[name].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v)
            ? v
            : @default;

    protected static decimal? ParseDecimalGroup(Match m, string name)
        => m.Groups[name].Success &&
           decimal.TryParse(m.Groups[name].Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var v)
            ? v
            : null;

    protected static int ParseVersion(Match m)
        => ParseIntGroup(m, "version", 1);

    protected static string GetGroupOrDefault(Match m, string name, string fallback)
    {
        if (!m.Groups[name].Success) return fallback;
        var g = m.Groups[name].Value.Trim();
        return string.IsNullOrEmpty(g) ? fallback : g;
    }

    protected string GetGroupName(Match m)
    {
        var group = GroupName;
        if (m.Groups["group"].Success)
        {
            group = m.Groups["group"].Value.Trim();
            group = string.IsNullOrEmpty(group) ? GroupName : group;
        }

        return group;
    }

    protected void InitMap()
    {
        LanguageMapSorted = LanguageMap
                           .Select(kv => (kv.Key.ToLowerInvariant(), kv.Value))
                           .OrderByDescending(t => t.Item1.Length).ToList();

        SubtitleTypeMapSorted = SubtitleTypeMap
                               .Select(kv => (kv.Key.ToLowerInvariant(), kv.Value))
                               .OrderByDescending(t => t.Item1.Length).ToList();

        GroupNameMapSorted = GroupNameMap
                            .OrderByDescending(p => p.Key.ToString().Length)
                            .Select(p => (p.Key, p.Value)).ToList();
    }
}
