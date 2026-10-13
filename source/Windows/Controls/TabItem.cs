using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TextEditor
{
    public class TabItem : System.Windows.Controls.TabItem
    {
        private string RawData = null!;
        public bool IsDirty {
            get => this.RawData != this.Text;
        }
        private TextBox PathBox = null!;
        public string FilePath {
            get => this.PathBox.Text;
            private set {
                this.Header = Path.GetFileName(value);
                this.PathBox.Text = value;
            }
        }
        private TextBox TextBox = null!;
        public string Text {
            get => this.TextBox.Text;
            set => this.TextBox.Text = value;
        }
        public TabItem(string filePath)
        {
            Content = BuildGridLayout();
            this.FilePath = filePath;
            this.TextBox.Text = File.ReadAllText(filePath, Encoding.UTF8);
            this.RawData = this.Text;

            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs evt)
        {
            this.TextBox.Focus();
            this.TextBox.Select(this.TextBox.Text.Length, 0);
            this.TextBox.TextChanged += (object sender, TextChangedEventArgs evt) =>
            {
                var header = (string) this.Header;
                if (this.IsDirty) {
                    if (!header.StartsWith("*"))
                        this.Header = $"*{header}";
                }
                else if (header.StartsWith("*"))
                    this.Header = header.Substring(1);
            };
        }
        private Grid BuildGridLayout()
        {
            var gridLayout = new Grid();

            var row_1 = new RowDefinition{ Height = GridLength.Auto };
            var row_2 = new RowDefinition{ Height = new GridLength( 1D, GridUnitType.Star ) };

            gridLayout.RowDefinitions.Add( row_1 );
            gridLayout.RowDefinitions.Add( row_2 );

            var pathBox = BuildPathBox();
            var textBox = BuildTextBox();

            this.PathBox = pathBox;
            this.TextBox = textBox;

            Grid.SetRow(pathBox, 0);
            Grid.SetColumn(pathBox, 0);
            Grid.SetRow(textBox, 1);
            Grid.SetColumn(textBox, 0);

            gridLayout.Children.Add(pathBox);
            gridLayout.Children.Add(textBox);

            return gridLayout;
        }
        private TextBox BuildPathBox()
        {
            var pathBox = new TextBox{
                IsReadOnly = true,
                TabIndex = 1,
            };

            return pathBox;
        }
        private TextBox BuildTextBox()
        {
            var textBox = new TextBox {
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.NoWrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                FontFamily = new FontFamily( "Fira Code" ),
                FontSize = 16D,
                TabIndex = 0,  // 将 Tab 序提至最前，使切换至当前选项卡时的自动焦点落至自身
            };
            textBox.PreviewKeyDown += (object sender, KeyEventArgs evt) =>
            {
                if (evt.Key == Key.Tab) {
                    textBox.SelectedText = "    ";
                    textBox.CaretIndex += 4;  // 移除选区
                    evt.Handled = true;
                }
            };

            foreach (object obj in textBox.CommandBindings)
            {
                Console.WriteLine(obj);
            }

            return textBox;
        }
        public void Save()
        {
            if (this.IsDirty) {
                File.WriteAllText(this.FilePath, this.Text, Encoding.UTF8);
                this.RawData = this.Text;
                this.Header = ((string) this.Header).Substring(1);
            }
        }
    }
}