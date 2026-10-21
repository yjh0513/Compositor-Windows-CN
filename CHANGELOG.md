# 更新日志 / Changelog

本文件记录 `yjh0513/Compositor-Windows-CN` 的变更，条目只根据仓库的真实 git 历史整理。
格式参考 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.1.0/)。

版本号规则：`v<对应的 macOS 上游版本>-cn.<本仓库发布序号>`，例如 `v1.3.7-cn.1`。

## [Unreleased]

### 新增
- `CONTRIBUTING.md`：如何报告 bug、构建与测试命令、汉化安全边界、PR 检查清单（中文 + 简短英文）。
- Issue 模板（Bug 报告、功能建议、翻译问题）与 PR 模板（`.github/`）。
- `CHANGELOG.md`（本文件）。
- `.gitattributes`：`* text=auto`，并让 `windows/tests/**/*.cs` 始终以 LF 检出，
  用于修复 `ManifestFormatTests.SavingWritesSortedKeysAndReSavingIsStable` 在 CRLF 检出下失败的问题（待在 Windows 上验证）。

### 变更
- `README.md`：维护状态改为由 `yjh0513` 维护（处理 issue、PR 与发布）；新增英文摘要、路线图、参与贡献章节；
  更新已知问题 2 的说明。代码来源与原作者信息保持不变。
- `windows/README.md`：顶部中文说明中的维护状态同步更新（chenguisen 的英文原文未改动）。
- `AGENTS.md`：顶部新增 Windows 移植版的构建 / 测试说明。

未修改任何 C# 源代码。

## [v1.3.7-cn.1] - 2026-10-08

首个中文汉化版本，基于 chenguisen 的 Windows 移植版（对应 macOS 1.3.7）。

### 新增
- 由 AI 完成的界面中文化（按 Photoshop 中文版习惯）：菜单、右键菜单、对话框、面板、工具选项栏、
  约 40 条状态栏消息、图层默认命名、快捷键表标题与校验提示（`a46d322`）。
- `windows/src/Compositor.Desktop/Labels.cs`：枚举中文显示名的集中映射（`a46d322`）。
- 中文说明文档 `README.md`（`a46d322`）。

### 变更
- 3 个测试文件同步更新被汉化的期望值（`a46d322`）。
- README 补充便携版直接下载链接与 SHA256 / MD5 校验值（`d48146b`，2026-10-08，打 tag 之后的提交）。

## 汉化之前的历史 / Before the localization

在此之前的提交来自 Windows 移植者 **chenguisen**（`compositor_win` 分支，截至 2026-09-30 的 `c51be1e`）
以及 macOS 上游 **robbietilton/Compositor**（包括 Robbie 及其他贡献者的提交）。
这些变更请分别查看 [chenguisen/Compositor](https://github.com/chenguisen/Compositor) 与
[robbietilton/Compositor](https://github.com/robbietilton/Compositor) 的提交历史，此处不重复记录。

[Unreleased]: https://github.com/yjh0513/Compositor-Windows-CN/compare/v1.3.7-cn.1...HEAD
[v1.3.7-cn.1]: https://github.com/yjh0513/Compositor-Windows-CN/releases/tag/v1.3.7-cn.1
