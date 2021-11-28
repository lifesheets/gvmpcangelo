using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Wallpaper
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
        [JsonProperty("file")]
        public string File
        {
            get;
            set;
        }

        public Wallpaper(int id, string name, string file)
        {
            this.Id = id;
            this.Name = name;
            this.File = file;
        }
    }
}
