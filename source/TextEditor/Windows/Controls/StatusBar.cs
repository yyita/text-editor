namespace TextEditor.Controls;

public class StatusBar : System.Windows.Controls.Primitives.StatusBar
{
    public StatusBar()
    {
        this.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

        var textMode_textBlock = new System.Windows.Controls.TextBlock{ Text = "插入" };
        this.TextMode_TextBlock = textMode_textBlock;
        this.Items.Add(new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = textMode_textBlock,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        });
        this.Items.Add( new System.Windows.Controls.Separator
        {
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        });
        var caretCoordinates_dockPanel = new System.Windows.Controls.DockPanel();
        caretCoordinates_dockPanel.Children.Add( new System.Windows.Controls.TextBlock{ Text = "位置：" } );
        var coordinates_textBlock = new System.Windows.Controls.TextBlock{ Text = "1, 1" };
        this.Coordinates_TextBlock = coordinates_textBlock;
        caretCoordinates_dockPanel.Children.Add(coordinates_textBlock);
        this.Items.Add(new System.Windows.Controls.Primitives.StatusBarItem
        {
            Content = caretCoordinates_dockPanel,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        });
    }
    private System.Windows.Controls.TextBlock? TextMode_TextBlock;
    private System.Windows.Controls.TextBlock? Coordinates_TextBlock;

    public TextEditor.TextMode TextMode {
        get;
        set
        {
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
            field = value;
        }
    } = TextEditor.TextMode.Insert;

    public (int, int) Coordinates
    {
        get;
        set
        {
            var (row, column) = value;
            this.Coordinates_TextBlock!.Text = $"{row + 1}, {column + 1}";
            field = value;
        }
    } = (1, 1);
}
