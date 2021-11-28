using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP.Module
{
    public abstract class Module<T> : BaseModule where T : GVMP.Module.Module<T>
    {
        public static T Instance { get; private set; }

        protected Module() => GVMP.Module.Module<T>.Instance = (T)this;
    }
}
