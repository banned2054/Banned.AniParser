using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class OrionOriginParser : BaseTransferParser
{
    public override string GroupName => "猎户发布组";

    [GeneratedRegex(@"\[orion\sorigin](?<title>.+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\s?\[(?<resolution>\d+p)]\s\[(?<vCodec>H265)\s(?<aCodec>AAC)]\s\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();
    [GeneratedRegex(@"\[猎户(?:手抄|压制)部](?<title>.+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\s?\[(?<resolution>\d+p)]\s?\[(?<lang>.+?)](?:\s?\[\d{4}年\d{1,2}月番])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    public OrionOriginParser()
    {
        LanguageMap["CHS_JPN"] = EnumLanguage.JpSc;
        LanguageMap["CHT_JPN"] = EnumLanguage.JpTc;
        SingleEpisodePatterns  = [SinglePattern1(), SinglePattern2()];
        InitMap();
    }
}
