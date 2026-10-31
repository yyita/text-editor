using System.Windows.Input;

namespace TextEditor;

public class CaretCommands
{
    public static readonly RoutedUICommand MoveRightByCharacter = new RoutedUICommand("按字符右移", "MoveRightByCharacter", typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Right) });
    public static readonly RoutedUICommand MoveLeftByCharacter  = new RoutedUICommand("按字符左移", "MoveLeftByCharacter",  typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Left) });

    public static readonly RoutedUICommand MoveRightByWord = new RoutedUICommand("按单词右移", "MoveRightByWord", typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Right, ModifierKeys.Control) });
    public static readonly RoutedUICommand MoveLeftByWord  = new RoutedUICommand("按单词左移", "MoveLeftByWord",  typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Left,  ModifierKeys.Control) });

    public static readonly RoutedUICommand MoveToLineEnd   = new RoutedUICommand("移至行末", "MoveToLineEnd",   typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.End) });
    public static readonly RoutedUICommand MoveToLineStart = new RoutedUICommand("移至行首", "MoveToLineStart", typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Home) });

    public static readonly RoutedUICommand MoveDownByLine = new RoutedUICommand("按行下移", "MoveDownByLine", typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Down) });
    public static readonly RoutedUICommand MoveUpByLine   = new RoutedUICommand("按行上移", "MoveUpByLine",   typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Up) });

    public static readonly RoutedUICommand MoveToDocumentEnd   = new RoutedUICommand("移至文件末尾", "MoveToDocumentEnd",   typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.End, ModifierKeys.Control) });
    public static readonly RoutedUICommand MoveToDocumentStart = new RoutedUICommand("移至文件开头", "MoveToDocumentStart", typeof(CaretCommands), new InputGestureCollection{ new KeyGesture(Key.Home, ModifierKeys.Control) });
}
