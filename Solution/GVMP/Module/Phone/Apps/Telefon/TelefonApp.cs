using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    class TelefonApp : GVMP.Module.Module<TelefonApp>
    {
        public static List<PhoneCall> calls = new List<PhoneCall>();

        [RemoteEvent("callUserPhone")]
        public void CallUser(Client c, int number)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (number == dbPlayer.Id) return;

                if (SettingsApp.isFlugmodus(dbPlayer.Client))
                {
                    dbPlayer.SendNotification("Du hast aktuell kein Netz!", 3000, "red");
                    return;
                }

                DbPlayer dbTarget = PlayerHandler.GetPlayer(number);
                if (dbTarget == null)
                {
                    dbPlayer.SendNotification("Die angegebene Nummer ist derzeit nicht erreichbar.", 3000, "red");
                    return;
                }

                if (SettingsApp.isFlugmodus(dbTarget.Client))
                {
                    dbPlayer.SendNotification("Die angegebene Nummer ist derzeit nicht erreichbar.", 3000, "red");
                    return;
                }

                if (FindCall(dbTarget) != null)
                {
                    dbPlayer.SendNotification("Die angegebene Nummer ist derzeit besetzt.", 3000, "red");
                    return;
                }

                PhoneCall call = new PhoneCall
                {
                    Id = calls.Count,
                    Player1 = dbPlayer,
                    Player2 = dbTarget
                };
                calls.Add(call);
                c.TriggerEvent("setPhoneCallData",
                    "{\"name\":\"" + MessengerApp.getContactName(c, dbTarget.Id) + "\",\"method\":\"outcomming\"}");
                dbTarget.TriggerEvent("setPhoneCallData",
                    "{\"method\":\"incoming\",\"name\":\"" + MessengerApp.getContactName(dbTarget.Client, dbPlayer.Id) +
                    "\"}");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION callUserPhone] " + ex.Message);
                Logger.Print("[EXCEPTION callUserPhone] " + ex.StackTrace);
            }
        }
        
        [RemoteEvent("callDeclined")]
        public void CallDeclined(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                var call = FindCall(dbPlayer);
                if (call == null) return;
                call.Player1.Client.TriggerEvent("cancelCall");
                call.Player2.Client.TriggerEvent("cancelCall");
                call.Player1.SetSharedData("IN_CALL", "none");
                call.Player2.SetSharedData("IN_CALL", "none");
                calls.Remove(call);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION callDeclined] " + ex.Message);
                Logger.Print("[EXCEPTION callDeclined] " + ex.StackTrace);
            }
        }
        
        [RemoteEvent("callAccepted")]
        public void CallAccepted(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                var call = FindCall(dbPlayer);
                if (call == null)
                    return;
                
                call.Player1.Client.TriggerEvent("componentServerEvent", "CallManageApp", "acceptCall");
                call.Player2.Client.TriggerEvent("componentServerEvent", "CallManageApp", "acceptCall");
                int funk = new Random().Next(10000, 99999999);
                call.Player1.SetSharedData("IN_CALL", call.Player2.Name);
                call.Player2.SetSharedData("IN_CALL", call.Player1.Name);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION callAccepted] " + ex.Message);
                Logger.Print("[EXCEPTION callAccepted] " + ex.StackTrace);
            }
        }

        public static PhoneCall FindCall(DbPlayer dbPlayer)
        {
            PhoneCall phoneCall = calls.FirstOrDefault((PhoneCall call) => call.Player1.Id == dbPlayer.Id || call.Player2.Id == dbPlayer.Id);
            return phoneCall;
        }
    }
}
