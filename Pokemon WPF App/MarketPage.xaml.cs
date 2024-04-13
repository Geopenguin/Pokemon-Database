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
    public class MarketViewModel
    {
        public ObservableCollection<Card> Cards { get; set; }

            public MarketViewModel()
            {
                // Initialize the collection and add some sample data
                Cards = new ObservableCollection<Card>
                {
                    new Card { Name = "Card1", Description = "Description1" },
                    new Card { Name = "Card2", Description = "Description2" },
                    // Add more cards as needed
                };
            }
    }
}
