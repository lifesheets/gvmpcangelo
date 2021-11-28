using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Flag
    {
        public Vector3 Position { get; set; }
        public int Faction { get; set; } = 0;

        public Flag(Vector3 Position)
        {
            this.Position = Position;
        }
    }
}
