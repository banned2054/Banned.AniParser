# Changelog / 更新日志

本文件记录 Banned.AniParser 各版本的重要变化。每个版本包含内容对应的简体中文与英文说明。

All notable changes to Banned.AniParser are documented here, with matching Simplified Chinese and English notes for each version.

本格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.1.0/)，本项目遵循[语义化版本控制](https://semver.org/lang/zh-CN/)。

This format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/).

## 📘 Versions / 版本

- [🚀 Release v0.7.0 — New Parser Groups & .NET 11 Support / 新增字幕组解析与 .NET 11 支持](#release-v070)
- [🚀 Release v0.6.0 — 三明治摆烂组 Parser Support / 三明治摆烂组解析支持](#release-v060)
- [🚀 Release v0.5.0 — Parser API & Group Support Update / 解析器 API 与字幕组支持更新](#release-v050)
- [🚀 Release v0.4.0 — Enhanced Group Reliability & New Subtitle Support / 字幕组匹配可靠性增强与新字幕支持](#release-v040)
- [🛠️ Release v0.3.1 — Improved Origin Group Matching / Origin 字幕组匹配改进](#release-v031)
- [🚀 Release v0.3.0 — Enhanced Metadata Parsing & Regex Performance / 元数据解析与正则性能增强](#release-v030)

<a id="release-v070"></a>

## 🚀 Release v0.7.0 — New Parser Groups & .NET 11 Support / 新增字幕组解析与 .NET 11 支持

[简体中文](#简体中文-v070) | [English](#english-v070)

Release Date / 发布日期：2026-11-26

### 简体中文 v0.7.0

此版本新增晚街与灯字幕组、ToonsHub 与 Nix-Raws 搬运组解析支持，扩展绿茶字幕组多集标题识别，新增 `JpEng` 语言枚举；同时加入 NativeAOT 兼容性验证与持续集成流程，并将目标框架扩展至 .NET 11。

#### ✨ 新增

- 新增「晚街与灯 / 晚街與燈」翻译组解析器：支持单集（`[19 - 总第85]` 取季内集数）、剧场版（无集数字段时按标题识别）、合集（标题内 `01-19` 范围或 `[总第67~85]` / `[第67~85]` 范围，以及标题含「合集」但无集数字段的情况）；来源字段缺失时默认 `WebRip`，并将繁简组名统一规范化为「晚街与灯」。
- 新增 ToonsHub 搬运组解析支持，可识别 `[ToonsHub] 标题 - 集数 [平台][分辨率][语言][MKV/MP4]` 形式的命名，包括 `UNCENSORED`、`DUAL`、`RAW` 标记以及 `CHS&CHT&EN`、`CHT&EN`、`JP&EN`、`EN` 语言标签。
- 新增 Nix-Raws 搬运组解析支持，可识别带 CR / CATCHPLAY / ABEMA / AMZN 等平台与 WEB-DL / BDRip 等来源的单集命名，并映射 `SC_TC` 语言与内封字幕类型。
- `EnumLanguage` 新增 `JpEng`（日语 + 英语），供 ToonsHub 的 `JP&EN` 标签使用。
- 绿茶字幕组解析器新增多集／合集标题支持：可解析 `[01-10]`、`[01-12 修正合集 v3]` 等集数范围，兼容字幕信息单独成括号或与其他规格同括号的写法，并支持 `HEVC-10bit`、`AVC` 等编码与 `ASSx2` 字幕标记。
- 新增 `Banned.AniParser.AotSmoke` 项目，将核心库标记为 `IsAotCompatible`，通过 NativeAOT 发布后的冒烟程序验证解析结果。
- 新增 `.github/workflows/ci.yml`：在 push 与 pull request 时构建解决方案，并以 `linux-x64` 发布、运行 NativeAOT 冒烟程序。

#### 🔧 变更

- 目标框架新增 `net11.0`（核心库为 `net11.0;net10.0;net9.0;net8.0`）；测试与 NativeAOT 项目均升级至 `net11.0`。
- 更新测试依赖：coverlet.collector 10.1.1、Microsoft.NET.Test.Sdk 18.12.0、NUnit 5.0.0、NUnit.Analyzers 4.15.0、NUnit3TestAdapter 6.3.1、RestSharp 114.1.0、Spectre.Console 0.57.2。
- 训练数据工具改用 `System.Text.Json` 替代 `Newtonsoft.Json`；RSS 测试源改为 Mikan `Classic` 分类。
- `.gitignore` 新增 `.zcodeignore` 与 `/.workbuddy` 忽略项。

#### 🧪 测试

- 新增晚街与灯解析器回归测试，覆盖剧场版、单集、繁体组名与标题、合集范围、标题内范围优先、无集数合集及缺失来源字段等场景。
- NativeAOT 冒烟程序校验示例文件名解析出的标题与集数；CI 工作流在 `linux-x64` 上自动执行该程序。

### English v0.7.0

This release adds parser support for the 晚街与灯 subtitle group and the ToonsHub and Nix-Raws transfer groups, extends StudioGreenTea multi-episode title recognition, and adds a `JpEng` language enum value. It also introduces NativeAOT compatibility verification with continuous integration and expands the target frameworks to include .NET 11.

#### ✨ Added

- Added a parser for the 晚街与灯 / 晚街與燈 translation group: single episodes (`[19 - 总第85]`, using the in-season episode number), movies (recognized from the title when no episode field is present), and multi-episode releases (a `01-19` range in the title, `[总第67~85]` / `[第67~85]`, or a title containing 「合集」 with no episode field). A missing source field now defaults to `WebRip`, and simplified/traditional group names are normalized to 「晚街与灯」.
- Added ToonsHub transfer-group parsing for names in the form `[ToonsHub] title - episode [platform][resolution][language][MKV/MP4]`, including `UNCENSORED`, `DUAL`, and `RAW` flags and the `CHS&CHT&EN`, `CHT&EN`, `JP&EN`, and `EN` language tags.
- Added Nix-Raws transfer-group parsing for single episodes with platforms such as CR / CATCHPLAY / ABEMA / AMZN and sources such as WEB-DL / BDRip, mapping `SC_TC` to the language and muxed subtitle type.
- Added `EnumLanguage.JpEng` (Japanese + English) for ToonsHub's `JP&EN` tag.
- Added multi-episode title support to the StudioGreenTea parser: episode ranges such as `[01-10]` and `[01-12 修正合集 v3]`, subtitle information either in its own brackets or alongside other specs, and codecs such as `HEVC-10bit` / `AVC` with the `ASSx2` subtitle tag.
- Added the `Banned.AniParser.AotSmoke` project, marked the core library as `IsAotCompatible`, and added a smoke app that validates parsing after NativeAOT publishing.
- Added `.github/workflows/ci.yml` to build the solution on push and pull request, then publish and run the NativeAOT smoke app for `linux-x64`.

#### 🔧 Changed

- Added `net11.0` to the target frameworks (`net11.0;net10.0;net9.0;net8.0` for the core library); test and NativeAOT projects now target `net11.0`.
- Updated test dependencies: coverlet.collector 10.1.1, Microsoft.NET.Test.Sdk 18.12.0, NUnit 5.0.0, NUnit.Analyzers 4.15.0, NUnit3TestAdapter 6.3.1, RestSharp 114.1.0, and Spectre.Console 0.57.2.
- Switched the training-data tool from `Newtonsoft.Json` to `System.Text.Json`; changed the RSS test feed to the Mikan `Classic` category.
- Added `.zcodeignore` and `/.workbuddy` to `.gitignore`.

#### 🧪 Tests

- Added regression coverage for the 晚街与灯 parser, covering movies, single episodes, simplified/traditional group names and titles, episode ranges, title-range precedence, collections without an episode field, and missing source fields.
- The NativeAOT smoke app checks the title and episode parsed from a sample filename, and the CI workflow runs it automatically on `linux-x64`.

<a id="release-v060"></a>

## 🚀 Release v0.6.0 — 三明治摆烂组 Parser Support / 三明治摆烂组解析支持

[简体中文](#简体中文-v060) | [English](#english-v060)

Release Date / 发布日期：2026-06-26

### 简体中文 v0.6.0

此版本新增对「三明治摆烂组」发布的内置解析器支持，以及 smzase 字幕文件使用的别名。

#### ✨ 新增

- 新增 `三明治摆烂组` 解析器支持，可解析 DMHY 发布标题中的 `繁日内嵌`、`简繁日内封` 等变体。
- 新增对 smzase GitHub `.ass` 字幕文件名的支持，包括 `zh-hans` 与 `zh-hant` 轨道。
- 新增对 smzase 视频发布命名的支持，例如 `S01E01 - [CHT_JPN][WebRip H264 8bit 1080P]` 与 `S01E01 - [CHI_JPN][WebRip H265 10bit 1080P]`。

#### 🔧 变更

- 将 `smzase` 与 `三明治摆烂组` 发布标签统一为内置组名 `三明治摆烂组`。
- 将三明治摆烂组发布中的 `H264` / `H265` 编码标签规范化为 `AVC` / `HEVC`。

#### 🧪 测试

- 新增回归测试，覆盖三明治摆烂组 DMHY 标题、独立的 `.ass` 字幕文件名、合作标签，以及 `CHT_JPN` / `CHI_JPN` 视频发布。

### English v0.6.0

This release adds built-in parser support for 三明治摆烂组 releases and aliases used by smzase subtitle files.

#### ✨ Added

- Added `三明治摆烂组` parser support for DMHY release titles such as `繁日内嵌` and `简繁日内封` variants.
- Added support for smzase GitHub `.ass` subtitle file names, including `zh-hans` and `zh-hant` tracks.
- Added support for smzase video release names such as `S01E01 - [CHT_JPN][WebRip H264 8bit 1080P]` and `S01E01 - [CHI_JPN][WebRip H265 10bit 1080P]`.

#### 🔧 Changed

- Normalized `smzase` and `三明治摆烂组` release tags to the built-in group name `三明治摆烂组`.
- Normalized `H264` / `H265` codec tags in 三明治摆烂组 releases to `AVC` / `HEVC`.

#### 🧪 Tests

- Added regression coverage for 三明治摆烂组 DMHY titles, standalone `.ass` subtitle names, collaboration tags, and `CHT_JPN` / `CHI_JPN` video releases.

<a id="release-v050"></a>

## 🚀 Release v0.5.0 — Parser API & Group Support Update / 解析器 API 与字幕组支持更新

[简体中文](#简体中文-v050) | [English](#english-v050)

Release Date / 发布日期：2026-06-16

### 简体中文 v0.5.0

此版本汇总自 **v0.4.0** 以来的所有变更：新增 TSDM字幕组支持、可选的并行批量解析，将来源元数据统一为枚举，改进语言／字幕检测性能，并包含多项解析器可靠性修复。

#### ✨ 新增

- 新增 `TSDM字幕组` 解析器支持，包括 `[TSDM]` 文件名与 `【TSDM字幕组】` Mikan RSS 标题。
- 新增针对常见 TSDM 标签的语言与字幕类型检测，例如 `CHS_JP&CHT_JP`、`CHS_JP`、`CHT_JP`、`简日双语内嵌`、`繁日双语内嵌`、`简繁日内封字幕`。
- `ParseBatch` 新增可选的并行解析支持。
- 新增 `EnumSource` 及规范化来源元数据的 JSON 转换支持。

#### 🔧 变更

- `ParseResult.Source` 由原始来源字符串改为规范化后的 `EnumSource` 值。
- 将 TSDM 注册为默认翻译解析器，并将 `TSDM` / `TSDM字幕组` 组名统一为 `TSDM字幕组`。
- 将解决方案文件从 `.sln` 迁移至 `.slnx`。
- 更新原生构建 GitHub Actions 工作流的依赖。
- 新增独立的手动 GitHub Actions 工作流，用于实验性的 Windows ARM64 NativeAOT 构建。
- 使用集合表达式简化空集合返回。
- 更新支持的组列表，加入 TSDM字幕组。

#### ⚡ 性能

- 使用基于 `Span` 的匹配优化语言与字幕检测，减少字符串分配。

#### 🐞 修复

- 修复已匹配来源的传播问题，使解析器返回实际检测到的来源而非默认来源。
- 修复字幕语言检测的边界情况。
- 修复 VCB-Studio 色彩位深解析。
- 更新多个解析器规则以使用规范化来源枚举路径，包括 ANK-Raws、jsum、Moozzi2、Orion、UHA-Wing 与 VCB-Studio。

#### 🧪 测试

- 新增枚举来源序列化／反序列化及来源匹配的测试覆盖。
- 新增 TSDM 文件名解析与 Mikan RSS 标题解析的回归测试。

#### 📦 说明

- `ParseResult.Source` 现在返回 `EnumSource`。此前将 `Source` 当作字符串使用的调用方，应改用枚举值，或通过 JSON 转换器／字符串工具获取序列化输出。

### English v0.5.0

This release covers all changes since **v0.4.0**. It adds TSDM字幕组 support, introduces optional parallel batch parsing, standardizes source metadata as an enum, improves language/subtitle detection performance, and includes several parser reliability fixes.

#### ✨ Added

- Added `TSDM字幕组` parser support, including `[TSDM]` file names and `【TSDM字幕组】` Mikan RSS titles.
- Added language and subtitle-type detection for common TSDM tags such as `CHS_JP&CHT_JP`, `CHS_JP`, `CHT_JP`, `简日双语内嵌`, `繁日双语内嵌`, and `简繁日内封字幕`.
- Added optional parallel parsing support to `ParseBatch`.
- Added `EnumSource` and JSON conversion support for normalized source metadata.

#### 🔧 Changed

- Changed `ParseResult.Source` from a raw source string to the normalized `EnumSource` value.
- Registered TSDM as a default translation parser and normalized `TSDM` / `TSDM字幕组` group names to `TSDM字幕组`.
- Migrated the solution file from `.sln` to `.slnx`.
- Updated the native build GitHub Actions workflow dependencies.
- Added a separate manual GitHub Actions workflow for experimental Windows ARM64 NativeAOT builds.
- Simplified empty collection returns using collection expressions.
- Updated the supported group list to include TSDM字幕组.

#### ⚡ Performance

- Optimized language and subtitle detection with `Span`-based matching to reduce string allocations.

#### 🐞 Fixed

- Fixed matched source propagation so parsers return the actual detected source instead of the default source.
- Fixed subtitle language detection edge cases.
- Fixed VCB-Studio color bit depth parsing.
- Updated several parser rules to use the normalized source enum path, including ANK-Raws, jsum, Moozzi2, Orion, UHA-Wing, and VCB-Studio.

#### 🧪 Tests

- Added coverage for enum source serialization/deserialization and source matching.
- Added regression coverage for TSDM file-name parsing and Mikan RSS title parsing.

#### 📦 Notes

- `ParseResult.Source` now exposes `EnumSource`. Existing consumers that treated `Source` as a string should switch to the enum value or use the JSON converter/string utility path for serialized output.

<a id="release-v040"></a>

## 🚀 Release v0.4.0 — Enhanced Group Reliability & New Subtitle Support / 字幕组匹配可靠性增强与新字幕支持

[简体中文](#简体中文-v040) | [English](#english-v040)

Release Date / 发布日期：2026-02-18

### 简体中文 v0.4.0

此版本重点提升解析引擎在可选字幕组匹配方面的健壮性：通过改进量词逻辑，使「StudioGreenTea」的识别更加稳定，并新增对「S1YURICON」的支持。

#### ✨ 新增

- 新增字幕组支持：**S1百综字幕组（S1YURICON）**，添加专用解析规则以正确识别并提取该组发布文件的元数据。

#### 🐞 修复

- 可选组量词逻辑：修复某些元数据标签缺失时匹配失败的问题。通过对非捕获组应用可选量词（`?`），解析器现在能正确处理包含或不包含这些字段的文件名。
- StudioGreenTea 识别：修复解析绿茶字幕组（StudioGreenTea）发布时反复出现的失败，尤其是省略可选括号信息的情况。

#### 🔧 变更

- 正则模式健壮性：重构内部正则结构，优先保证匹配不被打断，使单个元数据字段（如组别名）缺失时不会导致整个解析过程失败。

#### 📦 说明

- 推荐处理多种发布来源的用户升级。此版本保持与 v0.3.x 分支的 100% API 兼容性，无需修改现有代码。

### English v0.4.0

This release focuses on improving the robustness of the parsing engine by addressing issues with optional group matching. It ensures more stable recognition for "StudioGreenTea" by refining quantifier logic and expands the library's support to include "S1YURICON".

#### ✨ Added

- New subtitle group support: **S1百综字幕组 (S1YURICON)** — added dedicated parsing rules to correctly identify and extract metadata for S1YURICON releases.

#### 🐞 Fixed

- Optional group quantifier logic: fixed a bug where matching would fail when certain metadata tags were missing. By applying optional quantifiers (`?`) to non-capturing groups, the parser now correctly handles filenames with or without these specific segments.
- StudioGreenTea recognition: resolved a recurring failure when parsing 绿茶字幕组 (StudioGreenTea) releases, specifically in scenarios where optional bracketed information was omitted.

#### 🔧 Changed

- Regex pattern robustness: refactored internal regex structures to prioritize non-breaking matches, ensuring that the absence of a single metadata field (like a group alias) doesn't cause the entire parsing process to fail.

#### 📦 Notes

- This update is a recommended upgrade for users processing diverse release sources. It maintains 100% API compatibility with the v0.3.x branch. No changes to existing code-behind are necessary.

<a id="release-v031"></a>

## 🛠️ Release v0.3.1 — Improved Origin Group Matching / Origin 字幕组匹配改进

[简体中文](#简体中文-v031) | [English](#english-v031)

Release Date / 发布日期：2026-02-16

### 简体中文 v0.3.1

此补丁版本专注于优化特定发布组的解析逻辑，提高元数据准确性，特别是 **Origin（Origin Origin / 猎户手抄组）** 命名模式。

#### 🐞 修复

- 发布组识别：
  - 改进 **Origin（Origin Origin / 猎户手抄组）** 组标签的提取。
  - 新增正则模式，以便在组名此前被误识别或跳过的情况下正确识别这些发布。

#### 🔧 变更

- 解析器优化：
  - 更新内部 `Orion`（猎户）解析器逻辑以处理特定的 "Origin" 命名约定，确保这些文件名可以无缝提取元数据。

#### 📦 说明

- 这是改善追踪 **Origin** 组发布体验的小型补丁，保持与 **v0.3.0** 的完整 API 兼容性，现有集成无需修改代码。

### English v0.3.1

This patch release focuses on refining the parsing logic for specific release groups to ensure higher metadata accuracy, specifically for the **Origin (Origin Origin / 猎户手抄组)** patterns.

#### 🐞 Fixed

- **Release Group Recognition**:
  - Improved the extraction of the **Origin (Origin Origin / 猎户手抄组)** group tag.
  - Added new Regex patterns to correctly identify these releases in scenarios where the group name was previously misidentified or skipped.

#### 🔧 Changed

- **Parser Refinement**:
  - Updated the internal `Orion` (猎户) parser logic to handle the specific "Origin" naming convention, ensuring seamless metadata extraction for these filenames.

#### 📦 Notes

- This is a minor patch to improve the out-of-the-box experience for users tracking **Origin** group releases. It maintains full API compatibility with **v0.3.0**. No code changes are required for existing integrations.

<a id="release-v030"></a>

## 🚀 Release v0.3.0 — Enhanced Metadata Parsing & Regex Performance / 元数据解析与正则性能增强

[简体中文](#简体中文-v030) | [English](#english-v030)

Release Date / 发布日期：2026-01-11

### 简体中文 v0.3.0

此版本对核心解析引擎进行了重大重构，改用 **Source Generated Regex** 提升性能。同时大幅扩展 `ParseResult` 模型，加入详细媒体元数据（视频／音频编码、色深、本地化标题），并引入默认解析器的**静态共享实例**以降低内存开销。

> ⚠️ **说明：** 此版本开始采用**语义化版本控制**。版本号从 v0.2.x 跳至 v0.3.0，以反映新增功能与 API 变更。

#### ✨ 新增

- `ParseResult` 新增元数据字段：
  - `OriginalTitle` – 存储原始标题字符串。
  - `Titles` – 从文件名解析出的本地化标题列表（`List<LocalizedTitle>?`）。
  - `VideoCodec` – 识别视频流格式（如 HEVC、AVC）。
  - `AudioCodec` – 识别音频流格式（如 AAC、FLAC）。
  - `ColorBitDepth` – 提取色彩位深（如 8-bit、10-bit）。
- 新增解析器支持：新增 **Orion（猎户发布组）** 解析规则。

#### 🔧 变更

- 正则引擎升级：所有内部正则模式迁移至 **.NET Source Generators**（`[GeneratedRegex]`），提升启动性能并降低大批量解析的吞吐延迟。
- 解析器实例优化：默认解析器现在使用**静态共享实例**，而非每次请求创建新对象，显著降低默认配置下的内存分配。
- 正则组名标准化：统一所有默认解析器的命名捕获组：
  - 视频编码：`vCodec`
  - 音频编码：`aCodec`
  - 色深：`rate`（或从 `Ma10p` 等 Profile 名称推导）
  - 所有内置解析器均已更新，可自动提取这些新字段。

#### 📦 说明

- 此版本聚焦**性能**与**数据丰富度**。升级到 v0.3.0 的开发者应注意 `ParseResult` 现返回更多数据字段。如果实现了自定义 `BaseParser` 类，建议（非强制）更新正则模式以使用新的标准组名（`vCodec`、`aCodec`、`rate`），从而利用自动填充属性。

### English v0.3.0

This release brings a major overhaul to the core parsing engine, switching to **Source Generated Regex** for better performance. It also significantly expands the `ParseResult` model with detailed media metadata (Codec, Bit Depth, Localized Titles) and introduces **static shared instances** for default parsers to reduce memory overhead.

> ⚠️ **Note:** This version adopts **Semantic Versioning**. The version number has jumped from v0.2.x to v0.3.0 to reflect the new feature set and API changes.

#### ✨ Added

- **New Metadata Fields in `ParseResult`**:
  - `OriginalTitle` – Stores the raw/original title string.
  - `Titles` – A list of localized titles (`List<LocalizedTitle>?`) parsed from the filename.
  - `VideoCodec` – Identifies the video stream format (e.g., HEVC, AVC).
  - `AudioCodec` – Identifies the audio stream format (e.g., AAC, FLAC).
  - `ColorBitDepth` – Extracts the color bit depth (e.g., 8-bit, 10-bit).
- **New Parser Support**:
  - Added support for **Orion (猎户发布组)** parsing rules.

#### 🔧 Changed

- **Regex Engine Upgrade**:
  - Migrated all internal Regex patterns to use **.NET Source Generators** (`[GeneratedRegex]`).
  - This improves startup performance and reduces throughput latency for high-volume parsing.
- **Parser Instance Optimization**:
  - Default parsers now use **static shared instances** instead of creating new objects per request.
  - This significantly reduces memory allocation when using the default configuration.
- **Regex Group Standardization**:
  - Unified named capture groups across all default parsers:
    - Video Codec: `vCodec`
    - Audio Codec: `aCodec`
    - Color Depth: `rate` (or derived from Profile names like `Ma10p`)
  - All built-in parsers have been updated to extract these new fields automatically.

#### 📦 Notes

- This release focuses on **performance** and **data richness**.
- Developers upgrading to v0.3.0 should note that `ParseResult` now returns more data fields. If you were implementing custom `BaseParser` classes, it is recommended (but not required) to update your regex patterns to use the new standard group names (`vCodec`, `aCodec`, `rate`) to take advantage of the auto-filling properties.
