using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Contact
    {
        [JsonProperty("name")]
        public string Name
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

        public Contact() { }

    }
}
