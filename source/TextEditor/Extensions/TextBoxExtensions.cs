namespace TextEditor;

public static class TextBoxExtensions
{
    extension(System.Windows.Controls.TextBox textBox)
    {
        public int SelectionEnd { get => textBox.SelectionStart + textBox.SelectionLength; }
    }
    public static void Write(this System.Windows.Controls.TextBox textBox, string content)
    {
        textBox.SelectedText = content;
    }
    public static void Remove(this System.Windows.Controls.TextBox textBox, Int32 startIndex, Int32 length)
    {
        textBox.BeginChange();
        textBox.Select(startIndex, length);
        textBox.SelectedText = "";
        textBox.EndChange();
    }
    public static (int, int) GetCoordinatesFromCharacterIndex(this System.Windows.Controls.TextBox textBox, int characterIndex)
    {
        var row = textBox.GetLineIndexFromCharacterIndex(characterIndex);
        var rowStart = textBox.GetCharacterIndexFromLineIndex(row);
        return (row, characterIndex - rowStart);
    }
    public static void CaretMoveToDocumentEnd(this System.Windows.Controls.TextBox textBox)
    {
        textBox.Select(textBox.Text.Length, 0);
    }
    public static void CaretMoveToCharacterIndex(this System.Windows.Controls.TextBox textBox, int characterIndex)
    {
        textBox.Select(characterIndex, 0);
    }
    public static void CaretMoveToLineIndex(this System.Windows.Controls.TextBox textBox, int lineIndex)
    {
        textBox.Select(textBox.GetCharacterIndexFromLineIndex(lineIndex), 0);
    }
}