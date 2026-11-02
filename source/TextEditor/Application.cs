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
    }
    protected override void OnStartup(System.Windows.StartupEventArgs evt)
    {
        base.OnStartup(evt);

        /*******
         单例应用
         *******/
        TextEditor.Application.Mutex = new System.Threading.Mutex(true, TextEditor.Application.MutexName, out bool noInstance);
        if (!noInstance)
        {
            System.Windows.MessageBox.Show("Only one instance of this app is allowed.");
            this.Shutdown();
            return;
        }

        /************
         命令行参数处理
         ************/
        var fullFilePaths = this.TryGetFullFilePaths(evt.Args);
        if (fullFilePaths.Count() != evt.Args.Length)
        {
            this.Shutdown();
            return;
        }

        /***************
         “当前工作目录”补丁
         ***************/
        var oldDirectory = Environment.CurrentDirectory;
        var newDirectory = AppDomain.CurrentDomain.BaseDirectory;

        if (String.Equals(System.IO.Path.TrimEndingDirectorySeparator(oldDirectory),
                          System.IO.Path.TrimEndingDirectorySeparator(newDirectory),
                          StringComparison.OrdinalIgnoreCase))
            Console.WriteLine($"Current working directory: {oldDirectory}");
        else
        {
            Environment.CurrentDirectory = newDirectory;
            Console.WriteLine($"Current working directory: {oldDirectory} -→ {newDirectory}");
        }

        /********
         启动主窗口
         ********/
        this.MainWindow = this.BuildMainWindow(fullFilePaths);
        this.MainWindow.Show();
    }
    private String[] TryGetFullFilePaths(string[] arguments)
    {
        var fullFilePaths = new List<String>();
        var failedArguments = new List<String>();
        foreach (string argument in arguments)
        {
            try
            {
                var fullFilePath = System.IO.Path.GetFullPath(argument);
                if (!System.IO.File.Exists(fullFilePath))
                    throw new Exception();
                fullFilePaths.Add(fullFilePath);
            }
            catch (Exception)
            {
                failedArguments.Add(argument);
            }
        }
        if (failedArguments.Count() > 0)
            System.Windows.MessageBox.Show(
                String.Format(
                    "Failed:\n{0}",
                    String.Join(Environment.NewLine, failedArguments)
                )
            );
        return fullFilePaths.ToArray();
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
