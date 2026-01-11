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
        ["BIG5"]        = EnumLanguage.Tc,
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
        { new("(?:Sakurato|[樱桜]都字幕[組组])", RegexOptions.IgnoreCase), "桜都字幕组" },
        { new(@"(?:喵萌Production|Nekomoe\skissaten)", RegexOptions.IgnoreCase), "喵萌奶茶屋" },
        { new("STYHSub", RegexOptions.IgnoreCase), "霜庭云花" },
        { new("(?:DMG|動漫國字幕組)", RegexOptions.IgnoreCase), "动漫国字幕组" },
        { new("(?:动漫国&)", RegexOptions.IgnoreCase), "动漫国字幕组" },
        { new("FLsnow", RegexOptions.IgnoreCase), "雪飘工作室" },
        { new("Haruhana", RegexOptions.IgnoreCase), "拨雪寻春" },
        { new("KitaujiSub", RegexOptions.IgnoreCase), "北宇治字幕组" },
        { new("MingY&", RegexOptions.IgnoreCase), "MingYSub&" },
        { new(@"Billion\sMeta\sLab", RegexOptions.IgnoreCase), "亿次研同好会" },
        { new("(?:悠哈璃羽字幕[社组]|UHA-Wing(?:S)?)", RegexOptions.IgnoreCase), "悠哈璃羽字幕社" },
        { new("Comicat", RegexOptions.IgnoreCase), "漫猫字幕组" },
        { new("BeanSub", RegexOptions.IgnoreCase), "豌豆字幕组" },
    };

    protected IReadOnlyList<(string Key, EnumLanguage Lang)>    LanguageMapSorted     = [];
    protected IReadOnlyList<(string Key, EnumSubtitleType Sub)> SubtitleTypeMapSorted = [];
    protected IReadOnlyList<(Regex Key, string Value)>          GroupNameMapSorted    = [];

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
            result.OriginTitle = filename;
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
            result.OriginTitle = filename;
            return (true, result);
        }

        return (false, null);
    }

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

        return new ParseResult
        {
            Title         = GetGroupOrDefault(match, "title", string.Empty),
            Episode       = ParseDecimalGroup(match, "episode"),
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Language      = lang,
            MediaType     = ParseSingleMediaType(match),
            Resolution    = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
            SubtitleType  = subType,
            Version       = ParseVersion(match),
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
        };
    }

    protected virtual ParseResult CreateParsedResultMultiple(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            Title         = GetGroupOrDefault(match, "title", string.Empty),
            StartEpisode  = ParseIntGroup(match, "start"),
            EndEpisode    = ParseIntGroup(match, "end"),
            Group         = GetGroupName(match),
            GroupType     = this.GroupType,
            Language      = lang,
            MediaType     = EnumMediaType.MultipleEpisode,
            Resolution    = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
            SubtitleType  = subType,
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = ParseColorBitDepth(match)
        };
    }

    protected virtual (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var s        = lang.AsSpan().Trim().ToString().ToLowerInvariant();
        var language = EnumLanguage.None;
        foreach (var (k, v) in LanguageMapSorted)
        {
            if (!s.Contains(k, StringComparison.Ordinal)) continue;
            language = v;
            break;
        }

        var subtitleType = EnumSubtitleType.None;
        foreach (var (k, v) in SubtitleTypeMapSorted)
        {
            if (!s.Contains(k, StringComparison.Ordinal)) continue;
            subtitleType = v;
            break;
        }

        return (language, subtitleType);
    }

    protected virtual int ParseColorBitDepth(Match match) => int.Parse(GetGroupOrDefault(match, "rate", "-1"));

    protected virtual string ParseVideoCodec(Match match) => GetGroupOrDefault(match, "vCodec", string.Empty).ToUpper()
       .Replace("X264", "AVC")
       .Replace("X.264", "AVC")
       .Replace("X265", "HEVC")
       .Replace("X.265", "HEVC");

    protected virtual string ParseAudioCodec(Match match) => GetGroupOrDefault(match, "aCodec", string.Empty).ToUpper();

    protected virtual EnumMediaType ParseSingleMediaType(Match match)
    {
        var mt = GetGroupOrDefault(match, "mediaType", string.Empty).ToLower();
        if (mt.Contains("movie") || mt.Contains("剧场版"))
        {
            return EnumMediaType.Movie;
        }

        if (mt.Contains("ova") || mt.Contains("oad"))
        {
            return EnumMediaType.Ova;
        }

        var title = GetGroupOrDefault(match, "Title", string.Empty);
        if (title.Contains("剧场版"))
        {
            return EnumMediaType.Movie;
        }

        return EnumMediaType.SingleEpisode;
    }

    protected virtual string GetGroupName(Match match)
    {
        var group = GroupName;
        if (!match.Groups["group"].Success) return group;
        group = match.Groups["group"].Value.Trim();
        group = string.IsNullOrEmpty(group) ? GroupName : group;
        return group;
    }

    protected virtual int ParseVersion(Match match) => ParseIntGroup(match, "version", 1);

    protected static int ParseIntGroup(Match match, string name, int @default = 0)
        => match.Groups[name].Success &&
           int.TryParse(match.Groups[name].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v)
            ? v
            : @default;

    protected static decimal? ParseDecimalGroup(Match match, string name)
    {
        if (!match.Groups[name].Success)
        {
            return null;
        }

        var rawValue        = match.Groups[name].Value;
        var normalizedValue = rawValue.Normalize(System.Text.NormalizationForm.FormKC).Trim();

        if (decimal.TryParse(normalizedValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v))
        {
            return v;
        }

        return null;
    }

    protected static string GetGroupOrDefault(Match match, string name, string fallback)
    {
        if (!match.Groups[name].Success) return fallback;
        var g = match.Groups[name].Value.Trim();
        return string.IsNullOrEmpty(g) ? fallback : g;
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
