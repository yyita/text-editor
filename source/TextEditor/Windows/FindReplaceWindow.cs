namespace TextEditor.Windows;

public class FindReplaceWindow : System.Windows.Window
{
    /*******
     构造函数
     *******/
    public FindReplaceWindow(TextEditor.Controls.TabControl tabControl, TextEditor.SearchMode mode)
    {
        this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
        this.UseLayoutRounding = true;
        this.WindowStyle = System.Windows.WindowStyle.None;
        this.ResizeMode = System.Windows.ResizeMode.NoResize;
        this.ShowInTaskbar = false;
        this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

        this.TabControl = tabControl;

        System.Windows.Shell.WindowChrome.SetWindowChrome(this, new System.Windows.Shell.WindowChrome{ CaptionHeight = 20D });
        this.Content = this.BuildGridLayout();

        var tabItem = tabControl.SelectedItem as TextEditor.Controls.TabItem;
        var hasTextBoxSelection = (tabItem != null
                                && tabItem.TextBox.SelectionLength != 0);

        if (hasTextBoxSelection)
            this.FindInput.Text = tabItem!.TextBox.SelectedText;

        if (mode == TextEditor.SearchMode.Find)
        {
            this.FindInput.Focus();
            this.FindInput.CaretMoveToDocumentEnd();
            this.ChangeReplacementAreaVisibility(false);
        }
        else if (mode == TextEditor.SearchMode.Replace)
        {
            if (hasTextBoxSelection)
                this.ReplaceInput.Focus();
            else
                this.FindInput.Focus();
            this.ChangeReplacementAreaVisibility(true);
        }
    }
    static FindReplaceWindow()
    {
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Windows.FindReplaceWindow), new System.Windows.Input.CommandBinding(TextEditor.SearchCommands.FindPrevious, FindPrevious_Execute, FindPrevious_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Windows.FindReplaceWindow), new System.Windows.Input.CommandBinding(TextEditor.SearchCommands.FindNext,     FindNext_Execute,     FindNext_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Windows.FindReplaceWindow), new System.Windows.Input.CommandBinding(TextEditor.ReplacementCommands.Replace,    Replace_Execute,    Found_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Windows.FindReplaceWindow), new System.Windows.Input.CommandBinding(TextEditor.ReplacementCommands.ReplaceAll, ReplaceAll_Execute, Found_CanExecute));
    }

    /*********************
     数据对象（字段 (Fields)）
     *********************/
    private TextEditor.Controls.TabControl TabControl = null!;  // 用于动态获取当前选项卡的编辑框
    private TextEditor.Controls.TextBox FindInput = null!;
    private TextEditor.Controls.TextBox ReplaceInput = null!;
    private System.Windows.Controls.CheckBox IgnoreCaseCheckBox = null!;
    private System.Windows.Controls.RowDefinition ReplaceRow = null!;
    private System.Windows.Controls.Primitives.ToggleButton ToggleButton = null!;
    private System.Windows.Controls.Button ReplaceButton = null!;
    private System.Windows.Controls.Button ReplaceAllButton = null!;
    public TextEditor.SearchDirection LastSearchDirection = TextEditor.SearchDirection.Forward;

    /*******
     类级函数
     *******/
    private static void FindPrevious_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var window = ((TextEditor.Windows.FindReplaceWindow) sender);
        if (!window.TrySearch(TextEditor.SearchDirection.Backward))
        {
            System.Windows.MessageBox.Show(window, "Text not found.");
            return;
        }
    }
    private static void FindPrevious_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        var window = ((TextEditor.Windows.FindReplaceWindow) sender);
        if (window.TabControl.SelectedItem is TextEditor.Controls.TabItem tabItem)
        {
            if (window.FindInput.Text != ""           // 1. 待查找文本不为空：查找空串不在预期用法内。
             && tabItem.TextBox.SelectionStart != 0)  // 2. 未超出文本下边界：边界之外找不到任何文本。
                evt.CanExecute = true;
        }
    }
    private static void FindNext_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var window = ((TextEditor.Windows.FindReplaceWindow) sender);
        if (!window.TrySearch(TextEditor.SearchDirection.Forward))
        {
            System.Windows.MessageBox.Show(window, "Text not found.");
            return;
        }
    }
    private static void FindNext_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        var window = ((TextEditor.Windows.FindReplaceWindow) sender);
        if (window.TabControl.SelectedItem is TextEditor.Controls.TabItem tabItem)
        {
            if (window.FindInput.Text != ""                                   // 1. 待查找文本不为空：查找空串不在预期用法内。
             && tabItem.TextBox.SelectionEnd != tabItem.TextBox.Text.Length)  // 2. 未超出文本上边界：边界之外找不到任何文本。
                evt.CanExecute = true;
        }
    }
    private static void Replace_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var window = ((TextEditor.Windows.FindReplaceWindow) sender);
        ((TextEditor.Controls.TabItem) window.TabControl.SelectedItem).TextBox.SelectedText = window.ReplaceInput.Text;
        window.TrySearch(window.LastSearchDirection);
    }
    private static void ReplaceAll_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var window = ((TextEditor.Windows.FindReplaceWindow) sender);
        var textBox = ((TextEditor.Controls.TabItem) window.TabControl.SelectedItem).TextBox;
        var stringComparison = window.IgnoreCaseCheckBox.IsChecked == true ? StringComparison.OrdinalIgnoreCase
                                                                           : StringComparison.Ordinal;
        textBox.Text = textBox.Text.Replace(window.FindInput.Text, window.ReplaceInput.Text, stringComparison);
    }
    private static void Found_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        var window = ((TextEditor.Windows.FindReplaceWindow) sender);
        if (window.TabControl.SelectedItem is TextEditor.Controls.TabItem tabItem)
        {
            var stringComparison = window.IgnoreCaseCheckBox.IsChecked == true ? StringComparison.OrdinalIgnoreCase
                                                                               : StringComparison.Ordinal;
            if (tabItem.TextBox.SelectedText.Equals(window.FindInput.Text, stringComparison))  // 要求至少已找到1个目标文本
                evt.CanExecute = true;
        }
    }

    /*******
     方法函数
     *******/
    private void ChangeReplacementAreaVisibility(Boolean value)
    {
        System.Windows.GridLength height;
        String toggleButtonContent;
        String toolTipContent;
        System.Windows.Visibility visibility;

        if (value)
        {
            height = System.Windows.GridLength.Auto;
            toggleButtonContent = "▲";
            toolTipContent = "Show Replace";
            visibility = System.Windows.Visibility.Visible;
        }
        else
        {
            height = new System.Windows.GridLength(0D);
            toggleButtonContent = "▼";
            toolTipContent = "Hide Replace";
            visibility = System.Windows.Visibility.Collapsed;
        }

        this.ReplaceRow.Height = height;
        this.ToggleButton.Content = toggleButtonContent;
        this.ToggleButton.ToolTip = new System.Windows.Controls.ToolTip{ Content = toolTipContent };
        this.ReplaceInput.Visibility = visibility;
        this.ReplaceButton.Visibility = visibility;
        this.ReplaceAllButton.Visibility = visibility;
    }
    protected override void OnClosing(System.ComponentModel.CancelEventArgs evt)
    {
        base.OnClosing(evt);
        if (this.Owner != null)
            this.Owner.Activate();
    }
    private System.Windows.Controls.Grid BuildGridLayout()
    {
        var gridLayout = new System.Windows.Controls.Grid();

        this.ReplaceRow = new System.Windows.Controls.RowDefinition();
        gridLayout.RowDefinitions.Add( new System.Windows.Controls.RowDefinition{ Height = System.Windows.GridLength.Auto } );
        gridLayout.RowDefinitions.Add( new System.Windows.Controls.RowDefinition{ Height = System.Windows.GridLength.Auto } );
        gridLayout.RowDefinitions.Add(this.ReplaceRow);

        gridLayout.ColumnDefinitions.Add( new System.Windows.Controls.ColumnDefinition{ Width = new System.Windows.GridLength(20D) } );
        gridLayout.ColumnDefinitions.Add( new System.Windows.Controls.ColumnDefinition{ Width = new System.Windows.GridLength(150D) } );
        gridLayout.ColumnDefinitions.Add( new System.Windows.Controls.ColumnDefinition{ Width = System.Windows.GridLength.Auto } );
        gridLayout.ColumnDefinitions.Add( new System.Windows.Controls.ColumnDefinition{ Width = new System.Windows.GridLength(20D) } );
        gridLayout.ColumnDefinitions.Add( new System.Windows.Controls.ColumnDefinition{ Width = new System.Windows.GridLength(20D) } );

        var titleBar = this.BuildTitleBar();
        System.Windows.Controls.Grid.SetRow(titleBar, 0);
        System.Windows.Controls.Grid.SetColumn(titleBar, 0);
        System.Windows.Controls.Grid.SetColumnSpan(titleBar, 6);
        gridLayout.Children.Add(titleBar);

        var closeButton = this.BuildCloseButton();
        System.Windows.Shell.WindowChrome.SetIsHitTestVisibleInChrome(closeButton, true);
        System.Windows.Controls.Grid.SetRow(closeButton, 0);
        System.Windows.Controls.Grid.SetColumn(closeButton, 4);
        gridLayout.Children.Add(closeButton);

        this.ToggleButton = this.BuildToggleButton();
        System.Windows.Controls.Grid.SetRow(this.ToggleButton, 1);
        System.Windows.Controls.Grid.SetRowSpan(this.ToggleButton, 2);
        System.Windows.Controls.Grid.SetColumn(this.ToggleButton, 0);
        gridLayout.Children.Add(this.ToggleButton);

        this.FindInput = this.BuildFindInputBox();
        System.Windows.Controls.Grid.SetRow(this.FindInput, 1);
        System.Windows.Controls.Grid.SetColumn(this.FindInput, 1);
        gridLayout.Children.Add(this.FindInput);

        this.IgnoreCaseCheckBox = this.BuildIgnoreCaseCheckBox();
        System.Windows.Controls.Grid.SetRow(this.IgnoreCaseCheckBox, 1);
        System.Windows.Controls.Grid.SetColumn(this.IgnoreCaseCheckBox, 2);
        gridLayout.Children.Add(this.IgnoreCaseCheckBox);

        var findPreviousButton = this.BuildFindPreviousButton();
        System.Windows.Controls.Grid.SetRow(findPreviousButton, 1);
        System.Windows.Controls.Grid.SetColumn(findPreviousButton, 3);
        gridLayout.Children.Add(findPreviousButton);

        var findNextButton = this.BuildFindNextButton();
        System.Windows.Controls.Grid.SetRow(findNextButton, 1);
        System.Windows.Controls.Grid.SetColumn(findNextButton, 4);
        gridLayout.Children.Add(findNextButton);

        this.ReplaceInput = this.BuildReplaceInputBox();
        System.Windows.Controls.Grid.SetRow(this.ReplaceInput, 2);
        System.Windows.Controls.Grid.SetColumn(this.ReplaceInput, 1);
        gridLayout.Children.Add(this.ReplaceInput);

        this.ReplaceButton = this.BuildReplaceButton();
        System.Windows.Controls.Grid.SetRow(this.ReplaceButton, 2);
        System.Windows.Controls.Grid.SetColumn(this.ReplaceButton, 3);
        gridLayout.Children.Add(this.ReplaceButton);

        this.ReplaceAllButton = this.BuildReplaceAllButton();
        System.Windows.Controls.Grid.SetRow(this.ReplaceAllButton, 2);
        System.Windows.Controls.Grid.SetColumn(this.ReplaceAllButton, 4);
        gridLayout.Children.Add(this.ReplaceAllButton);

        return gridLayout;
    }
    private System.Windows.Controls.DockPanel BuildTitleBar()
    {
        return new System.Windows.Controls.DockPanel
        {
            Height = 20D
        };
    }
    private System.Windows.Controls.Button BuildCloseButton()
    {
        var closeButton = new System.Windows.Controls.Button
        {
            Content = "✕",
            Background = System.Windows.Media.Brushes.White,
            BorderThickness = new System.Windows.Thickness(0D),
            ToolTip = new System.Windows.Controls.ToolTip{ Content = "关闭 (Esc)" }
        };
        closeButton.Click += (object sender, System.Windows.RoutedEventArgs evt) =>
        {
            this.Close();
        };
        return closeButton;
    }
    private System.Windows.Controls.Primitives.ToggleButton BuildToggleButton()
    {
        var toggleButton = new System.Windows.Controls.Primitives.ToggleButton
        {
            Background = System.Windows.Media.Brushes.White,
            BorderThickness = new System.Windows.Thickness(0D)
        };
        toggleButton.Click += (object sender, System.Windows.RoutedEventArgs evt) =>
        {
            this.ChangeReplacementAreaVisibility( ((System.Windows.Controls.Primitives.ToggleButton) sender).IsChecked == true );
        };
        return toggleButton;
    }
    private TextEditor.Controls.TextBox BuildFindInputBox()
    {
        return new TextEditor.Controls.TextBox
        {
            AcceptsReturn = true,
            Margin = new System.Windows.Thickness(0D, 0D, 2D, 0D),
            ToolTip = new System.Windows.Controls.ToolTip{ Content = "搜索对象" }
        };
    }
    private System.Windows.Controls.CheckBox BuildIgnoreCaseCheckBox()
    {
        return new System.Windows.Controls.CheckBox
        {
            IsChecked = true,
            Content = "忽略大小写",
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            Margin = new System.Windows.Thickness(2D, 0D, 2D, 0D)
        };
    }
    private System.Windows.Controls.Button BuildFindPreviousButton()
    {
        return new System.Windows.Controls.Button
        {
            Content = "↑",
            Command = TextEditor.SearchCommands.FindPrevious,
            CommandTarget = this,
            Margin = new System.Windows.Thickness(2D, 0D, 0D, 0D),
            ToolTip = new System.Windows.Controls.ToolTip{ Content = "查找上一个 (Ctrl+Up)" }
        };
    }
    private System.Windows.Controls.Button BuildFindNextButton()
    {
        return new System.Windows.Controls.Button
        {
            Content = "↓",
            Command = TextEditor.SearchCommands.FindNext,
            CommandTarget = this,
            ToolTip = new System.Windows.Controls.ToolTip{ Content = "查找下一个 (Ctrl+Down)" }
        };
    }
    private TextEditor.Controls.TextBox BuildReplaceInputBox()
    {
        return new TextEditor.Controls.TextBox
        {
            AcceptsReturn = true,
            Margin = new System.Windows.Thickness(0D, 0D, 2D, 0D),
            ToolTip = new System.Windows.Controls.ToolTip{ Content = "替换结果" }
        };
    }
    private System.Windows.Controls.Button BuildReplaceButton()
    {
        return new System.Windows.Controls.Button
        {
            Content = "↓",
            Command = TextEditor.ReplacementCommands.Replace,
            CommandTarget = this,
            Margin = new System.Windows.Thickness(2D, 0D, 0D, 0D),
            ToolTip = new System.Windows.Controls.ToolTip{ Content = "替换当前 (Ctrl+Enter)" }
        };
    }
    private System.Windows.Controls.Button BuildReplaceAllButton()
    {
        return new System.Windows.Controls.Button
        {
            Content = "⇊",
            Command = TextEditor.ReplacementCommands.ReplaceAll,
            CommandTarget = this,
            ToolTip = new System.Windows.Controls.ToolTip{ Content = "替换全部 (Ctrl+Shift+Enter)" }
        };
    }
    public Boolean TrySearch(TextEditor.SearchDirection direction)
    {
        String searchTarget;
        StringComparison stringComparison;
        TextEditor.Controls.TextBox textBox;
        Int32 searchStart, resultFirst;

        searchTarget = this.FindInput.Text;
        textBox = ((TextEditor.Controls.TabItem) this.TabControl.SelectedItem).TextBox;
        stringComparison = this.IgnoreCaseCheckBox.IsChecked == true ? StringComparison.OrdinalIgnoreCase
                                                                     : StringComparison.Ordinal;
        if (direction == TextEditor.SearchDirection.Forward)
        {
            searchStart = textBox.SelectionEnd;
            resultFirst = textBox.Text.IndexOf(searchTarget, searchStart, stringComparison);
        }
        else
        {
            searchStart = textBox.SelectionStart - 1;
            resultFirst = textBox.Text.LastIndexOf(searchTarget, searchStart, stringComparison);
        }
        try
        {
            if (resultFirst == -1)
                return false;
            else
            {
                textBox.Focus();
                textBox.Select(resultFirst, searchTarget.Length);
                return true;
            }
        }
        finally
        {
            this.LastSearchDirection = direction;
        }
    }
}
