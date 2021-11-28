using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Garage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 CarPoint { get; set; }

        public float Rotation { get; set; }

        public Garage() { }
    }
}
