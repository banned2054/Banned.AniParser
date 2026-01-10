using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class OrionOriginParser : BaseTransferParser
{
    public override string GroupName => "猎户发布组";

    [GeneratedRegex(@"\[猎户(?:手抄|压制)部](?<title>.+?)\[(?<episode>\d+)(?:v(?<version>\d+))?]\s?\[(?<resolution>\d+p)]\s?\[(?<lang>.+?)](?:\s?\[\d{4}年\d{1,2}月番])?",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    public OrionOriginParser()
    {
        SingleEpisodePatterns = [SinglePattern()];
        InitMap();
    }
}
