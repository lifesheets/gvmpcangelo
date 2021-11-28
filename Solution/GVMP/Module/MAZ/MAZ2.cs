using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using GVMP;
using MySql.Data.MySqlClient;

namespace GVMP
{
    public class MAZ2 : GVMP.Module.Module<MAZ2>
    {
        public static List<MAZ2> clothingList = new List<MAZ2>();
        public static List<MAZ2> BlockedZones = new List<MAZ2>();
        private static MAZ2 awd;

        public static object mySqlReaderCon { get; private set; }

        protected override bool OnLoad()
        {
            Vector3 Position = new Vector3(-1209.67, 1419.16, 169.98);

            ColShape val = NAPI.ColShape.CreateCylinderColShape(Position, 1.4f, 2.4f, 0);
            val.SetData("FUNCTION_MODEL", new FunctionModel("openMAZ2"));
            val.SetData("MESSAGE", new Message("Drücke E um MAZ aufzubrechen!", "", "red", 3000));

            NAPI.Marker.CreateMarker(1, Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 0, 0), false, 0);

         ;
         ;

            return true;
        }

        [RemoteEvent("openMAZ2")]
        public static void openMAZ2(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;
                Laboratory result = null;
                float distance = 99999;




                if (!dbPlayer.IsFarming)
                {

                    if (BlockedZones.Contains(awd))
                    {
                        dbPlayer.SendNotification("Absturz wurde bereits gemacht.", 3000, "Red", "Absturz");
                        return;
                    }

                    dbPlayer.disableAllPlayerActions(true);
                        dbPlayer.SendProgressbar(150000);
                        dbPlayer.IsFarming = true;
                        Notification.SendGlobalNotification("Zentrales Flugabwehrsystem: Das Flugzeug wird aufgebrochen (Yakuza Berg)!", 10000, "white", Notification.icon.bullhorn);
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.SendNotification("Du brichst das MAZ auf!");
                    WebhookSender.SendMessage("MAZ", "Der Spieler  " + dbPlayer.Name + " Schweisst das MAZ auff  ", Webhooks.mazlogs, "MAZ");

                    dbPlayer.PlayAnimation(33, "amb@world_human_welding@male@idle_a", "idle_a", 8f);
                        NAPI.Task.Run(delegate
                        {
                            dbPlayer.TriggerEvent("client:respawning");
                            dbPlayer.StopProgressbar();
                            dbPlayer.addMoney(new Random().Next(200000, 550000));
                            dbPlayer.UpdateInventoryItems("Gusenberg", 15, false);
                            dbPlayer.UpdateInventoryItems("Advancedrifle", 12, false);
                            dbPlayer.IsFarming = false;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.disableAllPlayerActions(false);
                            dbPlayer.StopAnimation();
                            BlockedZones.Add(awd);
                        }, 150000);
                    }
            }
            catch (Exception ex)
                    {
                        Logger.Print("[EXCEPTION openMAZ2] " + ex.Message);
                        Logger.Print("[EXCEPTION openMAZ2] " + ex.StackTrace);
            }
        }
    }
}
