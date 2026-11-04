namespace TextEditor.Windows;

public class EditorWindow : System.Windows.Window
{
    /*******
     构造函数
     *******/
    public EditorWindow()
    {
        this.Title = "文本编辑器";
        this.Width = 600D;
        this.Height = 650D;
        this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        this.Content = this.BuildGridLayout();
    }
    static EditorWindow()
    {
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Windows.EditorWindow), new System.Windows.Input.CommandBinding(TextEditor.FileCommands.Open,    TextEditor.Windows.EditorWindow.Open_Execute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Windows.EditorWindow), new System.Windows.Input.CommandBinding(TextEditor.EditCommands.Find,    TextEditor.Windows.EditorWindow.OpenFindReplaceWindowCallback(TextEditor.SearchMode.Find),    TextEditor.Windows.EditorWindow.FindReplace_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Windows.EditorWindow), new System.Windows.Input.CommandBinding(TextEditor.EditCommands.Replace, TextEditor.Windows.EditorWindow.OpenFindReplaceWindowCallback(TextEditor.SearchMode.Replace), TextEditor.Windows.EditorWindow.FindReplace_CanExecute));
    }

    /*********************
     数据对象（字段 (Fields)）
     *********************/
    public TextEditor.Controls.TabControl TabControl = null!;
    private TextEditor.Windows.FindReplaceWindow? FindReplaceWindow = null;

    /*******
     类级函数
     *******/
    private static void Open_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        if (sender is TextEditor.Windows.EditorWindow editorWindow)
        {
            var tabControl= editorWindow.TabControl;
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "All Files|*.*",
            };
            if (dialog.ShowDialog() == false)
                return;
            tabControl.New(dialog.FileName);
        }
    }
    private static System.Windows.Input.ExecutedRoutedEventHandler OpenFindReplaceWindowCallback(TextEditor.SearchMode mode)
    {
        return (object sender, System.Windows.Input.ExecutedRoutedEventArgs evt) =>
        {
            var editor = (TextEditor.Windows.EditorWindow) sender;
            var window = new TextEditor.Windows.FindReplaceWindow(editor.TabControl, mode){ Owner = editor };
            editor.FindReplaceWindow = window;
            window.Closed += (object? sender, EventArgs evt) =>
            {
                editor.FindReplaceWindow = null;
            };
            window.Show();
        };
    }
    private static void FindReplace_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        if (sender is TextEditor.Windows.EditorWindow editor
         && editor.TabControl.SelectedItem != null
         && editor.FindReplaceWindow == null)
           evt.CanExecute = true;
    }

    /*******
     方法函数
     *******/
    private System.Windows.Controls.Grid BuildGridLayout()
    {
        var gridLayout = new System.Windows.Controls.Grid();
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{ Height = System.Windows.GridLength.Auto });
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{ Height = new System.Windows.GridLength(1D, System.Windows.GridUnitType.Star) });

        this.TabControl = this.BuildTabControl();
        System.Windows.Controls.Grid.SetRow(this.TabControl, 1);
        System.Windows.Controls.Grid.SetColumn(this.TabControl, 0);
        gridLayout.Children.Add(this.TabControl);

        var mainMenu = this.BuildMainMenu();
        System.Windows.Controls.Grid.SetRow(mainMenu, 0);
        System.Windows.Controls.Grid.SetColumn(mainMenu, 0);
        gridLayout.Children.Add(mainMenu);

        return gridLayout;
    }
    private TextEditor.Controls.TabControl BuildTabControl()
    {
        return new TextEditor.Controls.TabControl();
    }
    private TextEditor.Controls.MainMenu BuildMainMenu()
    {
        return new TextEditor.Controls.MainMenu(this.TabControl);
    }
}
