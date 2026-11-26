using Banned.AniParser.Models.Enums;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Core.Parsers;

public partial class NixRawsParser : BaseTransferParser
{
    public override string GroupName => "Nix-Raws";

    // [Nix-Raws] 転生吉ブリンだけど質問ある？ / Tensei Goblin Dakedo Shitsumon Aru? S01E12 [CR WEB-DL 1080p AVC AAC][简繁内封][复制磁连]
    // [Nix-Raws] Chainsmoker Cat S01E07 [CATCHPLAY WEB-DL 1080p AVC AAC][SC_TC].mkv
    [GeneratedRegex(@"\[Nix-Raws](?<title>.+?)\s+S\d+E(?<episode>\d+)(?:v(?<version>\d+))?\s+\[(?<websource>CR|CATCHPLAY|ABEMA|AMZN|BAHA|IQIYI|NF|DSNP|BILIBILI)\s+(?<source>WEB-DL|WEBRip|BDRip|TVRip|DVDRip)\s+(?<resolution>\d+p)\s+(?<vCodec>AVC|HEVC|AV1)\s+(?<aCodec>AAC|EAC-3|FLAC|OPUS)]\s*\[(?<lang>[^\[\]]+)](?:\[[^\[\]]+])?(?:\.(?<extension>mkv|mp4))?$",
                    RegexOptions.IgnoreCase)]
    private static partial Regex SinglePattern();

    public NixRawsParser()
    {
        LanguageMap["SC_TC"] = EnumLanguage.ScTc;
        SubtitleTypeMap["SC_TC"] = EnumSubtitleType.Muxed;

        SingleEpisodePatterns = [SinglePattern()];
        InitMap();
    }
}
