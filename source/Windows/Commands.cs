using System.Windows.Input;

namespace TextEditor
{
    public class Commands
    {
        public static readonly Type OwnerType = typeof(Commands);
        public static readonly RoutedUICommand Open = new RoutedUICommand(
            "Open", "Open", OwnerType,
            new InputGestureCollection{
                new KeyGesture(Key.O, ModifierKeys.Control),
            }
        );
        public static readonly RoutedUICommand Save = new RoutedUICommand(
            "Save", "Save", OwnerType,
            new InputGestureCollection{
                new KeyGesture(Key.S, ModifierKeys.Control),
            }
        );
        public static readonly RoutedUICommand Close = new RoutedUICommand(
            "Close", "Close", OwnerType,
            new InputGestureCollection{
                new KeyGesture(Key.W, ModifierKeys.Control),
            }
        );
    }
}