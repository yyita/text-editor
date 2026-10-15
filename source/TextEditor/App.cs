using System.Windows;  // Application
using System.Windows.Input;  // StartupEventArgs

namespace TextEditor;

public class App : Application
{
    protected override void OnStartup(StartupEventArgs evt)
    {
        base.OnStartup(evt);

        var editor = new EditorWindow();
        this.MainWindow = editor;
        editor.Show();
    }
}