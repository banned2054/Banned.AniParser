# Changelog

All notable changes to this project will be documented in this file.  

This format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),  and this project adheres to [Semantic Versioning](https://semver.org/).

## 📘 Versions

- [🚀 Release v0.5.0 — Parser API & Group Support Update](#-release-v050--parser-api--group-support-update)

- [🚀 Release v0.4.0 — Enhanced Group Reliability & New Subtitle Support](#-release-v040--enhanced-group-reliability--new-subtitle-support)

- [🛠️ Release v0.3.1 — Improved Origin Group Matching](#-release-v031--improved-origin-group-matching)
- [🚀 Release v0.3.0 — Enhanced Metadata Parsing & Regex Performance](#-release-v030--enhanced-metadata-parsing--regex-performance)

## 🚀 Release v0.5.0 — Parser API & Group Support Update

Release Date: 2026-06-16

This release covers all changes since **v0.4.0**. It adds TSDM字幕组 support, introduces optional parallel batch parsing, standardizes source metadata as an enum, improves language/subtitle detection performance, and includes several parser reliability fixes.

### ✨ Added

- Added `TSDM字幕组` parser support, including `[TSDM]` file names and `【TSDM字幕组】` Mikan RSS titles.
- Added language and subtitle-type detection for common TSDM tags such as `CHS_JP&CHT_JP`, `CHS_JP`, `CHT_JP`, `简日双语内嵌`, `繁日双语内嵌`, and `简繁日内封字幕`.
- Added optional parallel parsing support to `ParseBatch`.
- Added `EnumSource` and JSON conversion support for normalized source metadata.

### 🔧 Changed

- Changed `ParseResult.Source` from a raw source string to the normalized `EnumSource` value.
- Registered TSDM as a default translation parser and normalized `TSDM` / `TSDM字幕组` group names to `TSDM字幕组`.
- Migrated the solution file from `.sln` to `.slnx`.
- Updated the native build GitHub Actions workflow dependencies.
- Added a separate manual GitHub Actions workflow for experimental Windows ARM64 NativeAOT builds.
- Simplified empty collection returns using collection expressions.
- Updated the supported group list to include TSDM字幕组.

### ⚡ Performance

- Optimized language and subtitle detection with `Span`-based matching to reduce string allocations.

### 🐞 Fixed

- Fixed matched source propagation so parsers return the actual detected source instead of the default source.
- Fixed subtitle language detection edge cases.
- Fixed VCB-Studio color bit depth parsing.
- Updated several parser rules to use the normalized source enum path, including ANK-Raws, jsum, Moozzi2, Orion, UHA-Wing, and VCB-Studio.

### 🧪 Tests

- Added coverage for enum source serialization/deserialization and source matching.
- Added regression coverage for TSDM file-name parsing and Mikan RSS title parsing.

### 📦 Notes

- `ParseResult.Source` now exposes `EnumSource`. Existing consumers that treated `Source` as a string should switch to the enum value or use the JSON converter/string utility path for serialized output.

## 🚀 Release v0.4.0 — Enhanced Group Reliability & New Subtitle Support
Release Date: 2026-02-18
This release focuses on improving the robustness of the parsing engine by addressing issues with optional group matching. It ensures more stable recognition for "StudioGreenTea" by refining quantifier logic and expands the library's support to include "S1YURICON".

###  ✨ Added
New Subtitle Group Support:
- S1百综字幕组 (S1YURICON): Added dedicated parsing rules to correctly identify and extract metadata for S1YURICON releases.

### 🐞 Fixed
Optional Group Quantifier Logic:

- Fixed a bug where matching would fail when certain metadata tags were missing. By applying optional quantifiers (`?`) to non-capturing groups, the parser now correctly handles filenames with or without these specific segments.
StudioGreenTea Recognition:
- Resolved a recurring failure when parsing 绿茶字幕组 (StudioGreenTea) releases, specifically in scenarios where optional bracketed information was omitted.

### 🔧 Changed
Regex Pattern Robustness:

- Refactored internal regex structures to prioritize non-breaking matches, ensuring that the absence of a single metadata field (like a group alias) doesn't cause the entire parsing process to fail.

### 📦 Notes
This update is a recommended upgrade for users processing diverse release sources. It maintains 100% API compatibility with the v0.3.x branch. No changes to existing code-behind are necessary.

## 🛠️ Release v0.3.1 — Improved Origin Group Matching

Release Date: 2026-02-16

This patch release focuses on refining the parsing logic for specific release groups to ensure higher metadata accuracy, specifically for the **Origin (Origin Origin / 猎户手抄组)** patterns.

---

### 🐞 Fixed

- **Release Group Recognition**:
  - Improved the extraction of the **Origin (Origin Origin / 猎户手抄组)** group tag.
  - Added new Regex patterns to correctly identify these releases in scenarios where the group name was previously misidentified or skipped.

---

### 🔧 Changed

- **Parser Refinement**:
  - Updated the internal `Orion` (猎户) parser logic to handle the specific "Origin" naming convention, ensuring seamless metadata extraction for these filenames.

---

### 📦 Notes

This is a minor patch to improve the out-of-the-box experience for users tracking **Origin** group releases. It maintains full API compatibility with **v0.3.0**. No code changes are required for existing integrations.

## 🚀 Release v0.3.0 — Enhanced Metadata Parsing & Regex Performance

**Release Date:** 2026-01-11

This release brings a major overhaul to the core parsing engine, switching to **Source Generated Regex** for better performance. It also significantly expands the `ParseResult` model with detailed media metadata (Codec, Bit Depth, Localized Titles) and introduces **static shared instances** for default parsers to reduce memory overhead.

> ⚠️ **Note:** This version adopts **Semantic Versioning**. The version number has jumped from v0.2.x to v0.3.0 to reflect the new feature set and API changes.

---

### ✨ Added

- **New Metadata Fields in `ParseResult`**:
  - `OriginalTitle` – Stores the raw/original title string.
  - `Titles` – A list of localized titles (`List<LocalizedTitle>?`) parsed from the filename.
  - `VideoCodec` – Identifies the video stream format (e.g., HEVC, AVC).
  - `AudioCodec` – Identifies the audio stream format (e.g., AAC, FLAC).
  - `ColorBitDepth` – Extracts the color bit depth (e.g., 8-bit, 10-bit).

- **New Parser Support**:
  - Added support for **Orion (猎户发布组)** parsing rules.

---

### 🔧 Changed

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

---

### 📦 Notes

This release focuses on **performance** and **data richness**.
Developers upgrading to v0.3.0 should note that `ParseResult` now returns more data fields. If you were implementing custom `BaseParser` classes, it is recommended (but not required) to update your regex patterns to use the new standard group names (`vCodec`, `aCodec`, `rate`) to take advantage of the auto-filling properties.
