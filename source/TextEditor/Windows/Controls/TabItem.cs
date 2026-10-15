namespace TextEditor;

public class TabItem : System.Windows.Controls.TabItem
{
    public TabItem(string filePath)
    {
        Content = BuildGridLayout();
        this.FilePath = filePath;
        this.TextBox.Text = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
        this.RawData = this.TextBox.Text;

        Loaded += OnLoaded;
    }
    public System.Windows.Controls.TextBox TextBox = null!;
    public bool IsDirty { get => this.RawData != this.TextBox.Text; }
    private string RawData = null!;
    private System.Windows.Controls.TextBox PathBox = null!;
    public string FilePath {
        get => this.PathBox.Text;
        private set {
            this.Header = System.IO.Path.GetFileName(value);
            this.PathBox.Text = value;
        }
    }
    private void OnLoaded(
        object sender,
        System.Windows.RoutedEventArgs evt
    ){
        this.TextBox.Focus();
        this.TextBox.CaretMoveToDocumentEnd();
        this.TextBox.TextChanged += this.TextBox_OnTextChanged;
    }
    private void TextBox_OnTextChanged(
        object sender,
        System.Windows.Controls.TextChangedEventArgs evt
    ){
        var header = (string) this.Header;
        if (this.IsDirty) {
            if (!header.StartsWith("*"))
                this.Header = $"*{header}";
        }
        else if (header.StartsWith("*"))
            this.Header = header.Substring(1);
    }
    private System.Windows.Controls.Grid BuildGridLayout()
    {
        var gridLayout = new System.Windows.Controls.Grid();

        var row_1 = new System.Windows.Controls.RowDefinition{ Height = System.Windows.GridLength.Auto };
        var row_2 = new System.Windows.Controls.RowDefinition{ Height = new System.Windows.GridLength( 1D, System.Windows.GridUnitType.Star ) };

        gridLayout.RowDefinitions.Add( row_1 );
        gridLayout.RowDefinitions.Add( row_2 );

        var pathBox = new System.Windows.Controls.TextBox {
            IsReadOnly = true,
            TabIndex = 1,
        };
        System.Windows.Controls.Grid.SetRow(pathBox, 0);
        System.Windows.Controls.Grid.SetColumn(pathBox, 0);
        gridLayout.Children.Add(pathBox);
        this.PathBox = pathBox;

        var textBox = new System.Windows.Controls.TextBox {
            AcceptsReturn = true,
            AcceptsTab = true,
            TextWrapping = System.Windows.TextWrapping.NoWrap,
            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
            FontFamily = new System.Windows.Media.FontFamily( "Fira Code" ),
            FontSize = 16D,
            TabIndex = 0,  // 将 Tab 序提至最前，使切换至当前选项卡时的自动焦点落至自身
        };
        System.Windows.Controls.Grid.SetRow(textBox, 1);
        System.Windows.Controls.Grid.SetColumn(textBox, 0);
        gridLayout.Children.Add(textBox);
        this.TextBox = textBox;

        return gridLayout;
    }
    public void Save()
    {
        if (this.IsDirty) {
            System.IO.File.WriteAllText(this.FilePath, this.TextBox.Text, System.Text.Encoding.UTF8);
            this.RawData = this.TextBox.Text;
            this.Header = ((string) this.Header).Substring(1);
        }
    }
}