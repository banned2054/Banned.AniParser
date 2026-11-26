using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class StudioGreenTeaParser : BaseParser
{
    public override string        GroupName => "绿茶字幕组";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[绿茶字幕组](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?](?:\[(?<source>WebRip)])?(?:\[(?<vCodec>HEVC)_?(?<aCodec>AAC)])?\[(?<resolution>\d+p)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[Studio\sGreenTea](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<source>WebRip)]\[(?<vCodec>HEVC)-?(?:(?<rate>\d+)bit)?\s?(?<resolution>\d+p)\s?(?<aCodec>AAC)(:?]\[|\s)(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    // [01-10][WebRip][1080p][简繁日内封] / [01-12 修正合集 v3][WebRip][HEVC_AAC][1080p][简繁日内封]
    [GeneratedRegex(@"\[(?:绿茶字幕组|Studio\sGreenTea)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)[^\]]*]\s*(?:\[(?<source>[a-zA-Z]+Rip)])?(?:\[(?<vCodec>HEVC|AVC)_(?<aCodec>AAC|FLAC|EAC3)])?(?:\[(?<resolution>\d+p)])?\[(?<lang>[^\[\]]+)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern1();

    // [01-10][WebRip][HEVC-10bit 1080p AAC ASSx2]（字幕信息在同一括号内）
    [GeneratedRegex(@"\[(?:绿茶字幕组|Studio\sGreenTea)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)[^\]]*]\s*\[(?<source>[a-zA-Z]+Rip)]\[(?<vCodec>HEVC|AVC)-?(?:(?<rate>\d+)bit)?\s?(?<resolution>\d+p)\s?(?<aCodec>AAC|FLAC|EAC3)(?:\s+(?<lang>[^\[\]]+?))?]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern2();

    // [01-13 修正合集][BDRip][HEVC-10bit 1080p FLAC ASSx2][简繁日内封]（字幕信息单独成括号）
    [GeneratedRegex(@"\[(?:绿茶字幕组|Studio\sGreenTea)](?<title>[^\[\]]+?)\[(?<start>\d+)-(?<end>\d+)[^\]]*]\s*\[(?<source>[a-zA-Z]+Rip)]\[(?<vCodec>HEVC|AVC)-?(?:(?<rate>\d+)bit)?\s?(?<resolution>\d+p)\s?(?<aCodec>AAC|FLAC|EAC3)[^\[\]]*]\[(?<lang>[^\[\]]+)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern3();

    public StudioGreenTeaParser()
    {
        LanguageMap["ASSx2"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["ASSx2"] = EnumSubtitleType.Muxed;
        SingleEpisodePatterns    = [SinglePattern1(), SinglePattern2()];
        MultipleEpisodePatterns  = [MultiplePattern3(), MultiplePattern2(), MultiplePattern1()];
        InitMap();
    }
}
