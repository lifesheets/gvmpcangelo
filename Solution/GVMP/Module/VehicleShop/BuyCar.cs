using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class BuyCar
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

        public BuyCar(string vehicle_name, int price)
        {
            Vehicle_Name = vehicle_name;
            Price = price;
        }
    }
}
