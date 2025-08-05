using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

internal class LoliHouseParser : BaseParser
{
    public override string        GroupName => "LoliHouse";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public LoliHouseParser()
    {
        LanguageMap["无字幕"]     = EnumLanguage.None;
        LanguageMap["TC"]      = EnumLanguage.Tc;
        LanguageMap["SC"]      = EnumLanguage.Sc;
        LanguageMap["英语"]      = EnumLanguage.Eng;
        SubtitleTypeMap["无字幕"] = EnumSubtitleType.None;
        SingleEpisodePatterns =
        [
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)-\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)-\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]\.(?<lang>[^\.]*)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)-\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]\.(?<lang>[^\.]*)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\s*(?<episode>\d+)(?:v(?<version>\d+))?\s*[^\[\]]*\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(
                @"\[(?<group>(LoliHouse|[^\[\]]+&LoliHouse))\](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)\s*[^\[\]]*\]\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)[^\[\]]*\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        ];
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
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

        var version = match.Groups["version"].Success
            ? int.TryParse(match.Groups["version"].Value, out _) ? int.Parse(match.Groups["version"].Value) : 1
            : 1;

        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Version      = version,
            Group        = group,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
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

        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
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
