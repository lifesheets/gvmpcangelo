using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class ConfirmationObject
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string Callback { get; set; }

        public string Arg1 { get; set; }
        public string Arg2 { get; set; }

        public ConfirmationObject() { }
    }
}
