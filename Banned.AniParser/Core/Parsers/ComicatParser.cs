using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class ComicatParser : BaseParser
{
    public override string        GroupName => "漫猫字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?漫猫字幕组(?:&[^\[\]]+)?)](?:\[\d+月新番])?(?:\[(?<nameCn>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)])?(?:\[(?<nameJp>[\u3040-\u309f\u30a0-\u30ff\u4e00-\u9fa5a-z0-9\s\p{P}]+)])?\[(?<nameEn>[a-z0-9\s\p{P}]+)]\[(?<episode>\d+(?:\.\d+)?)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)]\[(?<extension>mp4|mkv)](?:\[(?<langEng>.+?)])?\[(?<lang>[\u4e00-\u9fa5]+)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?Comicat(?:&[^\[\]]+)?)]\[(?<nameEn>[a-z0-9\s\p{P}]+)]\[(?<episode>\d+(?:\.\d+)?)(?:v(?<version>\d+))?]\[(?<resolution>\d+p)]\[(?<lang>.+?)]\[(?<extension>mp4|mkv)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?漫猫字幕组(?:&[^\[\]]+)?)](?<title>.+?)\((?<start>\d+)-(?<end>\d+)(?:Fin)?\s(?<source>WebRip)\s(?<resolution>\d+p)\s(?<codeV>AVC)\s(?<codeA>AAC)\s(?<extension>mp4|mkv)\s?(?:\d+年\d+月)?\s(?<lang>[\u4e00-\u9fa5]+)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public ComicatParser()
    {
        LanguageMap["繁中"]       = EnumLanguage.Tc;
        LanguageMap["BIG5"]     = EnumLanguage.Tc;
        LanguageMap["BIG5&JP"]  = EnumLanguage.JpTc;
        LanguageMap["简中"]       = EnumLanguage.Sc;
        LanguageMap["GB"]       = EnumLanguage.Sc;
        LanguageMap["GB&JP"]    = EnumLanguage.JpSc;
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2()];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var titleList = new List<string>();
        if (match.Groups["nameCn"].Success)
            titleList.Add(match.Groups["nameCn"].Value.Trim());
        if (match.Groups["nameJp"].Success)
            titleList.Add(match.Groups["nameJp"].Value.Trim());
        if (match.Groups["nameEn"].Success)
            titleList.Add(match.Groups["nameEn"].Value.Trim());
        var title = string.Join(" / ", titleList);

        return new ParseResult
        {
            Title        = title,
            Episode      = ParseDecimalGroup(match, "episode"),
            Group        = GetGroupName(match),
            GroupType    = this.GroupType,
            Language     = lang,
            MediaType    = EnumMediaType.SingleEpisode,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            SubtitleType = subType,
            Version      = ParseVersion(match),
        };
    }
}
