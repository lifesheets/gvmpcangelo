using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class TextInputBoxObject
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string Callback { get; set; }

        public string CloseCallback { get; set; }

        public TextInputBoxObject() { }
    }
}
