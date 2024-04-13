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
        public string Name { get; set; }
        public ObservableCollection<Card> Cards { get; set; }
        public User(string name)
        {
            Name = name;
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
