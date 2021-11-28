using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class NeonColor
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Color Color { get; set; }

        public NeonColor(string Label, string Name, Color Color, int Price)
        {
            this.Label = Label;
            this.Name = Name;
            this.Color = Color;
            this.Price = Price;
        }
    }
}
