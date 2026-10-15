namespace TextEditor;

public static class TextBoxExtensions
{
    extension(System.Windows.Controls.TextBox textBox)
    {
        public int SelectionEnd { get => textBox.SelectionStart + textBox.SelectionLength; }
    }
    static TextBoxExtensions(
    ){
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(System.Windows.Controls.TextBox), new System.Windows.Input.CommandBinding(TextEditor.Commands.ToUpper, ToUpper_Execute, HasSelection_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(System.Windows.Controls.TextBox), new System.Windows.Input.CommandBinding(TextEditor.Commands.ToLower, ToLower_Execute, HasSelection_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(System.Windows.Controls.TextBox), new System.Windows.Input.CommandBinding(TextEditor.Commands.Indent,  Indent_Execute,  Indent_CanExecute));
        System.Windows.Input.CommandManager.RegisterClassCommandBinding(typeof(System.Windows.Controls.TextBox), new System.Windows.Input.CommandBinding(TextEditor.Commands.Dedent,  Dedent_Execute,  Dedent_CanExecute));
    }
    public static void Remove(
        this System.Windows.Controls.TextBox textBox,
        Int32 startIndex,
        Int32 length
    ){
        textBox.Select(startIndex, length);
        textBox.SelectedText = "";
    }
    public static void CaretMoveToDocumentEnd(
        this System.Windows.Controls.TextBox textBox
    ){
        textBox.Select(textBox.Text.Length, 0);
    }
    private static void ToUpper_Execute(
        object sender,
        System.Windows.Input.ExecutedRoutedEventArgs evt
    ){
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.SelectedText = textBox.SelectedText.ToUpper();
    }
    private static void ToLower_Execute(
        object sender,
        System.Windows.Input.ExecutedRoutedEventArgs evt
    ){
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.SelectedText = textBox.SelectedText.ToLower();
    }
    private static void HasSelection_CanExecute(
        object sender,
        System.Windows.Input.CanExecuteRoutedEventArgs evt
    ){
        Console.WriteLine($"[INFO] TextBox.HasSelection_CanExecute() {{\n\tsender: {sender}\n\tSource: {evt.Source}\n\tOriginalSource: {evt.OriginalSource}\n}}");
        var textBox = (System.Windows.Controls.TextBox) sender;
        if (textBox.SelectionLength > 0)
            evt.CanExecute = true;
    }
    private static void Indent_Execute(
        object sender,
        System.Windows.Input.ExecutedRoutedEventArgs evt
    ){
        var textBox = (System.Windows.Controls.TextBox) sender;
        textBox.SelectedText = "    ";
        textBox.CaretIndex += 4;  // 移除选区
    }
    private static void Indent_CanExecute(
        object sender,
        System.Windows.Input.CanExecuteRoutedEventArgs evt
    ){
        if (sender is System.Windows.Controls.TextBox)
            evt.CanExecute = true;
    }
    private static void Dedent_Execute(
        object sender,
        System.Windows.Input.ExecutedRoutedEventArgs evt
    ){
        var textBox = (System.Windows.Controls.TextBox) sender;
        var caretIndexInAllText = textBox.CaretIndex;
        var caretLineIndexInAllText = textBox.GetLineIndexFromCharacterIndex(caretIndexInAllText);
        var caretLineStartInAllText = textBox.GetCharacterIndexFromLineIndex(caretLineIndexInAllText);
        var caretIndexInLine = caretIndexInAllText - caretLineStartInAllText;
        var lineText = textBox.GetLineText(caretLineIndexInAllText);
        var textBeforeCaret = lineText.Substring(0, caretIndexInLine);
        var removeSpaceCount = Math.Min(textBeforeCaret.CountTrailingSpaces(), 4);
        textBox.Remove(caretIndexInAllText - removeSpaceCount, removeSpaceCount);
    }
    private static void Dedent_CanExecute(
        object sender,
        System.Windows.Input.CanExecuteRoutedEventArgs evt
    ){
        Console.WriteLine($"[INFO] TextBox.Dedent_CanExecute() {{\n\tsender: {sender}\n\tSource: {evt.Source}\n\tOriginalSource: {evt.OriginalSource}\n}}");
        if (sender is System.Windows.Controls.TextBox)
            evt.CanExecute = true;
    }
}