namespace TextEditor;

public class MainMenu : System.Windows.Controls.Menu
{
    public MainMenu(TextEditor.TabControl tabControl)
    {
        this.Items.Add(this.FileMenu_Build(tabControl));
        this.Items.Add(this.EditMenu_Build(tabControl));
        this.Items.Add(this.FormatMenu_Build());
    }
    private System.Windows.Controls.MenuItem FileMenu_Build( TextEditor.TabControl tabControl )
    {
        var menu = new System.Windows.Controls.MenuItem{ Header = "文件" };
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FileCommands.Open,  CommandTarget = tabControl });
        menu.Items.Add(new System.Windows.Controls.Separator());
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FileCommands.Difference });
        menu.Items.Add(new System.Windows.Controls.Separator());
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FileCommands.Save,  CommandTarget = tabControl });
        menu.Items.Add(new System.Windows.Controls.Separator());
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FileCommands.Close, CommandTarget = tabControl });
        return menu;
    }
    private System.Windows.Controls.MenuItem EditMenu_Build( TextEditor.TabControl tabControl )
    {
        var menu = new System.Windows.Controls.MenuItem{ Header = "编辑" };
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Undo });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Redo });
        menu.Items.Add(new System.Windows.Controls.Separator());
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Cut });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Copy });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Paste });
        menu.Items.Add(new System.Windows.Controls.Separator());
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Find,    CommandTarget = tabControl });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Replace, CommandTarget = tabControl });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.EditCommands.Modify,  CommandTarget = tabControl });
        return menu;
    }
    private System.Windows.Controls.MenuItem FormatMenu_Build()
    {
        var menu = new System.Windows.Controls.MenuItem{ Header = "格式" };
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Upper });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Lower });
        menu.Items.Add(new System.Windows.Controls.Separator());
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Binary });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Octal });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Decimal });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Hexadecimal });
        menu.Items.Add(new System.Windows.Controls.Separator());
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Indent });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.Dedent });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.IndentRegion, InputGestureText = "Ctrl+]" });
        menu.Items.Add(new System.Windows.Controls.MenuItem{ Command = TextEditor.FormatCommands.DedentRegion, InputGestureText = "Ctrl+[" });
        return menu;
    }
}