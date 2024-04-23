using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NAudio.Wave;


namespace Pokemon_WPF_App
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        // Create dictionaries for sound files (allows for efficient retrieval on button presses)
        private Dictionary<string, IWavePlayer> wavePlayers = new Dictionary<string, IWavePlayer>();
        private Dictionary<string, AudioFileReader> audioFiles = new Dictionary<string, AudioFileReader>();

        public LoginWindow()
        {
            InitializeComponent();
            PreloadSounds();
        }
        private void PreloadSounds()
        {
            // Load sounds and prepare players
            LoadAndPrepareSound("Sounds/login-button-sound.wav");
            LoadAndPrepareSound("Sounds/sign-up-button-sound.wav");
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
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Validate the username and password against our database
            UserRepository userRepository = new UserRepository();
            User user = userRepository.AuthenticateUser(username, password);
            
            // If successful, show the main window
            if (user != null)
            {
                PlaySound("Sounds/login-button-sound.wav");
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Sounds/sign-up-button-sound.wav");
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
            this.Close();
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
    }
}
