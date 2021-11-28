using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class EventModule : GVMP.Module.Module<EventModule>
    {
        public static List<string> equipItems = new List<string>();
        public static List<EventModel> equipList = new List<EventModel>();

        protected override bool OnLoad()
        {
            equipList.Add(new EventModel
            {
                Name = "",
                Position = new Vector3(-1914.36, 1388.72, 219.51)
            });


            foreach (EventModel eventModel in equipList)
            {
                ColShape cb = NAPI.ColShape.CreateCylinderColShape(eventModel.Position, 1.4f, 1.4f, 0);
                cb.SetData("FUNCTION_MODEL", new FunctionModel("useEvent"));
                cb.SetData("MESSAGE", new Message("Benutze E um dein Equip zu abzuholen", "EVENT", "orange", 3000));
                NAPI.Marker.CreateMarker(1, new Vector3(-1914.36, 1388.72, 219.51), new Vector3(), new Vector3(), 1.4f, new Color(0, 0, 0), false, 0);
                // NAPI.Marker.CreateMarker(1, equippointModel.position, new Vector3(), new Vector3(), 7.0f, new Color(255, 140, 0), false, 0);
            }

            return true;
        }

        [RemoteEvent("useEvent")]
        public static void Eventi(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    if (dbPlayer.Event)
                    {
                        dbPlayer.SendNotification("Du hast dir bereits dein Equip abgeholt!", 3000, "red", "EVENT");
                        return;
                    }
                    else
                    {
                    dbPlayer.UpdateInventoryItems("Geschenk", 1, false);
                        dbPlayer.Event = true;
                        dbPlayer.SetAttribute("Event", 1);
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.SendNotification("Du hast dir dein Equip abgeholt. Lass es dir nicht abziehen!" + dbPlayer.Event, 3000, "orange","EVENT");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION useEquippoint] " + ex.Message);
                    Logger.Print("[EXCEPTION useEquippoint] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION useEquippoint] " + ex.Message);
                Logger.Print("[EXCEPTION useEquippoint] " + ex.StackTrace);
            }
        }
    }
}
