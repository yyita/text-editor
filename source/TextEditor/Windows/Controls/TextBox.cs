namespace TextEditor.Controls;

public class TextBox : System.Windows.Controls.TextBox
{
    public TextBox()
    {
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
    public int SelectionEnd { get =>  this.SelectionStart + this.SelectionLength; }
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
    public void Write(string content)
    {
        this.SelectedText = content;
    }
    public void Remove(int startIndex, int length)
    {
        this.BeginChange();
        this.Select(startIndex, length);
        this.SelectedText = "";
        this.EndChange();
    }
    public (int, int) GetCoordinatesFromCharacterIndex(int characterIndex)
    {
        var row = this.GetLineIndexFromCharacterIndex(characterIndex);
        var rowStart = this.GetCharacterIndexFromLineIndex(row);
        return (row, characterIndex - rowStart);
    }
    public (int, int) GetSelectionLineRange()
    {
        var start = this.GetLineIndexFromCharacterIndex(this.SelectionStart);
        var end = this.GetLineIndexFromCharacterIndex(this.SelectionEnd);

        // 换行符会被归入下一行，因此还需要进行修正：
        if (this.SelectedText.EndsWith('\r') || this.SelectedText.EndsWith('\n'))
            end--;

        return (start, end);
    }
    public void CaretMoveToDocumentEnd()
    {
        this.Select(this.Text.Length, 0);
    }
    public void CaretMoveToCharacterIndex(int characterIndex)
    {
        this.Select(characterIndex, 0);
    }
    public void CaretMoveToLineIndex(int lineIndex)
    {
        this.Select(this.GetCharacterIndexFromLineIndex(lineIndex), 0);
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
    public void Indent()
    {
        var (row, column) = this.GetCoordinatesFromCharacterIndex(this.CaretIndex);
        var lineText = this.GetLineText(row);
        var insertSpaceCount = 4 - (lineText.CountAdjoiningSpaces(column) & 3);  // 0, 1, 2, 3 -> 4, 3, 2, 1
        this.BeginChange();
        this.SelectedText = new String(' ', insertSpaceCount);
        this.CaretIndex += insertSpaceCount;  // 移除选区
        this.EndChange();
    }
    public void Dedent()
    {
        var caretIndex = this.CaretIndex;
        var (row, column) = this.GetCoordinatesFromCharacterIndex(caretIndex);
        var lineText = this.GetLineText(row);
        var textBeforeCaret = lineText.Substring(0, column);
        var trailingSpaceCount = textBeforeCaret.CountTrailingSpaces();
        if (trailingSpaceCount == 0)
            return;
        var removeSpaceCount = (((trailingSpaceCount & 3) + 3) & 3) + 1;  // 0, 1, 2, 3 -> 4, 1, 2, 3
        this.Remove(caretIndex - removeSpaceCount, removeSpaceCount);
    }
    public void IndentRegion()
    // 为选区内所有行首添加缩进。
    // 对于空行，仅当选区内仅有空行时才生效。
    {
        var (selectionFirstLine, selectionFinalLine) = this.GetSelectionLineRange();
        bool isSelectionAllEmptyLines = true;  // 默认值，后面改。

        this.BeginChange();
        for (int row = selectionFirstLine;  row <= selectionFinalLine;  row++)
        {
            var lineText = this.GetLineText(row);

            // 跳过空行：
            if (lineText.TrimEnd(new Char[2]{ '\r', '\n' }) == "")
                continue;

            // 插入缩进：
            var insertSpaceCount = 4 - (lineText.CountLeadingSpaces() & 3);
            this.CaretMoveToLineIndex(row);
            this.Write(new String(' ', insertSpaceCount));

            if (isSelectionAllEmptyLines)
            {
                isSelectionAllEmptyLines = false;
            }
        }
        if (isSelectionAllEmptyLines)
        {
            for (int row = selectionFirstLine;  row <= selectionFinalLine;  row++)
            {
                this.CaretMoveToLineIndex(row);
                this.Write(new String(' ', 4));
            }
        }
        this.EndChange();

        // 重设选区以提示生效区域并在重复当前操作时避免手动费力：
        var selectionStart = this.GetCharacterIndexFromLineIndex(selectionFirstLine);
        var endLineText = this.GetLineText(selectionFinalLine);
        var selectionLength = this.GetCharacterIndexFromLineIndex(selectionFinalLine) + endLineText.Length - selectionStart;
        this.Select(selectionStart, selectionLength);
    }
    public void DedentRegion()
    {
        var startLineIndex = this.GetLineIndexFromCharacterIndex(this.SelectionStart);
        var endLineIndex = this.GetLineIndexFromCharacterIndex(this.SelectionEnd);

        // 换行符会被归入下一行，因此还需要进行修正：
        if (this.SelectedText.EndsWith('\r') || this.SelectedText.EndsWith('\n'))
            endLineIndex--;

        // 循环移除缩进：
        this.BeginChange();
        for (int row = startLineIndex;  row <= endLineIndex;  row++)
        {
            var lineText = this.GetLineText(row);
            var leadingSpaceCount = lineText.CountLeadingSpaces();
            // 跳过无缩进的行：
            if (leadingSpaceCount == 0)
                continue;
            // 移除缩进：
            var removeSpaceCount = (((leadingSpaceCount & 3) + 3) & 3) + 1;  // 0, 1, 2, 3 -> 4, 1, 2, 3
            this.Remove(this.GetCharacterIndexFromLineIndex(row), removeSpaceCount);
        }
        this.EndChange();
        // 重设选区以提示生效区域并在重复当前操作时避免手动费力：
        var selectionStart = this.GetCharacterIndexFromLineIndex(startLineIndex);
        var endLineText = this.GetLineText(endLineIndex);
        var selectionLength = this.GetCharacterIndexFromLineIndex(endLineIndex) + endLineText.Length - selectionStart;
        this.Select(selectionStart, selectionLength);
    }
    private static void Indent_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt) { ( (TextEditor.Controls.TextBox)sender ).Indent(); }
    private static void Dedent_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt) { ( (TextEditor.Controls.TextBox)sender ).Dedent(); }
    private static void IndentRegion_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt) { ( (TextEditor.Controls.TextBox)sender ).IndentRegion(); }
    private static void DedentRegion_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt) { ( (TextEditor.Controls.TextBox)sender ).DedentRegion(); }
}
