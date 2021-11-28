using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class TuningCategory
    {
        public string Label
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public TuningCategory(string Label, string Name)
        {
            this.Label = Label;
            this.Name = Name;
        }
    }
}
