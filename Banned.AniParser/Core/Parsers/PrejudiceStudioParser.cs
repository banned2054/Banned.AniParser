using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class PrejudiceStudioParser : BaseTransferParser
{
    public override string GroupName => "Prejudice-Studio";

    [GeneratedRegex(@"\[Prejudice-Studio](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<websource>Bilibili)?\s?(?<source>WEB-DL|WebRip)\s(?<resolution>\d+p)\s(?<codeV>AVC|HEVC|H264)\s(?<videoRate>\d+bit)?\s?(?<codeA>AAC)\s?(?<extension>MP4|MKV)?]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    [GeneratedRegex(@"\[Prejudice-Studio](?<title>[^\[\]]+?)\s?\[(?<start>\d+)-(?<end>\d+)](?:\[无水印])?\[(?<websource>Bilibili)\s(?<source>WEB-DL|WebRip)\s(?<resolution>\d+p)\s(?<codeV>AVC|HEVC|H264)\s(?<videoRate>\d+bit)\s(?<codeA>AAC)\s?(?<extension>MP4|MKV)?]\[(?<lang>.+?)](?:\[v(?<version>\d+)])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public PrejudiceStudioParser()
    {
        LanguageMap["简繁英"]      = EnumLanguage.EngScTc;
        SingleEpisodePatterns   = [SinglePattern()];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
