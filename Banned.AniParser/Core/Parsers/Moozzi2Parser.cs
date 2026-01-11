using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class Moozzi2Parser : BaseParser
{
    public override string        GroupName => "Moozzi2";
    public override EnumGroupType GroupType => EnumGroupType.Compression;

    [GeneratedRegex(@"\[Moozzi2](?<title>[^\[\]]+?)-\s?(?<episode>\d+)\s(?:END|SP)?\s?\(BD\s(?<resolution>\d+x\d+)\s(?<vCodec>(?:x\.?265|x\.?264)(?:-?(?<rate>\d+)bit)?)\s(?<aCode>Flac)(?:x\d)?\)",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    public Moozzi2Parser()
    {
        SingleEpisodePatterns = [SinglePattern()];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        return new ParseResult
        {
            Title         = GetGroupOrDefault(match, "title", string.Empty),
            Episode       = ParseIntGroup(match, "episode"),
            Group         = this.GroupName,
            GroupType     = this.GroupType,
            Language      = EnumLanguage.None,
            MediaType     = EnumMediaType.SingleEpisode,
            Source        = "BDRip",
            SubtitleType  = EnumSubtitleType.None,
            Resolution    = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
            VideoCodec    = ParseVideoCodec(match),
            AudioCodec    = ParseAudioCodec(match),
            ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
        };
    }
}
