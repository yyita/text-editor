using System.Windows.Input;

namespace TextEditor;

public class SelectionCommands
{
    public static readonly RoutedUICommand SelectRightByCharacter = new RoutedUICommand("按字符向右拓展选区", "SelectRightByCharacter", typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.Right, ModifierKeys.Shift) });
    public static readonly RoutedUICommand SelectLeftByCharacter  = new RoutedUICommand("按字符向左拓展选区", "SelectLeftByCharacter",  typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.Left, ModifierKeys.Shift) });

    public static readonly RoutedUICommand SelectRightByWord = new RoutedUICommand("按单词向右拓展选区", "SelectRightByWord", typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.Right, ModifierKeys.Control | ModifierKeys.Shift) });
    public static readonly RoutedUICommand SelectLeftByWord  = new RoutedUICommand("按单词向左拓展选区", "SelectLeftByWord",  typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.Left, ModifierKeys.Control | ModifierKeys.Shift) });

    public static readonly RoutedUICommand SelectToLineEnd   = new RoutedUICommand("拓展选区至行末", "SelectToLineEnd",   typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.End, ModifierKeys.Shift) });
    public static readonly RoutedUICommand SelectToLineStart = new RoutedUICommand("拓展选区至行首", "SelectToLineStart", typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.Home, ModifierKeys.Shift) });

    public static readonly RoutedUICommand SelectDownByLine = new RoutedUICommand("按行向下拓展选区", "SelectDownByLine", typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.Down, ModifierKeys.Shift) });
    public static readonly RoutedUICommand SelectUpByLine   = new RoutedUICommand("按行向上拓展选区", "SelectUpByLine",   typeof(SelectionCommands), new InputGestureCollection{ new KeyGesture(Key.Up, ModifierKeys.Shift) });
}
