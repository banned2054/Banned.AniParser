using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class KiraraFantasiaParser : BaseTransferParser
{
    public override string GroupName => "Kirara Fantasia";

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
                @"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)](?<title>.+?)-\s?(?<episode>\d+)(?:v(?<version>\d+(?:\.\d+)?))?(\([a-z0-9]\))?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)](?<title>.+?)-\s?(?<media_type>电影)\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
            new(
                @"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)](?<title>.+?)-\s?(?<media_type>OVA)(?<episode>\d+)?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s?(?<extension>(MP4|MKV))\)",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
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

        var webSource = match.Groups["websource"].Value;

        var (lang, subType) = DetectLanguageSubtitle(webSource);

        return new ParseResult
        {
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = ParseDecimalGroup(match, "episode"),
            Group        = this.GroupName,
            GroupType    = this.GroupType,
            Language     = lang,
            MediaType    = mediaType,
            Resolution   = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
            SubtitleType = subType,
            Version      = ParseVersion(match),
            WebSource    = webSource,
        };
    }
}
