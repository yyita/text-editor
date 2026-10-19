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
        var window = new System.Windows.Window{
            Title = "未捕获的异常",
            Width = 480D,
            Height = 520D,
            ResizeMode = System.Windows.ResizeMode.NoResize,
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
        };
        var textBox = new System.Windows.Controls.TextBox{
            TextWrapping = System.Windows.TextWrapping.NoWrap,
            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            IsReadOnly = true,
            BorderThickness = new System.Windows.Thickness(0D)
        };
        textBox.AppendText(DateTime.Now.ToString($"yyyy-MM-dd HH:mm:ss{Environment.NewLine}"));
        textBox.AppendText($"{evt.Exception.Message}{Environment.NewLine}");
        textBox.AppendText($"{evt.Exception.ToString()}{Environment.NewLine}");
        window.Content = textBox;

        System.Media.SystemSounds.Hand.Play();
        window.Show();

        evt.Handled = true;
    }
    protected override void OnStartup(System.Windows.StartupEventArgs evt)
    {
        base.OnStartup(evt);

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
}