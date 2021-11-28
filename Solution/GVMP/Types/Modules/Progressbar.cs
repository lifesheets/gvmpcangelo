using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class Progressbar
    {
        public static void SendProgressbar(this DbPlayer dbPlayer, int duration)
        {
            Client c = dbPlayer.Client;

            c.TriggerEvent("sendProgressbar", duration);
        }

        public static void StopProgressbar(this DbPlayer dbPlayer)
        {
            Client c = dbPlayer.Client;

            c.TriggerEvent("componentServerEvent", "Progressbar", "StopProgressbar");
        }
    }
}
