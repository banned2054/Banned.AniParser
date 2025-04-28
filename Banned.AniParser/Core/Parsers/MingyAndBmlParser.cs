using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class MingyAndBmlParser : BaseParser
{
    public override string GroupName => "MingYSub&亿次研同好会";

    public MingyAndBmlParser()
    {
        LanguageMap["jpsc"]     = EnumLanguage.JpSc;
        SubtitleTypeMap["jpsc"] = EnumSubtitleType.Embedded;
        LanguageMap["jptc"]     = EnumLanguage.JpTc;
        SubtitleTypeMap["jptc"] = EnumSubtitleType.Embedded;
        LanguageMap["jpcn"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["jpcn"] = EnumSubtitleType.Muxed;
        //[MingY&Billion Meta Lab] mono女孩 / mono [02][1080p][简繁日内封] [387.26 MB]
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[MingY&Billion\sMeta\sLab\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY&Billion\sMeta\sLab\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY&Billion\sMeta\sLab\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[MingY&Billion\sMeta\sLab\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY&Billion\sMeta\sLab\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value.Trim();
        }

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            Resolution   = resolution,
            Language     = lang,
            SubtitleType = subType
        };
    }
}