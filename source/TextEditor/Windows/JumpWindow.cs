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
    private System.Windows.Controls.TextBox RowInputBox = null!;
    private System.Windows.Controls.TextBox ColumnInputBox = null!;
    private System.Windows.Controls.TextBox TextBox = null!;
    public const Double PADDING = 4D;
    public const Double HALF_OF_PADDING = PADDING / 2;
    public const Double INPUT_BOX_WIDTH_MAXIMUM = 76D;  // 10位
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
        var gridLayout = new System.Windows.Controls.Grid{Margin=new System.Windows.Thickness(JumpWindow.PADDING)};

        gridLayout.RowDefinitions.Add(new System.Windows.Controls.RowDefinition{Height=System.Windows.GridLength.Auto});

        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});
        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});
        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});
        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});
        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});
        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});
        gridLayout.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition{Width=System.Windows.GridLength.Auto});

        var rowThTextBlock = this.BuildThTextBlock();
        System.Windows.Controls.Grid.SetRow(rowThTextBlock, 0);
        System.Windows.Controls.Grid.SetColumn(rowThTextBlock, 0);
        gridLayout.Children.Add(rowThTextBlock);

        this.RowInputBox = this.BuildInputBox();
        System.Windows.Controls.Grid.SetRow(this.RowInputBox, 0);
        System.Windows.Controls.Grid.SetColumn(this.RowInputBox, 1);
        gridLayout.Children.Add(this.RowInputBox);

        var rowTextBlock = this.BuildRowTextBlock();
        System.Windows.Controls.Grid.SetRow(rowTextBlock, 0);
        System.Windows.Controls.Grid.SetColumn(rowTextBlock, 2);
        gridLayout.Children.Add(rowTextBlock);

        var commaTextBlock = this.BuildCommaTextBlock();
        System.Windows.Controls.Grid.SetRow(commaTextBlock, 0);
        System.Windows.Controls.Grid.SetColumn(commaTextBlock, 3);
        gridLayout.Children.Add(commaTextBlock);

        var columnThTextBlock = this.BuildThTextBlock();
        System.Windows.Controls.Grid.SetRow(columnThTextBlock, 0);
        System.Windows.Controls.Grid.SetColumn(columnThTextBlock, 4);
        gridLayout.Children.Add(columnThTextBlock);

        this.ColumnInputBox = this.BuildInputBox();
        System.Windows.Controls.Grid.SetRow(this.ColumnInputBox, 0);
        System.Windows.Controls.Grid.SetColumn(this.ColumnInputBox, 5);
        gridLayout.Children.Add(this.ColumnInputBox);

        var columnTextBlock = this.BuildColumnTextBlock();
        System.Windows.Controls.Grid.SetRow(columnTextBlock, 0);
        System.Windows.Controls.Grid.SetColumn(columnTextBlock, 6);
        gridLayout.Children.Add(columnTextBlock);

        return gridLayout;
    }
    private System.Windows.Controls.TextBlock BuildThTextBlock()
    {
        return new System.Windows.Controls.TextBlock
        {
            Text="第",
            HorizontalAlignment=System.Windows.HorizontalAlignment.Center,
            VerticalAlignment=System.Windows.VerticalAlignment.Center,
            Margin=new System.Windows.Thickness(0D, 0D, JumpWindow.HALF_OF_PADDING, 0D)
        };
    }
    private System.Windows.Controls.TextBox BuildInputBox()
    {
        var textBox = new System.Windows.Controls.TextBox
        {
            MaxWidth=JumpWindow.INPUT_BOX_WIDTH_MAXIMUM,
            HorizontalAlignment=System.Windows.HorizontalAlignment.Center,
            VerticalAlignment=System.Windows.VerticalAlignment.Center,
            Margin=new System.Windows.Thickness(JumpWindow.HALF_OF_PADDING, 0D, JumpWindow.HALF_OF_PADDING, 0D)
        };
        System.Windows.Input.InputMethod.SetIsInputMethodEnabled(textBox, false);
        textBox.PreviewTextInput += (object sender, System.Windows.Input.TextCompositionEventArgs evt) =>
        {
            var textBox = (System.Windows.Controls.TextBox) sender;
            var oldText = textBox.Text;
            var newText = oldText.Remove(textBox.SelectionStart, textBox.SelectionLength).Insert(textBox.SelectionStart, evt.Text);
            evt.Handled = !Int32.TryParse(newText, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out _);
        };
        return textBox;
    }
    private System.Windows.Controls.TextBlock BuildRowTextBlock()
    {
        return new System.Windows.Controls.TextBlock
        {
            Text="行",
            HorizontalAlignment=System.Windows.HorizontalAlignment.Center,
            VerticalAlignment=System.Windows.VerticalAlignment.Center,
            Margin=new System.Windows.Thickness(JumpWindow.HALF_OF_PADDING, 0D, 0D, 0D)
        };
    }
    private System.Windows.Controls.TextBlock BuildColumnTextBlock()
    {
        return new System.Windows.Controls.TextBlock
        {
            Text="列",
            HorizontalAlignment=System.Windows.HorizontalAlignment.Center,
            VerticalAlignment=System.Windows.VerticalAlignment.Center,
            Margin=new System.Windows.Thickness(JumpWindow.HALF_OF_PADDING, 0D, 0D, 0D)
        };
    }
    private System.Windows.Controls.TextBlock BuildCommaTextBlock()
    {
        return new System.Windows.Controls.TextBlock
        {
            Text="，",
            HorizontalAlignment=System.Windows.HorizontalAlignment.Center,
            VerticalAlignment=System.Windows.VerticalAlignment.Center
        };
    }
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
