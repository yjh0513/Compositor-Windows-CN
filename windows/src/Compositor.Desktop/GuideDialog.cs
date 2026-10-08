using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Compositor.Core.Format;

namespace Compositor.Desktop;

/// <summary>
/// View ▸ New Guide: which way the line runs and where it sits. Avalonia ships no such dialog, so this is one.
/// </summary>
internal sealed class GuideDialog : DialogWindow
{
    private readonly ComboBox _axis = new();
    private readonly TextBox _position;
    private (GuideAxis Axis, double Position)? _result;

    private GuideDialog(int width, int height)
    {
        Title = "新建参考线";
        Width = 380;
        SizeToContent = SizeToContent.Height;
        CanResize = false;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        _axis.ItemsSource = new[] { "水平(横向)", "垂直(纵向)" };
        _axis.SelectedIndex = 0;
        _axis.Width = 180;
        _position = new TextBox { Text = (height / 2).ToString(), Width = 100 };

        var ok = new Button { Content = "确定", IsDefault = true };
        var cancel = new Button { Content = "取消", IsCancel = true };
        ok.Click += (_, _) => Accept(width, height);
        cancel.Click += (_, _) => Close();

        Content = new StackPanel
        {
            Margin = new Thickness(16),
            Spacing = 8,
            Children =
            {
                Row("方向", _axis),
                Row("位置(像素)", _position),
                new TextBlock
                {
                    Text = $"画布为 {width} x {height}。参考线可以位于画布之外，延伸到画板区域。",
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                },
                new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 8,
                    Margin = new Thickness(0, 8, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Children = { cancel, ok },
                },
            },
        };
        Opened += (_, _) =>
        {
            _position.Focus();
            _position.SelectAll();
        };
    }

    private static Control Row(string label, Control control) => new StackPanel
    {
        Orientation = Orientation.Horizontal,
        Spacing = 8,
        Children =
        {
            new TextBlock { Text = label, Width = 120, VerticalAlignment = VerticalAlignment.Center },
            control,
        },
    };

    private void Accept(int width, int height)
    {
        if (!double.TryParse(_position.Text, out var position)) return;
        // A guide across the canvas is a horizontal one, so the first entry is the horizontal axis.
        var axis = _axis.SelectedIndex == 1 ? GuideAxis.Vertical : GuideAxis.Horizontal;
        var extent = axis == GuideAxis.Vertical ? width : height;
        if (position < 0 || position > extent) return;
        _result = (axis, position);
        Close();
    }

    /// <summary>What was asked for, or null when the dialog was dismissed or the number made no sense.</summary>
    public static async Task<(GuideAxis Axis, double Position)?> Ask(Window owner, int width, int height)
    {
        var dialog = new GuideDialog(width, height);
        await dialog.ShowDialog(owner);
        return dialog._result;
    }
}
