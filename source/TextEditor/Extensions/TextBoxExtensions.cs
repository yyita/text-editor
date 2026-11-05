namespace TextEditor;

public static class TextBoxExtensions
{
    public static Int32 CountColumn(this System.Windows.Controls.TextBox textBox, Int32 row)
        => textBox.GetLineText(row).TrimEnd(Environment.NewLine).Length + 1;
    public static Int32 CalculateCharacterIndexFromCoordinates(this System.Windows.Controls.TextBox textBox, Int32 row, Int32 column)
        => textBox.GetCharacterIndexFromLineIndex(row) + column;
    public static void See(this System.Windows.Controls.TextBox textBox, Int32 row, Int32 column)
    {
        textBox.Focus();
        textBox.Select(textBox.CalculateCharacterIndexFromCoordinates(row, column), 0);
    }
}
