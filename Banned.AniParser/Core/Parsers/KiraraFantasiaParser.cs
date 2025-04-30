using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class KiraraFantasiaParser : BaseParser
{
    public override string GroupName => "Kirara Fantasia";

    public KiraraFantasiaParser()
    {
        LanguageMap["B-Global"]             = EnumLanguage.English;
        SubtitleTypeMap["B-Global"]         = EnumSubtitleType.Muxed;
        LanguageMap["B-Global Donghua"]     = EnumLanguage.English;
        SubtitleTypeMap["B-Global Donghua"] = EnumSubtitleType.Muxed;
        LanguageMap["CR"]                   = EnumLanguage.English;
        SubtitleTypeMap["CR"]               = EnumSubtitleType.Muxed;
        LanguageMap["ABEMA"]                = EnumLanguage.None;
        SubtitleTypeMap["ABEMA"]            = EnumSubtitleType.None;
        LanguageMap["Baha"]                 = EnumLanguage.Tc;
        SubtitleTypeMap["Baha"]             = EnumSubtitleType.Embedded;
        //	[黒ネズミたち] 天命大主宰 / The Destiny Ruler - 35 (B-Global Donghua 1920x1080 HEVC AAC MKV)
        // [Up to 21°C] 噗妮露是可爱史莱姆 / Puniru wa Kawaii Slime - 12 (ABEMA 1920x1080 AVC AAC MP4) [708.89 MB] [复制磁连]
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[(黒ネズミたち|Up\sto\s21°C)\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?:((?<codec>(HEVC|AVC|AAC))\s?)+)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
        };
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var webSource = match.Groups["websource"].Value.Trim();

        var (lang, subType) = DetectLanguageSubtitle(webSource);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value.Replace("1920x1080", "1080p").Replace("3840x2160", "4K");
        }

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            Resolution   = resolution,
            WebSource    = webSource,
            Language     = lang,
            GroupType    = EnumGroupType.Transfer,
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