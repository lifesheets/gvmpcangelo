using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class ComputerWindow
    {
        public static void OpenComputer(this DbPlayer dbPlayer)
        {
            dbPlayer.TriggerEvent("openComputer");
        }

        public static void CloseComputer(this DbPlayer dbPlayer)
        {
            dbPlayer.TriggerEvent("closeComputer");
        }

        public static void responseComputerApps(this DbPlayer dbPlayer, string args)
        {
            dbPlayer.TriggerEvent("componentServerEvent", "DesktopApp", "responseComputerApps", args);
        }

        public static void responseTenants(this DbPlayer dbPlayer, string args)
        {
            dbPlayer.TriggerEvent("componentServerEvent", "HouseList", "responseTenants", args);
        }
    }
}
