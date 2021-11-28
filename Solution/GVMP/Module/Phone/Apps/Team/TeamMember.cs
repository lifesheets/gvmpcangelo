using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class TeamMember
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty("rank")]
        public int Rank
        {
            get;
            set;
        }
        [JsonProperty("manage")]
        public int Manage
        {
            get;
            set;
        }
        [JsonProperty("number")]
        public int Number
        {
            get;
            set;
        }

        public TeamMember() { }
    }
}
