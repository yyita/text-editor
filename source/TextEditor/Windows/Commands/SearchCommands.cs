using System.Windows.Input;

namespace TextEditor;

public class SearchCommands
{
    public static readonly RoutedCommand FindPrevious = new RoutedCommand("FindPrevious", typeof(SearchCommands), new InputGestureCollection{ new KeyGesture(Key.Up,    ModifierKeys.Control), });
    public static readonly RoutedCommand FindNext     = new RoutedCommand("FindNext",     typeof(SearchCommands), new InputGestureCollection{ new KeyGesture(Key.Down,  ModifierKeys.Control), });
}
