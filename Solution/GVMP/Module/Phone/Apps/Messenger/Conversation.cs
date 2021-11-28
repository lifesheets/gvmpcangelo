using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Conversation
    {
        public int Id { get; }
        public int Player1 { get; }
        public int Player2 { get; }
        public DateTime LastUpdated { get; }

        public Conversation(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            Player1 = reader.GetInt32("Player_1");
            Player2 = reader.GetInt32("Player_2");
            LastUpdated = reader.GetDateTime("Last_Updated");
        }
    }
}
