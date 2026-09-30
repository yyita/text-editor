using System.Windows.Forms;
using System.Drawing;

namespace TextEditor;

public class MyForm : Form
{
    public MyForm()
    {
        InitComponents();
    }

    private void InitComponents()
    {
        Text = "Text Edtior";
        ClientSize = new Size(600, 650);
        CenterToScreen();

        MenuStrip mainMenu = new MenuStrip();

        ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
        ToolStripMenuItem readItem = new ToolStripMenuItem("Read");
        readItem.ShortcutKeys = Keys.Control | Keys.O;
        ToolStripMenuItem writeItem = new ToolStripMenuItem("Write");
        writeItem.ShortcutKeys = Keys.Control | Keys.S;

        fileMenu.DropDownItems.Add(readItem);
        fileMenu.DropDownItems.Add(writeItem);

        ToolStripMenuItem editMenu = new ToolStripMenuItem("Edit");
        ToolStripMenuItem cutItem = new ToolStripMenuItem("Cut");
        cutItem.ShortcutKeys = Keys.Control | Keys.X;
        ToolStripMenuItem copyItem = new ToolStripMenuItem("Copy");
        copyItem.ShortcutKeys = Keys.Control | Keys.C;
        ToolStripMenuItem pasteItem = new ToolStripMenuItem("Paste");
        pasteItem.ShortcutKeys = Keys.Control | Keys.V;
        ToolStripMenuItem undoItem = new ToolStripMenuItem("Undo");
        undoItem.ShortcutKeys = Keys.Control | Keys.Z;
        ToolStripMenuItem redoItem = new ToolStripMenuItem("Redo");
        redoItem.ShortcutKeys = Keys.Control | Keys.Y;
        ToolStripMenuItem findItem = new ToolStripMenuItem("Find");
        findItem.ShortcutKeys = Keys.Control | Keys.F;
        ToolStripMenuItem replaceItem = new ToolStripMenuItem("Replace");
        replaceItem.ShortcutKeys = Keys.Control | Keys.R;

        editMenu.DropDownItems.Add(cutItem);
        editMenu.DropDownItems.Add(copyItem);
        editMenu.DropDownItems.Add(pasteItem);
        editMenu.DropDownItems.Add(new ToolStripSeparator());
        editMenu.DropDownItems.Add(undoItem);
        editMenu.DropDownItems.Add(redoItem);
        editMenu.DropDownItems.Add(new ToolStripSeparator());
        editMenu.DropDownItems.Add(findItem);
        editMenu.DropDownItems.Add(replaceItem);

        ToolStripMenuItem formatMenu = new ToolStripMenuItem("Format");
        ToolStripMenuItem upperItem = new ToolStripMenuItem("Upper");
        upperItem.ShortcutKeys = Keys.Alt | Keys.U;
        ToolStripMenuItem lowerItem = new ToolStripMenuItem("Lower");
        lowerItem.ShortcutKeys = Keys.Alt | Keys.L;
        ToolStripMenuItem capitalizeItem = new ToolStripMenuItem("Capitalize");
        capitalizeItem.ShortcutKeys = Keys.Alt | Keys.C;
        ToolStripMenuItem indentItem = new ToolStripMenuItem("Indent");
        indentItem.ShortcutKeys = Keys.Alt | Keys.OemOpenBrackets;
        indentItem.ShortcutKeyDisplayString = "Alt+]";
        ToolStripMenuItem dedentItem = new ToolStripMenuItem("Dedent");
        dedentItem.ShortcutKeys = Keys.Alt | Keys.OemCloseBrackets;
        dedentItem.ShortcutKeyDisplayString = "Alt+[";

        formatMenu.DropDownItems.Add(upperItem);
        formatMenu.DropDownItems.Add(lowerItem);
        formatMenu.DropDownItems.Add(capitalizeItem);
        formatMenu.DropDownItems.Add(new ToolStripSeparator());
        formatMenu.DropDownItems.Add(indentItem);
        formatMenu.DropDownItems.Add(dedentItem);

        mainMenu.Items.Add(fileMenu);
        mainMenu.Items.Add(editMenu);
        mainMenu.Items.Add(formatMenu);

        MainMenuStrip = mainMenu;

        RichTextBox textbox = new RichTextBox()
        {
            Multiline = true,
            ScrollBars = RichTextBoxScrollBars.Both,
            WordWrap = false,
            Dock = DockStyle.Fill
        };

        readItem.Click += (sender, eventArgs) =>
        {
            using var dialog = new OpenFileDialog
            {
            };
        
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                textbox.Text = File.ReadAllText(dialog.FileName);
            }
        };
        writeItem.Click += (sender, eventArgs) =>
        {
            using var dialog = new SaveFileDialog
            {
                AddExtension = true,
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, textbox.Text);
            }
        };

        cutItem.Click += (s, e) => textbox.Cut();
        copyItem.Click += (s, e) => textbox.Copy();
        pasteItem.Click += (s, e) => textbox.Paste();
        undoItem.Click += (s, e) => textbox.Undo();
        redoItem.Click += (s, e) => textbox.Redo();

        textbox.TextChanged += (s, e) =>
        {
            undoItem.Enabled = textbox.CanUndo;
            redoItem.Enabled = textbox.CanRedo;
        };
        pasteItem.Enabled = Clipboard.ContainsText();

        Controls.Add(textbox);
        Controls.Add(MainMenuStrip);
    }

    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.Run(new MyForm());
    }
}
