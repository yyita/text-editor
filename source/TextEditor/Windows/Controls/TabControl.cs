namespace TextEditor.Controls;

public class TabControl : System.Windows.Controls.TabControl
{
    public TabControl()
    {
        this.AllowDrop = true;
        this.Loaded += OnLoaded;
    }
    static TabControl()
    {
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Controls.TabControl), new System.Windows.Input.CommandBinding(TextEditor.FileCommands.Save,  Save_Execute,  Save_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(TextEditor.Controls.TabControl), new System.Windows.Input.CommandBinding(TextEditor.FileCommands.Close, Close_Execute, Close_CanExecute));
    }
    private void OnLoaded(object sender, System.Windows.RoutedEventArgs evt)
    {
        this.MouseDown += OnMiddleButtonPressed;
    }
    private void OnMiddleButtonPressed(object sender, System.Windows.Input.MouseButtonEventArgs evt)
    {
        if (evt.MiddleButton != System.Windows.Input.MouseButtonState.Pressed || evt.Handled) return;
        if (evt.Source is TextEditor.Controls.TabItem tabItem)
            this.Items.Remove(tabItem);
    }
    protected override void OnPreviewDragOver(System.Windows.DragEventArgs evt)
    {
        if (!evt.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            return;
        var filePaths = evt.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];
        evt.Effects = filePaths!.All(filePath => System.IO.File.Exists(filePath)) ? System.Windows.DragDropEffects.Copy : System.Windows.DragDropEffects.None;
        evt.Handled = true;
    }
    protected override void OnPreviewDrop(System.Windows.DragEventArgs evt)
    {
        if (!evt.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            return;
        var filePaths = evt.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];
        if (filePaths!.Length == 1)
            this.New(filePaths[0]);
        else
            this.New(filePaths);
    }
    public TextEditor.Controls.TabItem? TryGetTabItem(string filePath)
    {
        foreach (TextEditor.Controls.TabItem existing in this.Items)
        {
            if (existing.FilePath == filePath)
                return existing;
        }
        return null;
    }
    // 从单个文件新建标签页并选中。当文件已有对应标签页，选中时弹窗以提示，否则选中以提示。
    public void New(string filePath)
    {
        var tabItem = this.TryGetTabItem(filePath);
        if (tabItem == null)
        {
            tabItem = new TextEditor.Controls.TabItem(filePath);
            this.Items.Add(tabItem);
            this.SelectedItem = tabItem;
        }
        else
        {
            if (tabItem == this.SelectedItem)
                System.Windows.MessageBox.Show(System.Windows.Window.GetWindow(this), "当前已选中对应标签页，未对其进行重载。");
            else
                this.SelectedItem = tabItem;
        }
    }
    // 从多个文件新建标签页并选中第一个。当文件已有对应标签页时弹窗以提示。
    public void New(string[] filePaths)
    {
        TextEditor.Controls.TabItem? firstNewTab = null;
        var newTabFiles = new List<String>();
        var existingTabItemFiles = new List<String>();
        foreach (string filePath in filePaths)
        {
            var tabItem = this.TryGetTabItem(filePath);
            if (tabItem == null)
            {
                tabItem = new TextEditor.Controls.TabItem(filePath);
                this.Items.Add(tabItem);
                firstNewTab ??= tabItem;
                newTabFiles.Add(filePath);
            }
            else existingTabItemFiles.Add(filePath);
        }
        if (firstNewTab != null)
            this.SelectedItem = firstNewTab;
        if (existingTabItemFiles.Count > 0)
        {
            var messages = new System.Text.StringBuilder();
            if (newTabFiles.Count > 0)
                messages.Append($"已加载以下 {newTabFiles.Count} 个文件：\n{String.Join('\n', newTabFiles)}\n");
            messages.Append($"未重载以下 {existingTabItemFiles.Count} 个文件：\n{String.Join('\n', existingTabItemFiles)}");
            System.Windows.MessageBox.Show(System.Windows.Window.GetWindow(this), messages.ToString());
        }
    }
    private static void Save_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        if (evt.OriginalSource is TextEditor.Controls.TabControl tabControl)
            ((TextEditor.Controls.TabItem) tabControl.SelectedItem).Save();
    }
    private static void Save_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        if (evt.OriginalSource is TextEditor.Controls.TabControl tabControl) {
            TabItem? selectedItem = tabControl.SelectedItem as TabItem;
            evt.CanExecute = selectedItem != null && selectedItem.IsDirty;
        }
    }
    private static void Close_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        if (evt.OriginalSource is TextEditor.Controls.TabControl tabControl)
            tabControl.Items.Remove(tabControl.SelectedItem);
    }
    private static void Close_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        if (evt.OriginalSource is TextEditor.Controls.TabControl tabControl && tabControl.SelectedItem != null)
            evt.CanExecute = true;
    }
}
