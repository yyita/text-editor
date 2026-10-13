using System.Windows;
using System.Windows.Input;

namespace TextEditor
{
    public class App : Application
    {
        protected override void OnStartup(StartupEventArgs evt)
        {
            base.OnStartup(evt);

            var editor = new EditorWindow();
            editor.Show();
        }
    }
}