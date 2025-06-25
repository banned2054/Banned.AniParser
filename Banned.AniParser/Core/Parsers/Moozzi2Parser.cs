using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class Moozzi2Parser : BaseParser
{
    public override string GroupName => "Moozzi2";

    public Moozzi2Parser()
    {
        //[Moozzi2] Adachi to Shimamura - 12 END (BD 1920x1080 x265-10Bit Flac)
        SingleEpisodePatterns = new List<Regex>
        {
            new(@"\[Moozzi2\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s(?:END|SP)?\s?\(BD\s(?<resolution>\d+x\d+)\s(?<codeV>(?:x\.?265|x\.?264)(?:-10Bit|-8Bit)?)\s(?<codeA>Flac(?:x\d)?)\)",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var webSource = match.Groups["websource"].Value.Trim();

        var (lang, subType) = DetectLanguageSubtitle(webSource);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value
                              .Replace("1920x1080", "1080p")
                              .Replace("3840x2160", "4K");
        }

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = GroupName,
            Resolution   = resolution,
            Language     = lang,
            GroupType    = EnumGroupType.Compression,
            SubtitleType = subType,
            Source       = "BDRip",
        };
    }
}