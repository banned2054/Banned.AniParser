using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class PrejudiceStudioParser : BaseTransferParser
{
    public override string GroupName => "Prejudice-Studio";

    [GeneratedRegex(@"\[Prejudice-Studio](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>Bilibili)?\s?(?<source>WEB-DL|WebRip)\s(?<resolution>\d+p)\s(?<vCodec>AVC|HEVC|H264)\s(?:(?<rate>\d+)bit)?\s?(?<aCodec>AAC)\s?(?<extension>MP4|MKV)?]\[(?<lang>.+?)](?:\[v(?<version>\d+)])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[Prejudice-Studio](?<title>[^\[\]]+?)-\s?(?<mediaType>Movie|OVA)(?<episode>\d+)?(?:v(?<version>\d+))?\s?\[(?<websource>Bilibili)?\s?(?<source>WEB-DL|WebRip)\s(?<resolution>\d+p)\s(?<vCodec>AVC|HEVC|H264)\s(?:(?<rate>\d+)bit)?\s?(?<aCodec>AAC)\s?(?<extension>MP4|MKV)?]\[(?<lang>.+?)](?:\[v(?<version>\d+)])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[Prejudice-Studio](?<title>[^\[\]]+?)\s?\[(?<start>\d+)-(?<end>\d+)](?:\[无水印])?\[(?<websource>Bilibili)\s(?<source>WEB-DL|WebRip)\s(?<resolution>\d+p)\s(?<vCodec>AVC|HEVC|H264)\s(?:(?<rate>\d+)bit)\s(?<aCodec>AAC)\s?(?<extension>MP4|MKV)?]\[(?<lang>.+?)](?:\[v(?<version>\d+)])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public PrejudiceStudioParser()
    {
        LanguageMap["简繁英"]      = EnumLanguage.EngScTc;
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2()];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
