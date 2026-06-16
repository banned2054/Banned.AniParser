using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class TsdmSubParser : BaseParser
{
    public override string        GroupName => "TSDM字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:TSDM字幕组|TSDM)(?:&[^\[\]]+)?)[】\]]\s*\[(?<title>[^\[\]]+)](?:\[[^\[\]\d][^\[\]]*])?\[(?<episode>\d+(?:\.\d+)?)(?:v(?<version>\d+))?]\[(?<source>WebRip|WEB-DL|BDRip|TVRip|DVDRip)]\[(?<vCodec>HEVC|AVC|H264|H265|x264|x265)-?(?:(?<rate>\d+)bit)?\s*(?<resolution>\d+p)\s*(?<aCodec>AAC|FLAC|EAC3)?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:TSDM字幕组|TSDM)(?:&[^\[\]]+)?)[】\]]\s*\[(?<title>[^\[\]]+)](?:\[[^\[\]\d][^\[\]]*])?\[(?<episode>\d+(?:\.\d+)?)(?:v(?<version>\d+))?]\[(?<vCodec>HEVC|AVC|H264|H265|x264|x265)-?(?:(?<rate>\d+)bit)?\s*(?<resolution>\d+p)\s*(?<aCodec>AAC|FLAC|EAC3)?]\[(?<extension>MP4|MKV)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"[\[【](?<group>(?:[^\[\]]+&)?(?:TSDM字幕组|TSDM)(?:&[^\[\]]+)?)[】\]]\s*\[(?<title>[^\[\]]+)](?:\[[^\[\]\d][^\[\]]*])?\[(?<episode>\d+(?:\.\d+)?)(?:v(?<version>\d+))?]\[(?:GB|BIG5)]\[(?<resolution>\d+p)]\[(?<extension>MP4|MKV)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern3();

    public TsdmSubParser()
    {
        LanguageMap["CHS_JP&CHT_JP"] = EnumLanguage.JpScTc;
        LanguageMap["CHT_JP&CHS_JP"] = EnumLanguage.JpScTc;
        LanguageMap["CHS_JP"]        = EnumLanguage.JpSc;
        LanguageMap["CHT_JP"]        = EnumLanguage.JpTc;

        SubtitleTypeMap["CHS_JP&CHT_JP"] = EnumSubtitleType.Muxed;
        SubtitleTypeMap["CHT_JP&CHS_JP"] = EnumSubtitleType.Muxed;
        SubtitleTypeMap["CHS_JP"]        = EnumSubtitleType.Embedded;
        SubtitleTypeMap["CHT_JP"]        = EnumSubtitleType.Embedded;

        SingleEpisodePatterns = [SinglePattern1(), SinglePattern2(), SinglePattern3()];
        InitMap();
    }
}
