using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class NativeItem
    {
        public string Label
        {
            get;
            set;
        }

        public string selectionName
        {
            get;
            set;
        }

        public NativeItem(string Label, string selectionName)
        {
            this.Label = Label;
            this.selectionName = selectionName;
        }
    }
}
