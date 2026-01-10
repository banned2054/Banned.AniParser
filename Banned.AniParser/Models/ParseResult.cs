using Banned.AniParser.Models.Enums;

namespace Banned.AniParser.Models;

public class ParseResult
{
    /// <summary>
    /// 是多集还是单集,或者剧场版
    /// </summary>
    public EnumMediaType MediaType { get; init; } = EnumMediaType.SingleEpisode;

    /// <summary>
    /// 解析后的纯标题，可能多语言
    /// </summary>
    public string OriginTitle { get; set; } = string.Empty;

    /// <summary>
    /// 解析后的纯标题，可能多语言
    /// </summary>
    public string Title { get; set; } = string.Empty;

    public List<LocalizedTitle>? Titles { get; set; }

    /// <summary>
    /// 单集集数
    /// </summary>
    public decimal? Episode { get; init; }

    /// <summary>
    /// 单集版本
    /// </summary>
    public int Version { get; init; } = 1;

    /// <summary>
    /// 多集的第一集
    /// </summary>
    public int? StartEpisode { get; init; }

    /// <summary>
    /// 多集的最后一集
    /// </summary>
    public int? EndEpisode { get; init; }

    /// <summary>
    /// 字幕组、压制组或者搬运组
    /// </summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// 字幕组、压制组还是搬运组
    /// </summary>
    public EnumGroupType GroupType { get; init; }

    /// <summary>
    /// 字幕语言
    /// </summary>
    public EnumLanguage Language { get; init; }

    /// <summary>
    /// 字幕类型
    /// </summary>
    public EnumSubtitleType SubtitleType { get; init; }

    /// <summary>
    /// 分辨率
    /// </summary>
    public EnumResolution Resolution { get; init; }

    /// <summary>
    /// 来源，WebRip、BDRip或者BDMV
    /// </summary>
    public string Source { get; init; } = "WebRip";

    /// <summary>
    /// 搬运组专用，搬运组的源
    /// </summary>
    public string WebSource { get; init; } = string.Empty;

    public string VideoCodec    { get; set; } = string.Empty;
    public string AudioCodec    { get; set; } = string.Empty;
    public int    ColorBitDepth { get; set; }
}
