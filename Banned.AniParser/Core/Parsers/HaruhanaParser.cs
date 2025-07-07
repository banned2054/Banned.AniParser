using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class HaruhanaParser : BaseParser
{
    public override string GroupName => "拨雪寻春";

    public HaruhanaParser()
    {
        LanguageMap["CHI_JPN"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["CHI_JPN"] = EnumSubtitleType.Muxed;
        LanguageMap["CHT_JPN"]     = EnumLanguage.JpTc;
        SubtitleTypeMap["CHT_JPN"] = EnumSubtitleType.Embedded;
        LanguageMap["CHS_JPN"]     = EnumLanguage.JpSc;
        SubtitleTypeMap["CHS_JPN"] = EnumSubtitleType.Embedded;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[❀拨雪寻春❀\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?(?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<codec>HEVC-10bit)\s?(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[❀(?<group>拨雪寻春&[^\[\]+]+)❀\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?(\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<codec>HEVC-10bit)\s?(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[Haruhana\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?(?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<codec>HEVC-10bit)\s?(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>Haruhana&[^\[\]+]+)\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?(\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<codec>HEVC-10bit)\s?(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
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


        return new ParseResult
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
}
