# Notes for AI agents

## This repository: the Windows port (Chinese localization)

This repository (`yjh0513/Compositor-Windows-CN`) ships the **Windows port** in `windows/` (C# / .NET 10 /
Avalonia), with a Chinese-localized interface. The macOS sources below are kept for reference only and are not
built here.

- Requires the .NET 10 SDK. From `windows/`:
  - Build: `dotnet build Compositor.slnx -warnaserror --nologo` (the port is held at zero warnings).
  - Tests: `dotnet test tests/Compositor.Core.Tests/Compositor.Core.Tests.csproj --nologo`.
  - Headless window check: `dotnet run --project src/Compositor.Desktop/Compositor.Desktop.csproj -- --clicks clicks.png`
    (currently fails on the known guide-drag bug; see README.md).
- Localization: translate only human-facing text. Never translate `.comp` format keys or values, enum names, or
  Avalonia key names; enum display names go in `windows/src/Compositor.Desktop/Labels.cs`. See CONTRIBUTING.md.
- Port features from upstream by hand; do not merge the macOS `main` branch (see `windows/README.md`).
- Keep the attribution in README.md and ATTRIBUTION.md intact (robbietilton original, chenguisen port, AI localization).

## The macOS app (upstream)

Compositor is a macOS image editor for compositing and photo work, written in Swift (SwiftUI and AppKit, with some C for pixel work).

## Designing or editing a Compositor project

If you've been asked to make or change an image in a `.comp` project, you don't need the app's source code. Read [docs/writing-comp-files.md](docs/writing-comp-files.md): it covers the file format, the rules that make a project load, and how to write it safely while it's open, so the person can watch the canvas update as you work.

## Working on the app itself

- Build: open `Compositor.xcodeproj` and run the **Compositor** scheme, or `xcodebuild -project Compositor.xcodeproj -scheme Compositor -destination 'platform=macOS' build`.
- Tests: the `CompositorTests` target (`xcodebuild ... test -only-testing:CompositorTests`). CI runs these on every push.
- Match the surrounding code: its naming, its comment style and density.
- American spelling in code, comments and UI ("color", not "colour").
- The project file format is described in [docs/project-format.md](docs/project-format.md). A change to what's saved means a format version bump there and in `ProjectManifest.current`.
