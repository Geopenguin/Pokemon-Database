﻿using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient; 

namespace Pokemon_WPF_App
{
    public partial class MainWindow : Window
    {
        private UserLibraryPage userLibraryPage;
        private MarketPage marketPage;
        private GamblePage gamblePage;
        private User user;
        private AdminPage AdminPage; 

        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;

            // Create instances of the pages
            CardRepository repo = new CardRepository();
            MarketCards market = new MarketCards();
            AdminPage = new AdminPage(); 
            userLibraryPage = new UserLibraryPage(repo, user);
            marketPage = new MarketPage(repo, market,user);
            gamblePage = new GamblePage(user);
            

            // Navigate to the initial page
            LibraryFrame.NavigationService.Navigate(userLibraryPage);
        }
        
        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected TabItem
            TabItem selectedTab = MainTabControl.SelectedItem as TabItem;

            if (selectedTab != null)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "User Library":
                        userLibraryPage.RefreshLib(); 
                        LibraryFrame.NavigationService.Navigate(userLibraryPage);
                        break;
                    case "Market":
                        MarketFrame.NavigationService.Navigate(marketPage);
                        break;
                    case "Gamble":
                        GambleFrame.NavigationService.Navigate(gamblePage);
                        break;
                    case "Admin":
                        AdminFrame.NavigationService.Navigate(AdminPage);
                        break;                   
                }
            }
        }


        private void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Pokemon"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query Example
                    string query = "SELECT COUNT(*) FROM [Cards].[Cards]";
                    SqlCommand command = new SqlCommand(query, connection);

                    int rowCount = (int)command.ExecuteScalar();

                    MessageBox.Show($"Connection successful. Number of rows in Cards.Cards: {rowCount}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection failed. Error: {ex.Message}");
                }
            }
        }




    }
}

