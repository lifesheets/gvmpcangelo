using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class BarberObject
    {
        [JsonProperty(PropertyName = "barber")]
        public ListJsonBarberObject Barber { get; set; }

        [JsonProperty(PropertyName = "player")]
        public BarberPlayerObject Player { get; set; }

        public BarberObject() { }
    }
}
