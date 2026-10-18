namespace TextEditor;

public class TabItem : System.Windows.Controls.TabItem
{
    public TabItem(string filePath)
    {
        this.Content = this.BuildGridLayout();
        this.FilePath = filePath;
        this.TextBox.Text = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
        this.RawData = this.TextBox.Text;
        Loaded += OnLoaded;
    }
    public System.Windows.Controls.TextBox TextBox = null!;
    public bool IsDirty
    {
        get => this.RawData != this.TextBox.Text;
    }
    private string RawData = null!;
    private System.Windows.Controls.TextBox PathBox = null!;
    public string FilePath
    {
        get => this.PathBox.Text;
        private set {
            this.Header = System.IO.Path.GetFileName(value);
            this.PathBox.Text = value;
        }
    }
    private void OnLoaded(object sender, System.Windows.RoutedEventArgs evt)
    {
        this.TextBox.Focus();
        this.TextBox.TextChanged += this.TextBox_OnTextChanged;
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
    private System.Windows.Controls.Grid BuildGridLayout()
    {
        var gridLayout = new System.Windows.Controls.Grid();
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{ Height = System.Windows.GridLength.Auto });
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{ Height = new System.Windows.GridLength(1D, System.Windows.GridUnitType.Star) });

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

        return gridLayout;
    }
    private System.Windows.Controls.TextBox PathBox_Build()
    {
        var box = new System.Windows.Controls.TextBox
        {
            BorderThickness = new System.Windows.Thickness(0D),
            IsReadOnly = true,
            TabIndex = 1,
        };
        return box;
    }
    private System.Windows.Controls.TextBox TextBox_Build()
    {
        var box = new TextEditor.TextBox
        {
            TabIndex = 0,  // 将 Tab 序提至最前，使切换至当前选项卡时的自动焦点落至自身
        };
        return box;
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