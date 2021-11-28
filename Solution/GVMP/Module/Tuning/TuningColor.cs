using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class TuningColor
    {
        public int ColorId
        {
            get;
            set;
        }

        public string Label
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Price
        {
            get;
            set;
        }

        public TuningColor(string Label, string Name, int ColorId, int Price)
        {
            this.Label = Label;
            this.Name = Name;
            this.ColorId = ColorId;
            this.Price = Price;
        }
    }
}