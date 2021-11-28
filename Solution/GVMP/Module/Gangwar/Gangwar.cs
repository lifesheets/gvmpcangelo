using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Gangwar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Faction Faction { get; set; }
        public Vector3 Zone { get; set; }
        public Flag Flag1 { get; set; }
        public Flag Flag2 { get; set; }
        public Flag Flag3 { get; set; }
        public Flag Flag4 { get; set; }
        public Faction Attacker { get; set; } = null;
        public int FactionPoints { get; set; } = 0;
        public int AttackerPoints { get; set; } = 0;
        public DateTime StopDate { get; set; }

        public Gangwar() { }
    }
}
