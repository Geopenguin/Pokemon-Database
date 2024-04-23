using Microsoft.VisualBasic.ApplicationServices;
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

        private User currentUser;

        private string connectionString = ConfigurationManager.ConnectionStrings["Pokemon"].ConnectionString;

        private ObservableCollection<Card> wishlistedcards = new ObservableCollection<Card>();

        public MarketPage(CardRepository cr, MarketCards m, User u)
        {
            InitializeComponent();
            repo = cr;
            market = m;
            currentUser = u;
            foreach (Card card in repo.GetAllCards())
            {
                market.AddCard(card);
            }

            // Set the data context for the MarketPage
            DataContext = market;

        }

        /// <summary>
        /// Event when search is click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event to occur when Energy type is sorted 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Pedro Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WishlistButton_Click(object sender, RoutedEventArgs e)
        {
            Button Button = sender as Button;
            // SQL query to insert the card into the User.UserCards table
            string query = "INSERT INTO [User].[WishList] ([UserID], [CardID]) VALUES (@userId, @cardId)";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters to the command object
                command.Parameters.AddWithValue("@userId", currentUser.UserId);
                command.Parameters.AddWithValue("@cardId", GetCardFromWishListButton(Button).CardID);

                // Execute the query
                command.ExecuteNonQuery();
            }
        }

        private Card GetCardFromWishListButton(Button button)
        {
            // Find the parent Border of the clicked button
            Border cardBorder = button.Parent as Border;

            // If the parent is not a Border, traverse up the visual tree
            if (cardBorder == null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(button);
                while (parent != null && !(parent is Border))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                cardBorder = parent as Border;
            }

            // If the parent Border is found, get the Card object from its DataContext
            return cardBorder?.DataContext as Card;
        }

        /// <summary>
        /// Pedro added 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WishlistView_Click(object sender, RoutedEventArgs e)
        {
            // Get the wishlist cards
            var wishList = repo.GetUserWishListedCards(currentUser.UserId);

            if (wishList.Count != 0)
            {
                // Update the market cards with the wishlist cards
                market.MC.Clear();
                foreach (Card card in wishList)
                {
                    market.MC.Add(card);
                }

                // Update the filtered market cards to show the wishlist cards
                market.FilteredMarketCards = new ObservableCollection<Card>(market.MC);

            }
        }

        /// <summary>
        /// Pedro added this 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetMarketCards_Click(object sender, RoutedEventArgs e)
        {
            // Get all cards
            var allCards = repo.GetAllCards();

            // Update the market cards with all cards
            market.MC.Clear();
            foreach (Card card in allCards)
            {
                market.MC.Add(card);
            }

            // Update the filtered market cards to show all cards
            market.FilteredMarketCards = new ObservableCollection<Card>(market.MC);
        }
    }
}
