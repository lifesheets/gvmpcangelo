using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class NativeMenuDb
    {
        public static void ShowNativeMenu(this DbPlayer dbPlayer, NativeMenu nativeMenu)
        {
            Client client = dbPlayer.Client;
            client.SetData("PLAYER_CURRENT_NATIVEMENU", nativeMenu);
            client.TriggerEvent("componentServerEvent", "NativeMenu", "showNativeMenu", NAPI.Util.ToJson(nativeMenu), 0);
        }

        public static void CloseNativeMenu(this DbPlayer dbPlayer)
        {
            dbPlayer.Client.TriggerEvent("componentServerEvent", "NativeMenu", "hide");
        }
    }
}
