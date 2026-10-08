# 代码来源与署名 / Attribution

本仓库是三层派生作品。**这一页是给需要追溯来源、或要做二次分发的人看的。**
（普通用户看 [README.md](README.md) 就够了。）

---

## 一、原始版本（真正的原作者）

| 项 | 内容 |
|---|---|
| 仓库 | **https://github.com/robbietilton/Compositor** |
| 作者 | **robbietilton** — Wonder Assembly LLC |
| 平台 | macOS |
| 技术栈 | Swift + AppKit（另有少量 C 像素处理代码） |
| 许可证 | MIT（Copyright © 2026 Wonder Assembly LLC） |
| 规模 | 约 51,000 行，225 个 Swift 文件，13,000+ star |

**这个项目里全部的设计与实现都源自这里：** 文档模型（`.comp` 工程格式）、分块合成引擎、
每一个工具、每一个滤镜、整个界面布局。作者自述做它的理由是
「Photoshop 太贵，GIMP 又不顺手」。

> **作者对 Windows 版的明确态度（原文出处：[issue #87](https://github.com/robbietilton/Compositor/issues/87)）：**
> 他只有 Mac、没有 Windows 机器，无法构建、测试、审阅 PR 或支持用户，
> 并认为把 Windows 代码合并进官方仓库「会让它看起来有人维护而实际没有」。
> 因此官方**永不 in-tree**，只接受「另起一个仓库，之后在 README 里挂个链接」。
>
> **结论：本仓库是社区派生仓库，不是官方版本，官方不为它背书。**

---

## 二、Windows 移植版

| 项 | 内容 |
|---|---|
| 仓库 | **https://github.com/chenguisen/Compositor** |
| 分支 | **`compositor_win`** |
| 作者 | **chenguisen** |
| 平台 | Windows |
| 技术栈 | C# / .NET 10 / Avalonia 12.1.3 / SkiaSharp（另用到 Magick.NET、Sdcb.LibRaw、Svg.Skia） |
| 规模 | 约 202 个 `.cs` 文件、50,519 行；848 个 xunit 测试 + 一整套无头自检程序 |
| 许可证 | MIT（继承上游） |
| 对应的 macOS 版本 | **1.3.7**（滞后于官方最新版，功能非完全对等） |

这是把 macOS 版**逐一手写重写**到 .NET 的移植工程，不是套壳、不是模拟器。
它能读写与 macOS 版**相同的 `.comp` 工程文件**。

**本仓库的代码绝大部分（90% 以上）出自这里** —— 包括合成引擎、所有工具、所有滤镜、
对话框、快捷键系统与测试套件。

> 移植者自述该移植版**主要借助 AI（Codex 等）开发**，且未经验证与 Mac 版对等、未代码签名。

**本仓库在此基础上只做了一件事：中文汉化 + 打包分发。**

---

## 三、中文汉化与分发（本仓库）

| 项 | 内容 |
|---|---|
| 仓库 | **https://github.com/yjh0513/Compositor-Windows-CN** |
| 汉化执行 | **AI**（WorkBuddy 调用的 Claude 系模型） |
| 上传者 | **yjh0513**（仅做搬运与打包，未手写任何代码） |
| 汉化日期 | 2026-10-08 |
| 改动规模 | 31 个文件修改 + 1 个新增文件（`Labels.cs`），+1279 / −1207 行 |

### 本次改动清单

| 文件 | 改动 |
|---|---|
| `windows/src/Compositor.Desktop/MainWindow.cs` | 菜单、状态栏、面板文案（+617 / −583） |
| `windows/src/Compositor.Desktop/CameraRawPanel.cs` | Camera Raw 面板文案 |
| `windows/src/Compositor.Core/IO/Shortcuts.cs` | 快捷键表标题 + 校验提示 |
| `windows/src/Compositor.Desktop/Labels.cs` | **新增**：枚举中文显示名单源映射 |
| 其余 27 个文件 | 对话框 / 面板 / 工具选项栏文案，及 3 个测试文件的期望值 |
| `README.md` | 上游 macOS README 已移入 `README-MACOS-UPSTREAM.md`，本仓库根 README 改为中文 |

### 汉化的安全边界（二次修改必读）

以下内容**一律保持 ASCII 原样**，因为它们同时是程序标识符或文件格式常量：

- `.comp` 格式字段名与取值：`"blendMode": "Normal"`、`"sampling": "High quality"`、
  `"alignment": "Left"`、`"format": "com.compositor.project"`、`"colorSpace": "sRGB"`
  （共 21 个 `[JsonStringEnumMemberName]` 属性）
- C# 枚举名本身（`ShapeKind.Ellipse`、`GradientShape.Radial`、`HealingMode.ContentAware` …）
- Avalonia 按键名（`"OemOpenBrackets"`、`"Left"`、`"Space"`、`"Delete"` …）
- `%APPDATA%\Compositor\shortcuts.json` 的持久化键格式

**违反这条边界不会编译报错，而是会在运行期静默失效。**
最典型的一例：把快捷键表 `("Left", "Left")` 的第二个 `"Left"` 也翻成中文，
会让 16 行方向键快捷键变成不可按（`Enum.TryParse<Avalonia.Input.Key>` 解析失败），
而编译、启动、自检全部正常。本仓库已修正该问题。

### 验证结果

| 验证项 | 结果 |
|---|---|
| 编译（Desktop / Cli / Tests） | 0 警告 0 错误 |
| 无头自检 8 项 | 全部通过 |
| 单元测试 | 847 通过 / 1 失败（该 1 项为上游已存在的 CRLF 检出问题） |
| 文件格式线名扫描 | 21 个属性，0 处中文 |
| 端到端落盘核对 | `manifest.json` 线名全 ASCII，中文仅在 `name` / `content` 等用户数据字段 |
| 渲染往返 SHA256 | 原工程与「读回再写」的工程渲染结果完全一致（`b276cad7…`） |
| 自检 `--clicks` | 失败，**为上游既有缺陷**（参考线拖拽），已在原始提交上复现确认 |

---

## 四、需要保留的署名

任何二次分发、转载、或基于本仓库继续修改，请保留：

1. [LICENSE](LICENSE) 全文（MIT，Copyright © 2026 Wonder Assembly LLC）
2. 指向 **https://github.com/robbietilton/Compositor** 的链接（原作者）
3. 指向 **https://github.com/chenguisen/Compositor** 的链接（Windows 移植者）
4. 本页（或等效的来源说明）

MIT 许可证允许商业使用与再分发，条件仅为**保留版权声明与许可证文本**。
