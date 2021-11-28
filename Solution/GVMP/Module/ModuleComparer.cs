using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class ModuleComparer : IComparer<BaseModule>
    {
        public int Compare(BaseModule x, BaseModule y)
        {
            if (x == null && y != null)
                return -1;
            if (x != null && y == null)
                return 1;
            return x == null ? 0 : x.GetOrder().CompareTo(y.GetOrder());
        }
    }
}
