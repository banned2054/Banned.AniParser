using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class StyhSubParser : BaseParser
{
    public override string GroupName => "霜庭云花";

    public StyhSubParser()
    {
        LanguageMap["JpSc_JpTc"] = EnumLanguage.JpScTc;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[霜庭云花Sub\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:[Vv](?<version>\d+))?(\sEND)?\]\[(?<resolution>\d+[pP])\]\[(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\]\[(?<lang>.+?)\]\[(?<source>[a-zA-Z]+[Rr]ip)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[霜庭云花Sub\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:[Vv](?<version>\d+))?\s?\[(?<source>[a-zA-Z]+[Rr]ip)\s(?<resolution>\d+[pP])\s(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[STYHSub\]\[(?<title>[^\[\]]+?)\]\[(?<episode>\d+)(?:[Vv](?<version>\d+))?(\sEND)?\]\[(?<resolution>\d+[pP])\]\[(?<source>[a-zA-Z]+[Rr]ip)\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[STYHSub\](?<title>[^\[\]]+?)-\s?S(?<season>\d+)E(?<episode>\d+)\s?-\s?\[(?<source>[a-zA-Z])\s(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\s(?<lang>.+?)\s?([Vv](?<version1>\d+))?\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[霜庭云花Sub\]\[(?<title>[^\[\]]+?)\]\[(?<start>\d+)-(?<end>\d+)\s?(?<OAD>[a-zA-Z\u4e00-\u9fff]+)?\]\[(?<resolution>\d+[pP])\]\[(?<codec>(HEVC|AAC|AVC)(\s(HEVC|AAC|AVC))*)\]\[(?<lang>.+?)\]\[(?<source>[a-zA-Z]+[Rr]ip)\]",
                RegexOptions.IgnoreCase),
        };
    }
}