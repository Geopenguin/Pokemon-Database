using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_WPF_App
{
    public class Card
    {
        // Card properties 
        public int CardID { get; set; }
        public int SetID { get; set; }
        public int EnergyTypeID { get; set; }
        public string Rarity { get; set; }
        public string CardType { get; set; }
        public int HP { get; set; }
        public string CardName { get; set; }
        public string? TrainerEffect { get; set; }
        public string? ImagePath { get; set; }
        public int UserCardId { get; set; }

        //Card Constructor 
        public Card(int cardId, int setId, int energytypeId,string rarity, string cardtype, int hp, string cardname, string? trainer, string? image)
        {
            CardID = cardId;
            SetID = setId;
            EnergyTypeID = energytypeId;
            Rarity = rarity;
            CardType = cardtype;
            HP = hp;
            CardName = cardname;
            TrainerEffect = trainer;
            ImagePath = image;
        }
    }
}
