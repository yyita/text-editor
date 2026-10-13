using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TextEditor
{
    public class TabControl : System.Windows.Controls.TabControl
    {
        public TabControl()
        {
            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs evt)
        {
            MouseDown += OnMiddleButtonPressed;
        }
        private void OnMiddleButtonPressed(object sender, MouseButtonEventArgs evt)
        {
            if (evt.MiddleButton != MouseButtonState.Pressed || evt.Handled) return;
            if (evt.Source is TextEditor.TabItem tabItem)
                Items.Remove(tabItem);
        }
    }
}