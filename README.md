# Banned.AniParser

[**中文文档**](https://github.com/banned2054/Banned.AniParser/blob/master/Docs/README.md) | [**English Doc**](https://github.com/banned2054/Banned.AniParser/blob/master/README.md)

**Banned.AniParser** is a .NET library designed for parsing anime file names. It specializes in identifying and extracting key metadata (such as titles, episode numbers, resolutions, and codecs) from complex file naming conventions used by fansub and release groups.

> **Note**: This parser is currently optimized for **Chinese fansub naming conventions** (e.g., VCB-Studio, Nekomoe).

## Features

- **Multi-Group Support**: Handles naming schemes from various fansub/release groups.
- **Extensible Architecture**: Easily add custom parsers for specific naming patterns.
- **Built-in Parsers**: Pre-configured support for major release groups.
- **Batch Processing**: Efficiently parses lists of file names.
- **Customizable**: Configurable parsing rules and globalization settings.

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package Banned.AniParser
```

## Usage

### Basic Usage

```csharp
using Banned.AniParser;

// Create a parser instance
var parser = new AniParser();

// Parse a single file name
var result = parser.Parse("[Group] Anime Title - 01 [1080p].mp4");

// Parse a batch of file names
var fileNames = new[] { "File1.mp4", "File2.mp4" };
var results = parser.ParseBatch(fileNames);
```

### Custom Configuration

```csharp
var parser = new AniParser(options =>
{
    options.UseDefaultParsers = true;  // Enable default built-in parsers
    options.CustomParsers.Add(new YourCustomParser());  // Register a custom parser
    options.Globalization = EnumChineseGlobalization.Simplified;  // Convert Traditional Chinese titles to Simplified
});
```

### Listing Supported Groups

```csharp
var parser = new AniParser();
var groups = parser.GetParserList();
```

## Built-in Support

The library currently includes built-in support for the following groups (sorted alphabetically). For a complete list, please refer to the [Supported Groups List](https://github.com/banned2054/Banned.AniParser/blob/master/Docs/SupportedGroups.md)

- 北宇治字幕组
- 喵萌奶茶屋
- 桜都字幕组
- ANi
- jsum
- Kirara Fantasia
- Vcb-Studio

## Custom Parsers

You can create your own parser by inheriting from the `BaseParser` class:

```csharp
public class YourCustomParser : BaseParser
{
    public override string GroupName => "YourGroupName";
    public override EnumGroupType GroupType => EnumGroupType.Transfer; // Enum: Transfer, Translator, or Compression

    public YourCustomParser()
    {
        SingleEpisodePatterns = new List<Regex>
        {
            new(
                @"\[YourGroup\](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\[(?<resolution>\d+[pP])\]\[[^\[\]]+\]\[(?<lang>.+?)\]\[(?<source>[a-zA-Z]+[Rr]ip)\]",
                RegexOptions.IgnoreCase),
        };
    }
}
```

For more details on extending the library, please view the [Extension Documentation](https://github.com/banned2054/Banned.AniParser/blob/master/Docs/Extension.md).

## License

This project is licensed under the Apache-2.0 License. See the [LICENSE](https://github.com/banned2054/Banned.AniParser/blob/master/LICENSE) file for details.

## Contribution

Issues and Pull Requests are welcome!

## Support

If you encounter any issues while using this library, please open an Issue on GitHub.
