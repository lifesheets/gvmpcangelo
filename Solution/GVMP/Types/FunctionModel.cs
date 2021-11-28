using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class FunctionModel
    {
        public string Function
        {
            get;
            set;
        }

        public object Arg1
        {
            get;
            set;
        } = null;

        public object Arg2
        {
            get;
            set;
        } = null;

        public FunctionModel(string Function, object Arg1, object Arg2)
        {
            this.Function = Function;
            this.Arg1 = Arg1;
            this.Arg2 = Arg2;
        }

        public FunctionModel(string Function, object Arg1)
        {
            this.Function = Function;
            this.Arg1 = Arg1;
        }

        public FunctionModel(string Function)
        {
            this.Function = Function;
        }
    }
}
