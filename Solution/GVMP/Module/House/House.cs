using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class House
    {
        public int Id { get; set; }
        public Vector3 Entrance { get; set; }
        public int OwnerId { get; set; } = 0;
        public List<int> TenantsIds { get; set; } = new List<int>();
        public int Price { get; set; }
        public HouseClass Class { get; set; }
        public bool Locked { get; set; }
        public bool SeeTel { get; set; }
        public bool KellerBuilt { get; set; }
        public List<HouseClothes> Wardrobe { get; set; } = new List<HouseClothes>();
        public List<ItemModel> Inventory { get; set; } = new List<ItemModel>();
        public Dictionary<int, int> TenantPrices { get; set; } = new Dictionary<int, int>();
        public string PlayerName { get; set; }

        public House() { }
    }
}
