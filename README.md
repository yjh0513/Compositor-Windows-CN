# Compositor Windows 中文汉化版

**免费、开源的 Windows 图像编辑器，界面已按 Photoshop 中文版习惯汉化。**
原版 Compositor 是 macOS 上「买不起 Photoshop」的替代品，这个仓库提供的是它的 **Windows 移植版 + 中文界面**。

---

> **English summary.** Compositor Windows CN is a Chinese-localized build of the Windows port of
> [Compositor](https://github.com/robbietilton/Compositor), a free, open-source image editor originally written
> for macOS by **robbietilton** (Wonder Assembly LLC, MIT licence). The Windows port (C# / .NET 10 / Avalonia),
> which accounts for nearly all of the code here, was made by **chenguisen** with AI assistance; the Chinese
> localization in this repository was also done by AI, and none of the code has been audited line by line by a
> human. The repository is now maintained by **yjh0513**, who reviews issues and pull requests and publishes
> releases. It is not an official Compositor release and is not endorsed by the upstream author. See the
> [Roadmap](#路线图--roadmap), [CONTRIBUTING.md](CONTRIBUTING.md) and [CHANGELOG.md](CHANGELOG.md).

---

## ⚠️ 开篇必读

### 1. 代码由 AI 编写 / 汉化，未经人工逐行审计

**代码不是我手写的。** 具体说：

| 部分 | 谁写的 |
|---|---|
| macOS 原版 Compositor | 原作者 **robbietilton**（Wonder Assembly LLC），人类 |
| Windows 移植版（C# / Avalonia） | **AI 编写**（由移植者 chenguisen 用 AI 完成，见其 README 自述） |
| **中文汉化（本仓库的改动）** | **AI 完成**（WorkBuddy / Claude 系模型），全部界面文案与汉化后的测试期望值 |
| 上传、打包、维护本仓库 | `yjh0513`（负责审阅、合并与发布，没有手写代码） |

项目自带 848 个单元测试和一整套无头自检，但代码**没有经过人工逐行审计**，请自行评估风险。

### 2. 维护状态

本仓库现由 **`yjh0513` 维护**：会查看并回复 issue、审阅并合并 PR、发布新的 Release。
计划中的工作见下方 [路线图](#路线图--roadmap)，变更记录见 [CHANGELOG.md](CHANGELOG.md)。
这是个人业余维护，响应速度无法保证，也不是 Compositor 官方版本（见下文「代码来源与原作者」）。

### 3. 想自己动手修？可以借助 AI 助手，然后提 PR

这个项目的代码注释非常详尽（每个类、每个方法都写了「为什么这么做」），很适合交给 AI 编程助手阅读和修改。
即使你不熟悉 C#，也可以：

```bash
git clone https://github.com/yjh0513/Compositor-Windows-CN.git
cd Compositor-Windows-CN/windows
```

然后打开任意一个 AI 编程助手（WorkBuddy / Claude Code / Codex / Cursor / Copilot 等），
把**这个文件夹**交给它，用大白话描述问题，例如：

> 「我在 Windows 上编译这个项目，报了这个错：`<粘贴报错>`，帮我修好。」
> 「裁剪工具的选框在缩放超过 400% 后就对不齐了，帮我找到原因并修掉。」
> 「帮我把界面里还剩下的英文文案也翻成中文。」

改好并确认 `dotnet build` / `dotnet test` 通过后，欢迎**提交 PR**，说明里注明哪些部分由 AI 生成即可。
详见 [参与贡献](#参与贡献) 与 [CONTRIBUTING.md](CONTRIBUTING.md)。

---

## 这是什么

[Compositor](https://github.com/robbietilton/Compositor) 是一个开源的图像编辑器，
定位是 macOS 上 Photoshop 的免费替代品，在 GitHub 上有 **13,000+ star**。

本仓库是它的 **Windows 移植版（C# / .NET 10 / Avalonia）**，并且：

- ✅ **界面基本全部中文**（少量残留英文文案见[路线图](#路线图--roadmap)），按 Photoshop 中文版的用词与习惯翻译（菜单栏是 `文件(F) 编辑(E) 图层(L) 图像(I) 滤镜(T) 工具(O) 选择(S) 视图(V) 帮助(H)`）
- ✅ **读写与 macOS 版相同的 `.comp` 工程格式**（格式版本 v11），两个版本的文件可以互相打开
- ✅ **功能完整**：图层、蒙版、混合模式、选区、画笔、仿制图章、修复画笔、渐变、形状、文字、
  裁剪、自由变换、曲线 / 色阶 / 色相饱和度等调整图层、各种滤镜、Camera Raw、抖动、色彩范围……
- ✅ **免费、开源、无广告、无内购、不需要联网激活**

---

## 代码来源与原作者（重要）

这份代码的谱系是三层，**请在使用与转载时保留这些信息**：

| 层 | 项目 / 作者 | 说明 |
|---|---|---|
| ① **原始版本** | [**robbietilton/Compositor**](https://github.com/robbietilton/Compositor) <br>作者：**robbietilton**（Wonder Assembly LLC） | **真正的原作者。** macOS 版，Swift + AppKit，MIT 许可证，13k+ star。全部功能设计、文档模型、`.comp` 格式都出自这里 |
| ② **Windows 移植** | [**chenguisen/Compositor**](https://github.com/chenguisen/Compositor) 的 `compositor_win` 分支 <br>作者：**chenguisen** | 把 macOS 版**手写重写**成 C# / .NET 10 / Avalonia。**这个仓库的代码 90% 以上出自这里** |
| ③ **中文汉化 + 分发** | **本仓库**（`yjh0513/Compositor-Windows-CN`） <br>汉化：**AI**；上传：**yjh0513** | 只改了「给人看的文字」，以及配套的测试期望值 |

**关于原作者的明确态度：**
Compositor 的原作者 robbietilton 在 [issue #87](https://github.com/robbietilton/Compositor/issues/87) 里说明过 ——
他只有 Mac、没有 Windows 机器，无法构建、测试或维护 Windows 版本，
因此 Windows 版**永远不会合并进官方仓库**，只接受「另起仓库」的做法。
**所以本仓库不是官方版本，官方也不为它背书。** 官方只维护 macOS 版。

**移植版的版本基线：** 对应 macOS 版的 **1.3.7**。
macOS 官方已经更新到更高版本，**本移植版没有跟进**，属于滞后版本，功能不是完全对等。

---

## 下载与运行

### 方式一：下载打包好的便携版（推荐，不需要装任何东西）

**[⬇ 点这里直接下载 `Compositor-Windows-CN-portable.zip`（59 MB）](https://github.com/yjh0513/Compositor-Windows-CN/releases/latest/download/Compositor-Windows-CN-portable.zip)**

解压后双击 `Compositor.Desktop.exe` 即可运行。（也可以从 [Releases](releases) 页面下载）

```
SHA256  e91612f553c6fb878d742b10b0aa9c58f00b625c94db4dcadcc4e227f3fac887
MD5     d87dec1636a66f72255b09c853937ba4
```

- 需要 **64 位 Windows 10 / 11**
- 包内已自带 .NET 运行时，**不用另外安装任何环境**
- 完全绿色，不写注册表、不装驱动，删掉文件夹就是卸载
- 包内含 `使用说明.txt`、`LICENSE`、`ATTRIBUTION.md`

### 方式二：自己编译（需要 .NET SDK 10）

```bash
git clone https://github.com/yjh0513/Compositor-Windows-CN.git
cd Compositor-Windows-CN/windows

# 编译
dotnet build src/Compositor.Desktop/Compositor.Desktop.csproj -c Release

# 运行
./src/Compositor.Desktop/bin/Release/net10.0/Compositor.Desktop.exe
```

生成单文件便携版：

```bash
dotnet publish src/Compositor.Desktop/Compositor.Desktop.csproj \
  -c Release -r win-x64 --self-contained true
```

---

## 中文汉化改了什么

**只改了「给人看的文字」，没有动任何会影响功能的字符串。** 这是汉化最容易踩坑的地方，
详细说明如下（如果你要在此基础上继续改，请务必遵守）：

### 改了

- 菜单栏、右键菜单、按钮、窗口标题、对话框标签
- **状态栏提示**（约 40 条），例如 `Shape: Ellipse` → `形状：椭圆`、`Brush: 24 pixels` → `画笔：24 像素`
- 图层默认命名（`Exposure 1` → `曝光度 1`）
- 快捷键表的行标题，以及快捷键面板的校验提示
- 新增 `windows/src/Compositor.Desktop/Labels.cs`，集中存放枚举的中文显示名
- 配套修正了 3 个测试文件里被汉化的期望值

### 绝对没有改（这是安全边界）

- **`.comp` 工程格式的字段名与取值**，例如 `"blendMode": "Normal"`、`"sampling": "High quality"`、
  `"alignment": "Left"`、`"format": "com.compositor.project"` —— 保持全 ASCII，
  否则会和 macOS 版**互相打不开文件**
- **枚举名本身**（`ShapeKind.Ellipse` 等）—— 只加显示名映射，不改枚举
- **Avalonia 的按键名**（`"OemOpenBrackets"`、`"Left"`、`"Space"` 等）

> ⚠️ **汉化时最容易踩的一个坑，留个记录：**
> 快捷键表里 `("Left", "Left")` 的**第二个** `"Left"` 是 Avalonia 的按键名，第一个才是显示文字。
> 如果手滑把两个都翻成中文，`Enum.TryParse<Avalonia.Input.Key>("左")` 会失败，
> 这 16 行方向键快捷键会**静默失效** —— 菜单不显示按键、按键也彻底没反应，
> 而编译、启动、自检全都不会报错。本仓库已修正。

---

## 已知问题

以下问题**在汉化之前就存在**，不是汉化造成的（已通过对比原始提交验证）：

1. **参考线拖拽有缺陷**：从标尺拖出参考线后，再次拖动它时位置不跟随。
   在项目自带的自检 `--clicks` 里表现为 `FAILED: the guide ended at 110, not 40`。
2. **有 1 个单元测试在 Windows 上失败（已提交修复，待在 Windows 上验证）**：
   `ManifestFormatTests.SavingWritesSortedKeysAndReSavingIsStable`。
   原因：测试的期望值是写在 `ManifestFormatTests.cs` 源文件里的 C# 原始字符串字面量（`"""`），
   其换行符随源文件的换行符而定；而 `.comp` 写出器固定使用 `\n`。当 Windows 上 git 以 CRLF 检出
   （`core.autocrlf=true`）时，期望值里就变成了 `\r\n`，两者不相等。属于检出配置问题，**不是产品缺陷**，
   其余 **847 个测试全部通过**。
   现已新增 [`.gitattributes`](.gitattributes)，让 `windows/tests/` 下的 `*.cs` 始终以 LF 检出（`eol=lf`）。
   已有的本地克隆需要重新检出这些文件才会生效：重新 clone，或在没有未提交改动的前提下，
   删除 `windows/tests/Compositor.Core.Tests/` 下的 `*.cs` 后执行 `git checkout -- windows/tests`。
3. **旧版快捷键自定义不会迁移**：`%APPDATA%\Compositor\shortcuts.json` 里的覆盖项以
   `{组名}:{标题}` 为键，组名汉化后键变了，旧的自定义会静默回到默认值（重设一次即可）。
4. **移植版滞后于 macOS 官方版本**，部分新功能没有。

另外提醒：这是 AI 写的代码，**没有经过人工代码审计**。虽然项目自带了 848 个单元测试与
一整套无头自检来保证行为正确，但请自行评估风险。

---

## 路线图 / Roadmap

以下是近期计划，按大致优先级排列。都是计划而非承诺，进展会记录在 [CHANGELOG.md](CHANGELOG.md)。

- [ ] **修复参考线拖拽问题**（已知问题 1）：让 `--clicks` 自检里的参考线检查通过。
- [x] **修复 Windows 上 CRLF 导致的单元测试失败**（已知问题 2）：通过 `.gitattributes` 固定 LF，本次改动已完成，待在 Windows 上确认。
- [ ] **翻译剩余的英文文案**：继续遵守下方「安全边界」，只改给人看的文字。
- [ ] **在可行范围内跟进 macOS 上游 1.3.7 之后的新版本**：按移植者的做法逐个功能手工移植，不合并 macOS 代码。
- [ ] **发布时由 CI 自动构建便携版 zip**：在打 tag / 发布 Release 时自动生成 `Compositor-Windows-CN-portable.zip` 及校验值。

English: fix the guide-drag bug; fix the CRLF test failure via `.gitattributes` (done in this change, pending
verification on Windows); translate the remaining English strings; port newer upstream macOS features beyond
1.3.7 where feasible; build the portable zip in CI on release.

---

## 参与贡献

欢迎提交 bug 报告、翻译改进和 PR（包括借助 AI 助手完成的改动）。

- 贡献指南：[CONTRIBUTING.md](CONTRIBUTING.md)（编译 / 测试命令、汉化规则、PR 检查清单）
- 提 issue：[Bug 报告](https://github.com/yjh0513/Compositor-Windows-CN/issues/new?template=bug_report.yml) ·
  [功能建议](https://github.com/yjh0513/Compositor-Windows-CN/issues/new?template=feature_request.yml) ·
  [翻译问题](https://github.com/yjh0513/Compositor-Windows-CN/issues/new?template=translation.yml)
- 变更记录：[CHANGELOG.md](CHANGELOG.md)

如果问题在 macOS 原版上也存在，也可以考虑反馈给上游 [robbietilton/Compositor](https://github.com/robbietilton/Compositor/issues)；
但请**不要**把 Windows 版的问题提到上游，原作者已说明不维护 Windows 版。

---

## 许可证

本项目沿用上游的 **MIT 许可证**，版权归原作者 **Wonder Assembly LLC** 所有，
完整文本见 [LICENSE](LICENSE)。

MIT 许可证允许你自由使用、修改、再分发（包括商用），**条件只有一个**：
保留版权声明与许可证文本。

因此如果你要转载或二次分发，请务必同时保留：

- 本仓库的 [LICENSE](LICENSE)（MIT，Copyright © 2026 Wonder Assembly LLC）
- 上游来源：[robbietilton/Compositor](https://github.com/robbietilton/Compositor)
- 移植来源：[chenguisen/Compositor `compositor_win` 分支](https://github.com/chenguisen/Compositor)
- 详细谱系见 [ATTRIBUTION.md](ATTRIBUTION.md)

---

## 免责声明

本软件按 **「原样」** 提供，不附带任何明示或暗示的担保。使用本软件造成的任何损失
（包括但不限于数据丢失、图片损坏、工程文件损坏）由使用者自行承担。

**建议：处理重要图片前先备份。** 尤其是用本版本写出的 `.comp` 工程文件，
在确认 macOS 版能正常打开之前，不要删掉原图。

---

<div align="center">

**感谢原作者 [robbietilton](https://github.com/robbietilton) 做出了 Compositor，
感谢 [chenguisen](https://github.com/chenguisen) 做出了 Windows 移植版。**
本仓库在此基础上做中文汉化与日常维护。

</div>
