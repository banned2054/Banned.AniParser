using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class StyhSubParser : BaseParser
{
    public override string        GroupName => "霜庭云花";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    public StyhSubParser()
    {
        LanguageMap["JpSc_JpTc"] = EnumLanguage.JpScTc;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[霜庭云花Sub\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:V(?<version>\d+))?(\sEND)?\]\[(?<resolution>\d+p)\]\[(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\]\[(?<lang>.+?)\]\[(?<source>[a-z]+Rip)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[霜庭云花Sub\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:V(?<version>\d+))?\s?\[(?<source>[a-z]+Rip)\s(?<resolution>\d+p)\s(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[STYHSub\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:V(?<version>\d+))?(\sEND)?\]\[(?<resolution>\d+[pP])\]\[(?<source>[a-z]+Rip)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[STYHSub\](?<title>[^\[\]]+?)-\s?S(?<season>\d+)E(?<episode>\d+)(?:v(?<version>\d+))?\s?-\s?\[(?<source>[a-z])\s(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s(?<lang>.+?)\s?(V(?<version1>\d+))?\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[霜庭云花Sub\]\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)-(?<end>\d+)\s?(?<OAD>[a-z\u4e00-\u9fff]+)?\]\[(?<resolution>\d+p)\]\[(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\]\[(?<lang>.+?)\]\[(?<source>[a-z]+Rip)\]",
                RegexOptions.IgnoreCase),
        };
    }
}
