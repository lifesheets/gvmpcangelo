using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Tenant
    {
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("player_id")]
        public int Player_Id { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }

        public Tenant() { }
    }
}
