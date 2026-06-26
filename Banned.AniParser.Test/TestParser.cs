using Banned.AniParser.Models.Enums;
using Banned.AniParser.Test.Utils;
using System.Diagnostics;
using System.Text.Json;

namespace Banned.AniParser.Test;

public class TestParser
{
    [Test]
    public async Task TestParserWithUrl()
    {
        var aniParser = new AniParser();
        var url       = "https://mikanani.me/RSS/Search?searchstr=Tsdm%E5%AD%97%E5%B9%95%E7%BB%84";
        var rssString = await TestNetUtils.Fetch(url);

        var testList = TestRssUtils.GetAllTitle(rssString);

        foreach (var testStr in testList)
        {
            var result = aniParser.Parse(testStr);
            TestPrintUtils.PrintParserInfo(result, testStr);
        }
    }

    [Test]
    public void TestParserWithStringList()
    {
        var testStr = new List<string>
        {
            "[OguriClub&S1YURICON] Umamusume Cinderella Gray[23][1080p][WebRip][HEVC_EAC3][CHS_JP].mkv"
        };
        var aniParser = new AniParser();
        foreach (var str in testStr)
        {
            var result = aniParser.Parse(str);
            TestPrintUtils.PrintParserInfo(result, str);
        }
    }

    [Test]
    public void TestParserUsesMatchedSource()
    {
        var aniParser = new AniParser();

        var translationResult = aniParser.Parse("[KitaujiSub] Test Title[01][BDRip][HEVC_AAC][CHS].mkv");
        Assert.That(translationResult, Is.Not.Null);
        Assert.That(translationResult!.Source, Is.EqualTo(EnumSource.BDRip));

        var transferResult =
            aniParser.Parse("[Prejudice-Studio] Test Title - 01 [Bilibili WEB-DL 1080p AVC 10bit AAC MP4][CHS].mkv");
        Assert.That(transferResult, Is.Not.Null);
        Assert.That(transferResult!.Source, Is.EqualTo(EnumSource.WEB_DL));
        Assert.That(transferResult.WebSource, Is.EqualTo("Bilibili"));
    }

    [Test]
    public void TestSourceStringNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(JsonSerializer.Serialize(EnumSource.WEB_DL), Is.EqualTo("\"WEB-DL\""));
            Assert.That(JsonSerializer.Deserialize<EnumSource>("\"WEB-DL\""), Is.EqualTo(EnumSource.WEB_DL));
            Assert.That(JsonSerializer.Serialize(EnumSource.WEBRip), Is.EqualTo("\"WEBRip\""));
            Assert.That(JsonSerializer.Serialize(EnumSource.BDRip), Is.EqualTo("\"BDRip\""));
            Assert.That(JsonSerializer.Serialize(EnumSource.TVRip), Is.EqualTo("\"TVRip\""));
            Assert.That(JsonSerializer.Serialize(EnumSource.DVDRip), Is.EqualTo("\"DVDRip\""));
        });
    }

    [Test]
    public void TestTsdmSubParser()
    {
        var aniParser = new AniParser();

        var fileResult =
            aniParser.Parse("[TSDM][Awajima Hyakkei][09][WebRip][HEVC-10bit 1080p AAC][CHS_JP&CHT_JP].mkv");
        Assert.That(fileResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(fileResult!.Title, Is.EqualTo("Awajima Hyakkei"));
            Assert.That(fileResult.Group, Is.EqualTo("TSDM字幕组"));
            Assert.That(fileResult.GroupType, Is.EqualTo(EnumGroupType.Translation));
        });

        var rssResult =
            aniParser.Parse("【TSDM字幕组】[淡岛百景][Awajima Hyakkei][09][HEVC-10bit 1080p AAC][MKV][简繁日内封字幕]淡岛百景");
        Assert.That(rssResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(rssResult!.Title, Is.EqualTo("淡岛百景"));
            Assert.That(rssResult.Group, Is.EqualTo("TSDM字幕组"));
            Assert.That(rssResult.GroupType, Is.EqualTo(EnumGroupType.Translation));
        });
    }

    [Test]
    public void TestSmzaseSubParser()
    {
        var aniParser = new AniParser();

        var embeddedResult =
            aniParser.Parse("[三明治摆烂组] 落第贤者的学院无双～第二回转生，S等级作弊魔术师冒险记～ / Rakudai Kenja no Gakuin Musou / 落第賢者の学院無双 - 01 - [繁日内嵌][AVC 8bit 1080P]");
        Assert.That(embeddedResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(embeddedResult!.Title,
                        Is.EqualTo("落第贤者的学院无双～第二回转生，S等级作弊魔术师冒险记～ / Rakudai Kenja no Gakuin Musou / 落第賢者の学院無双"));
            Assert.That(embeddedResult.Group, Is.EqualTo("三明治摆烂组"));
            Assert.That(embeddedResult.GroupType, Is.EqualTo(EnumGroupType.Translation));
            Assert.That(embeddedResult.Episode, Is.EqualTo(1));
            Assert.That(embeddedResult.Language, Is.EqualTo(EnumLanguage.JpTc));
            Assert.That(embeddedResult.SubtitleType, Is.EqualTo(EnumSubtitleType.Embedded));
            Assert.That(embeddedResult.VideoCodec, Is.EqualTo("AVC"));
            Assert.That(embeddedResult.ColorBitDepth, Is.EqualTo(8));
        });

        var muxedResult =
            aniParser.Parse("[三明治摆烂组] 落第贤者的学院无双～第二回转生，S等级作弊魔术师冒险记～ / Rakudai Kenja no Gakuin Musou / 落第賢者の学院無双 - 01 - [简繁日内封][HEVC-10bit 1080P]");
        Assert.That(muxedResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(muxedResult!.Group, Is.EqualTo("三明治摆烂组"));
            Assert.That(muxedResult.Language, Is.EqualTo(EnumLanguage.JpScTc));
            Assert.That(muxedResult.SubtitleType, Is.EqualTo(EnumSubtitleType.Muxed));
            Assert.That(muxedResult.VideoCodec, Is.EqualTo("HEVC"));
            Assert.That(muxedResult.ColorBitDepth, Is.EqualTo(10));
        });

        var simplifiedAssResult =
            aniParser.Parse("[smzase] Ichijyoma Mankitsu Gurashi - S01E01.zh-hans.ass");
        Assert.That(simplifiedAssResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(simplifiedAssResult!.Title, Is.EqualTo("Ichijyoma Mankitsu Gurashi"));
            Assert.That(simplifiedAssResult.Group, Is.EqualTo("三明治摆烂组"));
            Assert.That(simplifiedAssResult.Episode, Is.EqualTo(1));
            Assert.That(simplifiedAssResult.Language, Is.EqualTo(EnumLanguage.Sc));
            Assert.That(simplifiedAssResult.SubtitleType, Is.EqualTo(EnumSubtitleType.External));
        });

        var collaborationAssResult =
            aniParser.Parse("[smzase&Meiko] Alma-chan Wants to Be a Family - S01E01.zh-hans.ass");
        Assert.That(collaborationAssResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(collaborationAssResult!.Title, Is.EqualTo("Alma-chan Wants to Be a Family"));
            Assert.That(collaborationAssResult.Group, Is.EqualTo("三明治摆烂组&Meiko"));
            Assert.That(collaborationAssResult.Language, Is.EqualTo(EnumLanguage.Sc));
            Assert.That(collaborationAssResult.SubtitleType, Is.EqualTo(EnumSubtitleType.External));
        });

        var traditionalVideoResult =
            aniParser.Parse("[smzase] Rakudai Kenja no Gakuin Musou - S01E01 - [CHT_JPN][WebRip H264 8bit 1080P].mp4");
        Assert.That(traditionalVideoResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(traditionalVideoResult!.Title, Is.EqualTo("Rakudai Kenja no Gakuin Musou"));
            Assert.That(traditionalVideoResult.Group, Is.EqualTo("三明治摆烂组"));
            Assert.That(traditionalVideoResult.Episode, Is.EqualTo(1));
            Assert.That(traditionalVideoResult.Language, Is.EqualTo(EnumLanguage.JpTc));
            Assert.That(traditionalVideoResult.SubtitleType, Is.EqualTo(EnumSubtitleType.Embedded));
            Assert.That(traditionalVideoResult.VideoCodec, Is.EqualTo("AVC"));
            Assert.That(traditionalVideoResult.ColorBitDepth, Is.EqualTo(8));
            Assert.That(traditionalVideoResult.Source, Is.EqualTo(EnumSource.WEBRip));
        });

        var muxedVideoResult =
            aniParser.Parse("[smzase] Rakudai Kenja no Gakuin Musou - S01E01 - [CHI_JPN][WebRip H265 10bit 1080P].mkv");
        Assert.That(muxedVideoResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(muxedVideoResult!.Title, Is.EqualTo("Rakudai Kenja no Gakuin Musou"));
            Assert.That(muxedVideoResult.Group, Is.EqualTo("三明治摆烂组"));
            Assert.That(muxedVideoResult.Episode, Is.EqualTo(1));
            Assert.That(muxedVideoResult.Language, Is.EqualTo(EnumLanguage.JpScTc));
            Assert.That(muxedVideoResult.SubtitleType, Is.EqualTo(EnumSubtitleType.Muxed));
            Assert.That(muxedVideoResult.VideoCodec, Is.EqualTo("HEVC"));
            Assert.That(muxedVideoResult.ColorBitDepth, Is.EqualTo(10));
            Assert.That(muxedVideoResult.Source, Is.EqualTo(EnumSource.WEBRip));
        });
    }

    [Test]
    public void TestGetAllParserName()
    {
        var aniParser = new AniParser();

        var translationParserList = aniParser.GetTranslationParserList();
        Console.WriteLine("翻译组");
        foreach (var translationParser in translationParserList)
        {
            Console.WriteLine($"- {translationParser}");
        }

        var transferParserList = aniParser.GetTransferParserList();
        Console.WriteLine("搬运组");
        foreach (var transferParser in transferParserList)
        {
            Console.WriteLine($"- {transferParser}");
        }

        var compressionParserList = aniParser.GetCompressionParserList();
        Console.WriteLine("压制组");
        foreach (var compressionParser in compressionParserList)
        {
            Console.WriteLine($"- {compressionParser}");
        }
    }

    [Test]
    public async Task TestRunTime()
    {
        var aniParser = new AniParser();

        var dataStr = await File.ReadAllTextAsync("Data/data.json");

        var testList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(dataStr) ?? [];

        // 创建并启动 Stopwatch
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var results = aniParser.ParseBatch(testList);

        // 停止计时并获取运行时间
        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;

        // 输出运行时间
        Console.WriteLine($"函数运行时间：{elapsed.TotalMilliseconds} 毫秒");

        Console.WriteLine($"测试样例数量:{testList.Count}\n匹配结果:{results.Count()}");
    }
}
