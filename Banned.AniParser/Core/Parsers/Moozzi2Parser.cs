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
        SingleEpisodePatterns =
        [
            new(@"\[Moozzi2](?<title>[^\[\]]+?)-\s?(?<episode>\d+)\s(?:END|SP)?\s?\(BD\s(?<resolution>\d+x\d+)\s(?<codeV>(?:x\.?265|x\.?264)-?(?<rate>\d+bit)?)\s(?<codeA>Flac(?:x\d)?)\)",
                RegexOptions.IgnoreCase),
        ];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        return new ParseResult
        {
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = ParseIntGroup(match, "episode"),
            Group        = this.GroupName,
            GroupType    = this.GroupType,
            Language     = EnumLanguage.None,
            MediaType    = EnumMediaType.SingleEpisode,
            Source       = "BDRip",
            SubtitleType = EnumSubtitleType.None,
            Resolution   = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
        };
    }
}
