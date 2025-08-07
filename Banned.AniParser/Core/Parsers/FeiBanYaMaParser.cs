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

        SingleEpisodePatterns =
        [
            new(
                @"\[Feibanyama\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>CR|ABEMA|AMZN|IQIYI)\s(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s(?<codeV>HEVC-10bit)\s(?<codeA>AAC|OPUS|E-AC-3)\s(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[沸班亚马制作组\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>CR|ABEMA|AMZN|IQIYI)\s(?<source>[a-z]+Rip)\s(?:AI)?(?<resolution>\d+p)\s(?<codeV>HEVC-10bit)\s(?<codeA>AAC|OPUS|E-AC-3)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        ];
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(match.Groups["episode"].Value);

        var webSource = match.Groups["websource"].Value.Trim();

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var version = match.Groups["version"].Success
            ? int.TryParse(match.Groups["version"].Value, out _) ? int.Parse(match.Groups["version"].Value) : 1
            : 1;

        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Version      = version,
            Group        = GroupName,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            WebSource    = webSource,
            Language     = lang,
            GroupType    = this.GroupType,
            SubtitleType = subType
        };
    }
}
