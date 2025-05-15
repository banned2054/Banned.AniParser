# 关于扩展

### 最简单的扩展

最简单的扩展方式就是用BaseParser里的函数，只需要对单集和多集写正则表达式，例如说

```csharp
SingleEpisodePatterns = new List<Regex>
{
	new(
    	@"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<episode>\d+)(?:v(?<version>\d+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
        RegexOptions.IgnoreCase),
};
MultipleEpisodePatterns = new List<Regex>()
{
    new(
    	@"\[[樱桜]都字幕[組组]\](?<title>[^\[\]]+?)\[(?<start>\d+)(?:v(?<version1>\d+))?-(?<end>\d+)(?:v(?<version2>\d+))?(?:END)?(?:\+(?<OAD>[a-zA-Z\u4e00-\u9fff]+))?\]\[(?<resolution>\d+[pP])\]\[(?<lang>.+?)\]",
        RegexOptions.IgnoreCase),
};
```

##### 正则表达式的组名

默认的匹配组包含:`title`，`resolution`，`lang`，`episode`

其中针对`lang`，会尝试匹配语言类型:
```csharp
protected Dictionary<string, EnumLanguage> LanguageMap = new()
{
    ["简繁日"]         = EnumLanguage.JpScTc,
    ["Chs&Cht&Jpn"] = EnumLanguage.JpScTc,
    ["简日"]          = EnumLanguage.JpSc,
    ["Chs&Jpn"]     = EnumLanguage.JpSc,
    ["JpSc"]        = EnumLanguage.JpSc,
    ["繁日"]          = EnumLanguage.JpTc,
    ["Cht&Jpn"]     = EnumLanguage.JpTc,
    ["JpTc"]        = EnumLanguage.JpTc,
    ["简繁"]          = EnumLanguage.ScTc,
    ["Chs&Cht"]     = EnumLanguage.ScTc,
    ["Cht&Chs"]     = EnumLanguage.ScTc,
    ["简体"]          = EnumLanguage.Sc,
    ["Chs"]         = EnumLanguage.Sc,
    ["繁体"]          = EnumLanguage.Tc,
    ["繁體"]          = EnumLanguage.Tc,
    ["Cht"]         = EnumLanguage.Tc,
    ["GB"]          = EnumLanguage.Sc,
    ["BIG5"]        = EnumLanguage.Sc,
};
```

如果没有，请自行在构造函数中扩充和修正。

##### 更多的扩展

如果要对正则的结果做二次处理，那你需要重写`CreateParsedResultSingle`和`CreateParsedResultMultiple`两个函数，保证返回结果是`ParserInfo`即可。

### 完全自定义

如果想完全靠自己来实现解析处理，可以直接重写`TryMatch`函数，只要保证结果是`ParserInfo?`即可。

