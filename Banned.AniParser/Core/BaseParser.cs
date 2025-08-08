using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core;

public abstract class BaseParser
{
    protected List<Regex> FilterList = [];

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

    protected List<Regex> SingleEpisodePatterns   = [];
    protected List<Regex> MultipleEpisodePatterns = [];

    public abstract string        GroupName { get; }
    public abstract EnumGroupType GroupType { get; }

    public virtual (bool Success, ParseResult? Info) TryMatch(string filename)
    {
        filename = filename.Trim();
        if (string.IsNullOrEmpty(filename) || FilterList.Any(e => e.Match(filename).Success)) return (false, null);

        GroupNameMap = GroupNameMap.OrderByDescending(pair => pair.Key.ToString().Length)
                                   .ToDictionary(pair => pair.Key, pair => pair.Value);
        foreach (var match in MultipleEpisodePatterns.Select(pattern => pattern.Match(filename))
                                                     .Where(match => match.Success))
        {
            var result = CreateParsedResultMultiple(match);
            result.Group = StringUtils.ReplaceWithRegex(result.Group, GroupNameMap);
            return (true, result);
        }

        foreach (var match in SingleEpisodePatterns.Select(pattern => pattern.Match(filename))
                                                   .Where(match => match.Success))
        {
            var result = CreateParsedResultSingle(match);
            result.Group = StringUtils.ReplaceWithRegex(result.Group, GroupNameMap);
            return (true, result);
        }

        return (false, null);
    }

    protected virtual ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 1;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var title     = match.Groups["title"].Value.Trim();
        var mediaType = EnumMediaType.SingleEpisode;
        if (title.Contains("剧场版"))
        {
            mediaType = EnumMediaType.Movie;
        }

        var version = match.Groups["version"].Success
            ? int.TryParse(match.Groups["version"].Value, out _) ? int.Parse(match.Groups["version"].Value) : 1
            : 1;
        return new ParseResult
        {
            MediaType    = mediaType,
            Title        = title,
            Episode      = episode,
            Version      = version,
            Group        = GroupName,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected virtual ParseResult CreateParsedResultMultiple(Match match)
    {
        var startEpisode = 0;
        var endEpisode   = 0;
        if (match.Groups["start"].Success)
        {
            startEpisode = int.Parse(Regex.Replace(match.Groups["start"].Value.Trim(), @"\D+", ""));
        }

        if (match.Groups["end"].Success)
        {
            endEpisode = int.Parse(Regex.Replace(match.Groups["end"].Value.Trim(), @"\D+", ""));
        }

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = GroupName,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected virtual (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var lowerLang    = lang.ToLower().Trim();
        var language     = EnumLanguage.None;
        var subtitleType = EnumSubtitleType.None;
        foreach (var (k, v) in LanguageMap.OrderByDescending(kvp => kvp.Key.Length))
        {
            if (!lowerLang.Contains(k.ToLower())) continue;
            language = v;
            break;
        }

        foreach (var (k, v) in SubtitleTypeMap.OrderByDescending(kvp => kvp.Key.Length))
        {
            if (!lowerLang.Contains(k.ToLower())) continue;
            subtitleType = v;
            break;
        }

        return (language, subtitleType);
    }
}
