using System.IO;  // File, 
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;  // OpenFileDialog, SaveFileDialog

namespace Text_Editor;

public static class EditorCommands
{
    // file menu
    public static readonly RoutedCommand Load = new RoutedCommand("Load", typeof(EditorCommands));
    public static readonly RoutedCommand Save = new RoutedCommand("Save", typeof(EditorCommands));
    // edit menu
    public static readonly RoutedCommand Find = new RoutedCommand("Find", typeof(EditorCommands));
    public static readonly RoutedCommand Replace = new RoutedCommand("Replace", typeof(EditorCommands));
    // format menu
    public static readonly RoutedCommand Upper = new RoutedCommand("Upper", typeof(EditorCommands));
    public static readonly RoutedCommand Lower = new RoutedCommand("Lower", typeof(EditorCommands));
    public static readonly RoutedCommand Capitalize = new RoutedCommand("Capitalize", typeof(EditorCommands));
    public static readonly RoutedCommand Indent = new RoutedCommand("Indent", typeof(EditorCommands));
    public static readonly RoutedCommand Dedent = new RoutedCommand("Dedent", typeof(EditorCommands));
    // Non-MenuItem
    public static readonly RoutedCommand Tab = new RoutedCommand("Tab", typeof(EditorCommands));
}

public class CustomizedTextBox : TextBox
{
    public (int, int) SelectionLineRange {
        get {
            int selectionHead = this.SelectionStart;
            int selectionTail = selectionHead + this.SelectionLength;
            int rowHead = this.GetLineIndexFromCharacterIndex(selectionHead);
            int rowTail = this.GetLineIndexFromCharacterIndex(selectionTail);
            // 修正末行：'\r\n'在`TextBox.GetLineIndexFromCharacterIndex`中被划分到下一行。
            if (
                rowTail > rowHead  // 选区 ≠ 空
             && this.GetCharacterIndexFromLineIndex(rowTail) == selectionTail  // 选区末行首字 = 选区尾字
            )
                rowTail--;
            return (rowHead, rowTail);
        }
    }
    public void ClearUndoQueue()
    {
        Int32 undo_limit = this.UndoLimit;
        this.UndoLimit = 0;
        this.UndoLimit = undo_limit;
    }
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private int CountLeadingSpaces(string text)
    {
        int count = 0;
        foreach (char character in text)
        {
            if (character == ' ')
                count++;
            else
                break;
        }
        return count;
    }
    public MainWindow()
    {
        InitializeComponent();

        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Load,
            (_, _) => {
                var dialog = new OpenFileDialog {
                    Filter = "All documents|*.*",
                };
                if (dialog.ShowDialog() == false)
                    return;
                textbox.Text = File.ReadAllText(dialog.FileName);
                textbox.ClearUndoQueue();
            }
        ));
        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Save,
            (_, _) => {
                var dialog = new SaveFileDialog {
                    Filter = "All documents|*.*",
                };
                if (dialog.ShowDialog() == false)
                    return;
                File.WriteAllText(dialog.FileName, textbox.Text);
            }
        ));
        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Upper,
            (_, _) => {
                textbox.SelectedText = textbox.SelectedText.ToUpper();
            },
            (_, evt) => evt.CanExecute = textbox.SelectionLength > 0
        ));
        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Lower,
            (_, _) => {
                textbox.SelectedText = textbox.SelectedText.ToLower();
            },
            (_, evt) => evt.CanExecute = textbox.SelectionLength > 0
        ));
        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Capitalize,
            (_, _) => {
                string selectedText = textbox.SelectedText;
                textbox.SelectedText = $"{selectedText[0].ToString().ToUpper()}{selectedText.Substring(1).ToLower()}";
            },
            (_, evt) => evt.CanExecute = textbox.SelectionLength > 0
        ));
        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Indent,
            (_, _) => {
                (int rowHead, int rowTail) = textbox.SelectionLineRange;
                textbox.BeginChange();
                try {
                    // 添加缩进
                    for (int row = rowHead; row <= rowTail; row++)
                    {
                        string text = textbox.GetLineText(row);
                        int space_count = CountLeadingSpaces(text);
                        int index_of_first_character = textbox.GetCharacterIndexFromLineIndex(row);
                        textbox.Select(index_of_first_character, 0);
                        textbox.SelectedText = new String(' ', 4 - space_count % 4);
                    }
                    // 重设选区
                    int index_of_selection_first_character = textbox.GetCharacterIndexFromLineIndex(rowHead);
                    int index_of_selection_final_character = textbox.GetCharacterIndexFromLineIndex(rowTail) + textbox.GetLineLength(rowTail);
                    textbox.Select(index_of_selection_first_character, index_of_selection_final_character - index_of_selection_first_character);
                }
                finally {
                    textbox.EndChange();
                }
            }
        ));
        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Dedent,
            (_, _) => {
                (int rowHead, int rowTail) = textbox.SelectionLineRange;
                textbox.BeginChange();
                try {
                    // 移除缩进
                    for (int row = rowHead; row <= rowTail; row++)
                    {
                        string text = textbox.GetLineText(row);
                        int space_count = CountLeadingSpaces(text);
                        int index_of_first_character = textbox.GetCharacterIndexFromLineIndex(row);
                        textbox.Select(index_of_first_character, Math.Min(space_count, 4));
                        textbox.SelectedText = "";
                    }
                    // 重设选区
                    int index_of_selection_first_character = textbox.GetCharacterIndexFromLineIndex(rowHead);
                    int index_of_selection_final_character = textbox.GetCharacterIndexFromLineIndex(rowTail) + textbox.GetLineLength(rowTail);
                    textbox.Select(index_of_selection_first_character, index_of_selection_final_character - index_of_selection_first_character);
                }
                finally {
                    textbox.EndChange();
                }
            }
        ));
        textbox.CommandBindings.Add(new CommandBinding(
            EditorCommands.Tab,
            (_, _) => {
                int row = textbox.GetLineIndexFromCharacterIndex(textbox.CaretIndex);
                string text = textbox.GetLineText(row);
                int space_count = CountLeadingSpaces(text);
                int added_count = 4 - space_count % 4;
                textbox.SelectedText = new String(' ', added_count);
                textbox.CaretIndex += added_count;  // 取消选区
            }
        ));

        textbox.SelectionChanged += (_, _) => {
            int caret_index = textbox.CaretIndex;
            int row = textbox.GetLineIndexFromCharacterIndex(caret_index);
            int column = caret_index - textbox.GetCharacterIndexFromLineIndex(row);
            position.Content = $"{row + 1}, {column + 1}";
            selected.Content = textbox.SelectionLength;
        };

        // 支持通过命令行加载文件
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            string file_path = args[1];
            if (File.Exists(file_path))
                textbox.Text = File.ReadAllText(file_path);
            else
                MessageBox.Show($"File not found: {file_path}");
        }

        textbox.Focus();
    }
}