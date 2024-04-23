using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class GamblePage : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Pokemon"].ConnectionString;
        private ObservableCollection<Card> allCards;
        private ObservableCollection<Card> gamblingCards;
        private User currentUser;

        public GamblePage(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Create a collection of all available cards
            allCards = new ObservableCollection<Card>{};

            // Initialize the gambling cards collection
            gamblingCards = new ObservableCollection<Card>();

            //// Bind the gambling cards collection to the UI
            //GamblingCardsListBox.ItemsSource = gamblingCards;
        }

        private void GambleButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the previous gambling cards
            GamblingCardsItemsControl.Items.Clear();

            // Get the card repository instance
            CardRepository cardRepository = new CardRepository();
            ObservableCollection<Card> allCards = cardRepository.GetAllCards();

            // Randomly select three cards from the allCards collection
            Random random = new Random();
            int cardCount = allCards.Count;
            List<Card> selectedCards = new List<Card>();

            while (selectedCards.Count < 3)
            {
                int index = random.Next(cardCount);
                Card card = allCards[index];
                if (!selectedCards.Contains(card))
                {
                    selectedCards.Add(card);
                }
            }

            // Add the selected cards to the ItemsControl
            foreach (Card card in selectedCards)
            {
                GamblingCardsItemsControl.Items.Add(card);
            }
        }


        private void ClaimButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the clicked button
            Button claimButton = sender as Button;

            // Find the Card object associated with the clicked button
            Card claimedCard = GetCardFromButton(claimButton);

            // Get the currently logged-in user's ID
            int loggedInUserId = GetLoggedInUserId(); // You need to implement this method

            // Insert the claimed card into the User.UserCards table
            InsertCardForUser(claimedCard, loggedInUserId);
        }


        private Card GetCardFromButton(Button button)
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


        private void InsertCardForUser(Card card, int userId)
        {
            // SQL query to insert the card into the User.UserCards table
            string query = "INSERT INTO [User].[UserCards] (UserId, CardId, Quantity) VALUES (@UserId, @CardId, 1)";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@CardId", card.CardID);

                // Execute the query
                command.ExecuteNonQuery();
            }
        }

        private int GetLoggedInUserId()
        {
            return currentUser.UserId;
        }

    }
}
