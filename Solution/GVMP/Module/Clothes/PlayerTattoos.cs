using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    public class PlayerTattoos : GVMP.Module.Module<PlayerTattoos>
    {
        public tattooPart Tattoo
        {
            get;
            set;
        }

    }
}
