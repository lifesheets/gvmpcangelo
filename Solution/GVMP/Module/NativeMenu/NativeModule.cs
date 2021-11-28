using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class NativeModule : GVMP.Module.Module<NativeModule>
    {
        [RemoteEvent("m")]
        public static void nativeMenu(Client client, string id)
        {
            try
            {
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (string.IsNullOrEmpty(id)) return;

                if (id != "NaN")
                {
                    NativeMenu nativeMenu = (NativeMenu)dbPlayer.GetData("PLAYER_CURRENT_NATIVEMENU");
                    if (nativeMenu != null && nativeMenu.Items.Count >= Convert.ToInt32(id) && nativeMenu.Items[Convert.ToInt32(id)] != null)
                    {
                        client.Eval("mp.events.callRemote('nM-" + nativeMenu.Title + "', '" +
                                    nativeMenu.Items[Convert.ToInt32(id)].selectionName + "');");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION m] " + ex.Message);
                Logger.Print("[EXCEPTION m] " + ex.StackTrace);
            }
        }
    }
}
