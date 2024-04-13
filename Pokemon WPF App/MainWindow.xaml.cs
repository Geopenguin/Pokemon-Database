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

namespace Pokemon_WPF_App
{
    public partial class MainWindow : Window
    {
        private UserLibraryPage userLibraryPage;
        private MarketPage marketPage;
        private GamblePage gamblePage;
        private User user;

        public MainWindow()
        {
            InitializeComponent();

            // Create instances of the pages
            userLibraryPage = new UserLibraryPage();
            marketPage = new MarketPage();
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
                        LibraryFrame.NavigationService.Navigate(userLibraryPage);
                        break;
                    case "Market":
                        MarketFrame.NavigationService.Navigate(marketPage);
                        break;
                    case "Gamble":
                        GambleFrame.NavigationService.Navigate(gamblePage);
                        break;
                }
            }
        }

    }
}

