using System.Windows.Input;

namespace TextEditor;

public class Commands
{
    public static readonly RoutedUICommand Open         = new RoutedUICommand("Open",          "Open",         typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.O, ModifierKeys.Control), });
    public static readonly RoutedUICommand Save         = new RoutedUICommand("Save",          "Save",         typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.S, ModifierKeys.Control), });
    public static readonly RoutedUICommand Close        = new RoutedUICommand("Close",         "Close",        typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.W, ModifierKeys.Control), });
    public static readonly RoutedUICommand ToFind       = new RoutedUICommand("To Find",       "ToFind",       typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.F, ModifierKeys.Control), });
    public static readonly RoutedUICommand ToReplace    = new RoutedUICommand("To Replace",    "ToReplace",    typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.R, ModifierKeys.Control), });
    public static readonly RoutedUICommand ToModify     = new RoutedUICommand("To Modify",     "ToModify",     typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.M, ModifierKeys.Control), });
    public static readonly RoutedUICommand ToUpper      = new RoutedUICommand("To Upper",      "ToUpper",      typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.U, ModifierKeys.Control), });
    public static readonly RoutedUICommand ToLower      = new RoutedUICommand("To Lower",      "ToLower",      typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.L, ModifierKeys.Control), });
    public static readonly RoutedUICommand Indent       = new RoutedUICommand("Indent",        "Indent",       typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.Tab), });
    public static readonly RoutedUICommand Dedent       = new RoutedUICommand("Dedent",        "Dedent",       typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.Tab, ModifierKeys.Shift), });
    public static readonly RoutedUICommand IndentRegion = new RoutedUICommand("Indent Region", "IndentRegion", typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.OemCloseBrackets, ModifierKeys.Control), });
    public static readonly RoutedUICommand DedentRegion = new RoutedUICommand("Dedent Region", "DedentRegion", typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.OemOpenBrackets,  ModifierKeys.Control), });

    public static readonly RoutedCommand FindPrevious   = new RoutedCommand("FindPrevious", typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.Up,    ModifierKeys.Control), });
    public static readonly RoutedCommand FindNext       = new RoutedCommand("FindNext",     typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.Down,  ModifierKeys.Control), });
    public static readonly RoutedCommand Replace        = new RoutedCommand("Replace",      typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.Enter, ModifierKeys.Control), });
    public static readonly RoutedCommand ReplaceAll     = new RoutedCommand("ReplaceAll",   typeof(Commands), new InputGestureCollection{ new KeyGesture(Key.Enter, ModifierKeys.Control | ModifierKeys.Shift), });
}