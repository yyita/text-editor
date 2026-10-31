namespace TextEditor.Controls;

public class StatusBar : System.Windows.Controls.Primitives.StatusBar
{
    private System.Windows.Controls.TextBlock? Coordinates_TextBlock;
    private System.Windows.Controls.TextBlock? TextMode_TextBlock;
    private System.Windows.Controls.TextBlock? LineBreak_TextBlock;
    private System.Windows.Controls.TextBlock? Coding_TextBlock;

    private static (int, int) DefaultCoordinates = (0, 0);
    public (int, int) Coordinates
    {
        get;
        set
        {
            var (row, column) = value;
            this.Coordinates_TextBlock!.Text = $"第 {row + 1} 行，第 {column + 1} 列";
            field = value;
        }
    } = DefaultCoordinates;

    public TextEditor.TextMode TextMode {
        get;
        set
        {
            string displayText;
            if     ( value == TextEditor.TextMode.Insert )   { displayText = "插入"; }
            else if( value == TextEditor.TextMode.Overwrite ){ displayText = "覆写"; }
            else                                             { displayText = "只读"; }
            this.TextMode_TextBlock!.Text = displayText;
            field = value;
        }
    } = TextEditor.TextMode.Insert;

    private static string DefaultLineBreak = Environment.NewLine;
    public string LineBreak
    {
        get;
        set
        {
            string displayText;
            if     ( value == "\r\n" ){ displayText = "CRLF"; }
            else if( value == "\n"   ){ displayText = "LF"; }
            else{ throw new ArgumentException( "Only accept '\r\n' and '\n'." ); }
            this.LineBreak_TextBlock!.Text = displayText;
            field = value;
        }
    } = DefaultLineBreak;

    private static System.Text.Encoding DefaultCoding = new System.Text.UTF8Encoding();
    public System.Text.Encoding Coding
    {
        get;
        set
        {
            this.Coding_TextBlock!.Text = value.EncodingName;
            field = value;
        }
    } = DefaultCoding;

    public StatusBar()
    {
        this.Items.Add( AreaBuild_Coordinates() );
        this.Items.Add( AreaBuild_Coding() );
        this.Items.Add( Build_RightSeparator() );
        this.Items.Add( AreaBuild_LineBreak() );
        this.Items.Add( Build_RightSeparator() );
        this.Items.Add( AreaBuild_TextMode() );

        this.Coordinates = TextEditor.Controls.StatusBar.DefaultCoordinates;
        this.LineBreak = TextEditor.Controls.StatusBar.DefaultLineBreak;
        this.Coding = TextEditor.Controls.StatusBar.DefaultCoding;
    }
    private System.Windows.Controls.Separator Build_RightSeparator()
    {
        var separator = new System.Windows.Controls.Separator
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right ,
            Margin = new System.Windows.Thickness( 2D, 0D, 2D, 0D )
        };
        System.Windows.Controls.DockPanel.SetDock( separator, System.Windows.Controls.Dock.Right );
        return separator;
    }
    private System.Windows.Controls.Primitives.StatusBarItem AreaBuild_Coordinates()
    {
        this.Coordinates_TextBlock = new System.Windows.Controls.TextBlock();
        return new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.Coordinates_TextBlock,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
    }
    private System.Windows.Controls.Primitives.StatusBarItem AreaBuild_TextMode()
    {
        this.TextMode_TextBlock = new System.Windows.Controls.TextBlock{ Text = "插入" };
        var statusBarItem = new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.TextMode_TextBlock,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(statusBarItem, System.Windows.Controls.Dock.Right);
        return statusBarItem;
    }
    private System.Windows.Controls.Primitives.StatusBarItem AreaBuild_LineBreak()
    {
        this.LineBreak_TextBlock = new System.Windows.Controls.TextBlock();
        var statusBarItem = new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.LineBreak_TextBlock,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(statusBarItem, System.Windows.Controls.Dock.Right);
        return statusBarItem;
    }
    private System.Windows.Controls.Primitives.StatusBarItem AreaBuild_Coding()
    {
        this.Coding_TextBlock = new System.Windows.Controls.TextBlock();
        var statusBarItem = new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.Coding_TextBlock,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(statusBarItem, System.Windows.Controls.Dock.Right);
        return statusBarItem;
    }
}
