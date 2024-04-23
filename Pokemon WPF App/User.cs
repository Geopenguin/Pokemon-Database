using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_WPF_App
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        //public string Password { get; set; }
        public string Email { get; set; }

        
        public ObservableCollection<Card> Cards { get; set; }

        public User(int userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
            Cards = new ObservableCollection<Card>();
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
    }
}
