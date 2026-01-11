using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class SweetSubParser : BaseParser
{
    public override string        GroupName => "SweetSub";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    [GeneratedRegex(@"\[SweetSub]\[(?<title>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)]\[(?<engTitle>[a-z0-9\s\p{P}]+)]\[(?<episode>\d+)(?:v(?<version>\d+))?]\[(?<source>[a-z]+Rip)]\[(?<resolution>\d+p)]\[(?<vCodec>AVC|AV1)\s(?:(?<rate>\d+)bit)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    [GeneratedRegex(@"\[SweetSub](?<title>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)-\s(?<episode>\d+)(?:v(?<version>\d+))?\s?\[(?<source>[a-z]+Rip)]\[(?<resolution>\d+p)]\[(?<vCodec>AVC|AV1)\s(?:(?<rate>\d+)bit)]\[(?<lang>.+?)]",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern2();

    [GeneratedRegex(@"\[SweetSub]\[(?<title>[\u4e00-\u9fa5a-z0-9\s\p{P}]+)]\[(?<engTitle>[a-z0-9\s\p{P}]+)]\[(?<start>\d+)-(?<end>\d+)\s?(?<OAD>[a-z\u4e00-\u9fff]+)?]\[(?<source>[a-z]+Rip)]\[(?<resolution>\d+p)]\[(?<vCodec>AVC|AV1)\s(?:(?<rate>\d+)bit)]\[(?<lang>.+?)](\[(?:v(?<version1>\d+))])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern();

    public SweetSubParser()
    {
        SingleEpisodePatterns   = [SinglePattern1(), SinglePattern2()];
        MultipleEpisodePatterns = [MultiplePattern()];
        InitMap();
    }
}
