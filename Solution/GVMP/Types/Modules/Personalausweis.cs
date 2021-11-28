using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class Personalausweis
    {
        public static void ShowPersonalausweis(this DbPlayer dbPlayer, string firstName, string lastName, string birthday, string address, int level, int id, int casino, int govLevel)
        {
            dbPlayer.TriggerEvent("showPerso", firstName, lastName, birthday, address, level, id, casino, govLevel);
        }
    }
}
