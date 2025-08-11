using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core;

public abstract class BaseTransferParser : BaseParser
{
    public override EnumGroupType GroupType => EnumGroupType.Transfer;

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            MediaType    = EnumMediaType.SingleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = ParseIntGroup(match, "episode"),
            Version      = ParseVersion(match),
            Group        = GroupName,
            GroupType    = GroupType,
            WebSource    = match.Groups["websource"].Value,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            MediaType    = EnumMediaType.MultipleEpisode,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = ParseIntGroup(match, "start"),
            EndEpisode   = ParseIntGroup(match, "end"),
            Group        = GroupName,
            GroupType    = GroupType,
            WebSource    = match.Groups["websource"].Value,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            Language     = lang,
            SubtitleType = subType
        };
    }
}
