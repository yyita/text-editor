using System.Windows.Input;

namespace TextEditor;

public class EditCommands
{
    public static readonly RoutedUICommand Undo  = new RoutedUICommand("撤销", "Undo", typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.Z, ModifierKeys.Control) });
    public static readonly RoutedUICommand Redo  = new RoutedUICommand("重做", "Redo", typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.Y, ModifierKeys.Control) });

    public static readonly RoutedUICommand Cut   = new RoutedUICommand("剪切", "Cut",   typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.X, ModifierKeys.Control), new KeyGesture(Key.Delete, ModifierKeys.Control) });
    public static readonly RoutedUICommand Copy  = new RoutedUICommand("复制", "Copy",  typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.C, ModifierKeys.Control), new KeyGesture(Key.Insert, ModifierKeys.Control) });
    public static readonly RoutedUICommand Paste = new RoutedUICommand("粘贴", "Paste", typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.V, ModifierKeys.Control), new KeyGesture(Key.Insert, ModifierKeys.Shift) });

    public static readonly RoutedUICommand Find    = new RoutedUICommand("查找", "Find",    typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.F, ModifierKeys.Control), });
    public static readonly RoutedUICommand Replace = new RoutedUICommand("替换", "Replace", typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.R, ModifierKeys.Control), });
    public static readonly RoutedUICommand Modify  = new RoutedUICommand("修改", "Modify",  typeof(EditCommands), new InputGestureCollection{ new KeyGesture(Key.M, ModifierKeys.Control), });
}
