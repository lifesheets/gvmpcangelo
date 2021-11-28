using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class BuyHeli
    {
        public string Vehicle_Name
        {
            get;
            set;
        }

        public int Price
        {
            get;
            set;
        }

        public BuyHeli(string vehicle_name, int price)
        {
            Vehicle_Name = vehicle_name;
            Price = price;
        }
    }
}
