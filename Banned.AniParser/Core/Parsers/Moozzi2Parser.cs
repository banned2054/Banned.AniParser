using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class Moozzi2Parser : BaseParser
{
    public override string        GroupName => "Moozzi2";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    public Moozzi2Parser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(@"\[Moozzi2\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s(?:END|SP)?\s?\(BD\s(?<resolution>\d+x\d+)\s(?<codeV>(?:x\.?265|x\.?264)(?:-10Bit|-8Bit)?)\s(?<codeA>Flac(?:x\d)?)\)",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var webSource = match.Groups["websource"].Value.Trim();

        var (lang, subType) = DetectLanguageSubtitle(webSource);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value;
        }

        return new ParseResult
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            Resolution   = StringUtils.ResolutionStr2Enum(resolution),
            Language     = lang,
            GroupType    = GroupType,
            SubtitleType = subType,
            Source       = "BDRip",
        };
    }
}
