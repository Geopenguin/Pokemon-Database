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
    public partial class GamblePage : Page
    {
        private ObservableCollection<Card> allCards;
        private ObservableCollection<Card> gamblingCards;
        private User currentUser;

        public GamblePage(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Create a collection of all available cards
            allCards = new ObservableCollection<Card>
        {
            //new Card { CardName = "Pikachu", ImagePath = "/Images/Pikachu.png", /* ... */ },
            //new Card { CardName = "Charizard", ImagePath = "/Images/Charizard.png", /* ... */ },
            // Add more cards as needed
        };

            // Initialize the gambling cards collection
            gamblingCards = new ObservableCollection<Card>();

            // Bind the gambling cards collection to the UI
            GamblingCardsListBox.ItemsSource = gamblingCards;
        }

        private void GambleButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the previous gambling cards
            gamblingCards.Clear();

            // Randomly select three cards from the allCards collection
            Random random = new Random();
            int cardCount = allCards.Count;
            List<int> selectedIndices = new List<int>();

            while (selectedIndices.Count < 3)
            {
                int index = random.Next(cardCount);
                if (!selectedIndices.Contains(index))
                {
                }
            }
        }

        private void ChooseCardButtonClickChooseCardButton_Click() 
        {

        }
    }
}
