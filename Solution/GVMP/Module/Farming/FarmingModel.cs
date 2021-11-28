using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class FarmingModel
    {
        public int Id { get; set; }
        public string Item_1 { get; set; }
        public string Item_2 { get; set; }
        public Vector3 Sammler { get; set; }
        public int PickupCount { get; set; }
        public Vector3 Verarbeiter { get; set; }
        public int RemoveCount { get; set; }
        public int AddCount { get; set; }
        public float ColShapeRange { get; set; } = 5f;

        public FarmingModel() { }
    }
}
