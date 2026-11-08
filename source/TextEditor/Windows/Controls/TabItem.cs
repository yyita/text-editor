namespace TextEditor.Controls;

public class TabItem : System.Windows.Controls.TabItem
{
 // 构造函数
    public TabItem(string filePath)
    {
        /* 构建内部控件 */
        this.Header = new System.Windows.Controls.TextBlock
        {
            IsHitTestVisible = false
        };
        this.Content = this.BuildGridLayout();

        /* 设置路径与地址栏文本 */
        this.FilePath = filePath;

        /* 只读文件启用只读模式 */
        var attributes = System.IO.File.GetAttributes(filePath);
        if( (attributes & System.IO.FileAttributes.ReadOnly) > 0 )
        {
            this.TextBox.IsReadOnly = true;
            this.StatusBar.TextMode = TextEditor.TextMode.ReadOnly;
        }

        /* 载入文件数据 */
        this.FileStream = new System.IO.FileStream
        (
            filePath,
            System.IO.FileMode.Open,
            System.IO.FileAccess.ReadWrite,
            System.IO.FileShare.Read
        );
        this.TextBox.BeginChange();
        using ( var reader = new System.IO.StreamReader(
            this.FileStream, new System.Text.UTF8Encoding(),
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024, leaveOpen: true) )
        {
            string? line;
            while ( (line = reader.ReadLine()) != null )
                this.TextBox.AppendText($"{line}{Environment.NewLine}");
        }
        this.TextBox.EndChange();

        /* 记录原始数据 */
        this.RawData = this.TextBox.Text;

        /* 添加事件回调 */
        Loaded += OnLoaded;
    }
 // 数据槽
    private TextEditor.Controls.TextBox PathBox = null!;
    private String RawData = null!;
    private TextEditor.Controls.StatusBar StatusBar = null!;
    public System.IO.FileStream? FileStream = null;
    public TextEditor.Controls.TextBox TextBox = null!;
 // 访问器
    public String FilePath
    {
        get => this.PathBox.Text;
        private set
        {
            this.Title = System.IO.Path.GetFileName(value);
            this.PathBox.Text = value;
        }
    }
    public Boolean IsDirty => this.RawData != this.TextBox.Text;
    public String Title
    {
        get => ((System.Windows.Controls.TextBlock) this.Header).Text;
        set => ((System.Windows.Controls.TextBlock) this.Header).Text = value;
    }
 // 实例函数
    private System.Windows.Controls.Grid BuildGridLayout()
    {
        var gridLayout = new System.Windows.Controls.Grid();
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{ Height = System.Windows.GridLength.Auto });
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{ Height = new System.Windows.GridLength(1D, System.Windows.GridUnitType.Star) });
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{ Height = System.Windows.GridLength.Auto });

        var pathBox = this.PathBox_Build();
        this.PathBox = pathBox;
        System.Windows.Controls.Grid.SetRow(pathBox, 0);
        System.Windows.Controls.Grid.SetColumn(pathBox, 0);
        gridLayout.Children.Add(pathBox);

        var textBox = this.TextBox_Build();
        this.TextBox = textBox;
        System.Windows.Controls.Grid.SetRow(textBox, 1);
        System.Windows.Controls.Grid.SetColumn(textBox, 0);
        gridLayout.Children.Add(textBox);

        var statusBar = new TextEditor.Controls.StatusBar();
        this.StatusBar = statusBar;
        System.Windows.Controls.Grid.SetRow(statusBar, 2);
        System.Windows.Controls.Grid.SetColumn(statusBar, 0);
        gridLayout.Children.Add(statusBar);

        return gridLayout;
    }
    private TextEditor.Controls.TextBox PathBox_Build()
        => new TextEditor.Controls.TextBox
        {
            BorderThickness = new System.Windows.Thickness(0D),
            IsReadOnly = true,
            TabIndex = 1,
        };
    private TextEditor.Controls.TextBox TextBox_Build()
        => new TextEditor.Controls.TextBox
        {
            AcceptsReturn = true,
            TextWrapping = System.Windows.TextWrapping.NoWrap,
            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            FontFamily = new System.Windows.Media.FontFamily( "Fira Code" ),
            FontSize = 16D,
            TabIndex = 0,  // 将 Tab 序提至最前，使切换至当前选项卡时的自动焦点落至自身
        };
    private void OnLoaded(object sender, System.Windows.RoutedEventArgs evt)
    {
        if (this.TextBox.IsVisible)
            this.TextBox.Focus();
        this.TextBox.TextChanged += this.TextBox_OnTextChanged;
        this.TextBox.SelectionChanged += (object sender, System.Windows.RoutedEventArgs evt) =>
        {
            this.UpdateStatusBar();
        };
        this.TextBox.PreviewKeyDown += (object sender, System.Windows.Input.KeyEventArgs evt) =>
        {
            if (evt.Key == System.Windows.Input.Key.Insert)
            {
                if (this.StatusBar.TextMode == TextEditor.TextMode.Insert)
                    this.StatusBar.TextMode = TextEditor.TextMode.Overwrite;
                else
                    this.StatusBar.TextMode = TextEditor.TextMode.Insert;
            }
        };
        this.UpdateStatusBar();
    }
    private void UpdateStatusBar()
    {
        var textBox = this.TextBox;
        var (row, column) = textBox.GetCoordinatesFromCharacterIndex(textBox.CaretIndex);
        this.StatusBar.Coordinates = (row,  textBox.LineCount, column,  textBox.CountColumn(row));
    }
    private void TextBox_OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs evt)
    {
        var title = this.Title;

        if (this.IsDirty)
        {
            if (!title.StartsWith("*"))
                this.Title = $"*{title}";
        }
        else if (title.StartsWith("*"))
            this.Title = title.Substring(1);
    }
    public void CleanUp()
        => this.FileStream?.Dispose();
    public void Save()
    {
        if (this.IsDirty && this.FileStream != null)
        {
            var textBox = this.TextBox;
            var fileStream = this.FileStream;

            byte[] bytes = this.StatusBar.Coding.GetBytes(textBox.Text);
            fileStream.Seek(0, System.IO.SeekOrigin.Begin);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.SetLength(bytes.Length);
            fileStream.Flush();

            this.RawData = textBox.Text;
            this.Title = this.Title.Substring(1);
        }
    }
}
