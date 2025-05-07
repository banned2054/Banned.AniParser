# Banned.AniParser

Banned.AniParser 是一个用于解析动画文件名的 .NET 库，专门设计用于识别和提取动画文件名中的关键信息。

## 功能特点

- 支持多个字幕组/压制组的命名规则解析
- 可扩展的解析器架构
- 内置多个常用字幕组的解析支持
- 支持批量文件解析
- 可自定义解析规则

## 安装

通过 NuGet 包管理器安装：

```bash
dotnet add package Banned.AniParser
```

## 使用方法

### 基本使用

```csharp
// 创建解析器实例
var parser = new AniParser();

// 解析单个文件名
var result = parser.Parse("你的动画文件名.mp4");

// 批量解析文件名
var fileNames = new[] { "文件1.mp4", "文件2.mp4" };
var results = parser.ParseBatch(fileNames);
```

### 自定义配置

```csharp
var parser = new AniParser(options =>
{
    options.UseDefaultParsers = true;  // 使用默认解析器
    options.CustomParsers.Add(new YourCustomParser());  // 添加自定义解析器
});
```

### 获取支持的字幕组列表

```csharp
var parser = new AniParser();
var groups = parser.GetParserList();
```

## 内置解析器支持

目前支持以下字幕组/压制组的命名规则(按字典顺序)：

- 北宇治字幕组
- 拨雪寻春
- 动漫国字幕组
- 喵萌奶茶屋
- 霜庭云花
- 雪飘工作室
- 亿次研同好会
- 桜都字幕组
- 樱桃花字幕组
- ANi
- Kirara Fantasia
- LoliHouse
- MingYSub
- SweetSub

## 自定义解析器

你可以通过实现 `BaseParser` 类来创建自己的解析器：

```csharp
public class YourCustomParser : BaseParser
{
    public override string GroupName => "你的字幕组名称";

    protected override (bool success, ParserInfo? result) TryMatch(string filename)
    {
        // 实现你的解析逻辑
    }
}
```

## 许可证

本项目采用 Apache-2.0 许可证。详情请参见 [LICENSE](LICENSE) 文件。

## 贡献

欢迎提交 Issue 和 Pull Request！

## 支持

如果你在使用过程中遇到任何问题，请创建 Issue。
