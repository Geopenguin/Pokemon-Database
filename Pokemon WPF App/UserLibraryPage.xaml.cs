using Microsoft.VisualBasic.ApplicationServices;
using Pokemon_WPF_App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Pokemon_WPF_App
{
    public partial class UserLibraryPage : Page
    {
        private User currentUser;
        private ICommand removeCardCommand;
        private CardRepository repo;

        public UserLibraryPage(CardRepository cr, User user)
        {
            InitializeComponent();
            currentUser = user;
            repo = cr;

            foreach (Card card in repo.GetUserCards(currentUser.UserId))
            {
                currentUser.Cards.Add(card);
            }

            DataContext = currentUser;
            removeCardCommand = new RelayCommand(RemoveCard);

            // Retrieve user's decks from the database and populate the DeckNamesStackPanel
            List<string> deckNames = repo.GetUserDeckNames(currentUser.UserId);
            foreach (string deckName in deckNames)
            {
                // Create a StackPanel for each deck and add it to the DeckNamesStackPanel
                StackPanel deckStackPanel = CreateDeckStackPanel(deckName);
                DeckNamesStackPanel.Children.Add(deckStackPanel);
            }
        }

        private StackPanel CreateDeckStackPanel(string deckName)
        {
            StackPanel deckStackPanel = new StackPanel();
            deckStackPanel.Orientation = Orientation.Horizontal;

            TextBlock deckNameTextBlock = new TextBlock();
            deckNameTextBlock.Text = deckName;
            deckNameTextBlock.FontFamily = new FontFamily("Arial");
            deckNameTextBlock.FontSize = 16;
            deckNameTextBlock.Foreground = Brushes.White;
            deckStackPanel.Children.Add(deckNameTextBlock);

            TextBlock cardCounterTextBlock = new TextBlock();
            cardCounterTextBlock.Text = $" (0/60)";
            cardCounterTextBlock.Margin = new Thickness(5, 0, 0, 0);
            cardCounterTextBlock.FontFamily = new FontFamily("Arial");
            cardCounterTextBlock.FontSize = 14;
            cardCounterTextBlock.Foreground = Brushes.White;
            deckStackPanel.Children.Add(cardCounterTextBlock);

            Button deleteButton = new Button();
            deleteButton.Content = "Delete";
            deleteButton.Click += DeleteDeck_Click;
            deleteButton.Tag = deckName;
            deckStackPanel.Children.Add(deleteButton);

            Button viewButton = new Button();
            viewButton.Content = "View Deck";
            viewButton.Click += ViewDeck_Click;
            viewButton.Tag = deckName;
            deckStackPanel.Children.Add(viewButton);

            return deckStackPanel;
        }

        private void CreateNewDeck_Click(object sender, RoutedEventArgs e)
        {
            string deckName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the new deck:", "Create New Deck", "");
            if (!string.IsNullOrEmpty(deckName))
            {
                repo.CreateNewDeck(currentUser.UserId, deckName);

                // Create a StackPanel for the new deck and add it to the DeckNamesStackPanel
                StackPanel deckStackPanel = CreateDeckStackPanel(deckName);

                // Add the StackPanel to the DeckNamesStackPanel
                DeckNamesStackPanel.Children.Add(deckStackPanel);
            }
        }

        private void DeleteDeck_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            string deckName = deleteButton.Tag as string;

            // Remove the deck from the database
            repo.DeleteDeck(currentUser.UserId, deckName);

            // Remove the deck from the DeckNamesStackPanel
            StackPanel deckStackPanel = deleteButton.Parent as StackPanel;
            DeckNamesStackPanel.Children.Remove(deckStackPanel);
        }

        private void ViewDeck_Click(object sender, RoutedEventArgs e)
        {
            Button viewButton = sender as Button;
            string deckName = viewButton.Tag as string;

            // Retrieve the cards in the selected deck from the database
            List<Card> deckCards = repo.GetDeckCards(currentUser.UserId, deckName);

            // Update the CardItemsControl to display the cards in the deck
            CardItemsControl.ItemsSource = deckCards;
        }

        private void AddToDeck_Click(object sender, RoutedEventArgs e)
        {
            Button addToDeckButton = sender as Button;
            Card selectedCard = addToDeckButton.DataContext as Card;

            // Get the list of deck names
            List<string> deckNames = GetDeckNames();

            // Show the DeckSelectionWindow
            DeckSelectionWindow deckSelectionWindow = new DeckSelectionWindow(deckNames);
            if (deckSelectionWindow.ShowDialog() == true)
            {
                string selectedDeckName = deckSelectionWindow.SelectedDeckName;
                AddCardToDeck(selectedCard, selectedDeckName);

                // Update the card counter for the selected deck
                TextBlock cardCounterTextBlock = GetCardCounterTextBlock(selectedDeckName);
                if (cardCounterTextBlock != null)
                {
                    string countText = cardCounterTextBlock.Text.Trim();
                    if (countText.StartsWith("(") && countText.EndsWith(")"))
                    {
                        countText = countText.Substring(1, countText.Length - 2);
                    }
                    string[] countParts = countText.Split('/');
                    if (countParts.Length == 2 && int.TryParse(countParts[0], out int currentCount))
                    {
                        int newCount = currentCount + 1;
                        cardCounterTextBlock.Text = $"({newCount}/60)";
                    }
                }
            }

        }
        private void ViewAllCards_Click(object sender, RoutedEventArgs e)
        {
            // Update the CardItemsControl to display all cards in the user's library
            CardItemsControl.ItemsSource = currentUser.Cards;
        }

        private List<string> GetDeckNames()
        {
            List<string> deckNames = new List<string>();
            foreach (var child in DeckNamesStackPanel.Children)
            {
                if (child is StackPanel deckStackPanel)
                {
                    TextBlock deckNameTextBlock = deckStackPanel.Children[0] as TextBlock;
                    deckNames.Add(deckNameTextBlock.Text);
                }
            }
            return deckNames;
        }

        private TextBlock GetCardCounterTextBlock(string deckName)
        {
            foreach (var child in DeckNamesStackPanel.Children)
            {
                if (child is StackPanel deckStackPanel)
                {
                    TextBlock deckNameTextBlock = deckStackPanel.Children[0] as TextBlock;
                    if (deckNameTextBlock.Text == deckName)
                    {
                        return deckStackPanel.Children[1] as TextBlock;
                    }
                }
            }
            return null;
        }

        private void AddCardToDeck(Card card, string deckName)
        {
            // Implement the logic to add the card to the specified deck
            // You can use the `repo` object to interact with the database
            // and update the card counter for the deck
        }

        public ICommand RemoveCardCommand
        {
            get { return removeCardCommand; }
        }

        private void RemoveCard(object parameter)
        {
            // Get the card to remove
            Card cardToRemove = parameter as Card;
            if (cardToRemove != null)
            {
                // Remove the card from the user's collection
                currentUser.Cards.Remove(cardToRemove);
            }
        }

        // RelayCommand implementation
        private class RelayCommand : ICommand
        {
            private Action<object> execute;

            public RelayCommand(Action<object> execute)
            {
                this.execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                execute(parameter);
            }

            public event EventHandler CanExecuteChanged;
        }
    }
}