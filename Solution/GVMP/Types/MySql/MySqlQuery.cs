using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class MySqlQuery
    {
        public string Query { get; set; }

        public List<MySqlParameter> Parameters { get; set; } = new List<MySqlParameter>();

        public void AddParameter(string val, object obj)
        {
            this.Parameters.Add(new MySqlParameter(val, obj));
        }

        public MySqlQuery(string Query)
        {
            this.Query = Query;
            this.Parameters.Clear();
        }
    }
}
