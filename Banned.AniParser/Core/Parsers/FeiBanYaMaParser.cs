using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class FeiBanYaMaParser : BaseParser
{
    public override string        GroupName => "沸班亚马制作组";
    public override EnumGroupType GroupType => EnumGroupType.Transfer;

    public FeiBanYaMaParser()
    {
        LanguageMap["Multi-Subs"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["Multi-Subs"] = EnumSubtitleType.Muxed;

        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[Feibanyama\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>CR|ABEMA|AMZN|IQIYI)\s(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])\s(?<codeV>HEVC-10bit)\s(?<codeA>AAC|OPUS|E-AC-3)\s(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[沸班亚马制作组\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>CR|ABEMA|AMZN|IQIYI)\s(?<source>[a-zA-Z]+[Rr]ip)\s(?:AI)?(?<resolution>\d+[pP])\s(?<codeV>HEVC-10bit)\s(?<codeA>AAC|OPUS|E-AC-3)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var webSource = match.Groups["websource"].Value.Trim();

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value.Trim());


        return new ParseResult
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            WebSource    = webSource,
            Language     = lang,
            GroupType    = GroupType,
            SubtitleType = subType
        };
    }
}
