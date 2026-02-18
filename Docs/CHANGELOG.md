# Changelog

All notable changes to this project will be documented in this file.  

This format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),  and this project adheres to [Semantic Versioning](https://semver.org/).

## 📘 Versions

- [🚀 Release v0.4.0 — Enhanced Group Reliability & New Subtitle Support](#-release-v040--enhanced-group-reliability--new-subtitle-support)

- [🛠️ Release v0.3.1 — Improved Origin Group Matching](#-release-v031--improved-origin-group-matching)
- [🚀 Release v0.3.0 — Enhanced Metadata Parsing & Regex Performance](#-release-v030--enhanced-metadata-parsing--regex-performance)

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