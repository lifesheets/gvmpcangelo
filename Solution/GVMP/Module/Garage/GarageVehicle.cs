using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class GarageVehicle
    {
        public int Id
        {
            get;
            set;
        }

        public int OwnerID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Plate
        {
            get;
            set;
        } = "";

        public List<int> Keys { get; set; } = new List<int>();

        public GarageVehicle() { }
    }
}
