using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace Pokemon_WPF_App
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["Pokemon"].ConnectionString;

        public AdminPage()
        {
            InitializeComponent();
            
        }

        public void AgQuery1_Click(object sender, RoutedEventArgs e)
        {
            // SQL query to see how addicted users are to the gamer 
            string query = @"
            SELECT AVG(PullCount) as AvgPulls
            FROM (
                SELECT uu.UserID, COUNT(*) as PullCount
                FROM [User].GachaHistory ug
                INNER JOIN [User].[Users] uu ON ug.UserID = uu.UserID
                GROUP BY uu.UserID
            ) as UserPulls;";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Execute the query and display the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int avgPulls = reader.GetInt32(0);
                        MessageBox.Show($"Average Gacha Pulls: {avgPulls}");
                        
                    }
                }
            }
        }

        // SQL query to determine the card name and weakness type for a specific energy type
        private void AgQuery2_Click(object sender, RoutedEventArgs e)
        {
            
            string query = @"
        SELECT C.CardName, W.WeaknessType
        FROM [Cards].[Cards] AS C
        JOIN [Cards].Weakness AS W ON C.CardID = W.CardID
        WHERE W.EnergyTypeID = @energyTypeID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cardName = "";
                string weaknessType = "";
                StringBuilder sb = new StringBuilder();

                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Add parameter to the command object
                command.Parameters.AddWithValue("@energyTypeID", 3); // all pokemon that energytypeID is weak to: 
                                                                     //energytype 3 is fire and we pull all cards fire is weak to

                // Execute the query and display the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cardName = reader.GetString(0);
                        weaknessType = reader.GetString(1);
                        sb.AppendLine($"Card Name: {cardName}");
                        sb.AppendLine($"Weakness Type: {weaknessType}");
                        sb.AppendLine(new string('-', 20));
                    }
                }

                // Display the results in a message box
                MessageBox.Show(sb.ToString(), "Results", (MessageBoxButton)MessageBoxButtons.OK);
            }
        }

        // SQL query to find the total number of cards a user has in all their decks, along with the most common card type and average hit points
        private void AgQuery3_Click(object sender, RoutedEventArgs e)
        {

            string query = @"
            SELECT
            u.UserName,
            c.CardName,
            et.TypeName AS EnergyType,
            COUNT(uc.CardID) AS TotalCards,
            AVG(c.HitPoints) AS AverageHitPoints
            FROM [User].Users u
            JOIN [User].UserCards uc ON u.UserID = uc.UserID
            JOIN [Cards].[Cards] c ON uc.CardID = c.CardID
            JOIN [Cards].EnergyType et ON c.EnergyTypeID = et.EnergyTypeID
            GROUP BY u.UserName, c.CardName, et.TypeName
            ORDER BY TotalCards DESC";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Execute the query and display the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string userID = reader.GetString(0);
                        string userName = reader.GetString(1);
                        string rarity = reader.GetString(2);
                        int cardCount = reader.GetInt32(3);

                        MessageBox.Show($"User ID: {userID}, User Name: {userName}, Rarity: {rarity}, Card Count: {cardCount}");
                    }
                }
            }
        }

        // SQL query to find the most duplicated card in the user's library
        private void AgQuery4_Click(object sender, RoutedEventArgs e)
        {
            
            string query = @"
            SELECT CardID, COUNT(*) as Quantity
            FROM [User].UserCards
            GROUP BY CardID
            ORDER BY Quantity DESC;";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Execute the query and display the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int cardID = reader.GetInt32(0);
                        int quantity = reader.GetInt32(1);
                        MessageBox.Show($"Card ID: {cardID}, Quantity: {quantity}");
                    }
                }
            }
        }
    }
}
