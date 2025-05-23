﻿using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public class MingYSubParser : BaseParser
{
    public override string GroupName => "MingYSub";

    public MingYSubParser()
    {
        SubtitleTypeMap["JpSc"] = EnumSubtitleType.Embedded;
        SubtitleTypeMap["JpTc"] = EnumSubtitleType.Embedded;
        LanguageMap["JpCn"]     = EnumLanguage.JpScTc;
        SubtitleTypeMap["JpCn"] = EnumSubtitleType.Muxed;
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>MingY&[^\[\]]+)\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[(?<group>MingY&[^\[\]]+)\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
        MultipleEpisodePatterns = new List<Regex>()
        {
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
            new(
                @"\[MingY\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\](?:\[(?<source>[a-zA-Z]+[Rr]ip)\])?\[(?<lang>.+?)\]",
                RegexOptions.IgnoreCase),
        };
    }

    protected override ParserInfo CreateParsedResultSingle(Match match)
    {
        var episode = 0;
        if (match.Groups["episode"].Success)
            episode = int.Parse(Regex.Replace(match.Groups["episode"].Value, @"\D+", ""));

        var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);

        var resolution = "1080p";
        if (match.Groups["resolution"].Success)
        {
            resolution = match.Groups["resolution"].Value.Trim();
        }

        var group = GroupName;
        if (match.Groups["group"].Success)
        {
            group = match.Groups["group"].Value.Trim();
        }


        return new ParserInfo
        {
            IsMultiple   = false,
            Title        = match.Groups["title"].Value.Trim(),
            Episode      = episode,
            Group        = group,
            Resolution   = resolution,
            Language     = lang,
            SubtitleType = subType
        };
    }
}