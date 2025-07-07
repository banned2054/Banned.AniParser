using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

internal class LoliHouseParser : BaseParser
{
    public override string GroupName => "LoliHouse";

    public LoliHouseParser()
    {
        LanguageMap["无字幕"]     = EnumLanguage.None;
        LanguageMap["TC"]      = EnumLanguage.Tc;
        LanguageMap["SC"]      = EnumLanguage.Sc;
        LanguageMap["英语"]      = EnumLanguage.English;
        SubtitleTypeMap["无字幕"] = EnumSubtitleType.None;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)-\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)-\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]\.(?<lang>[^\.]*)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)-\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]\.(?<lang>[^\.]*)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?\s*[^\[\]]*\]\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);
        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value.Trim();
        }

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = group,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParserInfo CreateParsedResultMultiple(Match match)
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
        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value.Trim();
        }

        return new ParserInfo
        {
            IsMultiple   = true,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = group,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
