using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Shop
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("customBlip")]
        public int Blip { get; set; } = 0;
        [JsonProperty("customBlipColor")]
        public int BlipColor { get; set; }
        [JsonProperty("position")]
        public Vector3 Position { get; set; }
        [JsonProperty("items")]
        public List<BuyItem> Items { get; set; }

        public Shop() { }
    }
}
