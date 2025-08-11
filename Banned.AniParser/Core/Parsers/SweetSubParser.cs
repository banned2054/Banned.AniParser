using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class SweetSubParser : BaseParser
{
    public override string        GroupName => "SweetSub";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public SweetSubParser()
    {
        SingleEpisodePatterns =
        [
            new(
                @"\[SweetSub]\[(?<title>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)]\[(?<engTitle>[a-z0-9\s\p{P}]+)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<source>[a-z]+Rip)]\[(?<resolution>\d+p)]\[(?<codeV>AVC|AV1)\s(?<rate>\d+bit)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
            new(
                @"\[SweetSub](?<title>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)-\s(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<source>[a-z]+Rip)]\[(?<resolution>\d+p)]\[(?<codeV>AVC|AV1)\s(?<rate>\d+bit)]\[(?<lang>.+?)]",
                RegexOptions.IgnoreCase),
        ];
        MultipleEpisodePatterns =
        [
            new(
                @"\[SweetSub]\[(?<title>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)]\[(?<engTitle>[a-z0-9\s\p{P}]+)]\[(?<start>\d+)-(?<end>\d+)\s?(?<OAD>[a-z\u4e00-\u9fff]+)?]\[(?<source>[a-z]+Rip)]\[(?<resolution>\d+p)]\[(?<codeV>AVC|AV1)\s(?<rate>\d+bit)]\[(?<lang>.+?)](\[(?:v(?<version1>\d+))])?",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }
}
