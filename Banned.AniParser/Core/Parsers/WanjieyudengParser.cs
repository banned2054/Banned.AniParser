using Banned.AniParser.Models;
using Banned.AniParser.Models.Enums;
using Banned.AniParser.Utils;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

/// <summary>
/// 晚街与灯（繁体写作「晚街與燈」）字幕组。
/// 命名格式：[组名][标题][集数][来源][规格][字幕语言][复制磁连]
/// - 单集：[19 - 总第85]，取季内集数
/// - 合集：标题带范围「01-19」时取标题（方括号里的「第67~85」是总集数）；否则取 [总第67~85] / [第67~85]；或标题含「合集」且无集数字段
/// - 剧场版：无集数字段，标题含「剧场版」
/// - 来源字段可缺失
/// </summary>
public partial class WanjieyudengParser : BaseParser
{
    public override string        GroupName => "晚街与灯";
    public override EnumGroupType GroupType => EnumGroupType.Translation;

    private const string Group  = @"\[(?<group>晚街[与與][灯燈])]";
    private const string Title  = @"\[(?<title>[^\[\]]+?)]";
    private const string Source = @"\[(?<source>BDRip|WebRip|WEB-DL)(?:\sRemux)?]";
    private const string Spec   = @"\[(?<resolution>\d+P)_(?<vCodec>HEVC|AVC|X264|X265)(?:-(?<rate>\d+)bit)?_(?<aCodec>FLAC|AAC)]";
    private const string Lang   = @"\[(?<lang>[^\[\]]+?)](?:\[复制磁连])?";

    [GeneratedRegex(Group + Title + @"\[(?<episode>\d+)\s*-\s*[总總]第\d+]" + Source + Spec + Lang,
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern1();

    // 标题自带范围（如「第四季 01-19」），取标题里的起止集；方括号里的「第67~85」是总集数，忽略
    [GeneratedRegex(Group + @"\[(?<title>[^\[\]]*?)(?<start>\d{1,3})-(?<end>\d{1,3})[^\[\]]*?\]" +
                    @"\[(?:(?:总第|總第|第)\d+~\d+)]" + Source + Spec + Lang,
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePatternWithTitleRange();

    [GeneratedRegex(Group + Title + @"\[(?:(?:总第|總第|第)(?<start>\d+)~(?<end>\d+))]" + Source + Spec + Lang,
                    RegexOptions.IgnoreCase)]
    private static partial Regex MultiplePattern1();

    [GeneratedRegex(Group + Title + Source + Spec + Lang, RegexOptions.IgnoreCase)]
    private static partial Regex NoEpisodePattern();

    [GeneratedRegex(Group + Title + Spec + Lang, RegexOptions.IgnoreCase)]
    private static partial Regex NoEpisodeNoSourcePattern();

    public WanjieyudengParser()
    {
        GroupNameMap.Add(new Regex("晚街[与與][灯燈]", RegexOptions.IgnoreCase), "晚街与灯");

        LanguageMap["简繁日双语内封PGS"] = EnumLanguage.JpScTc;
        LanguageMap["简繁日内封PGS"]    = EnumLanguage.JpScTc;
        LanguageMap["繁日内封PGS"]      = EnumLanguage.JpTc;
        LanguageMap["简日双语内嵌"]     = EnumLanguage.JpSc;
        LanguageMap["繁日双语内嵌"]     = EnumLanguage.JpTc;

        SubtitleTypeMap["内封PGS"] = EnumSubtitleType.Muxed;
        SubtitleTypeMap["内封"]    = EnumSubtitleType.Muxed;

        SingleEpisodePatterns =
        [
            SinglePattern1(),
            NoEpisodeNoSourcePattern(),
            NoEpisodePattern()
        ];
        MultipleEpisodePatterns = [MultiplePatternWithTitleRange(), MultiplePattern1()];
        InitMap();
    }

    protected override ParseResult CreateParsedResultSingle(Match match)
    {
        // 无集数字段的合集：标题含「合集」，起止集数未知
        if (GetGroupOrDefault(match, "title", string.Empty).Contains("合集"))
        {
            var (lang, subType) = DetectLanguageSubtitle(match.Groups["lang"].Value);
            return new ParseResult
            {
                Title         = GetGroupOrDefault(match, "title", string.Empty),
                Group         = GetGroupName(match),
                GroupType     = GroupType,
                MediaType     = EnumMediaType.MultipleEpisode,
                Language      = lang,
                SubtitleType  = subType,
                Resolution    = StringUtils.ResolutionStr2Enum(GetGroupOrDefault(match, "resolution", "1080p")),
                Source        = match.Groups["source"].Success
                    ? ParseSource(match)
                    : StringUtils.SourceStr2Enum("WebRip"),
                VideoCodec    = ParseVideoCodec(match),
                AudioCodec    = ParseAudioCodec(match),
                ColorBitDepth = int.Parse(GetGroupOrDefault(match, "rate", "-1"))
            };
        }

        return base.CreateParsedResultSingle(match);
    }

    protected override EnumMediaType ParseSingleMediaType(Match match)
    {
        var title = GetGroupOrDefault(match, "title", string.Empty);
        if (title.Contains("剧场版") || title.Contains("Movie", StringComparison.OrdinalIgnoreCase))
            return EnumMediaType.Movie;
        if (title.Contains("OVA", StringComparison.OrdinalIgnoreCase) ||
            title.Contains("OAD", StringComparison.OrdinalIgnoreCase))
            return EnumMediaType.Ova;
        return EnumMediaType.SingleEpisode;
    }
}
