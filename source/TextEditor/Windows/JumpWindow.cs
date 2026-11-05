namespace TextEditor.Windows;

public class JumpWindow : System.Windows.Window
{
 // 构造函数
    public JumpWindow(System.Windows.Controls.TextBox textBox)
    {
        this.Title = "跳转";
        this.ResizeMode = System.Windows.ResizeMode.NoResize;
        this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
        this.UseLayoutRounding = true;
        this.ShowInTaskbar = false;
        this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
        this.Content = this.BuildGridLayout();
        this.TextBox = textBox;
        this.Loaded += this.OnLoaded;
    }
 // 数据槽
    private TextEditor.Controls.TextBox RowInputBox = null!;
    private TextEditor.Controls.TextBox ColumnInputBox = null!;
    private System.Windows.Controls.TextBox TextBox = null!;
 // 实例函数
    protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs evt)
    {
        if (evt.Key == System.Windows.Input.Key.Escape)
        {
            this.Close();
            evt.Handled = true;
        }
        else if (evt.Key == System.Windows.Input.Key.Enter)
        {
            this.TryJump();
            evt.Handled = true;
        }
    }
    private void OnLoaded(object sender, System.Windows.RoutedEventArgs evt)
    {
        this.RowInputBox.Focus();
    }
    private System.Windows.Controls.Grid BuildGridLayout()
    {
        var gridLayout = new System.Windows.Controls.Grid();

        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{Height=System.Windows.GridLength.Auto});
        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{Height=System.Windows.GridLength.Auto});

        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});
        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});

        var rowTextBlock = this.BuildRowTextBlock();
        System.Windows.Controls.Grid.SetRow(rowTextBlock, 0);
        System.Windows.Controls.Grid.SetColumn(rowTextBlock, 0);
        gridLayout.Children.Add(rowTextBlock);

        this.RowInputBox = this.BuildRowInputBox();
        System.Windows.Controls.Grid.SetRow(this.RowInputBox, 0);
        System.Windows.Controls.Grid.SetColumn(this.RowInputBox, 1);
        gridLayout.Children.Add(this.RowInputBox);

        var columnTextBlock = this.BuildColumnTextBlock();
        System.Windows.Controls.Grid.SetRow(columnTextBlock, 1);
        System.Windows.Controls.Grid.SetColumn(columnTextBlock, 0);
        gridLayout.Children.Add(columnTextBlock);

        this.ColumnInputBox = this.BuildColumnInputBox();
        System.Windows.Controls.Grid.SetRow(this.ColumnInputBox, 1);
        System.Windows.Controls.Grid.SetColumn(this.ColumnInputBox, 1);
        gridLayout.Children.Add(this.ColumnInputBox);

        return gridLayout;
    }
    private System.Windows.Controls.TextBlock BuildRowTextBlock()
        => new System.Windows.Controls.TextBlock
        {
            Text="行",
            HorizontalAlignment=System.Windows.HorizontalAlignment.Center,
            Margin=new System.Windows.Thickness(4D, 0D, 2D, 0D)
        };
    private TextEditor.Controls.TextBox BuildRowInputBox()
        => new TextEditor.Controls.TextBox
        {
            Width=300D,
            HorizontalContentAlignment=System.Windows.HorizontalAlignment.Center,
            Margin=new System.Windows.Thickness(2D, 0D, 0D, 0D)
        };
    private System.Windows.Controls.TextBlock BuildColumnTextBlock()
        => new System.Windows.Controls.TextBlock
        {
            Text="列",
            HorizontalAlignment=System.Windows.HorizontalAlignment.Center,
            Margin=new System.Windows.Thickness(4D, 0D, 2D, 0D)
        };
    private TextEditor.Controls.TextBox BuildColumnInputBox()
        => new TextEditor.Controls.TextBox
        {
            Width=300D,
            HorizontalContentAlignment=System.Windows.HorizontalAlignment.Center,
            Margin=new System.Windows.Thickness(2D, 0D, 0D, 0D)
        };
    private void TryJump()
    {
        var textBox = this.TextBox;
        if (!Int32.TryParse(this.RowInputBox.Text, out Int32 row))
        {
            System.Windows.MessageBox.Show(this, "行号： 你的输入不是一个整数。");
            return;
        }
        if (row <= 0)
        {
            System.Windows.MessageBox.Show(this, "行号： 你的输入超出范围下限。");
            return;
        }
        if (row > textBox.LineCount)
        {
            System.Windows.MessageBox.Show(this, "行号： 你的输入超出范围上限。");
            return;
        }
        row--;
        if (!Int32.TryParse(this.ColumnInputBox.Text, out Int32 column))
        {
            System.Windows.MessageBox.Show(this, "列号： 你的输入不是一个整数。");
            return;
        }
        if (column <= 0)
        {
            System.Windows.MessageBox.Show(this, "列号： 你的输入超出范围下限。");
            return;
        }
        if (column > textBox.CountColumn(row))
        {
            System.Windows.MessageBox.Show(this, "列号： 你的输入超出范围上限。");
            return;
        }
        column--;
        this.Close();
        textBox.See(row, column);
    }
}
