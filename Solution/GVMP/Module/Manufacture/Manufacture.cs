using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Manufacture
    {
        public int Id { get; set; }
        public string WeaponName { get; set; }
        public WeaponHash Weapon { get; set; }
        public Vector3 Position { get; set; }
        public int RemoveCount { get; set; }
        public int Price { get; set; }

        public Manufacture() { }
    }
}
