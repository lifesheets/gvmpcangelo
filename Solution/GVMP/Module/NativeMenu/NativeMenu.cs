using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class NativeMenu
    {
        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public List<NativeItem> Items
        {
            get;
            set;
        }

        public NativeMenu(string Title, string Description, List<NativeItem> Items)
        {
            this.Title = Title;
            this.Description = Description;
            this.Items = Items;
        }
    }
}
