using System.Windows.Input;

namespace TextEditor;

public class FormatCommands
{
    public static readonly RoutedUICommand Upper = new RoutedUICommand("大写", "Upper", typeof(FormatCommands), new InputGestureCollection{ new KeyGesture(Key.U, ModifierKeys.Control), });
    public static readonly RoutedUICommand Lower = new RoutedUICommand("小写", "Lower", typeof(FormatCommands), new InputGestureCollection{ new KeyGesture(Key.L, ModifierKeys.Control), });

    public static readonly RoutedUICommand Indent       = new RoutedUICommand("延伸",    "Indent",       typeof(FormatCommands), new InputGestureCollection{ new KeyGesture(Key.Tab), });
    public static readonly RoutedUICommand Dedent       = new RoutedUICommand("收缩",    "Dedent",       typeof(FormatCommands), new InputGestureCollection{ new KeyGesture(Key.Tab, ModifierKeys.Shift), });
    public static readonly RoutedUICommand IndentRegion = new RoutedUICommand("区域延伸", "IndentRegion", typeof(FormatCommands), new InputGestureCollection{ new KeyGesture(Key.OemCloseBrackets, ModifierKeys.Control), });
    public static readonly RoutedUICommand DedentRegion = new RoutedUICommand("区域收缩", "DedentRegion", typeof(FormatCommands), new InputGestureCollection{ new KeyGesture(Key.OemOpenBrackets,  ModifierKeys.Control), });
}
