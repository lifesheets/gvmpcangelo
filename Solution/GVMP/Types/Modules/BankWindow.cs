using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class BankWindow
    {
        public static void OpenBank(this DbPlayer dbPlayer, Bank bank)
        {
            dbPlayer.TriggerEvent("openWindow", "Bank", NAPI.Util.ToJson(bank));
        }
    }
}
