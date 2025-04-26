using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core;

public abstract class BaseParser
{
    protected Dictionary<string, EnumLanguage> LanguageMap = new()
    {
        ["简繁日"]         = EnumLanguage.JpScTc,
        ["chs&cht&jpn"] = EnumLanguage.JpScTc,
        ["简日"]          = EnumLanguage.JpSc,
        ["chs&jpn"]     = EnumLanguage.JpSc,
        ["jpsc"]        = EnumLanguage.JpSc,
        ["繁日"]          = EnumLanguage.JpTc,
        ["cht&jpn"]     = EnumLanguage.JpTc,
        ["jptc"]        = EnumLanguage.JpTc,
        ["简繁"]          = EnumLanguage.ScTc,
        ["chs&cht"]     = EnumLanguage.ScTc,
        ["cht&chs"]     = EnumLanguage.ScTc,
        ["简体"]          = EnumLanguage.Sc,
        ["chs"]         = EnumLanguage.Sc,
        ["繁体"]          = EnumLanguage.Tc,
        ["cht"]         = EnumLanguage.Tc,
    };

    protected Dictionary<string, EnumSubtitleType> SubtitleTypeMap = new()
    {
        ["内嵌"] = EnumSubtitleType.Embedded,
        ["內嵌"] = EnumSubtitleType.Embedded,
        ["內封"] = EnumSubtitleType.Embedded,
        ["内封"] = EnumSubtitleType.Muxed,
        ["外挂"] = EnumSubtitleType.External,
    };

    protected List<Regex> SingleEpisodePatterns   = new();
    protected List<Regex> MultipleEpisodePatterns = new();

    public abstract string GroupName { get; }

    public (bool Success, ParserInfo? Info) TryMatch(string filename)
    {
        foreach (var match in MultipleEpisodePatterns.Select(pattern => pattern.Match(filename))
                                                     .Where(match => match.Success))
        {
            return (true, CreateParsedResultMultiple(match));
        }

        foreach (var match in SingleEpisodePatterns.Select(pattern => pattern.Match(filename))
                                                   .Where(match => match.Success))
        {
            return (true, CreateParsedResultSingle(match));
        }

        return (false, null);
    }

    protected virtual ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            SourceGroup  = GroupName,
            Resolution   = match.Groups["resolution"].Value,
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected virtual ParserInfo CreateParsedResultMultiple(Match match)
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

        return new ParserInfo
        {
            IsMultiple   = true,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            SourceGroup  = GroupName,
            Resolution   = match.Groups["resolution"].Value,
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