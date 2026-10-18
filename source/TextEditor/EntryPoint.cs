public static class EntryPoint
{
    [STAThread]
    static void Main()
    {
        var textEditor = new TextEditor.Application();
        textEditor.Run();
    }
}