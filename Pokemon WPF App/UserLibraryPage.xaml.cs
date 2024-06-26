﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        }

        public void RefreshLib()
        {
            ClearLib();
            foreach (Card card in repo.GetUserCards(currentUser.UserId))
            {
                currentUser.Cards.Add(card);
            }
        }

        public void ClearLib()
        {
            foreach (Card card in repo.GetUserCards(currentUser.UserId))
            {
                currentUser.Cards.Clear();
            }
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

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the Button that was clicked
            Button button = sender as Button;

            // Find the parent Border control
            DependencyObject parent = button;
            while (parent != null && !(parent is Border))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            Border border = parent as Border;

            // Get the Card object from the Border's DataContext
            Card cardToRemove = border?.DataContext as Card;

            // Remove the Card object from the Cards collection
            if (cardToRemove != null)
            {
                repo.RemoveCardFromUser(cardToRemove.UserCardId);
            }
            RefreshLib();
        }
    }
}
