using Compositor.Core.Document;
using Compositor.Core.Format;

namespace Compositor.Desktop;

/// <summary>
/// What the enums are called where a person reads them. The enums' own names belong to the file format and to
/// the commands, so they are never shown: a status line says what the thing is called in this language rather
/// than naming the value behind it. One place per family, so a menu row and the status line cannot drift.
/// </summary>
internal static class Labels
{
    /// <summary>What the Shape tool is drawing.</summary>
    internal static string Shape(ShapeKind kind) => kind switch
    {
        ShapeKind.Rectangle => "矩形",
        ShapeKind.Ellipse => "椭圆",
        _ => "直线",
    };

    /// <summary>How the Gradient tool's colour runs.</summary>
    internal static string Gradient(GradientShape shape) =>
        shape == GradientShape.Radial ? "径向" : "线性";

    /// <summary>Which way a guide runs.</summary>
    internal static string Axis(GuideAxis axis) =>
        axis == GuideAxis.Vertical ? "垂直" : "水平";

    /// <summary>How the Spot Healing brush works out what to paint in.</summary>
    internal static string Healing(HealingMode mode) => mode switch
    {
        HealingMode.ContentAware => "内容识别",
        HealingMode.CreateTexture => "创建纹理",
        _ => "近似匹配",
    };

    /// <summary>What Auto Levels stretches.</summary>
    internal static string Auto(LevelsAuto mode) => mode switch
    {
        LevelsAuto.Contrast => "自动对比度",
        LevelsAuto.Color => "自动颜色",
        _ => "自动颜色 + 中和中间调",
    };
}
