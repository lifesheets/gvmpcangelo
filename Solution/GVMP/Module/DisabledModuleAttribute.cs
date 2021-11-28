using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisabledModuleAttribute : Attribute
    {
    }
}
