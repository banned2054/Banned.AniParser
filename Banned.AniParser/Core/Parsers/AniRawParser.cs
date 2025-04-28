using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class AniRawParser : BaseParser
{
    public override string GroupName => "ANi";

    public AniRawParser()
    {
        LanguageMap["CHT CHS"]     = EnumLanguage.ScTc;
        SubtitleTypeMap["CHT CHS"] = EnumSubtitleType.Muxed;
        SubtitleTypeMap["CHT"]     = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHS"]     = EnumSubtitleType.Embedded;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[ANi\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<resolution>\d+[pP])\]\[(?<websource>[^\[\]]+)\]\[[^\[\]]+\]\[[^\[\]]+\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        //[ANi] Neko ni Tensei Shita Ojisan / 转生成猫咪的大叔 - 29 [1080P][Baha][WEB-DL][AAC AVC][CHT][MP4]
        MultipleEpisodePatterns = new List<Regex>()
        {
        };
    }

    protected override (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var lowerLang    = lang.ToLower().Trim();
        var language     = EnumLanguage.None;
        var subtitleType = EnumSubtitleType.Embedded;
        foreach (var (k, v) in LanguageMap.OrderByDescending(kvp => kvp.Key.Length))
        {
            if (!lowerLang.Contains(k.ToLower())) continue;
            language = v;
            break;
        }

        return (language, subtitleType);
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
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
            Group        = GroupName,
            GroupType    = EnumGroupType.Transfer,
            WebSource    = match.Groups["websource"].Value,
            Resolution   = match.Groups["resolution"].Value,
            Language     = lang,
            SubtitleType = subType
        };
    }
}