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

        SingleEpisodePatterns =
        [
            new(
                @"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)\](?<title>.+?)-\s?(?<episode>\d+)(?:v(?<version>\d+(?:\.\d+)?))?(\([a-z0-9]\))?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)\](?<title>.+?)-\s?(?<media_type>电影)\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)\](?<title>.+?)-\s?(?<media_type>OVA)(?<episode>\d+)?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
        ];
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var mediaType = EnumMediaType.SingleEpisode;
        if (match.Groups["media_type"].Success)
        {
            var mediaTypeStr = match.Groups["media_type"].Value;
            if (Regex.IsMatch(mediaTypeStr, "电影", RegexOptions.IgnoreCase))
            {
                mediaType = EnumMediaType.Movie;
            }
            else if (Regex.IsMatch(mediaTypeStr, "OVA", RegexOptions.IgnoreCase))
            {
                mediaType = EnumMediaType.Ova;
            }
        }

        var episode = 0m;
        if (match.Groups["episode"].Success)
            episode = decimal.Parse(match.Groups["episode"].Value);

        var webSource = match.Groups["websource"].Value.Trim();

        var (lang, subType) = DetectLanguageSubtitle(webSource);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value;
        }

        var version = match.Groups["version"].Success
            ? int.TryParse(match.Groups["version"].Value, out _) ? int.Parse(match.Groups["version"].Value) : 1
            : 1;

        return new ParseResult
        {
            MediaType    = mediaType,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Version      = version,
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
