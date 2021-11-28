using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Ticket
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("created_at")]
        public DateTime Created { get; set; }

        public Ticket() { }
    }
}
