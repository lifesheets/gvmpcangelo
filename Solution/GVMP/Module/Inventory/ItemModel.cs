using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class ItemModel
    {
        public int Id { get; set; }
        public int Slot { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int Amount { get; set; }

        public ItemModel() { }
    }
}
