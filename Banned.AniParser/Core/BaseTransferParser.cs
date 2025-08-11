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
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = ParseIntGroup(match, "episode"),
            Group        = GroupName,
            GroupType    = GroupType,
            Language     = lang,
            MediaType    = EnumMediaType.SingleEpisode,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            SubtitleType = subType,
            Version      = ParseVersion(match),
            WebSource    = match.Groups["websource"].Value,
        };
    }

    protected override ParseResult CreateParsedResultMultiple(Match match)
    {
        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        return new ParseResult
        {
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = ParseIntGroup(match, "start"),
            EndEpisode   = ParseIntGroup(match, "end"),
            Group        = GroupName,
            GroupType    = GroupType,
            Language     = lang,
            MediaType    = EnumMediaType.MultipleEpisode,
            Resolution   = StringUtils.ResolutionStr2Enum(match.Groups["resolution"].Value),
            SubtitleType = subType,
            WebSource    = match.Groups["websource"].Value,
        };
    }
}
