using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pokemon_WPF_App
{
    public partial class MarketPage : Page
    {
        private MarketCards market;

        public MarketPage()
        {
            InitializeComponent();

            // Create a new Market object
            market = new MarketCards();

            // Add some sample cards to the market
            // ...
            for (int i = 0; i < 30; i++)
            {
                market.AddCard(new Card
                {
                    Name = "Charizard " + i,
                    Description = "A Fire/Flying type Pokémon",
                    ImagePath = "/Images/Charizard.png",
                    Type = "Fire/Flying",
                    HP = 78,
                    Attack = 84,
                    Defense = 78
                });
            }
            for (int i = 0; i < 30; i++)
            {
                market.AddCard(new Card
                {
                    Name = "Squirtle" + i,
                    Description = "A Fire/Flying type Pokémon",
                    ImagePath = "/Images/Charizard.png",
                    Type = "Fire/Flying",
                    HP = 78,
                    Attack = 84,
                    Defense = 78
                });
            }

            // Set the data context for the MarketPage
            DataContext = market;

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If search term is empty or whitespace, show all cards
                market.FilteredMarketCards = new ObservableCollection<Card>(market.MC);
            }
            else
            {
                // Filter the market cards based on the search term
                market.FilteredMarketCards = new ObservableCollection<Card>(
                    market.MC.Where(card => card.Name.ToLower().Contains(searchTerm)));
            }
        }

        private void EnergyTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnergyTypeComboBox.SelectedItem != null)
            {
                string selectedEnergyType = ((ComboBoxItem)EnergyTypeComboBox.SelectedItem).Content.ToString();

                if (selectedEnergyType == "All")
                {
                    // Show all cards
                    market.FilteredMarketCards = new ObservableCollection<Card>(market.MC);
                }
                else
                {
                    // Filter the market cards based on the selected energy type
                    market.FilteredMarketCards = new ObservableCollection<Card>(
                        market.MC.Where(card => card.Type.ToLower().Contains(selectedEnergyType.ToLower())));
                }
            }
        }


    }
}
