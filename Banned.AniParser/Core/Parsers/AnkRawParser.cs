using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class AnkRawParser : BaseParser
{
    public override string GroupName { get; } = "ANK-Raws";

    public AnkRawParser()
    {
        //[ANK-Raws] 進撃の巨人 02 (BDrip 1920x1080 HEVC-YUV420P10 FLAC).mkv 1.59 GB
        SingleEpisodePatterns = new List<Regex>
        {
            new(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\s(?<episode>\d+)\s\((?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+x\d+)\s(?<vcodec>HEVC-YUV420P10)\s(?<acodec>FLAC)\sDTS-HDMA\)\.mkv",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(@"\[(?<group>(?:[^\[\]]+&)?ANK-Raws(?:&[^\[\]]+)?)\](?<title>[^\[\]]+?)\((?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+x\d+)\s(?<vcodec>HEVC-YUV420P10)\s(?<acodec>FLAC)\sDTS-HDMA\)",
                RegexOptions.IgnoreCase),
        };
        // [ANK-Raws] 物语系列 第二季 / Monogatari Series: Second Season / 物語シリーズ セカンドシーズン (BDrip 1920x1080 HEVC-YUV420P10 FLAC) 
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0f;
        if (match.Groups["episode"].Success)
        {
            episode = float.Parse(match.Groups["episode"].Value);
        }
        else if (match.Groups["special_episode"].Success)
        {
            episode = int.Parse(Regex.Replace(match.Groups["special_episode"].Value, @"\D+", ""));
        }

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var title = match.Groups["title"].Value.Trim();
        if (match.Groups["special_season"].Success)
        {
            title = $"{title} {match.Groups["special_season"].Value}";
        }

        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = title,
            Episode      = episode,
            Group        = GroupName,
            GroupType    = EnumGroupType.Compression,
            Resolution   = match.Groups["resolution"].Value,
            Language     = lang,
            SubtitleType = subType
        };
    }

    protected override ParserInfo CreateParsedResultMultiple(Match match)
    {
        var startEpisode = 0;
        var endEpisode   = 0;
        if (match.Groups["start"].Success)
        {
            startEpisode = int.Parse(Regex.Replace(match.Groups["start"].Value.Trim(), @"\D+", ""));
        }

        if (match.Groups["end"].Success)
        {
            endEpisode = int.Parse(Regex.Replace(match.Groups["end"].Value.Trim(), @"\D+", ""));
        }


        return new ParserInfo
        {
            IsMultiple   = true,
            Title        = match.Groups["title"].Value.Trim(),
            StartEpisode = startEpisode,
            EndEpisode   = endEpisode,
            Group        = GroupName,
            GroupType    = EnumGroupType.Compression,
            Resolution   = match.Groups["resolution"].Value,
            Language     = EnumLanguage.None,
            SubtitleType = EnumSubtitleType.None
        };
    }
}