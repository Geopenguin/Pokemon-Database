using Pokemon_WPF_App;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pokemon_WPF_App
{

    public partial class UserLibraryPage : Page
    {
        private User currentUser;
        private ICommand removeCardCommand;

        public UserLibraryPage()
        {
            InitializeComponent();

            // Create a new User object
            currentUser = new User("John Doe");

            // Add some sample cards
            // ...
            for (int i = 0; i < 6; i++) 
            {
                currentUser.AddCard(new Card
                {
                    Name = "Charizard " + i,
                    Description = "A Fire/Flying type Pokémon",
                    ImagePath = "/Images/Charizard.png",
                    Type = "Fire/Flying",
                    HP = 78,
                    Attack = 84,
                    Defense = 78
                });
            }
            // Set the data context for the UserLibraryPage
            DataContext = currentUser;

            // Create the remove card command
            removeCardCommand = new RelayCommand(RemoveCard);
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





