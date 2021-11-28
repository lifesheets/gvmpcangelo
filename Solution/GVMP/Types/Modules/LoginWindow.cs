using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class LoginWindow
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("auth")]
        public string Auth { get; set; }

        public LoginWindow() { }
    }
}
