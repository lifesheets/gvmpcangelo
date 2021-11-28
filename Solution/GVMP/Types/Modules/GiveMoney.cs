using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class GiveMoney
    {
        public static void OpenGiveMoney(this DbPlayer dbPlayer, string name)
        {
            object giveMoneyObject = new
            {
                name = name
            };

            dbPlayer.TriggerEvent("openWindow", nameof(GiveMoney), NAPI.Util.ToJson(giveMoneyObject));
        }
    }
}
