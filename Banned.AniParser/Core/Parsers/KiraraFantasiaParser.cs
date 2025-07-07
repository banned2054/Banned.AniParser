using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class KiraraFantasiaParser : BaseParser
{
    public override string        GroupName => "Kirara Fantasia";
    public override EnumGroupType GroupType => EnumGroupType.Transfer;

    public KiraraFantasiaParser()
    {
        LanguageMap["B-Global"]             = EnumLanguage.Eng;
        SubtitleTypeMap["B-Global"]         = EnumSubtitleType.Muxed;
        LanguageMap["B-Global Donghua"]     = EnumLanguage.Eng;
        SubtitleTypeMap["B-Global Donghua"] = EnumSubtitleType.Muxed;
        LanguageMap["CR"]                   = EnumLanguage.Eng;
        SubtitleTypeMap["CR"]               = EnumSubtitleType.Muxed;
        LanguageMap["ABEMA"]                = EnumLanguage.None;
        SubtitleTypeMap["ABEMA"]            = EnumSubtitleType.None;
        LanguageMap["Baha"]                 = EnumLanguage.Tc;
        SubtitleTypeMap["Baha"]             = EnumSubtitleType.Embedded;

        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?(\([a-z-A-Z0-9]\))?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var webSource = match.Groups["websource"].Value.Trim();

        var (lang, subType) = DetectLanguageSubtitle(webSource);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value;
        }

        return new ParseResult
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            Resolution   = StringUtils.ResolutionStr2Enum(resolution),
            WebSource    = webSource,
            Language     = lang,
            GroupType    = GroupType,
            SubtitleType = subType
        };
    }

    protected override (EnumLanguage Language, EnumSubtitleType SubtitleType) DetectLanguageSubtitle(string lang)
    {
        var lowerLang    = lang.ToLower().Trim();
        var language     = EnumLanguage.None;
        var subtitleType = EnumSubtitleType.None;
        foreach (var (k, v) in LanguageMap.OrderByDescending(kvp => kvp.Key.Length))
        {
            if (!lowerLang.Contains(k.ToLower())) continue;
            language = v;
            break;
        }

        foreach (var (k, v) in SubtitleTypeMap.OrderByDescending(kvp => kvp.Key.Length))
        {
            if (!lowerLang.Contains(k.ToLower())) continue;
            subtitleType = v;
            break;
        }

        return (language, subtitleType);
    }
}
