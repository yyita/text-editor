using System.Windows.Input;

namespace TextEditor;

public class FileCommands
{
    public static readonly RoutedUICommand Open = new RoutedUICommand("打开", "Open", typeof(FileCommands), new InputGestureCollection{ new KeyGesture(Key.O, ModifierKeys.Control), });

    public static readonly RoutedUICommand Save = new RoutedUICommand("保存", "Save", typeof(FileCommands), new InputGestureCollection{ new KeyGesture(Key.S, ModifierKeys.Control), });

    public static readonly RoutedUICommand Close = new RoutedUICommand("关闭", "Close", typeof(FileCommands), new InputGestureCollection{ new KeyGesture(Key.W, ModifierKeys.Control), });

    public static readonly RoutedUICommand Difference = new RoutedUICommand("差异", "Difference", typeof(FileCommands));
}
