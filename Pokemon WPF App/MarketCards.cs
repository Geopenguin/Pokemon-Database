using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_WPF_App
{
    public class MarketCards : INotifyPropertyChanged
    {
        private ObservableCollection<Card> _marketCards;
        private ObservableCollection<Card> _filteredMarketCards;

        public ObservableCollection<Card> MC
        {
            get { return _marketCards; }
            set
            {
                _marketCards = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Card> FilteredMarketCards
        {
            get { return _filteredMarketCards; }
            set
            {
                _filteredMarketCards = value;
                OnPropertyChanged();
            }
        }

        public List<EnumValue> EnergyTypes { get; set; }

        public EnergyType SelectedEnumValue { get; set; }

        public MarketCards()
        {
            EnergyTypes = Enum.GetValues(typeof(EnergyType))
                              .Cast<EnergyType>()
                              .Select(v => new EnumValue { Value = v, Key = v.ToString() })
                              .ToList();

            // Set the default selection to "ALL"
            SelectedEnumValue = EnergyType.NONE;
            MC = new ObservableCollection<Card>();
            FilteredMarketCards = new ObservableCollection<Card>();
        }

        public class EnumValue
        {
            public EnergyType Value { get; set; }
            public string Key { get; set; }
        }
        public void AddCard(Card card)
        {
            MC.Add(card);
            FilteredMarketCards.Add(card); // Add to filtered list by default
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}




