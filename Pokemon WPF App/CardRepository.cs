using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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

        public void RemoveCardFromUser(int userCardId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM [User].[UserCards] WHERE UserCardId = @UserCardId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserCardId", userCardId);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Method to return all cards from the Cards.Cards table 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Card> GetUserCards(int userId)
        {
            ObservableCollection<Card> cardsList = new ObservableCollection<Card>();
            
            string query = "SELECT uc.UserCardId, c.CardID, c.SetID, c.EnergyTypeID, c.Rarity, c.CardType, c.HitPoints, c.CardName, c.TrainerEffect, c.ImageURL " +

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
                            int userCardId = reader.GetInt32(0); // Get the UserCardId first
                            Card card = new Card(
                                reader.GetInt32(1), // card id
                                reader.GetInt32(2), // card setid
                                reader.GetInt32(3), // card energy type id
                                reader.GetString(4), // card rarity
                                reader.GetString(5), // cardtype
                                reader.GetInt32(6), // hitpoints
                                reader.GetString(7), // CardName
                                reader.GetString(8), // trainer effect, nullable
                                reader.GetString(9) // image url
                            );
                            card.UserCardId = userCardId; // Set the UserCardId property
                            cardsList.Add(card);
                        }
                    }
                }
            }

            return cardsList;
        }

        /// <summary>
        /// Method to return all cards from the Cards.Cards table 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Card> GetUserWishListedCards(int userId)
        {
            ObservableCollection<Card> wishList = new ObservableCollection<Card>();

            string query = "SELECT c.CardID, c.SetID, c.EnergyTypeID, c.Rarity, c.CardType, c.HitPoints, c.CardName, c.TrainerEffect, c.ImageURL " +
               "FROM [Cards].[Cards] c JOIN [User].[WishList] w ON c.CardID = w.CardID " +
               "WHERE w.UserID = @UserId";

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
                            wishList.Add(card);
                        }
                    }
                }
            }

            return wishList;
        }

    }
}
