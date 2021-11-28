using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Animation
    {
        [JsonProperty("slot")]
        public int Slot
        {
            get;
            set;
        }
        [JsonProperty("icon")]
        public string Icon
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
        [JsonProperty("select")]
        public string Select
        {
            get;
            set;
        }
        [JsonProperty("flag")]
        public int Flag
        {
            get;
            set;
        }

        public Animation() { }
    }
}
