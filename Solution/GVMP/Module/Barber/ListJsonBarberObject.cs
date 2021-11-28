using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class ListJsonBarberObject
    {
        [JsonProperty(PropertyName = "hairs")]
        public List<JsonBarberObject> Hairs { get; set; }

        [JsonProperty(PropertyName = "beards")]
        public List<JsonBarberObject> Beards { get; set; }

        [JsonProperty(PropertyName = "chests")]
        public List<JsonBarberObject> Chests { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public List<JsonBarberObject> Colors { get; set; }

        public ListJsonBarberObject() { }
    }
}
