using System.Text.RegularExpressions;
using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Core.Parsers;

public class StyhSubParser : BaseParser
{
    public override string GroupName => "霜庭云花";

    public StyhSubParser()
    {
        LanguageMap["JpSc_JpTc"] = EnumLanguage.JpScTc;
        //[STYHSub][Aharen-san wa Hakarenai Season 2][02][1080P][WebRip][CHS&JPN].mp4
        //[STYHSub] Nageki no Bourei wa Intai Shitai - S01E01 - [WEB HEVC AAC ASS_JPSC_JPTC v2].mkv
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