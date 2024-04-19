using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
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
        /// <summary>
        /// Market object to use
        /// </summary>
        private MarketCards market;

        /// <summary>
        /// Repo class instantiated 
        /// </summary>
        private CardRepository repo;

        public List<EnergyType> EnergyTypes { get; set; }

        public EnergyType SelectedEnumValue { get; set; }

       
        public MarketPage(CardRepository cr,MarketCards m)
        {
            InitializeComponent();
            repo = cr;
            market = m;
            foreach (Card card in repo.GetAllCards())
            {

                market.AddCard(card); 
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
                    market.MC.Where(card => card.CardName.ToLower().Contains(searchTerm)));
            }
        }

        private void EnergyTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var selectedEnergyType = (EnergyType)comboBox.SelectedValue;

            if (selectedEnergyType != EnergyType.NONE)
            {
                // Filter the market cards based on the selected energy type
                market.FilteredMarketCards = new ObservableCollection<Card>(
                    market.MC.Where(card => card.EnergyTypeID == (int)selectedEnergyType));
            }
            else
            {
                // Show all cards
                market.FilteredMarketCards = new ObservableCollection<Card>(market.MC);
            }
        }

    }
}
