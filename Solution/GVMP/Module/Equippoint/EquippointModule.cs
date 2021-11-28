using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class EquippointModule : GVMP.Module.Module<EquippointModule>
    {
        public static List<string> equipItems = new List<string>();
        public static List<int> alreadyEquipped = new List<int>();
        public static List<EquippointModel> equipList = new List<EquippointModel>();

        protected override bool OnLoad()
        {
            equipItems.Add("Advancedrifle");
            equipItems.Add("Gusenberg");
            equipItems.Add("Assaultrifle");
            equipItems.Add("Compactrifle");
            equipItems.Add("Compactrifle");
            equipItems.Add("Compactrifle");

            equipList.Add(new EquippointModel
            {
                Name = "Würfelpark",
                Position = new Vector3(186.21584, -914.0723, 24.040178)
            });


            foreach (EquippointModel equippointModel in equipList)
            {
                ColShape cb = NAPI.ColShape.CreateCylinderColShape(equippointModel.Position, 7.4f, 2.4f, 0);
                cb.SetData("FUNCTION_MODEL", new FunctionModel("useEquippoint"));
                cb.SetData("MESSAGE", new Message("Benutze E um dich auszurüsten.", "Equippoint", "blue", 3000));

               // NAPI.Marker.CreateMarker(1, equippointModel.position, new Vector3(), new Vector3(), 7.0f, new Color(255, 140, 0), false, 0);
                NAPI.Blip.CreateBlip(313, equippointModel.Position, 1f, 0, "Equippoint " + equippointModel.Name, 255, 0.0f, true, 0, 0);


            }

            return true;
        }

        [RemoteEvent("useEquippoint")]
        public static void equipPlayer(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    if (alreadyEquipped.Contains(dbPlayer.Id))
                    {
                        dbPlayer.SendNotification("Du hast dein Equip für den jetzigen Restart bereits abgeholt.", 3000,
                            "blue", "Equippoint");
                        return;
                    }

                    alreadyEquipped.Add(dbPlayer.Id);
                    dbPlayer.SendProgressbar(5000);
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(() =>
                    {
                        var r = new Random();
                        string item = equipItems[r.Next(0, equipItems.Count)];
                        dbPlayer.UpdateInventoryItems("HeavyPistol", 1, false);
                        dbPlayer.UpdateInventoryItems("Schutzweste", 5, false);
                        dbPlayer.UpdateInventoryItems(item, 1, false);
                        dbPlayer.StopProgressbar();
                        dbPlayer.SendNotification(
                            "Du hast dein Equip für den jetzigen Restart abgeholt. (+ 1x " + item + ")", 3000, "blue",
                            "Equippoint");
                        dbPlayer.StopAnimation();
                        dbPlayer.disableAllPlayerActions(false);
                    }, 5000);
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
