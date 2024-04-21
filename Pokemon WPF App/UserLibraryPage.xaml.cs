﻿using Pokemon_WPF_App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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