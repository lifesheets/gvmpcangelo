using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Ban
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Reason { get; set; }
        public string Account { get; set; }

        public Ban() { }
    }
}
