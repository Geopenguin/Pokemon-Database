using Microsoft.VisualBasic.ApplicationServices;
using NAudio.Wave;
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
using System.Windows.Threading;

namespace Pokemon_WPF_App
{
    public partial class GamblePage : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Pokemon"].ConnectionString;
        private ObservableCollection<Card> allCards;
        private ObservableCollection<Card> gamblingCards;
        private User currentUser;
        private readonly DispatcherTimer _slotTimer;
        private int _slotIteration;
        private readonly List<Card> _finalCards;
        private int _cardIndex; // Track the index of the card being generated
        private Dictionary<string, IWavePlayer> wavePlayers = new Dictionary<string, IWavePlayer>();
        private Dictionary<string, AudioFileReader> audioFiles = new Dictionary<string, AudioFileReader>();

        public GamblePage(User user)
        {
            InitializeComponent();
            PreloadSounds();
            currentUser = user;

            // Create a collection of all available cards
            allCards = new ObservableCollection<Card> { };

            // Initialize the gambling cards collection
            gamblingCards = new ObservableCollection<Card>();

            _slotTimer = new DispatcherTimer();
            _slotTimer.Interval = TimeSpan.FromMilliseconds(100); // Adjust this value to control the speed of the slot machine effect
            _slotTimer.Tick += SlotTimer_Tick;

            _finalCards = new List<Card>();
        }


        private void PreloadSounds()
        {
        // Load sounds and prepare players
        LoadAndPrepareSound("Sounds/gamble-sound.wav");
        }
        private void LoadAndPrepareSound(string path)
        {
            // Instantiate WaveOutEvent and AudioFileReader objects
            var player = new WaveOutEvent();
            var audioFile = new AudioFileReader(path);
            player.Init(audioFile);
            // Store those two objects in dictionaries, keyed by the sound file path (allows quick access for playing the sound)
            wavePlayers[path] = player;
            audioFiles[path] = audioFile;
        }
        private void PlaySound(string soundPath)
        {
            // Retrieve the WaveOutEvent player and AudioFileReader for the given sound file from the dictionaries
            if (wavePlayers.TryGetValue(soundPath, out var player) && audioFiles.TryGetValue(soundPath, out var file))
            {
                // Set AudioFileReader to 0 to ensure the sound file plays from the beginning
                file.Position = 0;
                player.Play();
            }
        }

        private void GambleButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the previous gambling cards
            GamblingCardsItemsControl.Items.Clear();

            // Get the card repository instance
            CardRepository cardRepository = new CardRepository();
            allCards = cardRepository.GetAllCards();

            _slotIteration = 0;
            _finalCards.Clear();
            _cardIndex = 0; // Reset the card index

            // Start the slot machine effect
            _slotTimer.Start();
        }

        private void SlotTimer_Tick(object sender, EventArgs e)
        {
            _slotIteration++;
                if (_cardIndex < 3) // Generate a new card if the card index is less than 3
                {
                    if (_slotIteration <= 1) // Adjust this value to control the number of iterations
                    {
                        // Randomly select a card from the allCards collection
                        Random random = new Random();
                        int cardCount = allCards.Count;
                        int index = random.Next(cardCount);
                        Card card = allCards[index];

                        // Add the selected card to the final cards list
                        _finalCards.Add(card);

                        // Update the ItemsControl with all the cards in the final cards list
                        GamblingCardsItemsControl.Items.Clear();
                        foreach (Card finalCard in _finalCards)
                        {
                            GamblingCardsItemsControl.Items.Add(finalCard);
                        }
                }
                else
                    {
                        // Stop the slot machine effect for the current card
                        _slotTimer.Stop();

                        _slotIteration = 0;
                        _cardIndex++; // Move to the next card

                        // Start the slot machine effect for the next card
                        if (_cardIndex < 3)
                        {
                            _slotTimer.Start();
                        }
                }
                PlaySound("Sounds/gamble-sound.wav");
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