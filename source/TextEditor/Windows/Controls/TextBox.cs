namespace TextEditor.Controls;

public class TextBox : System.Windows.Controls.TextBox
{
 // 构造函数
    public TextBox()
    {
        this.IsInactiveSelectionHighlightEnabled = true;
        this.Style = this.BuildStyle();
        this.BindingCommands();
    }
 // 数据槽
    public Int32 IndentWidth = 4;
 // 访问器
    public Int32 SelectionEnd => this.SelectionStart + this.SelectionLength;
 // 类级函数
    private static void Indent_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
        => ((TextEditor.Controls.TextBox) sender).Indent();
    private static void Dedent_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
        => ((TextEditor.Controls.TextBox) sender).Dedent();
    private static void IndentRegion_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
        => ((TextEditor.Controls.TextBox) sender).IndentRegion(TextEditor.Direction.Forward);
    private static void DedentRegion_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs evt)
        => ((TextEditor.Controls.TextBox) sender).IndentRegion(TextEditor.Direction.Backward);
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
 // 实例函数
    protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs evt)
    {
        // 按键 Tab 自带内置行为，并且会令绑定了它的命令的快捷键失效，
        // 即使对 evt.Key 等于 Key.Tab 的 PreviewKeyDown 事件进行阻塞。
        // 因此，此处为绑定的命令进行了补发，硬编码的触发条件仅用作过渡：
        if (evt.Key == System.Windows.Input.Key.Tab)
        {
            evt.Handled = true;
            var textBox = (System.Windows.Controls.TextBox) evt.Source;
            if ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) > 0
             && TextEditor.FormatCommands.Dedent.CanExecute(null, textBox))
                TextEditor.FormatCommands.Dedent.Execute(null, textBox);
            else if (TextEditor.FormatCommands.Indent.CanExecute(null, textBox))
                TextEditor.FormatCommands.Indent.Execute(null, textBox);
        }
        base.OnPreviewKeyDown(evt);
    }
    private System.Windows.Style BuildStyle()
    {
        var style = new System.Windows.Style(typeof(TextEditor.Controls.TextBox));

        var noKeyboardFocusTrigger = new System.Windows.Trigger{Property=TextEditor.Controls.TextBox.IsKeyboardFocusedProperty, Value=false};
        noKeyboardFocusTrigger.Setters.Add(new System.Windows.Setter{Property=SelectionBrushProperty, Value=System.Windows.Media.Brushes.Gray});
        style.Triggers.Add(noKeyboardFocusTrigger);

        return style;
    }
    private void BindingCommands()
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
    private Int32 CalculateInsertSpaceCount(Int32 currentWidth)
        => this.IndentWidth - (currentWidth % this.IndentWidth);
    private Int32 CalculateRemoveSpaceCount(Int32 currentWidth)
        => (((currentWidth % this.IndentWidth) + this.IndentWidth - 1) % this.IndentWidth) + 1;
    public void Write(String content)
        => this.SelectedText = content;
    public void Remove(Int32 startIndex, Int32 length)
    {
        this.BeginChange();
        this.Select(startIndex, length);
        this.SelectedText = "";
        this.EndChange();
    }
    public (Int32, Int32) GetCoordinatesFromCharacterIndex(Int32 characterIndex)
    {
        var row = this.GetLineIndexFromCharacterIndex(characterIndex);
        var rowStart = this.GetCharacterIndexFromLineIndex(row);
        return (row, characterIndex - rowStart);
    }
    public (Int32, Int32) GetSelectionLineRange()
    {
        var start = this.GetLineIndexFromCharacterIndex(this.SelectionStart);
        var end = this.GetLineIndexFromCharacterIndex(this.SelectionEnd);

        // 换行符会被归入下一行，因此还需要进行修正：
        if (this.SelectedText.EndsWith('\r') || this.SelectedText.EndsWith('\n'))
            end--;

        return (start, end);
    }
    public void CaretMoveToDocumentEnd()
        => this.Select(this.Text.Length, 0);
    public void CaretMoveToCharacterIndex(Int32 characterIndex)
        => this.Select(characterIndex, 0);
    public void CaretMoveToLineIndex(Int32 lineIndex)
        => this.Select(this.GetCharacterIndexFromLineIndex(lineIndex), 0);
    public void Indent()
    {
        var (row, column) = this.GetCoordinatesFromCharacterIndex(this.CaretIndex);
        var lineText = this.GetLineText(row);
        var insertSpaceCount = this.CalculateInsertSpaceCount(lineText.CountAdjoiningSpaces(column));
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
        var removeSpaceCount = this.CalculateRemoveSpaceCount(trailingSpaceCount);
        this.Remove(caretIndex - removeSpaceCount, removeSpaceCount);
    }
    public void IndentRegion(TextEditor.Direction direction)
    {
        var (selectionFirstLine, selectionFinalLine) = this.GetSelectionLineRange();
        var lineCount = selectionFinalLine - selectionFirstLine + 1;
        var lines = new List<String>(lineCount);
        if (direction == TextEditor.Direction.Forward)
        {
            bool? hasNonEmptyLine = null;
            for (Int32 row = selectionFirstLine;  row <= selectionFinalLine;  row++)
            {
                var lineText = this.GetLineText(row);
                lines.Add(lineText);
                if (hasNonEmptyLine is null && lineText != Environment.NewLine)
                    hasNonEmptyLine = true;
            }
            var indent = new String(' ', this.IndentWidth);
            for (Int32 index = 0;  index < lineCount;  index++)
            {
                var lineText = lines[index];
                if (hasNonEmptyLine == true)
                {
                    if (lineText == Environment.NewLine)
                        continue;
                    var insertSpaceCount = this.CalculateInsertSpaceCount(lineText.CountLeadingSpaces());
                    lines[index] = lineText.Insert(0, new String(' ', insertSpaceCount));
                }
                else lines[index] = lineText.Insert(0, indent);
            }
        }
        else
        {
            for (Int32 row = selectionFirstLine;  row <= selectionFinalLine;  row++)
            {
                var lineText = this.GetLineText(row);
                var leadingSpaceCount = lineText.CountLeadingSpaces();
                if (leadingSpaceCount != 0)
                    lineText = lineText.Remove(0, this.CalculateRemoveSpaceCount(leadingSpaceCount));
                lines.Add(lineText);
            }
        }
        var selectionStart = this.GetCharacterIndexFromLineIndex(selectionFirstLine);
        var selectionEnd = this.GetCharacterIndexFromLineIndex(selectionFinalLine);
        var endLineLength = this.GetLineText(selectionFinalLine).Length;
        this.BeginChange();
        this.Select(selectionStart,  selectionEnd + endLineLength - selectionStart);
        this.SelectedText = String.Concat(lines);
        this.EndChange();
    }
}
