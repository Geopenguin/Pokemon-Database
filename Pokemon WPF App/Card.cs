using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_WPF_App
{
    public class Card
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; } // e.g., Fire, Water, Grass
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        // Add any other properties you need for a Pokémon card
    }
}
