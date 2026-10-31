namespace TextEditor;

public class Application : System.Windows.Application
{
    private const string GUID = "AA29FA7E-E4F1-49E0-9F54-83B3B777B442";
    private const string MutexName = $"Local\\Yyita.TextEditor.{{{GUID}}}";
    private const string PipeName  = $"Local\\Yyita.TextEditor.{{{GUID}}}";
    private static System.Threading.Mutex? Mutex;
    public Application()
    {
        this.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
        this.DispatcherUnhandledException += this.OnDispatcherUnhandledException;
    }
    private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs evt)
    {
        Console.WriteLine( "[Error]" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") );
    }
    protected override void OnStartup(System.Windows.StartupEventArgs evt)
    {
        base.OnStartup(evt);

        var oldDirectory = Environment.CurrentDirectory;
        var newDirectory = AppDomain.CurrentDomain.BaseDirectory;
        if (String.Equals(
            System.IO.Path.TrimEndingDirectorySeparator(oldDirectory),
            System.IO.Path.TrimEndingDirectorySeparator(newDirectory),
            StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Current working directory: {oldDirectory}");
        }
        else
        {
            Environment.CurrentDirectory = newDirectory;
            Console.WriteLine($"Current working directory: {oldDirectory} -→ {newDirectory}");
        }

        if (!this.IsValidCommandLineArguments(evt.Args))
        {
            this.Shutdown();
            return;
        }

        TextEditor.Application.Mutex = new System.Threading.Mutex(true, TextEditor.Application.MutexName, out bool noInstance);
        if (!noInstance)
        {
            System.Windows.MessageBox.Show("Only one instance of this app is allowed.");
            this.Shutdown();
            return;
        }

        this.MainWindow = this.BuildMainWindow(evt.Args);
        this.MainWindow.Show();
    }
    private bool IsValidCommandLineArguments(string[] arguments)
    {
        // 效验命令行参数，仅在命令行参数全部有效时放行。
        // 不过滤首个参数： Args 只包含传递的命令行参数。
        object? firstInvalidArgument = null;
        foreach (string argument in arguments)
        {
            if (!System.IO.File.Exists(argument))
            {
                System.Windows.MessageBox.Show($"File not found: {argument} .");
                firstInvalidArgument ??= true;
            }
        }
        return firstInvalidArgument == null;
    }
    private System.Windows.Window BuildMainWindow(string[] arguments)
    {
        var editor = new EditorWindow();

        if (arguments.Length > 0)
            editor.TabControl.New(arguments);

        return editor;
    }
    [STAThread]
    static void Main()
    {
        var textEditor = new TextEditor.Application();
        textEditor.Run();
    }
}
