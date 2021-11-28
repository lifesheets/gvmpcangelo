using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    class PlayerMenu : Script
    {
        [RemoteEvent("REQUEST_PEDS_PLAYER_GIVEITEM")]
        public static void REQUEST_PEDS_PLAYER_GIVEITEM(Client c, Client giveItem = null)
        {
            if (c == null) return;
            
            try
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;
                if (giveItem == null) return;

                DbPlayer dbPlayer2 = giveItem.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                c.SetData("PLAYER_GIVEITEM", giveItem);
                Inventory.requestItems(c);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_GIVEITEM] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_GIVEITEM] " + ex.StackTrace);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_GIVEMONEY_DIALOG")]
        public static void REQUEST_PEDS_PLAYER_GIVEMONEY_DIALOG(Client c, Client giveMoney = null)
        {
            if (c == null) return;
            
            try
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                if (giveMoney == null) return;

                DbPlayer dbPlayer2 = giveMoney.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                dbPlayer.OpenGiveMoney(dbPlayer2.Name);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_GIVEMONEY_DIALOG] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_GIVEMONEY_DIALOG] " + ex.StackTrace);
            }
        }

        [RemoteEvent("GivePlayerMoney")]
        public void GivePlayerMoney(Client c, string name, int money)
        {
            try
            {
                if (c == null) return;
                
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (money < 1)
                {
                    dbPlayer.SendNotification("hoff nicht");
                    return;
                }

                if (dbPlayer.DeathData.IsDead) return;

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }

                if (dbPlayer.Money >= money)
                {
                    dbPlayer.removeMoney(money);
                    dbPlayer2.addMoney(money);

                    WebhookSender.SendMessage("Spieler gibt Geld",
                        "Der Spieler " + dbPlayer.Name + " hat dem Spieler " + dbPlayer2.Name + " " + money +
                        "$ gegeben.", Webhooks.geldlogs, "Geld");

                    dbPlayer.SendNotification("Du hast dem Spieler erfolgreich Geld übergeben.", 3000, "green");
                    dbPlayer2.SendNotification($"Dir wurde von {dbPlayer.Name} {money.ToDots()}$ übergeben.", 3000,
                        "green");
                }
                else
                {
                    dbPlayer.SendNotification("Nicht genug Geld.", 3000, "red");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION GivePlayerMoney] " + ex.Message);
                Logger.Print("[EXCEPTION GivePlayerMoney] " + ex.StackTrace);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_TIE")]
        public static void REQUEST_PEDS_PLAYER_TIE(Client c, Client target = null)
        {
            try
            {
                if (c == null) return;
                if (target == null) return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.IsCuffed)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                DbPlayer dbPlayer2 = target.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                if (!dbPlayer2.HasData("PBZone")) return;
                if (dbPlayer2.GetData("PBZone") != null) return;
                if (dbPlayer2.DeathData.IsDead) return;


                if (!dbPlayer2.IsCuffed)
                {
                    if (dbPlayer.GetItemAmount("Fesseln") >= 0)
                    {
                            dbPlayer2.disableAllPlayerActions(true);
                            dbPlayer2.PlayAnimation(49, "mp_arresting", "idle");
                            dbPlayer2.IsCuffed = true;
                            dbPlayer2.IsFarming = true;
                            dbPlayer2.RefreshData(dbPlayer2);
                            dbPlayer.SendNotification("Du hast " + dbPlayer2.Name + " gefesselt.", 3000, "black");
                            dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Name + " gefesselt.", 3000, "black");
                            NAPI.Player.SetPlayerCurrentWeapon(dbPlayer2.Client, WeaponHash.Unarmed);
                        /*dbPlayer.UpdateInventoryItems("Fesseln", 1, true);*/
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du besitzt keine Fesseln!", 3000, "red");
                    }
                }
                else
                {
                    dbPlayer2.disableAllPlayerActions(true);
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.PlayAnimation(33, "mp_arresting", "a_uncuff");
                    dbPlayer2.PlayAnimation(33, "mp_arresting", "b_uncuff");
                    NAPI.Task.Run(() =>
                    {
                        dbPlayer2.TriggerEvent("updateCuffed", false);
                        dbPlayer2.IsCuffed = false;
                        dbPlayer2.IsFarming = false;
                        dbPlayer2.RefreshData(dbPlayer2);
                        dbPlayer.SendNotification("Du hast " + dbPlayer2.Name + " entfesselt.", 3000);
                        dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Name + " entfesselt.", 3000);
                        dbPlayer2.StopAnimation();
                        dbPlayer.StopAnimation();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer2.disableAllPlayerActions(false);
                    }, 4300);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_TIE] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_TIE] " + ex.StackTrace);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_FRISK")]
        public static void REQUEST_PEDS_PLAYER_FRISK(Client c, Client target = null)
        {
            try
            {
                if (c == null) return;
                if (target == null) return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.IsCuffed)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                DbPlayer dbPlayer2 = target.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                if (dbPlayer.IsFarming)
                {
                    dbPlayer.SendNotification("Du kannst aktuell niemanden durchsuchen!", 3000, "red");
                    return;
                }

                if (!dbPlayer2.IsCuffed && !dbPlayer2.DeathData.IsDead)
                {
                    dbPlayer2.SendNotification("Der Spieler ist nicht gefesselt.", 3000, "red");
                    return;
                }

                dbPlayer.IsFarming = true;
                dbPlayer.RefreshData(dbPlayer);
                dbPlayer.disableAllPlayerActions(true);
                dbPlayer.SendProgressbar(5000);
                dbPlayer.PlayAnimation(33, "mini@repair", "fixing_a_player", 8f);
                NAPI.Task.Run(() =>
                {
                    dbPlayer.IsFarming = false;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.SendNotification("Du hast den Spieler " + dbPlayer2.Name + " durchsucht.", 3000);
                    dbPlayer.StopAnimation();
                    dbPlayer.disableAllPlayerActions(false);
                    dbPlayer.TriggerEvent("openWindow", "Inventory",
                        "{\"inventory\":[{\"Id\":1,\"Name\":\"Inventar von Spieler\",\"Money\":1,\"Blackmoney\":0,\"PlayerInventory\":false,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" +
                        NAPI.Util.ToJson(dbPlayer2.GetInventoryItems()) + "}]}");
                }, 5000);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_FRISK] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_FRISK] " + ex.StackTrace);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_SHOW_PERSO")]
        public static void REQUEST_PEDS_PLAYER_SHOW_PERSO(Client c, Client target = null)
        {
            try
            {
                if (c == null) return;
                if (target == null) return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.IsCuffed)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                DbPlayer dbPlayer2 = target.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                string houseId = "Obdachlos";

                House house = HouseModule.houses.FirstOrDefault((House house2) =>
                    house2.OwnerId == dbPlayer.Id || house2.TenantsIds.Contains(dbPlayer.Id));
                if (house != null) houseId = house.Id.ToString();

                dbPlayer.SendNotification($"Du hast dem Spieler {dbPlayer2.Name} deinen Personalausweis gezeigt.");
                dbPlayer2.SendNotification($"Dir wurde der Personalausweis von {dbPlayer.Name} gezeigt.");
                dbPlayer2.ShowPersonalausweis(dbPlayer.Name.Split("_")[0], dbPlayer.Name.Split("_")[1],
                    dbPlayer.Faction.Name, houseId, dbPlayer.Level, dbPlayer.Id, 0, 0);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_SHOW_PERSO] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_SHOW_PERSO] " + ex.StackTrace);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_STABALIZE")]
        public void REQUEST_PEDS_PLAYER_STABALIZE(Client c, Client target = null)
        {
            try
            {
                if ((Entity)(object)c == (Entity)null || (Entity)(object)target == (Entity)null)
                {
                    return;
                }
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null || dbPlayer.IsCuffed || !dbPlayer.CanInteractAntiFlood(1) || dbPlayer.DeathData.IsDead || !dbPlayer.Medic)
                {
                    return;
                }
                if (dbPlayer.IsFarming)
                {
                    dbPlayer.SendNotification("Du kannst das aktuell nicht!");
                    return;
                }
                DbPlayer dbPlayer2 = target.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(ignorelogin: true) || !PlayerHandler.GetPlayers().Contains(dbPlayer2))
                {
                    return;
                }
                if (!dbPlayer2.DeathData.IsDead)
                {
                    dbPlayer.SendNotification("Dieser Spieler lebt!", 3000, "red");
                    return;
                }
                if (dbPlayer2.Faction.Id != dbPlayer.Faction.Id)
                {
                    dbPlayer.SendNotification("Du kannst nur deine Fraktion reviven!", 3000, "red");
                    return;
                }
                dbPlayer.AllActionsDisabled = true;
                dbPlayer.IsFarming = true;
                dbPlayer.RefreshData(dbPlayer);
                dbPlayer.PlayAnimation(33, "amb@medic@standing@tendtodead@idle_a", "idle_a");
                dbPlayer.SendNotification("Du fängst an einen Spieler wiederzubeleben...", 3000, "green");
                NAPI.Task.Run((Action)delegate
                {
                    if (NAPI.Pools.GetAllPlayers().Contains(dbPlayer.Client))
                    {
                        dbPlayer.AllActionsDisabled = false;
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.StopAnimation();
                        if (dbPlayer2 != null && dbPlayer2.IsValid(ignorelogin: true) && PlayerHandler.GetPlayers().Contains(dbPlayer2))
                        {
                            dbPlayer2.SpawnPlayer(dbPlayer2.Position);
                            dbPlayer2.AllActionsDisabled = false;
                            dbPlayer2.SetAttribute("Death", 0);
                            dbPlayer2.StopAnimation();
                            dbPlayer2.SetInvincible(val: false);
                            dbPlayer2.DeathData = new DeathData
                            {
                                IsDead = false,
                                DeathTime = new DateTime(0L)
                            };
                            dbPlayer2.RefreshData(dbPlayer2);
                            dbPlayer2.StopScreenEffect("DeathFailOut");
                            dbPlayer.SendNotification("Du hast den Spieler " + dbPlayer2.Name + " wiederbelebt!", 3000, "red", "medic");
                            dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Name + " wiederbelebt!", 3000, "red", "medic");
                           // WebhookSender.SendMessage("Spieler wird revived", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + dbPlayer2.Name + " revived.", Webhooks.playerrevivelogs, "Revive");
                        }
                    }
                }, 10000L);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_STABALIZE] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_STABALIZE] " + ex.StackTrace);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_GETPERSO")]
        public static void REQUEST_PEDS_PLAYER_GETPERSO(Client c, Client target = null)
        {
            try
            {
                if (c == null) return;
                if (target == null) return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.IsCuffed)
                    return;

                DbPlayer dbPlayer2 = target.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                string houseId = "Obdachlos";

                House house = HouseModule.houses.FirstOrDefault((House house2) =>
                    house2.OwnerId == dbPlayer2.Id || house2.TenantsIds.Contains(dbPlayer2.Id));
                if (house != null) houseId = house.Id.ToString();

                if (!dbPlayer2.IsCuffed && !dbPlayer2.DeathData.IsDead)
                {
                    dbPlayer2.SendNotification("Der Spieler ist nicht gefesselt.", 3000, "red");
                    return;
                }

                dbPlayer.SendNotification($"Du hast dem Spieler {dbPlayer2.Name} den Personalausweis gezogen.");
                dbPlayer2.SendNotification($"Dir wurde der Personalausweis von {dbPlayer.Name} gezogen.");
                dbPlayer.ShowPersonalausweis(dbPlayer2.Name.Split("_")[0], dbPlayer2.Name.Split("_")[1],
                    dbPlayer2.Faction.Name, houseId, dbPlayer2.Level, dbPlayer2.Id, 0, 0);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_GETPERSO] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_PEDS_PLAYER_GETPERSO] " + ex.StackTrace);
            }
        }
    }
}
