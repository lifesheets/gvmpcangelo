using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class MySqlResult
    {
        public MySqlDataReader Reader { get; set; }

        public MySqlConnection Connection { get; set; }

        public MySqlResult(MySqlDataReader Reader, MySqlConnection Connection)
        {
            this.Reader = Reader;
            this.Connection = Connection;
        }
    }
}
