using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class ComicatParser : BaseParser
{
    public override string        GroupName => "漫猫字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public ComicatParser()
    {
        LanguageMap["繁中"]      = EnumLanguage.Tc;
        LanguageMap["BIG5"]    = EnumLanguage.Tc;
        LanguageMap["BIG5&JP"] = EnumLanguage.JpTc;
        LanguageMap["简中"]      = EnumLanguage.Sc;
        LanguageMap["GB"]      = EnumLanguage.Sc;
        LanguageMap["GB&JP"]   = EnumLanguage.JpSc;
        SingleEpisodePatterns =
        [
            new(@"\[漫猫字幕组\](?:\[\d+月新番\])?(?:\[(?<nameCn>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)\])?(?:\[(?<nameJp>[\u3040-\u309f\u30a0-\u30ff\u4e00-\u9fa5a-z0-9\s\p{P}]+)\])?\[(?<nameEn>[a-z0-9\s\p{P}]+)\]\[(?<episode>\d+(?:\.\d+)?)(?:v(?<version>\d+))?\]\[(?<resolution>\d+p)\]\[(?<extension>mp4|mkv)\](?:\[(?<langEng>.+?)\])?\[(?<lang>[\u4e00-\u9fa5]+)\]",
                RegexOptions.IgnoreCase),
            new(@"\[Comicat\]\[(?<nameEn>[a-z0-9\s\p{P}]+)\]\[(?<episode>\d+(?:\.\d+)?)(?:v(?<version>\d+))?\]\[(?<resolution>\d+p)\]\[(?<lang>.+?)\]\[(?<extension>mp4|mkv)\]",
                RegexOptions.IgnoreCase),
        ];
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0m;
        if (match.Groups["episode"].Success)
            episode = decimal.Parse(match.Groups["episode"].Value);

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var group = GroupName;

        var version = match.Groups["version"].Success
            ? int.TryParse(match.Groups["version"].Value, out _) ? int.Parse(match.Groups["version"].Value) : 1
            : 1;
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
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = title,
            Episode      = episode,
            Version      = version,
            Group        = group,
            GroupType    = this.GroupType,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
