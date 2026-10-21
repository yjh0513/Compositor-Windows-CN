# 参与贡献 / Contributing

感谢你愿意帮忙改进 Compositor Windows 中文汉化版！本仓库由 `yjh0513` 维护，欢迎 bug 报告、翻译改进和 PR。

在开始之前请先了解：

- 本仓库的代码绝大部分来自 [chenguisen/Compositor](https://github.com/chenguisen/Compositor) 的 `compositor_win` 分支
  （AI 编写的 Windows 移植版），原始设计来自 [robbietilton/Compositor](https://github.com/robbietilton/Compositor)（macOS 版）。
  中文汉化由 AI 完成。代码**未经人工逐行审计**。
- 本仓库**不是** Compositor 官方版本。Windows 版的问题请提到本仓库，**不要**提到上游 robbietilton/Compositor。
- 借助 AI 编程助手完成的改动同样欢迎，请在 PR 说明里注明哪些部分由 AI 生成，并确保自己运行过构建和测试。

---

## 报告 bug

1. 先在 [issues](https://github.com/yjh0513/Compositor-Windows-CN/issues) 里搜索，确认没有重复，
   并看一眼 [README 的「已知问题」](README.md#已知问题)。
2. 使用 [Bug 报告模板](https://github.com/yjh0513/Compositor-Windows-CN/issues/new?template=bug_report.yml) 新建 issue，写清楚：
   - 使用的版本（Release 版本号，或自己编译时的提交哈希）
   - Windows 版本（Windows 10 / 11）、是便携版还是自己编译
   - 复现步骤、期望结果、实际结果
   - 截图或报错信息；如果和某个 `.comp` 工程文件有关，尽量附上一个能复现问题的最小文件
3. 翻译不准确、漏翻、用词不符合 Photoshop 中文版习惯，请使用
   [翻译问题模板](https://github.com/yjh0513/Compositor-Windows-CN/issues/new?template=translation.yml)。

## 构建与测试

需要 **.NET 10 SDK**。所有命令都在 `windows/` 目录下执行：

```bash
cd windows

# 构建整个解决方案（CI 使用 -warnaserror，提交前请保证 0 警告）
dotnet build Compositor.slnx -warnaserror --nologo

# 运行单元测试
dotnet test tests/Compositor.Core.Tests/Compositor.Core.Tests.csproj --nologo

# 无头自检：在 Avalonia 的无头平台上用指针驱动真实窗口
dotnet run --project src/Compositor.Desktop/Compositor.Desktop.csproj -- --clicks clicks.png

# 生成便携版
dotnet publish src/Compositor.Desktop/Compositor.Desktop.csproj -c Release -r win-x64 --self-contained true
```

说明：

- `--clicks` 自检目前会因为已知问题 1（参考线拖拽）报 `FAILED: the guide ended at 110, not 40`，
  这是汉化之前就存在的问题，已列入路线图。
- 如果 `ManifestFormatTests.SavingWritesSortedKeysAndReSavingIsStable` 在 Windows 上失败，通常是测试源文件以 CRLF 检出。
  仓库的 `.gitattributes` 已让 `windows/tests/**/*.cs` 以 LF 检出；旧的克隆需要重新检出这些文件（或重新 clone）。
- 更多自检命令（`--window`、`--tools`、`--dialogs` 等）见 [windows/README.md](windows/README.md)。

## 汉化规则（安全边界）

**只改「给人看的文字」，不要动任何会影响功能的字符串。** 具体：

### 可以翻译

- 菜单栏、右键菜单、按钮、窗口标题、对话框标签
- 状态栏提示（例如 `Shape: Ellipse` → `形状：椭圆`）
- 图层默认命名（例如 `Exposure 1` → `曝光度 1`）
- 快捷键表的行标题、快捷键面板的提示文字
- 枚举的**显示名**：统一放在 `windows/src/Compositor.Desktop/Labels.cs` 里做映射

### 绝对不要翻译

- **`.comp` 工程格式的字段名与取值**，例如 `"blendMode": "Normal"`、`"sampling": "High quality"`、
  `"alignment": "Left"`、`"format": "com.compositor.project"` —— 必须保持全 ASCII，
  否则会和 macOS 版**互相打不开文件**
- **枚举名本身**（如 `ShapeKind.Ellipse`）—— 只加显示名映射，不改枚举
- **Avalonia 的按键名**（如 `"OemOpenBrackets"`、`"Left"`、`"Space"`）

> ⚠️ 快捷键表里 `("Left", "Left")` 的**第二个** `"Left"` 是 Avalonia 的按键名，第一个才是显示文字。
> 如果两个都翻成中文，`Enum.TryParse<Avalonia.Input.Key>("左")` 会失败，对应快捷键会**静默失效**，
> 而编译、启动、自检都不会报错。

用词请尽量与 Photoshop 中文版保持一致。如果改动了被测试断言的界面文字，请同步更新对应测试的期望值，
并在 PR 里说明。

## 提交 PR

1. Fork 本仓库，从 `main` 新建分支（例如 `fix/guide-drag`、`l10n/filter-dialog`）。
2. 一个 PR 只做一件事，尽量保持改动小而清楚。
3. 遵循周围代码的风格：命名、注释风格与密度（代码注释用英文，界面文字用中文）。
4. 提交前完成下面的检查清单（PR 模板里也有）：
   - [ ] `dotnet build Compositor.slnx -warnaserror` 通过，0 警告
   - [ ] `dotnet test` 通过（如有失败，说明是否为已知问题）
   - [ ] 没有翻译 `.comp` 格式字段 / 取值、枚举名、Avalonia 按键名
   - [ ] 涉及界面的改动附了截图
   - [ ] 注明了哪些部分由 AI 生成（如有）
   - [ ] 必要时更新了 `README.md` / `CHANGELOG.md` 的 `Unreleased` 部分
5. 所有贡献按本仓库的 MIT 许可证发布，请勿删除或修改 `LICENSE` 和 `ATTRIBUTION.md` 中的版权与来源信息。

---

## English (short)

This repository is a Chinese-localized build of the Windows port of Compositor, maintained by `yjh0513`.
Bug reports, translation fixes and pull requests (including AI-assisted ones) are welcome.

- **Bugs:** search existing issues first, then open one with the bug report template: version, Windows version,
  steps to reproduce, expected vs. actual result, screenshots or a minimal `.comp` file if relevant.
  Windows-port issues belong here, not in the upstream robbietilton/Compositor repository.
- **Build and test** (.NET 10 SDK, from `windows/`):
  `dotnet build Compositor.slnx -warnaserror` and
  `dotnet test tests/Compositor.Core.Tests/Compositor.Core.Tests.csproj`.
  The headless `--clicks` self-check currently fails on the known guide-drag bug.
- **Localization rules:** translate only human-facing text. Never translate `.comp` format keys or values,
  enum names, or Avalonia key names; add display names in `Labels.cs` instead.
- **Pull requests:** one change per PR, zero build warnings, tests passing, screenshots for UI changes, and say
  which parts were AI-generated. Keep the MIT licence and attribution intact.
