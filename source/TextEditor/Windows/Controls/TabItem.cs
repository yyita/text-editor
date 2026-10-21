namespace TextEditor.Controls;

public class TabItem : System.Windows.Controls.TabItem
{
    public TabItem(string filePath)
    {
        this.Content = this.BuildGridLayout();
        this.FilePath = filePath;

        // 加载数据：
        // BeginChange、EndChange： 避免触发 TextChanged 和 SelectionChanged 事件导致意外行为。
        this.TextBox.BeginChange();
        using ( var reader = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8) )
        {
            string? line;
            while ( ( line = reader.ReadLine() ) != null )
            {
                this.TextBox.AppendText($"{line}{Environment.NewLine}");
            }
        }
        this.TextBox.EndChange();

        this.RawData = this.TextBox.Text;
        Loaded += OnLoaded;
    }
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
    {
        var box = new TextEditor.Controls.TextBox
        {
            BorderThickness = new System.Windows.Thickness(0D),
            IsReadOnly = true,
            TabIndex = 1,
        };
        return box;
    }
    private TextEditor.Controls.TextBox TextBox_Build()
    {
        var box = new TextEditor.Controls.TextBox
        {
            AcceptsReturn = true,
            AcceptsTab = true,
            TextWrapping = System.Windows.TextWrapping.NoWrap,
            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            FontFamily = new System.Windows.Media.FontFamily( "Fira Code" ),
            FontSize = 16D,
            TabIndex = 0,  // 将 Tab 序提至最前，使切换至当前选项卡时的自动焦点落至自身
        };
        return box;
    }
    public TextEditor.Controls.TextBox TextBox = null!;
    public bool IsDirty { get => this.RawData != this.TextBox.Text; }
    private string RawData = null!;
    private TextEditor.Controls.TextBox PathBox = null!;
    public string FilePath
    {
        get => this.PathBox.Text;
        private set
        {
            this.Header = System.IO.Path.GetFileName(value);
            this.PathBox.Text = value;
        }
    }
    private TextEditor.Controls.StatusBar StatusBar = null!;
    private void OnLoaded(object sender, System.Windows.RoutedEventArgs evt)
    {
        this.TextBox.Focus();
        this.TextBox.TextChanged += this.TextBox_OnTextChanged;
        this.TextBox.SelectionChanged += (object sender, System.Windows.RoutedEventArgs evt) =>
        {
            var textBox = (TextEditor.Controls.TextBox) sender;
            this.StatusBar.Coordinates = textBox.GetCoordinatesFromCharacterIndex(textBox.CaretIndex);
        };
        this.TextBox.PreviewKeyDown += (object sender, System.Windows.Input.KeyEventArgs evt) =>
        {
            if (evt.Key == System.Windows.Input.Key.Insert)
            {
                if (this.StatusBar.TextMode == TextEditor.TextMode.Insert)
                {
                    this.StatusBar.TextMode = TextEditor.TextMode.Overwrite;
                }
                else
                {
                    this.StatusBar.TextMode = TextEditor.TextMode.Insert;
                }
            }
        };
    }
    private void TextBox_OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs evt)
    {
        var header = (string) this.Header;
        if (this.IsDirty)
        {
            if (!header.StartsWith("*"))
            {
                this.Header = $"*{header}";
            }
        }
        else if (header.StartsWith("*"))
        {
            this.Header = header.Substring(1);
        }
    }
    public void Save()
    {
        if (this.IsDirty)
        {
            System.IO.File.WriteAllText(this.FilePath, this.TextBox.Text, System.Text.Encoding.UTF8);
            this.RawData = this.TextBox.Text;
            this.Header = ((string) this.Header).Substring(1);
        }
    }
}
