namespace TextEditor.Controls;

public class StatusBar : System.Windows.Controls.Primitives.StatusBar
{
 // 构造函数
    public StatusBar()
    {
        this.Items.Add(BuildCoordinatesArea());
        this.Items.Add(BuildCodingArea());
        this.Items.Add(BuildRightSeparator());
        this.Items.Add(BuildLineBreakArea());
        this.Items.Add(BuildRightSeparator());
        this.Items.Add(BuildTextModeArea());

        this.Coordinates = TextEditor.Controls.StatusBar.DefaultCoordinates;
        this.TextMode = TextEditor.Controls.StatusBar.DefaultTextMode;
        this.LineBreak = TextEditor.Controls.StatusBar.DefaultLineBreak;
        this.Coding = TextEditor.Controls.StatusBar.DefaultCoding;
    }
 // 数据槽
    private static (Int32, Int32, Int32, Int32) DefaultCoordinates = (0, 1, 0, 1);
    private static TextEditor.TextMode DefaultTextMode = TextEditor.TextMode.Insert;
    private static String DefaultLineBreak = Environment.NewLine;
    private static System.Text.Encoding DefaultCoding = new System.Text.UTF8Encoding();
    private System.Windows.Controls.TextBlock? CoordinatesTextBlock;
    private System.Windows.Controls.TextBlock? TextModeTextBlock;
    private System.Windows.Controls.TextBlock? LineBreakTextBlock;
    private System.Windows.Controls.TextBlock? CodingTextBlock;
 // 访问器
    public (Int32, Int32, Int32, Int32) Coordinates
    {
        get;
        set
        {
            var (row, rowCount, column, columnCount) = value;
            this.CoordinatesTextBlock!.Text = $"第 {row + 1} 行，共 {rowCount} 行；第 {column + 1} 列，共 {columnCount} 列";
            field = value;
        }
    }
    public TextEditor.TextMode TextMode
    {
        get;
        set
        {
            string displayText;
            if      (value == TextEditor.TextMode.Insert)    { displayText = "插入"; }
            else if (value == TextEditor.TextMode.Overwrite) { displayText = "覆写"; }
            else                                             { displayText = "只读"; }
            this.TextModeTextBlock!.Text = displayText;
            field = value;
        }
    }
    public string LineBreak
    {
        get;
        set
        {
            string displayText;
            if      (value == "\r\n") { displayText = "CRLF"; }
            else if (value == "\n")   { displayText = "LF"; }
            else { throw new ArgumentException("Only accept '\r\n' and '\n'."); }
            this.LineBreakTextBlock!.Text = displayText;
            field = value;
        }
    }
    public System.Text.Encoding Coding
    {
        get;
        set
        {
            this.CodingTextBlock!.Text = value.EncodingName;
            field = value;
        }
    }
 // 实例函数
    private System.Windows.Controls.Separator BuildRightSeparator()
    {
        var separator = new System.Windows.Controls.Separator
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(separator, System.Windows.Controls.Dock.Right);
        return separator;
    }
    private System.Windows.Controls.Primitives.StatusBarItem BuildCoordinatesArea()
    {
        this.CoordinatesTextBlock = new System.Windows.Controls.TextBlock();
        return new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.CoordinatesTextBlock,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
    }
    private System.Windows.Controls.Primitives.StatusBarItem BuildTextModeArea()
    {
        this.TextModeTextBlock = new System.Windows.Controls.TextBlock{ Text = "插入" };
        var statusBarItem = new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.TextModeTextBlock,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(statusBarItem, System.Windows.Controls.Dock.Right);
        return statusBarItem;
    }
    private System.Windows.Controls.Primitives.StatusBarItem BuildLineBreakArea()
    {
        this.LineBreakTextBlock = new System.Windows.Controls.TextBlock();
        var statusBarItem = new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.LineBreakTextBlock,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(statusBarItem, System.Windows.Controls.Dock.Right);
        return statusBarItem;
    }
    private System.Windows.Controls.Primitives.StatusBarItem BuildCodingArea()
    {
        this.CodingTextBlock = new System.Windows.Controls.TextBlock();
        var statusBarItem = new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.CodingTextBlock,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(statusBarItem, System.Windows.Controls.Dock.Right);
        return statusBarItem;
    }
}
