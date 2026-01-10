using Banned.AniParser.Test.Utils;
using System.Diagnostics;

namespace Banned.AniParser.Test;

public class TestParser
{
    [Test]
    public async Task TestParserWithUrl()
    {
        var aniParser = new AniParser();
        var url =
            "https://mikanani.me/RSS/Search?searchstr=%E6%A8%B1%E6%A1%83%E8%8A%B1%E5%AD%97%E5%B9%95%E7%BB%84";
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
            "【喵萌奶茶屋】★01月新番★[阿尔涅的事件簿 / 阿尔涅事件簿 / Arne no Jikenbo][01][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 超自然武装当哒当 / 胆大党 / Dandadan [13-24 精校合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[超自然武装当哒当 / 胆大党 / Dandadan][13-24][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[超自然武装当哒当 / 胆大党 / Dandadan][13-24][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][13][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★剧场版★[BLOODY ESCAPE 地狱逃亡剧][1080p][繁体] [复制磁连]",
            "【喵萌奶茶屋】★剧场版★[BLOODY ESCAPE 地狱逃亡剧][1080p][简体] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 24 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][END] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][24][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][24][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][12][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][12][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][07+ES07][WebRip 1080p HEVC-10bit AAC][简繁日内封] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][07+ES07][1080p][繁日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 23 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][23][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][23][1080p][简日双语][招募翻译] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 11 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 琉璃的宝石 / Ruri no Houseki [01-13 修正合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 和雨和你 / 雨天遇见狸 / Ame to Kimi to [01-12 修正合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[琉璃的宝石 / Ruri no Houseki][01-13][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[琉璃的宝石 / Ruri no Houseki][01-13][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][07+ES07][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][11][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[和雨和你 / 雨天遇见狸 / Ame to Kimi to][01-12][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[和雨和你 / 雨天遇见狸 / Ame to Kimi to][01-12][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][11][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][06+ES06][WebRip 1080p HEVC-10bit AAC][简繁日内封] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][06+ES06][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][06+ES06][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][05+ES05][WebRip 1080p HEVC-10bit AAC][简繁日内封] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][05+ES05][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][05+ES05][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 22 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 21 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 20 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 19 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 18 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 17 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][22][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][22][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][21][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][21][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][20][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][20][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][19][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][19][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][18][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][17][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][17][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][18][1080p][简日双语][招募翻译] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 10 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][10][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][10][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 09 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][09][1080p][繁日双语] [复制磁连]",
            "[喵萌奶茶屋&VCB-Studio] 银砂糖师与黑妖精 ~ Sugar Apple Fairy Tale ~ / シュガーアップル・フェアリーテイル 10-bit 1080p HEVC BDRip [Fin] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][09][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 08 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][08][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][08][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][04+ES04][WebRip 1080p HEVC-10bit AAC][简繁日内封] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][04+ES04][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][04+ES04][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 07 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][03+ES03][WebRip 1080p HEVC-10bit AAC][简繁日内封] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][03+ES03][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][03+ES03][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][07][1080p][繁日双语] [复制磁连]",
            "[喵萌奶茶屋&VCB-Studio] 金牌得主 / Medalist / メダリスト 10-bit 1080p HEVC BDRip [Fin] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][07][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 - 04 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 - 03 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][04][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][04][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][03][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][03][1080p][简日双语] [复制磁连]",
            "[个人制作合集][喵萌奶茶屋&LoliHouse] 直至魔女消逝 / 直到某魔女死去 / Aru Majo ga Shinu Made - 01-12 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[个人制作合集][喵萌奶茶屋&LoliHouse] 末日后酒店 / Apocalypse Hotel - 01-12 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 06 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][06][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][06][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 - 02 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 05 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][02][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][02][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][05][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[Chi。-关于地球的运动- / Chi. Chikyuu no Undou ni Tsuite][25][1080p][简繁日内封][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[Chi。-关于地球的运动- / Chi. Chikyuu no Undou ni Tsuite][24][1080p][简繁日内封][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[Chi。-关于地球的运动- / Chi. Chikyuu no Undou ni Tsuite][21][1080p][简繁日内封][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[Chi。-关于地球的运动- / Chi. Chikyuu no Undou ni Tsuite][23][1080p][简繁日内封][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[Chi。-关于地球的运动- / Chi. Chikyuu no Undou ni Tsuite][22][1080p][简繁日内封][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][05][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 04 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 16 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][02+ES02][WebRip 1080p HEVC-10bit AAC][简繁日内封] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][02+ES02][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][02+ES02][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][01+ES01][WebRip 1080p HEVC-10bit AAC][简繁日内封] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][01+ES01][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[弹珠汽水瓶里的千岁同学 / 千歳くんはラムネ瓶のなか / Chitose-kun wa Ramune Bin no Naka][01+ES01][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][04][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][16][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][16][1080p][简日双语][招募翻译] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 恋爱无法用双子除尽 / 恋爱无法用双子除尽 / Koi wa Futago de Warikirenai / 恋は双子で割り切れない [01-12 合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][04][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 冻牌 / Touhai [01-25 合集][WebRip 1080p HEVC-10bit AAC][简繁内封字幕][Fin] [复制磁连]",
            "【喵萌奶茶屋】[冻牌 / Touhai: Ura Rate Mahjong Touhai Roku][01-25][1080p][繁体][含小剧场] [复制磁连]",
            "【喵萌奶茶屋】[冻牌 / Touhai: Ura Rate Mahjong Touhai Roku][01-25][1080p][简体][含小剧场] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 药师少女的独语 / 药屋少女的呢喃 / Kusuriya no Hitorigoto [25-48 修正合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "【喵萌奶茶屋】★01月新番★[药师少女的独语 / 药屋少女的呢喃 / Kusuriya no Hitorigoto][25-48][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★01月新番★[药师少女的独语 / 药屋少女的呢喃 / Kusuriya no Hitorigoto][25-48][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★04月新番★[搞笑漫画日和GO / Gyagu Manga Biyori GO][01-12][1080p][繁体中文] [复制磁连]",
            "【喵萌奶茶屋】★04月新番★[搞笑漫画日和GO / Gyagu Manga Biyori GO][01-12][1080p][简体中文] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 圣女因太过完美一点也不讨人喜欢而被废除婚约卖到邻国 / 圣女因太过完美一点也不讨人喜欢而被废除婚约卖到邻国 / Kanpekiseijo [01-12 合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 更衣人偶坠入爱河 / Sono Bisque Doll wa Koi wo Suru - 22 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 更衣人偶坠入爱河 / Sono Bisque Doll wa Koi wo Suru - 21 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 更衣人偶坠入爱河 / Sono Bisque Doll wa Koi wo Suru - 20 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 更衣人偶坠入爱河 / Sono Bisque Doll wa Koi wo Suru - 19 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][22][1080p][繁体] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][22][1080p][简体] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][21][1080p][繁体] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][21][1080p][简体] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][20][1080p][繁体] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][20][1080p][简体] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][19][1080p][繁体] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[更衣人偶坠入爱河 / 恋上换装娃娃 / Sono Bisque Doll wa Koi wo Suru][19][1080p][简体] [复制磁连]",
            "【喵萌奶茶屋】[恋爱无法用双子除尽 / Koi wa Futago de Warikirenai][01-12][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】[恋爱无法用双子除尽 / Koi wa Futago de Warikirenai][01-12][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】[金牌得主 / Medalist][01-13][BDRip][1080p][简日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】[金牌得主 / Medalist][01-13][BDRip][1080p][繁日双语][招募翻译] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 欢迎来到颠沛流离食堂！/ Tsuihousha Shokudou e Youkoso! [01-12 合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi - 03 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★04月新番★[圣女因太过完美一点也不讨人喜欢而被废除婚约卖到邻国 / Kanpekiseijo][01-12][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★04月新番★[圣女因太过完美一点也不讨人喜欢而被废除婚约卖到邻国 / Kanpekiseijo][01-12][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[欢迎来到颠沛流离食堂！/ Tsuihousha Shokudou e Youkoso!][01-12][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★07月新番★[欢迎来到颠沛流离食堂！/ Tsuihousha Shokudou e Youkoso!][01-12][1080p][简日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 - 01 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][01][1080p][繁日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[不动声色的柏田同学和喜形于色的太田君 / 无口的柏田小姐与元气的太田君 / Kao ni Denai Kashiwada-san to Kao ni Deru Oota-kun][01][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][03][1080p][繁日双语] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024) - 15 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][15][1080p][繁日双语][招募翻译] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[乱马 1/2 2024年版 / Ranma ½ / Ranma 1/2 (2024)][15][1080p][简日双语][招募翻译] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] Silent Witch 沉默魔女的秘密 / Silent Witch - Chinmoku no Majo no Kakushigoto - [01-13 合集][WebRip 1080p HEVC-10bit AAC][简繁日内封字幕][Fin] [复制磁连]",
            "[喵萌奶茶屋&LoliHouse] 想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / watatabe - 02 [WebRip 1080p HEVC-10bit AAC][简繁日内封字幕] [复制磁连]",
            "[喵萌奶茶屋&VCB-Studio] 剧场版 奇巧计程车: 扑朔谜林 / Eiga Odd Taxi: In the Woods 10-bit 1080p HEVC BDRip [MOVIE Reseed Fin] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][03][1080p][简日双语] [复制磁连]",
            "【喵萌奶茶屋】★10月新番★[想吃掉我的非人少女 / 对我垂涎欲滴的非人少女 / 私を喰べたい、ひとでなし / Watashi wo Tabetai, Hitodenashi][02][1080p][繁日双语] [复制磁连]",
            "[喵萌奶茶屋&VCB-Studio] 奇巧计程车 / ODDTAXI / オッドタクシー 10-bit 1080p HEVC BDRip [Reseed Fin] [复制磁连]",
        };
        // 2. 预热 (Warm-up)：让 JIT 先把代码编译好，不计入时间
        Console.WriteLine("正在预热...");
        var aniParser = new AniParser();
        for (int i = 0; i < 100; i++) // 多跑几圈预热
        {
            foreach (var str in testStr)
            {
                aniParser.Parse(str);
            }
        }

        // 3. 正式测试：加大压力，循环 1000 次 (总共处理 15万条)
        Console.WriteLine("开始测试...");
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        int loopCount = 1000;
        for (int i = 0; i < loopCount; i++)
        {
            foreach (var str in testStr)
            {
                // 关键：绝对不要在这里 Print 输出！
                var result = aniParser.Parse(str);

                // 防止 Release 模式下编译器觉得 result 没用把代码优化掉了
                // 稍微用一下 result
                if (result == null) continue;
            }
        }

        stopwatch.Stop();

        Console.WriteLine($"处理 {testStr.Count * loopCount} 条数据");
        Console.WriteLine($"总耗时：{stopwatch.Elapsed.TotalMilliseconds} ms");
        Console.WriteLine($"平均每条耗时：{stopwatch.Elapsed.TotalMilliseconds / (testStr.Count * loopCount)} ms");
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

        var testList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(dataStr);

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
