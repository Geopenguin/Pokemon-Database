using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Pokemon_WPF_App
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            UserRepository userRepository = new UserRepository();
            bool isRegistered = userRepository.RegisterUser(username, email, password);

            if (isRegistered)
            {
                MessageBox.Show("Registration successful. You can now log in.");
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed. Please try again.");
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}