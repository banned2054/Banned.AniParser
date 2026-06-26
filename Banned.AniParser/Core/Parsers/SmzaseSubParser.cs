using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class SmzaseSubParser : BaseParser
{
    public override string        GroupName => "三明治摆烂组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:三明治摆烂组|smzase)(?:&[^\[\]]+)?)[】\]]\s*(?<title>.+?)\s*-\s*(?<episode>\d+(?:\.\d+)?)\s*-\s*\[(?<lang>.+?)]\[(?<vCodec>HEVC|AVC|H264|H265|x264|x265)[-\s]*(?:(?<rate>\d+)bit\s*)?(?<resolution>\d+p)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?smzase(?:&[^\[\]]+)?)\]\s*(?<title>.+?)\s*-\s*S(?<season>\d+)E(?<episode>\d+(?:\.\d+)?)\.(?<lang>zh-hans|zh-hant)\.ass$",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[(?<group>(?:[^\[\]]+&)?smzase(?:&[^\[\]]+)?)\]\s*(?<title>.+?)\s*-\s*S(?<season>\d+)E(?<episode>\d+(?:\.\d+)?)\s*-\s*\[(?<lang>CHS_JPN|CHT_JPN|CHI_JPN)]\[(?<source>WebRip|WEB-DL|BDRip|TVRip|DVDRip)\s+(?<vCodec>H264|H265|AVC|HEVC|x264|x265)\s+(?<rate>\d+)bit\s+(?<resolution>\d+p)]\.(?<extension>mp4|mkv)$",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    public SmzaseSubParser()
    {
        LanguageMap["zh-hans"] = EnumLanguage.Sc;
        LanguageMap["zh-hant"] = EnumLanguage.Tc;
        LanguageMap["CHS_JPN"] = EnumLanguage.JpSc;
        LanguageMap["CHT_JPN"] = EnumLanguage.JpTc;
        LanguageMap["CHI_JPN"] = EnumLanguage.JpScTc;

        SubtitleTypeMap["zh-hans"] = EnumSubtitleType.External;
        SubtitleTypeMap["zh-hant"] = EnumSubtitleType.External;
        SubtitleTypeMap["CHS_JPN"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHT_JPN"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHI_JPN"] = EnumSubtitleType.Muxed;

        SingleEpisodePatterns = [SinglePattern1(), SinglePattern2(), SinglePattern3()];
        InitMap();
    }

    protected override string ParseVideoCodec(Match match) => base.ParseVideoCodec(match).Replace("H264", "AVC");
}
