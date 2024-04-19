using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
