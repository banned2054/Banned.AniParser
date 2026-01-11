using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class KiraraFantasiaParser : BaseTransferParser
{
    public override string GroupName => "Kirara Fantasia";

    [GeneratedRegex(@"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)](?<title>.+?)-\s?(?<episode>\d+)(?:v(?<version>\d+(?:\.\d+)?))?(\([a-z0-9]\))?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<vCodec>HEVC|AVC)\s?(?<aCodec>HEVC|AAC|AVC)*\s?(?<extension>(MP4|MKV))\)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)](?<title>.+?)-\s?(?<mediaType>电影)\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<vCodec>HEVC|AVC)\s?(?<aCodec>HEVC|AAC|AVC)*\s?(?<extension>(MP4|MKV))\)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(黒ネズミたち|Up\sto\s21°C|Dynamis\sOne)](?<title>.+?)-\s?(?<mediaType>OVA)(?<episode>\d+)?\s?\((?<websource>(B-Global(\sDonghua)?|CR|ABEMA|Baha))\s?(?<resolution>\d+x\d+)\s?(?<vCodec>HEVC|AVC)\s?(?<aCodec>HEVC|AAC|AVC)*\s?(?<extension>(MP4|MKV))\)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

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

        SingleEpisodePatterns = [SinglePattern1(), SinglePattern2(), SinglePattern3(),];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var webSource = match.Groups["websource"].Value;

        var (lang, subType) = DetectLanguageSubtitle(webSource);

        return new ParseResult
        {
            Title        = GetGroupOrDefault(match, "title", string.Empty),
            Episode      = ParseDecimalGroup(match, "episode"),
            Group        = this.GroupName,
            GroupType    = this.GroupType,
            Language     = lang,
            MediaType    = ParseSingleMediaType(match),
            Resolution   = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
            SubtitleType = subType,
            Version      = ParseVersion(match),
            WebSource    = webSource,
            VideoCodec   = ParseVideoCodec(match),
            AudioCodec   = ParseAudioCodec(match)
        };
    }
}
