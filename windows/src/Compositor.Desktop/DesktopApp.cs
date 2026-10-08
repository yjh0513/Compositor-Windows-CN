using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace Compositor.Desktop;

public sealed class DesktopApp : Application
{
    /// <summary>
    /// The face the whole interface is drawn in. Inter carries no CJK glyphs, so the Chinese localisation
    /// names a Chinese face first and keeps the Latin ones behind it as fallbacks — otherwise every
    /// translated label would draw as empty boxes.
    /// </summary>
    private static readonly FontFamily UiFont =
        new("Microsoft YaHei UI, Microsoft YaHei, Noto Sans SC, Source Han Sans SC, Segoe UI, Inter");

    public override void Initialize()
    {
        // The Mac build asks for the dark appearance and draws its own greys on top of it
        // (ContentView's `.preferredColorScheme(.dark)`), so the port does the same rather than
        // following whatever the machine is set to — a light menu bar over a dark canvas was
        // unreadable, and the Mac has no light appearance to match.
        RequestedThemeVariant = ThemeVariant.Dark;
        _theme = new FluentTheme();
        Styles.Add(_theme);

        // TextElement.FontFamily inherits, so naming it once on the window carries down to every label,
        // menu row, button and dialog body the window owns.
        Styles.Add(new Style(x => x.OfType<Window>())
        {
            Setters = { new Setter(TextElement.FontFamilyProperty, UiFont) },
        });
    }

    private FluentTheme? _theme;

    public override void OnFrameworkInitializationCompleted()
    {
        // The palette is moved onto the Mac's once the theme is attached: until then its resources have not
        // been read off its own XAML, and a dictionary with nothing in it has nothing to move.
        if (_theme is { } theme) Skin.Apply(theme);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        base.OnFrameworkInitializationCompleted();
    }
}
