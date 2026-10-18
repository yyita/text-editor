namespace TextEditor;

public class TextBox : System.Windows.Controls.TextBox
{
    public TextBox()
    {
        this.AcceptsReturn = true;
        this.AcceptsTab = true;
        this.TextWrapping = System.Windows.TextWrapping.NoWrap;
        this.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
        this.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
        this.FontFamily = new System.Windows.Media.FontFamily( "Fira Code" );
        this.FontSize = 16D;

        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.EditCommands.Undo,  Undo_Execute,  Undo_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.EditCommands.Redo,  Redo_Execute,  Redo_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.EditCommands.Cut,   Cut_Execute,   HasSelection_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.EditCommands.Copy,  Copy_Execute,  HasSelection_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.EditCommands.Paste, Paste_Execute, Paste_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.FormatCommands.Upper,        Upper_Execute,         HasSelection_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.FormatCommands.Lower,        Lower_Execute,         HasSelection_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.FormatCommands.Indent,       Indent_Execute,        Focused_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.FormatCommands.Dedent,       Dedent_Execute,        Focused_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.FormatCommands.IndentRegion, IndentRegion_Execute,  Focused_CanExecute));
        this.CommandBindings.Add(new System.Windows.Input.CommandBinding(TextEditor.FormatCommands.DedentRegion, DedentRegion_Execute,  Focused_CanExecute));
    }
    protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs evt)
    {
        // 按键 Tab 自带内置行为，并且会令绑定了它的命令的快捷键失效，
        // 即使对 evt.Key 等于 Key.Tab 的 PreviewKeyDown 事件进行阻塞。
        // 因此，此处为绑定的命令进行了补发，硬编码的触发条件仅用作过渡：
        if (evt.Key == System.Windows.Input.Key.Tab)
        {
            evt.Handled = true;
            var textBox = (System.Windows.Controls.TextBox) evt.Source;
            if (
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) > 0
             && TextEditor.FormatCommands.Dedent.CanExecute(null, textBox)
            )
                TextEditor.FormatCommands.Dedent.Execute(null, textBox);
            else if (TextEditor.FormatCommands.Indent.CanExecute(null, textBox))
                TextEditor.FormatCommands.Indent.Execute(null, textBox);
        }
        base.OnPreviewKeyDown(evt);
    }
    private static void Focused_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        if (sender is System.Windows.Controls.TextBox)
            evt.CanExecute = true;
    }
    private static void HasSelection_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        if (textBox.SelectionLength > 0)
            evt.CanExecute = true;
    }
    private static void Undo_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.Undo();
    }
    private static void Undo_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        if (sender is System.Windows.Controls.TextBox textBox)
            evt.CanExecute = textBox.CanUndo;
    }
    private static void Redo_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.Redo();
    }
    private static void Redo_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        if (sender is System.Windows.Controls.TextBox textBox)
            evt.CanExecute = textBox.CanRedo;
    }
    private static void Cut_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.Cut();
    }
    private static void Copy_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.Copy();
    }
    private static void Paste_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.Paste();
    }
    private static void Paste_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs evt)
    {
        if (sender is System.Windows.Controls.TextBox textBox)
            evt.CanExecute = System.Windows.Clipboard.ContainsText();
    }
    private static void Upper_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.SelectedText = textBox.SelectedText.ToUpper();
    }
    private static void Lower_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.SelectedText = textBox.SelectedText.ToLower();
    }
    private static void Indent_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        var (row, column) = textBox.GetCoordinatesFromCharacterIndex(textBox.CaretIndex);
        var lineText = textBox.GetLineText(row);
        var insertSpaceCount = 4 - (lineText.CountAdjoiningSpaces(column) & 3);  // 0, 1, 2, 3 -> 4, 3, 2, 1
        textBox.BeginChange();
        textBox.SelectedText = new String(' ', insertSpaceCount);
        textBox.CaretIndex += insertSpaceCount;  // 移除选区
        textBox.EndChange();
    }
    private static void Dedent_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        var caretIndex = textBox.CaretIndex;
        var (row, column) = textBox.GetCoordinatesFromCharacterIndex(caretIndex);
        var lineText = textBox.GetLineText(row);
        var textBeforeCaret = lineText.Substring(0, column);
        var removeSpaceCount = Math.Min(textBeforeCaret.CountTrailingSpaces(), 4);
        textBox.Remove(caretIndex - removeSpaceCount, removeSpaceCount);
    }
    private static void IndentRegion_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        var startLineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.SelectionStart);
        var endLineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.SelectionEnd);

        // 在 System.Windows.Controls.TextBox 中，换行符会被归入下一行，因此还需要进行修正：
        if (textBox.SelectedText.EndsWith('\r') || textBox.SelectedText.EndsWith('\n'))
            endLineIndex--;

        // 循环插入缩进：
        textBox.BeginChange();
        for (int row = startLineIndex;  row <= endLineIndex;  row++)
        {
            var lineText = textBox.GetLineText(row);

            // 跳过空行：
            if (lineText.TrimEnd(new Char[2]{ '\r', '\n' }) == "")
                continue;

            // 插入缩进：
            var insertSpaceCount = 4 - (lineText.CountLeadingSpaces() & 3);
            textBox.CaretMoveToLineIndex(row);
            textBox.Write(new String(' ', insertSpaceCount));
        }
        textBox.EndChange();

        // 重设选区以提示生效区域并在重复当前操作时避免手动费力：
        var selectionStart = textBox.GetCharacterIndexFromLineIndex(startLineIndex);
        var endLineText = textBox.GetLineText(endLineIndex);
        var selectionLength = textBox.GetCharacterIndexFromLineIndex(endLineIndex) + endLineText.Length - selectionStart;
        textBox.Select(selectionStart, selectionLength);
    }
    private static void DedentRegion_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
    {
        var textBox = (System.Windows.Controls.TextBox) sender;
        var startLineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.SelectionStart);
        var endLineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.SelectionEnd);

        // 在 System.Windows.Controls.TextBox 中，换行符会被归入下一行，因此还需要进行修正：
        if (textBox.SelectedText.EndsWith('\r') || textBox.SelectedText.EndsWith('\n'))
            endLineIndex--;

        // 循环移除缩进：
        textBox.BeginChange();
        for (int row = startLineIndex;  row <= endLineIndex;  row++)
        {
            var lineText = textBox.GetLineText(row);
            var leadingSpaceCount = lineText.CountLeadingSpaces();
            // 跳过无缩进的行：
            if (leadingSpaceCount == 0)
                continue;
            // 移除缩进：
            var removeSpaceCount = (((leadingSpaceCount & 3) + 3) & 3) + 1;  // 0, 1, 2, 3 -> 4, 1, 2, 3
            textBox.Remove(textBox.GetCharacterIndexFromLineIndex(row), removeSpaceCount);
        }
        textBox.EndChange();
        // 重设选区以提示生效区域并在重复当前操作时避免手动费力：
        var selectionStart = textBox.GetCharacterIndexFromLineIndex(startLineIndex);
        var endLineText = textBox.GetLineText(endLineIndex);
        var selectionLength = textBox.GetCharacterIndexFromLineIndex(endLineIndex) + endLineText.Length - selectionStart;
        textBox.Select(selectionStart, selectionLength);
    }
}