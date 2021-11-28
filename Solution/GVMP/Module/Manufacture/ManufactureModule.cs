using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    class ManufactureModule : GVMP.Module.Module<ManufactureModule>
    {
        public static List<Manufacture> weaponManufactures = new List<Manufacture>();

        protected override bool OnLoad()
        {
            weaponManufactures.Add(new Manufacture
            {
                Id = 1,
                WeaponName = "Advancedrifle",
                Weapon = WeaponHash.AdvancedRifle,
                Position = new Vector3(1094.31, -2003.37, 29.97),
                Price = 40000,
                RemoveCount = 30
            });

            weaponManufactures.Add(new Manufacture
            {
                Id = 2,
                WeaponName = "Assaultrifle",
                Weapon = WeaponHash.AssaultRifle,
                Position = new Vector3(1103.55, -2000.89, 28.65),
                Price = 20000,
                RemoveCount = 15
            });

            weaponManufactures.Add(new Manufacture
            {
                Id = 3,
                WeaponName = "Gusenberg",
                Weapon = WeaponHash.Gusenberg,
                Position = new Vector3(1108.45, -2007.66, 29.9),
                Price = 40000,
                RemoveCount = 30
            });

            foreach (Manufacture weaponManufacture in weaponManufactures)
            {
                ColShape c = NAPI.ColShape.CreateCylinderColShape(weaponManufacture.Position, 20.0f, 2.4f, 0);
                c.SetData("FUNCTION_MODEL", new FunctionModel("openManufacturing", weaponManufacture.Id));
                c.SetData("MESSAGE", new Message("Benutze E um Waffen herzustellen.", "Waffenherstellung", "green", 3000));

                NAPI.Blip.CreateBlip(156, weaponManufacture.Position, 1f, 0, "Waffenherstellung " + weaponManufacture.WeaponName, 255, 0, true, 0, 0);
            }

            return true;
        }

        [RemoteEvent("openManufacturing")]
        public void openManufacturing(Client c, int Id)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                Manufacture weaponManufacture =
                    weaponManufactures.FirstOrDefault((Manufacture weaponManufacture2) => weaponManufacture2.Id == Id);
                if (weaponManufacture == null) return;

                List<NativeItem> nativeItems = new List<NativeItem>();
                nativeItems.Add(new NativeItem(
                    weaponManufacture.WeaponName + " - " + weaponManufacture.Price.ToDots() + "$ - " +
                    weaponManufacture.RemoveCount + " Waffenteile", Id.ToString()));
                NativeMenu nativeMenu = new NativeMenu("Waffenherstellung", "", nativeItems);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openManufacturing] " + ex.Message);
                Logger.Print("[EXCEPTION openManufacturing] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Waffenherstellung")]
        public void Waffenherstellung(Client c, string selection)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                int Id = 0;
                bool Id2 = int.TryParse(selection, out Id);
                if (!Id2) return;
                if (Id == 0) return;

                Manufacture weaponManufacture = weaponManufactures.FirstOrDefault((Manufacture weaponManufacture2) => weaponManufacture2.Id == Id);
                if (weaponManufacture == null) return;

                if (dbPlayer.GetItemAmount("Waffenteile") < weaponManufacture.RemoveCount)
                {
                    dbPlayer.SendNotification("Du besitzt zu wenig Waffenteile! Benötigt: " + weaponManufacture.RemoveCount, 3000, "red");
                    return;
                }

                if (dbPlayer.Money < weaponManufacture.Price)
                {
                    dbPlayer.SendNotification("Du besitzt zu wenig Geld! Benötigt: " + weaponManufacture.Price, 3000, "red");
                    return;
                }

                dbPlayer.CloseNativeMenu();

                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.removeMoney(weaponManufacture.Price);
                    dbPlayer.UpdateInventoryItems("Waffenteile", weaponManufacture.RemoveCount, true);
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.SendProgressbar(30000);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        if (c.Dimension != 0)
                        {
                            dbPlayer.SendNotification("Du kannst gerade keine Waffen herstellen.", 3000, "red");
                            return;
                        }
                        else
                        {
                            dbPlayer.GiveWeapon(weaponManufacture.Weapon, 9999, true);
                            dbPlayer.IsFarming = false;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.StopAnimation();
                            dbPlayer.StopProgressbar();
                            dbPlayer.disableAllPlayerActions(false);
                        }
                    }, 30000);
                }
            }
            catch(Exception ex)
            {
                Logger.Print("[EXCEPTION Waffenherstellung] " + ex.Message);
                Logger.Print("[EXCEPTION Waffenherstellung] " + ex.StackTrace);
            }
        }
    }
}
