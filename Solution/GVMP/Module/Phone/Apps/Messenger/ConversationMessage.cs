using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class ConversationMessage
    {
        public int Id { get; }
        public int KonversationId { get; }
        public string Message { get; }
        public DateTime TimeStamp { get; }
        public int SenderId { get; }

    }
}
