using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class PrejudiceStudioParser : BaseParser
{
    public override string        GroupName => "Prejudice-Studio";
    public override EnumGroupType GroupType => EnumGroupType.Transfer;

    public PrejudiceStudioParser()
    {
        LanguageMap["简繁英"] = EnumLanguage.EngScTc;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[Prejudice-Studio\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>Bilibili)?\s?(?<source>WEB-DL|WebRip)\s(?<resolution>\d+p)\s(?<codeV>AVC)\s(?<videoRate>\d+bit)\s(?<codeA>AAC)\s?(?<extension>MP4|MKV)?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[Prejudice-Studio\](?<title>[^\[\]]+?)\s?\[(?<start>\d+)-(?<end>\d+)\]\[(?<websource>Bilibili)\s(?<source>WEB-DL|WebRip)\s(?<resolution>\d+p)\s(?<codeV>AVC)\s(?<videoRate>\d+bit)\s(?<codeA>AAC)\s?(?<extension>MP4|MKV)?\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase)
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var webSource = match.Groups["websource"].Success ? match.Groups["websource"].Value : "Unknown";
        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            GroupType    = GroupType,
            WebSource    = webSource,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

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

        return new ParseResult
        {
            MediaType   = true,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = GroupName,
            GroupType    = GroupType,
            WebSource    = match.Groups["websource"].Value,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
