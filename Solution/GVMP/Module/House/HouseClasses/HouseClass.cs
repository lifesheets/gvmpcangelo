using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class HouseClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 HouseLocation { get; set; }
        public Vector3 LagerLocation { get; set; }
        public Vector3 KleiderschrankLocation { get; set; }
        public int MaxTenants { get; set; }

        public HouseClass() { }
    }
}
