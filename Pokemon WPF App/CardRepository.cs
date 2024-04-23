using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;

namespace Pokemon_WPF_App
{
    
    public class CardRepository
    {
        /// <summary>
        /// Private string for connecting to SQL 
        /// </summary>
        private string connectionString = ConfigurationManager.ConnectionStrings["Pokemon"].ConnectionString;

        /// <summary>
        /// Private field for cards list 
        /// </summary>
        private ObservableCollection<Card> cards = new ObservableCollection<Card>();

        /// <summary>
        /// Method to return all cards from the Cards.Cards table 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Card> GetAllCards()
        {
            ObservableCollection<Card> cardsList = new ObservableCollection<Card>();

            // SQL query to select all cards from the database
            string query = "SELECT * FROM [Cards].[Cards]";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Execute the query and get the result set
                SqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                     
                    // Create a new card object
                    Card card = new Card(

                        reader.GetInt32(0), //card id
                        reader.GetInt32(1), //card setid
                        reader.GetInt32(2), //card energy type id
                        reader.GetString(3), //card rarity
                        reader.GetString(4), //cardtype 
                        reader.GetInt32(5), //hitpoints
                        reader.GetString(6), //CardName
                        reader.GetString(7), //trainer effect, nullable
                        reader.GetString(8) //image url
                    ); 

                    // Add the card object to the list
                    cardsList.Add(card);
                }

                // Close the reader
                reader.Close();
            }
            cards = cardsList;
            return cards;
        }


        /// <summary>
        /// Method to return all cards from the Cards.Cards table 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Card> GetUserCards(int userId)
        {
            ObservableCollection<Card> cardsList = new ObservableCollection<Card>();
            string query = "SELECT c.CardID, c.SetID, c.EnergyTypeID, c.Rarity, c.CardType, c.HitPoints, c.CardName, c.TrainerEffect, c.ImageUrl " +
                           "FROM [User].UserCards uc JOIN Cards.Cards c ON uc.CardID = c.CardID " +
                           "WHERE uc.UserID = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Card card = new Card(
                                reader.GetInt32(0), // card id
                                reader.GetInt32(1), // card setid
                                reader.GetInt32(2), // card energy type id
                                reader.GetString(3), // card rarity
                                reader.GetString(4), // cardtype
                                reader.GetInt32(5), // hitpoints
                                reader.GetString(6), // CardName
                                reader.GetString(7), // trainer effect, nullable
                                reader.GetString(8) // image url
                            );
                            cardsList.Add(card);
                        }
                    }
                }
            }

            return cardsList;
        }

        public void CreateNewDeck(int userId, string deckName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO [User].Deck (UserID, DeckName, CreatedOn) VALUES (@UserId, @DeckName, GETDATE());";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@DeckName", deckName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Card> GetDeckCards(int userId, string deckName)
        {
            List<Card> deckCards = new List<Card>();

            string query = @"
        SELECT c.CardID, c.SetID, c.EnergyTypeID, c.Rarity, c.CardType, c.HitPoints, c.CardName, c.TrainerEffect, c.ImageUrl
        FROM [User].Deck d
        JOIN [User].DeckCard dc ON d.DeckID = dc.DeckID
        JOIN Cards.Cards c ON dc.CardID = c.CardID
        WHERE d.UserID = @UserId AND d.DeckName = @DeckName
    ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@DeckName", deckName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Card card = new Card(
                                reader.GetInt32(0), // card id
                                reader.GetInt32(1), // card setid
                                reader.GetInt32(2), // card energy type id
                                reader.GetString(3), // card rarity
                                reader.GetString(4), // cardtype
                                reader.GetInt32(5), // hitpoints
                                reader.GetString(6), // CardName
                                reader.IsDBNull(7) ? null : reader.GetString(7), // trainer effect, nullable
                                reader.GetString(8) // image url
                            );
                            deckCards.Add(card);
                        }
                    }
                }
            }

            return deckCards;
        }

        public List<string> GetUserDeckNames(int userId)
        {
            List<string> deckNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DeckName FROM [User].Deck WHERE UserID = @UserId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string deckName = reader.GetString(0);
                            deckNames.Add(deckName);
                        }
                    }
                }
            }
            return deckNames;
        }

        public void DeleteDeck(int userId, string deckName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM [User].Deck WHERE UserID = @UserId AND DeckName = @DeckName;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@DeckName", deckName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddCardToDeck(int deckID, int cardID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO [User].DeckCard (DeckID, CardID) VALUES (@DeckID, @CardID);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DeckID", deckID);
                    command.Parameters.AddWithValue("@CardID", cardID);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
