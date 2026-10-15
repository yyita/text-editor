using System.Windows;
using System.Windows.Input;
using System.Windows.Media;  // Brushes
using System.Windows.Shell;  // WindowChrome
using System.Windows.Controls;
using System.Windows.Controls.Primitives;  // ToggleButton

namespace TextEditor;

public class FindReplaceWindow : Window
{
    public enum Mode { Find, Replace, Modify };
    private TabControl TabControl = null!;  // 用于动态获取当前选项卡的编辑框
    private TextBox FindInput = null!;
    private TextBox ReplaceInput = null!;
    private CheckBox IgnoreCaseCheckBox = null!;
    private RowDefinition ReplaceRow = null!;
    private ToggleButton ToggleButton = null!;
    private Button ReplaceButton = null!;
    private Button ReplaceAllButton = null!;
    private void ChangeReplacementAreaVisibility(bool value)
    {
        GridLength height;
        string toggleButtonContent;
        string toolTipContent;
        Visibility visibility;

        if (value) {
            height = GridLength.Auto;
            toggleButtonContent = "▲";
            toolTipContent = "Show Replace";
            visibility = Visibility.Visible;
        }
        else {
            height = new GridLength(0D);
            toggleButtonContent = "▼";
            toolTipContent = "Hide Replace";
            visibility = Visibility.Collapsed;
        }

        ReplaceRow.Height = height;
        this.ToggleButton.Content = toggleButtonContent;
        this.ToggleButton.ToolTip = new ToolTip{ Content = toolTipContent };
        this.ReplaceInput.Visibility = visibility;
        this.ReplaceButton.Visibility = visibility;
        this.ReplaceAllButton.Visibility = visibility;
    }
    public FindReplaceWindow(TabControl tabControl, Mode mode)
    {
        this.TabControl = tabControl;

        this.SizeToContent = SizeToContent.WidthAndHeight;
        this.UseLayoutRounding = true;
        this.WindowStyle = WindowStyle.None;
        this.ResizeMode = ResizeMode.NoResize;
        this.ShowInTaskbar = false;
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        WindowChrome.SetWindowChrome(this, new WindowChrome{ CaptionHeight = 20D });
        Content = BuildGridLayout();

        // “自动填入选区”与“自动调整焦点”的行为对照：
        // 无选区、查找模式 => 焦点->查找框
        // 有选区、查找模式 => 焦点->查找框|选区->查找框
        // 无选区、替换模式 => 焦点->查找框
        // 有选区、替换模式 => 焦点->替换框|选区->查找框
        // 无选区、修改模式 => 焦点->查找框
        // 有选区、修改模式 => 焦点->替换框|选区->查找框、替换框

        var tabItem = tabControl.SelectedItem as TabItem;
        var hasTextBoxSelection = (
            tabItem != null
         && tabItem.TextBox.SelectionLength != 0
        );

        if (hasTextBoxSelection)
            this.FindInput.Text = tabItem!.TextBox.SelectedText;

        switch (mode)
        {
            case Mode.Find:
                this.FindInput.Focus();
                this.FindInput.CaretMoveToDocumentEnd();
                ChangeReplacementAreaVisibility(false);
                break;

            case Mode.Replace:
                if (hasTextBoxSelection)
                    this.ReplaceInput.Focus();
                else
                    this.FindInput.Focus();
                ChangeReplacementAreaVisibility(true);
                break;

            case Mode.Modify:
                if (hasTextBoxSelection)
                {
                    this.ReplaceInput.Text = tabItem!.TextBox.SelectedText;
                    this.ReplaceInput.Focus();
                    this.ReplaceInput.CaretMoveToDocumentEnd();
                }
                else
                    this.FindInput.Focus();
                ChangeReplacementAreaVisibility(true);
                break;
        }
    }
    static FindReplaceWindow()
    {
        CommandManager.RegisterClassCommandBinding(typeof(FindReplaceWindow), new CommandBinding(Commands.FindPrevious, FindPrevious_Execute, FindPrevious_CanExecute));
        CommandManager.RegisterClassCommandBinding(typeof(FindReplaceWindow), new CommandBinding(Commands.FindNext,     FindNext_Execute,     FindNext_CanExecute));
        CommandManager.RegisterClassCommandBinding(typeof(FindReplaceWindow), new CommandBinding(Commands.Replace,      Replace_Execute,      Found_CanExecute));
        CommandManager.RegisterClassCommandBinding(typeof(FindReplaceWindow), new CommandBinding(Commands.ReplaceAll,   ReplaceAll_Execute,   Found_CanExecute));
    }
    protected override void OnClosing(
        System.ComponentModel.CancelEventArgs evt
    ){
        base.OnClosing(evt);
        // BUG #2026-10-15_1 修复：
        if (this.Owner != null)
            this.Owner.Activate();
    }
    private Grid BuildGridLayout()
    {
        var gridLayout = new Grid();

        var row_0 = new RowDefinition{ Height = GridLength.Auto };
        gridLayout.RowDefinitions.Add(row_0);

        var row_1 = new RowDefinition{ Height = GridLength.Auto };
        gridLayout.RowDefinitions.Add(row_1);

        var row_2 = new RowDefinition();
        this.ReplaceRow = row_2;
        gridLayout.RowDefinitions.Add(row_2);

        var column_0 = new ColumnDefinition{ Width = new GridLength(20D) };
        var column_1 = new ColumnDefinition{ Width = new GridLength(150D) };
        var column_2 = new ColumnDefinition{ Width = GridLength.Auto };
        var column_3 = new ColumnDefinition{ Width = new GridLength(20D) };
        var column_4 = new ColumnDefinition{ Width = new GridLength(20D) };
        gridLayout.ColumnDefinitions.Add(column_0);
        gridLayout.ColumnDefinitions.Add(column_1);
        gridLayout.ColumnDefinitions.Add(column_2);
        gridLayout.ColumnDefinitions.Add(column_3);
        gridLayout.ColumnDefinitions.Add(column_4);

        var titleBar = new DockPanel{ Height = 20D };
        Grid.SetRow(titleBar, 0);
        Grid.SetColumn(titleBar, 0);
        Grid.SetColumnSpan(titleBar, 6);
        gridLayout.Children.Add(titleBar);

        var closeButton = new Button{
            Content = "✕",
            Background = Brushes.White,
            BorderThickness = new Thickness(0D),
            ToolTip = new ToolTip{ Content = "Close (Esc)" }
        };
        closeButton.Click += (object sender, RoutedEventArgs evt) => {
            this.Close();
        };
        WindowChrome.SetIsHitTestVisibleInChrome(closeButton, true);
        Grid.SetRow(closeButton, 0);
        Grid.SetColumn(closeButton, 4);
        gridLayout.Children.Add(closeButton);

        var toggleButton = new ToggleButton{
            Background = Brushes.White,
            BorderThickness = new Thickness(0D)
        };
        toggleButton.Click += (object sender, RoutedEventArgs evt) => {
            ChangeReplacementAreaVisibility(
                ((ToggleButton) sender).IsChecked == true
            );
        };
        this.ToggleButton = toggleButton;
        Grid.SetRow(toggleButton, 1);
        Grid.SetRowSpan(toggleButton, 2);
        Grid.SetColumn(toggleButton, 0);
        gridLayout.Children.Add(toggleButton);

        var findInput = new TextBox{
            AcceptsReturn = true,
            Margin = new Thickness(0D, 0D, 2D, 0D),
            ToolTip = new ToolTip{ Content = "Find What" }
        };
        this.FindInput = findInput;
        Grid.SetRow(findInput, 1);
        Grid.SetColumn(findInput, 1);
        gridLayout.Children.Add(findInput);

        var ignoreCaseCheckBox = new CheckBox{
            IsChecked = true,
            Content = "Ignore Case",
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(2D, 0D, 2D, 0D)
        };
        this.IgnoreCaseCheckBox = ignoreCaseCheckBox;
        Grid.SetRow(ignoreCaseCheckBox, 1);
        Grid.SetColumn(ignoreCaseCheckBox, 2);
        gridLayout.Children.Add(ignoreCaseCheckBox);

        var findPreviousButton = new Button{
            Content = "↑",
            Command = Commands.FindPrevious,
            CommandTarget = this,
            Margin = new Thickness(2D, 0D, 0D, 0D),
            ToolTip = new ToolTip{ Content = "Find Previous (Ctrl+Up)" }
        };
        Grid.SetRow(findPreviousButton, 1);
        Grid.SetColumn(findPreviousButton, 3);
        gridLayout.Children.Add(findPreviousButton);

        var findNextButton = new Button{
            Content = "↓",
            Command = Commands.FindNext,
            CommandTarget = this,
            ToolTip = new ToolTip{ Content = "Find Next (Ctrl+Down)" }
        };
        Grid.SetRow(findNextButton, 1);
        Grid.SetColumn(findNextButton, 4);
        gridLayout.Children.Add(findNextButton);

        var replaceInput = new TextBox{
            AcceptsReturn = true,
            Margin = new Thickness(0D, 0D, 2D, 0D),
            ToolTip = new ToolTip{ Content = "Replace As" }
        };
        this.ReplaceInput = replaceInput;
        Grid.SetRow(replaceInput, 2);
        Grid.SetColumn(replaceInput, 1);
        gridLayout.Children.Add(replaceInput);

        var replaceButton = new Button{
            Content = "↓",
            Command = Commands.Replace,
            CommandTarget = this,
            Margin = new Thickness(2D, 0D, 0D, 0D),
            ToolTip = new ToolTip{ Content = "Replace (Ctrl+Enter)" }
        };
        this.ReplaceButton = replaceButton;
        Grid.SetRow(replaceButton, 2);
        Grid.SetColumn(replaceButton, 3);
        gridLayout.Children.Add(replaceButton);

        var replaceAllButton = new Button{
            Content = "⇊",
            Command = Commands.ReplaceAll,
            CommandTarget = this,
            ToolTip = new ToolTip{ Content = "Replace All (Ctrl+Shift+Enter)" }
        };
        this.ReplaceAllButton = replaceAllButton;
        Grid.SetRow(replaceAllButton, 2);
        Grid.SetColumn(replaceAllButton, 4);
        gridLayout.Children.Add(replaceAllButton);

        return gridLayout;
    }
    private static void FindPrevious_Execute(object sender, ExecutedRoutedEventArgs evt)
    {
        var window = ((FindReplaceWindow) sender);
        var searchTarget = window.FindInput.Text;
        var textBox = ((TabItem) window.TabControl.SelectedItem).TextBox;
        var searchStart = textBox.SelectionStart - 1;
        var stringComparison = window.IgnoreCaseCheckBox.IsChecked == true ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var resultFirst = textBox.Text.LastIndexOf(searchTarget, searchStart, stringComparison);

        if (resultFirst == -1)
        {
            MessageBox.Show(window, "Text not found.");
            return;
        }

        textBox.Focus();
        textBox.Select(resultFirst, searchTarget.Length);
    }
    private static void FindPrevious_CanExecute(object sender, CanExecuteRoutedEventArgs evt)
    {
        var window = ((FindReplaceWindow) sender);
        if (window.TabControl.SelectedItem is TabItem tabItem)
        {
            if (
                window.FindInput.Text != ""          // 1. 待查找文本不为空：查找空串不在预期用法内。
             && tabItem.TextBox.SelectionStart != 0  // 2. 未超出文本下边界：边界之外找不到任何文本。
            )
                evt.CanExecute = true;
        }
    }
    private static void FindNext_Execute(object sender, ExecutedRoutedEventArgs evt)
    {
        var window = ((FindReplaceWindow) sender);
        var searchTarget = window.FindInput.Text;
        var textBox = ((TabItem) window.TabControl.SelectedItem).TextBox;
        var searchStart = textBox.SelectionEnd;
        var stringComparison = window.IgnoreCaseCheckBox.IsChecked == true ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var resultFirst = textBox.Text.IndexOf(searchTarget, searchStart, stringComparison);

        if (resultFirst == -1)
        {
            MessageBox.Show(window, "Text not found.");
            return;
        }

        textBox.Focus();
        textBox.Select(resultFirst, searchTarget.Length);
    }
    private static void FindNext_CanExecute(object sender, CanExecuteRoutedEventArgs evt)
    {
        var window = ((FindReplaceWindow) sender);
        if (window.TabControl.SelectedItem is TabItem tabItem)
        {
            if (
                window.FindInput.Text != ""                                  // 1. 待查找文本不为空：查找空串不在预期用法内。
             && tabItem.TextBox.SelectionEnd != tabItem.TextBox.Text.Length  // 2. 未超出文本上边界：边界之外找不到任何文本。
            )
                evt.CanExecute = true;
        }
    }
    private static void Replace_Execute(object sender, ExecutedRoutedEventArgs evt)
    {
        var window = ((FindReplaceWindow) sender);
        ((TabItem) window.TabControl.SelectedItem).TextBox.SelectedText = window.ReplaceInput.Text;
    }
    private static void ReplaceAll_Execute(object sender, ExecutedRoutedEventArgs evt)
    {
        var window = ((FindReplaceWindow) sender);
        var textBox = ((TabItem) window.TabControl.SelectedItem).TextBox;
        var stringComparison = window.IgnoreCaseCheckBox.IsChecked == true ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        textBox.Text = textBox.Text.Replace(window.FindInput.Text, window.ReplaceInput.Text, stringComparison);
    }
    private static void Found_CanExecute(object sender, CanExecuteRoutedEventArgs evt)
    {
        var window = ((FindReplaceWindow) sender);
        if (window.TabControl.SelectedItem is TabItem tabItem)
        {
            var stringComparison = window.IgnoreCaseCheckBox.IsChecked == true ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (tabItem.TextBox.SelectedText.Equals(window.FindInput.Text, stringComparison))  // 要求至少已找到1个目标文本
                evt.CanExecute = true;
        }
    }
}
