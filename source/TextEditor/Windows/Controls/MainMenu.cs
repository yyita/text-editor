namespace TextEditor;

public class MainMenu : System.Windows.Controls.Menu
{
    public MainMenu(TextEditor.TabControl tabControl)
    {
        var fileMenu = new System.Windows.Controls.MenuItem{ Header = "File" };
        fileMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.Open,  CommandTarget = tabControl });
        fileMenu.Items.Add(new System.Windows.Controls.Separator());
        fileMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.Save,  CommandTarget = tabControl });
        fileMenu.Items.Add(new System.Windows.Controls.Separator());
        fileMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.Close, CommandTarget = tabControl });
        this.Items.Add(fileMenu);

        var editMenu = new System.Windows.Controls.MenuItem{ Header = "Edit" };
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = System.Windows.Input.ApplicationCommands.Undo });
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = System.Windows.Input.ApplicationCommands.Redo });
        editMenu.Items.Add(new System.Windows.Controls.Separator());
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = System.Windows.Input.ApplicationCommands.Cut });
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = System.Windows.Input.ApplicationCommands.Copy });
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = System.Windows.Input.ApplicationCommands.Paste });
        editMenu.Items.Add(new System.Windows.Controls.Separator());
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.ToFind,    CommandTarget = tabControl });
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.ToReplace, CommandTarget = tabControl });
        editMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.ToModify,  CommandTarget = tabControl });
        this.Items.Add(editMenu);

        var formatMenu = new System.Windows.Controls.MenuItem{ Header = "Format" };
        formatMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.ToUpper });
        formatMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.ToLower });
        formatMenu.Items.Add(new System.Windows.Controls.Separator());
        formatMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.Indent });
        formatMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.Dedent });
        formatMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.IndentRegion, InputGestureText = "Ctrl+]" });
        formatMenu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.Commands.DedentRegion, InputGestureText = "Ctrl+[" });
        this.Items.Add(formatMenu);
    }
}