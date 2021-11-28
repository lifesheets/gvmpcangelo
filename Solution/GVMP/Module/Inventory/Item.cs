using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace GVMP
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int WeightInG { get; set; }
        public int MaxStackSize { get; set; }
        public WeaponHash Whash { get; set; }

        public Item() { }
    }
}
