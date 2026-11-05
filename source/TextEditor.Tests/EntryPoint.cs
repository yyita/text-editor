namespace TextEditor.Tests;

public static class EntryPoint
{
    [STAThread]
    static void Main()
    {
        StringExtensionsTestCases.Test();
        var application = new System.Windows.Application();
        var window = new System.Windows.Window();
        var textBox = new System.Windows.Controls.TextBox();
        window.Content = textBox;
        (new TextEditor.Windows.JumpWindow(textBox)).Show();
        application.Run(window);
    }
}
