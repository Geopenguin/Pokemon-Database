using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pokemon_WPF_App
{
    /// <summary>
    /// Interaction logic for DeckSelectionWindow.xaml
    /// </summary>
    public partial class DeckSelectionWindow : Window
    {
        public string SelectedDeckName { get; private set; }

        public DeckSelectionWindow(IEnumerable<string> deckNames)
        {
            InitializeComponent();
            DeckListBox.ItemsSource = deckNames;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDeckName = DeckListBox.SelectedItem as string;
            DialogResult = true;
        }
    }
}
