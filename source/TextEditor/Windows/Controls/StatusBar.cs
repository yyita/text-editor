namespace TextEditor.Controls;

public class StatusBar : System.Windows.Controls.Primitives.StatusBar
{
    private System.Windows.Controls.TextBlock? TextMode_TextBlock;
    private System.Windows.Controls.TextBlock? Coordinates_TextBlock;
    private System.Windows.Controls.TextBlock? Coding_TextBlock;

    public TextEditor.TextMode TextMode {
        get;
        set
        {
            field = value;
            string displayString;
            if (value == TextEditor.TextMode.Insert)
            {
                displayString = "插入";
            }
            else if (value == TextEditor.TextMode.Overwrite)
            {
                displayString = "覆写";
            }
            else
            {
                displayString = "只读";
            }
            this.TextMode_TextBlock!.Text = displayString;
        }
    } = TextEditor.TextMode.Insert;

    private static (int, int) DefaultCoordinates = (0, 0);
    public (int, int) Coordinates
    {
        get;
        set
        {
            field = value;
            var (row, column) = value;
            this.Coordinates_TextBlock!.Text = $"第 {row + 1} 行，第 {column + 1} 列";
        }
    } = DefaultCoordinates;

    private static System.Text.Encoding DefaultCoding = System.Text.Encoding.UTF8;
    public System.Text.Encoding Coding
    {
        get;
        set
        {
            field = value;
            this.Coding_TextBlock!.Text = value.EncodingName;
        }
    } = DefaultCoding;

    public StatusBar()
    {
        System.Windows.Controls.Separator separator;

        this.Items.Add( AreaBuild_Coordinates() );
        this.Items.Add( AreaBuild_Coding() );
        separator = new System.Windows.Controls.Separator
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(separator, System.Windows.Controls.Dock.Right);
        this.Items.Add( separator );
        this.Items.Add( AreaBuild_TextMode() );
    }
    private System.Windows.Controls.Primitives.StatusBarItem AreaBuild_Coordinates()
    {
        this.Coordinates_TextBlock = new System.Windows.Controls.TextBlock();
        this.Coordinates = TextEditor.Controls.StatusBar.DefaultCoordinates;
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
    private System.Windows.Controls.Primitives.StatusBarItem AreaBuild_Coding()
    {
        this.Coding_TextBlock = new System.Windows.Controls.TextBlock();
        this.Coding = TextEditor.Controls.StatusBar.DefaultCoding;
        var statusBarItem = new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = this.Coding_TextBlock,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
        System.Windows.Controls.DockPanel.SetDock(statusBarItem, System.Windows.Controls.Dock.Right);
        return statusBarItem;
    }
}
