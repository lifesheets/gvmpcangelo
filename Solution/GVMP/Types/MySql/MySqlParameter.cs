using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class MySqlParameter
    {
        public string Name { get; set; }

        public object Obj { get; set; }

        public MySqlParameter(string Name, object Obj)
        {
            this.Name = Name;
            this.Obj = Obj;
        }
    }
}
