using System.Windows.Input;

namespace TextEditor;

public class ReplacementCommands
{
    public static readonly RoutedCommand Replace    = new RoutedCommand("Replace",    typeof(ReplacementCommands), new InputGestureCollection{ new KeyGesture(Key.Enter, ModifierKeys.Control), });
    public static readonly RoutedCommand ReplaceAll = new RoutedCommand("ReplaceAll", typeof(ReplacementCommands), new InputGestureCollection{ new KeyGesture(Key.Enter, ModifierKeys.Control | ModifierKeys.Shift), });
}
