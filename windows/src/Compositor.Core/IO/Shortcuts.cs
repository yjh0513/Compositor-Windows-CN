using System.Text.Json;
using System.Text.Json.Serialization;

namespace Compositor.Core.IO;

/// <summary>
/// What a shortcut is held down with. The bits are the port's own, and they sit where the Mac build's do: the
/// Mac's Command is this port's Ctrl and its Option is this port's Alt, so a chord ports across untouched.
/// </summary>
[Flags]
public enum ShortcutModifiers
{
    None = 0,
    Control = 1,
    Alt = 2,
    Shift = 4,
}

/// <summary>
/// One key combination: a key named after Avalonia's own <c>Avalonia.Input.Key</c> member, and the modifiers
/// held with it. The key is a string because this assembly does not depend on Avalonia — the window is what
/// turns the name into a key, and the window's <c>--shortcuts</c> check is what proves every name is one it
/// knows, so a table that names a key the build has never heard of is reported rather than obeyed.
/// </summary>
public readonly record struct ShortcutChord(string Key, ShortcutModifiers Modifiers = ShortcutModifiers.None)
{
    /// <summary>A row with a place on the list and no key on it, which the editor may leave alone or fill.</summary>
    public static ShortcutChord Unbound { get; } = new("");

    /// <summary>Whether there is a key on this chord at all.</summary>
    [JsonIgnore]
    public bool IsBound => Key.Length > 0;

    /// <summary>How the chord reads to a person: "Ctrl+Shift+Z", "Alt+Delete", "[", "Space".</summary>
    [JsonIgnore]
    public string Label
    {
        get
        {
            if (!IsBound) return "";
            var text = "";
            if (Modifiers.HasFlag(ShortcutModifiers.Control)) text += "Ctrl+";
            if (Modifiers.HasFlag(ShortcutModifiers.Alt)) text += "Alt+";
            if (Modifiers.HasFlag(ShortcutModifiers.Shift)) text += "Shift+";
            return text + Spell(Key);
        }
    }

    public override string ToString() => Label;

    /// <summary>The name of the key as a person reads it, which is not always the enum member's own name.</summary>
    private static string Spell(string key) => key switch
    {
        "OemPlus" => "=",
        "OemMinus" => "-",
        "OemOpenBrackets" => "[",
        "OemCloseBrackets" => "]",
        "OemSemicolon" => ";",
        "OemQuotes" => "'",
        "OemComma" => ",",
        "OemPeriod" => ".",
        _ when key.Length == 2 && key[0] == 'D' && char.IsAsciiDigit(key[1]) => key[1..],
        _ => key,
    };
}

/// <summary>
/// One row of the shortcut list: what it is called, which group it is in, and the key it starts on. The id is
/// the Mac build's own "group:title", so a table written by either build names the same rows.
/// </summary>
public sealed record ShortcutDefinition(string ID, string Title, string Group, ShortcutChord Original)
{
    /// <summary>Whether this row is one of the menus', which is what its place in the title bar is drawn from.</summary>
    [JsonIgnore]
    public bool IsMenu => Group == Shortcuts.Menus;
}

/// <summary>
/// Every key the app answers to, as one list. This is the Mac build's KeyboardShortcuts.swift, in this port's
/// terms: the same rows and the same grouping, with the Mac's chords kept — Command read as Ctrl — except
/// where Windows has its own claim on a combination, and with rows for the verbs this port has that the Mac
/// reaches another way.
/// </summary>
public static class Shortcuts
{
    /// <summary>The group of the rows that are drawn in the menus, and shown there with their key.</summary>
    public const string Menus = "菜单";

    /// <summary>The group of the rows the canvas and the layers answer to, which are not in any menu.</summary>
    public const string Canvas = "画布与图层";

    /// <summary>The groups in the order the list shows them, which is the Mac build's order.</summary>
    public static IReadOnlyList<string> Groups { get; } = [Menus, Canvas];

    public static IReadOnlyList<ShortcutDefinition> Definitions { get; } = Build();

    /// <summary>
    /// The table with the defaults filled in, so every row has the key it would fire on: what the window
    /// delivers from, and what the list is drawn from.
    /// </summary>
    public static Dictionary<string, ShortcutChord> Effective(IReadOnlyDictionary<string, ShortcutChord> overrides)
    {
        var table = new Dictionary<string, ShortcutChord>();
        foreach (var definition in Definitions)
        {
            table[definition.ID] = overrides.TryGetValue(definition.ID, out var chosen)
                ? chosen
                : definition.Original;
        }
        return table;
    }

    /// <summary>
    /// What is wrong with a table, or null when nothing is — the Mac build's own check (its <c>problem(in:)</c>)
    /// in this port's terms. A row may be left with no key on it, which is how a row the Mac gives no key to is
    /// written down; a key may not be on two rows, and may not be one Windows answers itself.
    /// </summary>
    public static string? Problem(IReadOnlyDictionary<string, ShortcutChord> overrides)
    {
        var assigned = new Dictionary<ShortcutChord, string>();
        foreach (var definition in Definitions)
        {
            var chord = overrides.TryGetValue(definition.ID, out var chosen) ? chosen : definition.Original;
            if (!chord.IsBound) continue;
            if (Mislaid(chord)) return $"{chord.Label} 必须是一个按键，可加 Ctrl、Alt 或 Shift";
            if (Reserved(chord)) return $"{chord.Label} 已被 Windows 占用";
            if (assigned.TryGetValue(chord, out var other))
            {
                return $"{chord.Label} 同时用在「{other}」和「{definition.Title}」上";
            }
            assigned[chord] = definition.Title;
        }
        return null;
    }

    private const ShortcutModifiers Held =
        ShortcutModifiers.Control | ShortcutModifiers.Alt | ShortcutModifiers.Shift;

    /// <summary>Whether a chord is shaped like something a keyboard can produce at all.</summary>
    private static bool Mislaid(ShortcutChord chord) =>
        chord.Key.Length > 32 || chord.Key.Contains('+') || (chord.Modifiers & ~Held) != 0;

    /// <summary>
    /// Whether Windows has its own claim on the combination: the two window-switching keys, the one that opens
    /// the Start menu, the one every debugger takes, and — because every menu here is reached by holding Alt
    /// and pressing its own letter — Alt with a letter and no Ctrl.
    /// </summary>
    private static bool Reserved(ShortcutChord chord) => chord switch
    {
        { Key: "F4", Modifiers: ShortcutModifiers.Alt } => true,
        { Key: "Tab", Modifiers: ShortcutModifiers.Alt } => true,
        { Key: "Escape", Modifiers: ShortcutModifiers.Control } => true,
        { Key: "F12", Modifiers: ShortcutModifiers.None } => true,
        { Key: var key, Modifiers: var held }
            when held.HasFlag(ShortcutModifiers.Alt) && !held.HasFlag(ShortcutModifiers.Control)
                && key.Length == 1 && char.IsAsciiLetter(key[0]) => true,
        _ => false,
    };

    /// <summary>
    /// The list itself. The menus' rows read as the port's own menu rows read; a row the port has that the Mac
    /// reaches another way — the wand's, the ellipse's, a second duplicate — is written with a key of its own
    /// rather than left out, and a row with no sensible key left unbound.
    /// </summary>
    private static IReadOnlyList<ShortcutDefinition> Build()
    {
        const ShortcutModifiers Ctrl = ShortcutModifiers.Control;
        const ShortcutModifiers Alt = ShortcutModifiers.Alt;
        const ShortcutModifiers Shift = ShortcutModifiers.Shift;

        var rows = new List<ShortcutDefinition>();
        // Qualified, because the local function's own name would otherwise stand in for the group's.
        void Menu(string title, string key = "", ShortcutModifiers modifiers = ShortcutModifiers.None) =>
            rows.Add(new ShortcutDefinition($"{Shortcuts.Menus}:{title}", title, Shortcuts.Menus,
                new ShortcutChord(key, modifiers)));
        void Canvas(string title, string key = "", ShortcutModifiers modifiers = ShortcutModifiers.None) =>
            rows.Add(new ShortcutDefinition($"{Shortcuts.Canvas}:{title}", title, Shortcuts.Canvas,
                new ShortcutChord(key, modifiers)));

        // The File menu, then Edit, then Layer, then the view's own switches — the Mac's order.
        Menu("还原", "Z", Ctrl);
        Menu("重做", "Z", Ctrl | Shift);
        Menu("新建项目", "N", Ctrl);
        Menu("打开项目", "O", Ctrl);
        Menu("存储", "S", Ctrl);
        Menu("存储为", "S", Ctrl | Shift);
        Menu("导出 PNG", "E", Ctrl | Shift);
        Menu("导出 JPEG", "S", Ctrl | Alt | Shift);
        Menu("关闭标签页", "W", Ctrl);
        Menu("适合画布", "D0", Ctrl);
        Menu("实际像素", "D1", Ctrl);
        Menu("放大", "OemPlus", Ctrl);
        Menu("缩小", "OemMinus", Ctrl);
        Menu("显示变换控件", "H", Ctrl);
        Menu("剪切", "X", Ctrl);
        Menu("拷贝", "C", Ctrl);
        Menu("合并拷贝", "C", Ctrl | Shift);
        Menu("粘贴", "V", Ctrl);
        Menu("填充前景色", "Delete", Alt);
        Menu("填充背景色", "Delete", Ctrl);
        Menu("内容识别填充", "Delete", Alt | Shift);
        Menu("全选", "A", Ctrl);
        Menu("取消选择", "D", Ctrl);
        Menu("反选", "I", Ctrl | Shift);
        Menu("曲线", "M", Ctrl);
        Menu("色阶", "L", Ctrl);
        Menu("色相/饱和度", "U", Ctrl);
        Menu("反相", "I", Ctrl);
        Menu("画布大小", "C", Ctrl | Alt);
        Menu("图像大小", "I", Ctrl | Alt);
        Menu("通过拷贝的图层", "J", Ctrl);
        // The port has the Mac's one duplicate row twice over: Layer via Copy takes the copy that is bounded by
        // the selection, and the layer's own duplicate takes Shift with it rather than the same key twice.
        Menu("复制图层", "J", Ctrl | Shift);
        Menu("切换剪切蒙版", "G", Ctrl | Alt);
        Menu("图层编组", "G", Ctrl);
        Menu("新建空白图层", "N", Ctrl | Shift);
        Menu("向上移动图层", "OemCloseBrackets", Ctrl);
        Menu("向下移动图层", "OemOpenBrackets", Ctrl);
        Menu("合并图层", "E", Ctrl);
        Menu("重命名图层", "F2");
        Menu("删除图层", "Delete");
        Menu("显示网格", "OemQuotes", Ctrl);
        Menu("显示参考线", "OemSemicolon", Ctrl);
        Menu("显示标尺", "R", Ctrl);
        Menu("对齐", "OemSemicolon", Ctrl | Shift);
        Menu("锁定参考线", "OemSemicolon", Ctrl | Alt);
        // The port adds a guide from the View menu; the Mac gives that row no key at all, and so does this.
        Menu("新建参考线");

        // The canvas's own: the tools, the colours, the brush, and what the arrows do.
        Canvas("抓手工具", "H");
        Canvas("移动/变换工具", "V");
        Canvas("选框工具", "M");
        Canvas("套索工具", "L");
        Canvas("魔棒", "W");
        Canvas("画笔工具", "B");
        Canvas("仿制图章", "S");
        Canvas("模糊/涂抹/液化", "R");
        Canvas("污点修复画笔", "J");
        Canvas("吸管工具", "I");
        Canvas("文字工具", "T");
        Canvas("裁剪工具", "C");
        Canvas("形状工具", "U");
        Canvas("渐变工具", "G");
        Canvas("交换前景色/背景色", "X");
        Canvas("复位颜色", "D");
        Canvas("临时抓手工具(按住)", "Space");
        Canvas("减小画笔大小", "OemOpenBrackets");
        Canvas("增大画笔大小", "OemCloseBrackets");
        Canvas("减小画笔硬度", "OemOpenBrackets", Shift);
        Canvas("增大画笔硬度", "OemCloseBrackets", Shift);
        Canvas("上一个混合模式", "OemMinus", Shift);
        Canvas("下一个混合模式", "OemPlus", Shift);
        Canvas("切换形状类型", "U", Shift);
        for (var digit = 0; digit <= 9; digit++)
        {
            Canvas($"不透明度数字 {digit}（连按两次可输入精确百分比）", $"D{digit}");
        }
        // The first field says the direction the way a row reads it; the second is the key as Avalonia knows
        // it, which is what ShortcutKeys.Known puts through Enum.TryParse<Key>. Translating the key name
        // itself would leave the row Unbound: the menu would show no gesture and the arrow would be dead.
        foreach (var (direction, key) in new[]
                 {
                     ("向左", "Left"), ("向右", "Right"), ("向上", "Up"), ("向下", "Down"),
                 })
        {
            Canvas($"{direction}轻移 1 像素", key);
            Canvas($"{direction}轻移 10 像素", key, Shift);
            // The Mac's Command-with-an-arrow rows: they move the pixels inside the selection rather than the
            // layer the plain arrows move — the one place a modifier changes what an arrow does.
            Canvas($"{direction}移动选区像素 1 像素", key, Ctrl);
            Canvas($"{direction}移动选区像素 10 像素", key, Ctrl | Shift);
        }
        Canvas("应用画布操作", "Enter");
        Canvas("取消画布操作", "Escape");
        return rows;
    }
}

/// <summary>
/// The shortcut keys a person has changed, which belong to the person rather than to a document: a row left on
/// its original key is not written out, and a file that would break a rule is dropped whole rather than
/// followed — which is what the Mac build does with a saved table that does not pass its own check.
/// </summary>
public sealed class ShortcutDefaults
{
    private static readonly JsonSerializerOptions Json = new() { WriteIndented = true };

    /// <summary>Where the table is kept for whoever is using the app.</summary>
    public static string DefaultPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Compositor", "shortcuts.json");

    /// <summary>Only the rows that are not on their original key.</summary>
    public Dictionary<string, ShortcutChord> Overrides { get; set; } = [];

    /// <summary>The rows as they were last left, or no changes at all when there is nothing to read.</summary>
    public static ShortcutDefaults Load(string path)
    {
        try
        {
            if (!File.Exists(path)) return new ShortcutDefaults();
            var read = JsonSerializer.Deserialize<ShortcutDefaults>(File.ReadAllText(path), Json);
            return read?.Kept() ?? new ShortcutDefaults();
        }
        catch (Exception error) when (error is IOException or JsonException or UnauthorizedAccessException)
        {
            // A kept table is a convenience: one that cannot be read is not worth failing to start over.
            return new ShortcutDefaults();
        }
    }

    /// <summary>Writes the table out, quietly doing nothing when it cannot be written.</summary>
    public void Save(string path)
    {
        try
        {
            var folder = Path.GetDirectoryName(Path.GetFullPath(path));
            if (folder is not null) Directory.CreateDirectory(folder);
            File.WriteAllText(path, JsonSerializer.Serialize(Kept(), Json));
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
        }
    }

    /// <summary>
    /// The same table with anything a hand-edited file could have got wrong put right: a table that does not
    /// pass the check is emptied rather than half followed, rows this build has no place for are dropped, and a
    /// row written down on the key it already had is dropped as the nothing it is.
    /// </summary>
    private ShortcutDefaults Kept()
    {
        if (Shortcuts.Problem(Overrides) is not null) return new ShortcutDefaults();
        var kept = new Dictionary<string, ShortcutChord>();
        foreach (var definition in Shortcuts.Definitions)
        {
            if (!Overrides.TryGetValue(definition.ID, out var chord)) continue;
            if (chord == definition.Original) continue;
            kept[definition.ID] = chord;
        }
        return new ShortcutDefaults { Overrides = kept };
    }
}
