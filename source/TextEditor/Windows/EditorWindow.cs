using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TextEditor;

public class EditorWindow : Window
{
    public EditorWindow()
    {
        this.Title = "Text Editor";
        this.Width = 600D;
        this.Height = 650D;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.Content = this.BuildGridLayout();
        this.TabControl.Focus();
    }
    static EditorWindow(
    ){
        CommandManager.RegisterClassCommandBinding(typeof(EditorWindow), new CommandBinding(Commands.Open,      Open_Execute));
        CommandManager.RegisterClassCommandBinding(typeof(EditorWindow), new CommandBinding(Commands.ToFind,    OpenFindReplaceWindowCallback(TextEditor.FindReplaceWindow.Mode.Find),    FindReplace_CanExecute));
        CommandManager.RegisterClassCommandBinding(typeof(EditorWindow), new CommandBinding(Commands.ToReplace, OpenFindReplaceWindowCallback(TextEditor.FindReplaceWindow.Mode.Replace), FindReplace_CanExecute));
        CommandManager.RegisterClassCommandBinding(typeof(EditorWindow), new CommandBinding(Commands.ToModify,  OpenFindReplaceWindowCallback(TextEditor.FindReplaceWindow.Mode.Modify),  FindReplace_CanExecute));
    }
    private TabControl TabControl = null!;
    private FindReplaceWindow? FindReplaceWindow = null;
    private Grid BuildGridLayout()
    {
        var gridLayout = new Grid();
        gridLayout.RowDefinitions.Add(new RowDefinition{ Height = GridLength.Auto });
        gridLayout.RowDefinitions.Add(new RowDefinition{ Height = new GridLength(1D, GridUnitType.Star) });

        var tabControl = new TabControl();
        this.TabControl = tabControl;
        Grid.SetRow(tabControl, 1);
        Grid.SetColumn(tabControl, 0);
        gridLayout.Children.Add(tabControl);

        var topMenu = new TextEditor.MainMenu(tabControl);
        Grid.SetRow(topMenu, 0);
        Grid.SetColumn(topMenu, 0);
        gridLayout.Children.Add(topMenu);

        return gridLayout;
    }
    private static void Open_Execute(object sender, ExecutedRoutedEventArgs evt)
    {
        if (sender is EditorWindow editorWindow) {
            var tabControl= editorWindow.TabControl;
            var dialog = new Microsoft.Win32.OpenFileDialog{
                Filter = "All Files|*.*",
            };
            if (dialog.ShowDialog() == false) return;
            tabControl.New(dialog.FileName);
        }
    }
    private static System.Windows.Input.ExecutedRoutedEventHandler OpenFindReplaceWindowCallback(
        TextEditor.FindReplaceWindow.Mode mode
    ){
        return (object sender, ExecutedRoutedEventArgs evt) => {
            var editor = (EditorWindow) sender;
            var window = new FindReplaceWindow(editor.TabControl, FindReplaceWindow.Mode.Find){ Owner = editor };
            editor.FindReplaceWindow = window;
            window.Closed += (object? sender, EventArgs evt) => {
                editor.FindReplaceWindow = null;
            };
            window.Show();
        };
    }
    private static void FindReplace_CanExecute(object sender, CanExecuteRoutedEventArgs evt)
    {
        if (
            sender is EditorWindow editor
         && editor.TabControl.SelectedItem != null
         && editor.FindReplaceWindow == null
        )
           evt.CanExecute = true;
    }
}