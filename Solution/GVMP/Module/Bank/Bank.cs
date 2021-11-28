using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Bank
    {
        [JsonProperty("playername")]
        public string Playername { get; set; }

        [JsonProperty("playerId")]
        public string Bankaccount { get; set; }

        [JsonProperty("money")]
        public int Money { get; set; }

        [JsonProperty("balance")]
        public int Balance { get; set; }

        [JsonProperty("history")]
        public List<BankHistory> History { get; set; }

        public Bank() { }
    }
}
