using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class BarberPlayerObject
    {
        [JsonProperty(PropertyName = "hair")]
        public int Hair { get; set; }

        [JsonProperty(PropertyName = "hairColor")]
        public int HairColor { get; set; }

        [JsonProperty(PropertyName = "hairColor2")]
        public int HairColor2 { get; set; }

        [JsonProperty(PropertyName = "beard")]
        public int Beard { get; set; }

        [JsonProperty(PropertyName = "beardColor")]
        public int BeardColor { get; set; }

        [JsonProperty(PropertyName = "beardOpacity")]
        public float BeardOpacity { get; set; }

        [JsonProperty(PropertyName = "chestHair")]
        public int Chest { get; set; }

        [JsonProperty(PropertyName = "chestHairColor")]
        public int ChestHairColor { get; set; }

        [JsonProperty(PropertyName = "chestHairOpacity")]
        public float ChestHairOpacity { get; set; }

        public BarberPlayerObject() { }
    }
}
