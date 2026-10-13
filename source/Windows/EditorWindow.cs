using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TextEditor
{
    public class EditorWindow : Window
    {
        public EditorWindow()
        {
            Title = "Text Editor";  Width = 600D;  Height = 650D;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Content = BuildGridLayout();
        }
        static EditorWindow()
        {
            CommandManager.RegisterClassCommandBinding(typeof(EditorWindow), new CommandBinding(Commands.Open, Open_Execute));
            CommandManager.RegisterClassCommandBinding(typeof(TabControl), new CommandBinding(Commands.Save, Save_Execute, Save_CanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(TabControl), new CommandBinding(Commands.Close, Close_Execute, Close_CanExecute));
        }
        private TabControl TabCtrl = null!;
        private Grid BuildGridLayout()
        {
            var gridLayout = new Grid();

            var row_1 = new RowDefinition{ Height = GridLength.Auto };
            var row_2 = new RowDefinition{ Height = new GridLength( 1D, GridUnitType.Star ) };

            gridLayout.RowDefinitions.Add( row_1 );
            gridLayout.RowDefinitions.Add( row_2 );

            var tabCtrl = new TabControl();  this.TabCtrl = tabCtrl;
            var topMenu = BuildMainMenu();

            Grid.SetRow(topMenu, 0);
            Grid.SetColumn(topMenu, 0);
            Grid.SetRow(tabCtrl, 1);
            Grid.SetColumn(tabCtrl, 0);

            gridLayout.Children.Add(topMenu);
            gridLayout.Children.Add(tabCtrl);

            return gridLayout;
        }
        private Menu BuildMainMenu()
        {
            var topMenu = new Menu();

            topMenu.Items.Add(BuildFileMenu());
            topMenu.Items.Add(BuildEditMenu());

            return topMenu;
        }
        private MenuItem BuildFileMenu()
        {
            var fileMenu = new MenuItem{ Header = "File" };

            var openMenuItem = new MenuItem{ Header = "Open",  Command = Commands.Open,  CommandTarget = this.TabCtrl };
            fileMenu.Items.Add(openMenuItem);
            fileMenu.Items.Add(new Separator());

            var saveMenuItem = new MenuItem{ Header = "Save",  Command = Commands.Save,  CommandTarget = this.TabCtrl };
            fileMenu.Items.Add(saveMenuItem);
            fileMenu.Items.Add(new Separator());

            var closeMenuItem = new MenuItem{ Header = "Close",  Command = Commands.Close,  CommandTarget = this.TabCtrl };
            fileMenu.Items.Add(closeMenuItem);

            return fileMenu;
        }
        private MenuItem BuildEditMenu()
        {
            var editMenu = new MenuItem{ Header = "Edit" };

            var undoMenuItem = new MenuItem{ Header = "Undo",  Command = ApplicationCommands.Undo };
            var redoMenuItem = new MenuItem{ Header = "Redo" };

            editMenu.Items.Add(undoMenuItem);
            editMenu.Items.Add(redoMenuItem);
            editMenu.Items.Add(new Separator());

            var cutMenuItem = new MenuItem{ Header = "Cut" };
            var copyMenuItem = new MenuItem{ Header = "Copy" };
            var pasteMenuItem = new MenuItem{ Header = "Paste" };

            editMenu.Items.Add(cutMenuItem);
            editMenu.Items.Add(copyMenuItem);
            editMenu.Items.Add(pasteMenuItem);
            editMenu.Items.Add(new Separator());

            var findMenuItem = new MenuItem{ Header = "Find" };
            var replaceMenuItem = new MenuItem{ Header = "Replace" };

            editMenu.Items.Add(findMenuItem);
            editMenu.Items.Add(replaceMenuItem);

            return editMenu;
        }
        private static void Open_Execute(object sender, ExecutedRoutedEventArgs evt)
        {
            Console.WriteLine($"EditorWindow.Open_Execute{{\n\t发送者：{sender}\n\t源头：{evt.Source}\n\t原始源头：{evt.OriginalSource}\n}}");
            if (sender is EditorWindow editorWindow) {
                var tabCtrl = editorWindow.TabCtrl;
                var dialog = new Microsoft.Win32.OpenFileDialog{
                    Filter = "All Files|*.*",
                };
                if (dialog.ShowDialog() == false) return;
                foreach (TabItem existing in tabCtrl.Items) {
                    if (existing.FilePath == dialog.FileName) {
                        tabCtrl.SelectedItem = existing;
                        return;
                    }
                }
                var tab = new TextEditor.TabItem(dialog.FileName);
                tabCtrl.Items.Add(tab);
                tabCtrl.SelectedItem = tab;
            }
        }
        private static void Save_Execute(object sender, ExecutedRoutedEventArgs evt)
        {
            Console.WriteLine($"EditorWindow.Save_Execute{{\n\t发送者：{sender}\n\t源头：{evt.Source}\n\t原始源头：{evt.OriginalSource}\n}}");
            if (evt.OriginalSource is TabControl tabCtrl)
                ((TabItem) tabCtrl.SelectedItem).Save();
        }
        private static void Save_CanExecute(object sender, CanExecuteRoutedEventArgs evt)
        {
            if (evt.OriginalSource is TabControl tabCtrl) {
                TabItem? selectedItem = tabCtrl.SelectedItem as TabItem;
                evt.CanExecute = selectedItem != null && selectedItem.IsDirty;
            }
        }
        private static void Close_Execute(object sender, ExecutedRoutedEventArgs evt)
        {
            Console.WriteLine($"EditorWindow.Close_Execute{{\n\t发送者：{sender}\n\t源头：{evt.Source}\n\t原始源头：{evt.OriginalSource}\n}}");
            if (evt.OriginalSource is TabControl tabCtrl)
                tabCtrl.Items.Remove(tabCtrl.SelectedItem);
        }
        private static void Close_CanExecute(object sender, CanExecuteRoutedEventArgs evt)
        {
            if (evt.OriginalSource is TabControl tabCtrl && tabCtrl.SelectedItem != null)
                evt.CanExecute = true;
        }
    }
}