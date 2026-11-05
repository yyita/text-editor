namespace TextEditor.Windows;

public class Application : System.Windows.Application
{
    /*******
     程序入口
     *******/
    [STAThread]
    static void Main()
    {
        (new TextEditor.Windows.Application()).Run();
    }

    /*******
     构造函数
     *******/
    public Application()
    {
        this.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
    }

    /*********************
     数据对象（字段 (Fields)）
     *********************/
    private const String GUID = "AA29FA7E-E4F1-49E0-9F54-83B3B777B442";
    private const String MutexName = $"Local\\Yyita.TextEditor.{{{GUID}}}";
    private static System.Threading.Mutex? Mutex;

    /*******
     实例函数
     *******/
    protected override void OnStartup(System.Windows.StartupEventArgs evt)
    {
        base.OnStartup(evt);

        /*******
         单例应用
         *******/
        TextEditor.Windows.Application.Mutex = new System.Threading.Mutex(true, TextEditor.Windows.Application.MutexName, out bool noInstance);
        if (!noInstance)
        {
            System.Windows.MessageBox.Show("Only one instance of this app is allowed.");
            this.Shutdown();
            return;
        }

        /***************
         “当前工作目录”补丁
         ***************/
        var oldWorkingDirectory = Environment.CurrentDirectory;
        var newWorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

        if (String.Equals(System.IO.Path.TrimEndingDirectorySeparator(oldWorkingDirectory),
                          System.IO.Path.TrimEndingDirectorySeparator(newWorkingDirectory),
                          StringComparison.OrdinalIgnoreCase))
            Console.WriteLine($"Current working directory: {oldWorkingDirectory}");
        else
        {
            Environment.CurrentDirectory = newWorkingDirectory;
            Console.WriteLine($"Current working directory: {oldWorkingDirectory} -→ {newWorkingDirectory}");
        }

        /********
         准备主窗口
         ********/
        var editor = new TextEditor.Windows.EditorWindow();
        if (evt.Args.Length != 0)
        {
            /************
             命令行参数处理
             ************/
            var fullFilePaths = new List<String>();
            var failedArguments = new List<String>();
            foreach (String argument in evt.Args)
            {
                try
                {
                    var fullFilePath = System.IO.Path.GetFullPath(argument, oldWorkingDirectory);
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
            {
                System.Windows.MessageBox.Show(String.Format("Failed:\n{0}",  String.Join(Environment.NewLine, failedArguments)));
                this.Shutdown();
                return;
            }
            editor.TabControl.New(fullFilePaths.ToArray());
        }
        this.MainWindow = editor;

        /********
         启动主窗口
         ********/
        this.MainWindow.Show();
    }
}
