using GTANetworkAPI;
using GVMP.Handlers;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class CommandModule : GVMP.Module.Module<CommandModule>
    {

        public static List<Faction> factionList = new List<Faction>();
        public static List<Command> commandList = new List<Command>();
        public static List<ClothingModel> clothingList = new List<ClothingModel>();

        protected override bool OnLoad()
        {
            commandList.Add(new Command((dbPlayer, args) =>
            {
                try
                {
                    Client client = dbPlayer.Client;

                    if (!dbPlayer.HasData("PLAYER_ADUTY"))
                    {
                        dbPlayer.SetData("PLAYER_ADUTY", false);
                    }

                    dbPlayer.ACWait();

                    WebhookSender.SendMessage("Spieler wechselt Aduty", "Der Spieler " + dbPlayer.Name + " hat den Adminmodus " + (dbPlayer.GetData("PLAYER_ADUTY") ? "betreten" : "verlassen") + ".", Webhooks.adminlogs, "Admin");

                    client.TriggerEvent("setPlayerAduty", !dbPlayer.GetData("PLAYER_ADUTY"));
                    client.TriggerEvent("updateAduty", !dbPlayer.GetData("PLAYER_ADUTY"));
                    dbPlayer.SetData("PLAYER_ADUTY", !dbPlayer.GetData("PLAYER_ADUTY"));
                    dbPlayer.SpawnPlayer(new Vector3(client.Position.X, client.Position.Y, client.Position.Z + 0.52f));
                    if (dbPlayer.GetData("PLAYER_ADUTY"))
                    {
                        dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", true);
                        dbPlayer.SendNotification("Du hast den Admin-Dienst betreten.", 3000, "red", "ADMIN");
                        Adminrank adminrank = dbPlayer.Adminrank;
                        int num = (int)adminrank.ClothingId;
                        dbPlayer.SetClothes(3, 9, 0);
                        PlayerClothes.setAdmin(dbPlayer, num);
                        dbPlayer.SetClothes(5, 0, 0);
                        return;
                    }
                    else
                    {
                        dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", false);
                        dbPlayer.SendNotification("Du hast den Admin-Dienst verlassen.", 3000, "red", "ADMIN");
                    }
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @user LIMIT 1");
                    mySqlQuery.AddParameter("@user", client.Name);

                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PlayerClothes playerClothes = NAPI.Util.FromJson<PlayerClothes>(reader.GetString("Clothes"));

                                    //  dbPlayer.SetClothes(2, playerClothes.Haare.drawable, playerClothes.Haare.texture);
                                    client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                                    client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                                    dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                                    dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                                    dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                                    dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                                    dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                                    dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                                }
                            }
                        }
                        finally
                        {
                            reader.Dispose();
                        }
                    }
                    finally
                    {
                        mySqlReaderCon.Connection.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION ADUTY] " + ex.Message);
                    Logger.Print("[EXCEPTION ADUTY] " + ex.StackTrace);
                }
            }, "aduty", 1, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                NativeMenu nativeMenu = new NativeMenu("Adminmenu", dbPlayer.Faction.Name, new List<NativeItem>()
                        {
                            new NativeItem("Aduty", "aduty"),
                            new NativeItem("Vanish", "vanish"),
                            new NativeItem("Revive Self", "revivemenu"),
                            new NativeItem("Der rest soon..", "soon"),
                            new NativeItem("Kick Player", "kickplayer"),
                            new NativeItem("Ban Player", "banplayer"),
                            new NativeItem("Spieler Bringen", "bringmenu"),
                            new NativeItem("Zu Spieler TPn", "gotomenu"),
                        });
                //nativeMenu.Items.Add(new NativeItem("Waffenaufsätze", "weaponcomponents"));
                dbPlayer.ShowNativeMenu(nativeMenu);


            }, "dutymenu", 94, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Spieler: " + NAPI.Pools.GetAllPlayers().Count + " Insgesamt, Eingeloggt: " + PlayerHandler.GetPlayers().Count);
            }, "onlist", 0, 0));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    Item item = ItemModule.itemRegisterList.FirstOrDefault((Item x) => x.Name == args[1]);
                    if (item == null) return;

                    dbPlayer.UpdateInventoryItems(item.Name, Convert.ToInt32(args[2]), false);
                    dbPlayer.SendNotification("Du hast dir das Item " + item.Name + " gegeben.", 3000, "green", "Inventar");
                    WebhookSender.SendMessage("additem", " Der Admin " + dbPlayer.Name + " hat sich das item  " + item.Name + " " + args[2] + " mal gegeben ", Webhooks.additemlogs, "Additem Logs");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION ADDITEM] " + ex.Message);
                    Logger.Print("[EXCEPTION ADDITEM] " + ex.StackTrace);
                }
            }, "additem", 100, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                try
                {
                    if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    {
                        dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                        return;
                    }
                    Item item = ItemModule.itemRegisterList.FirstOrDefault((Item x) => x.Name == "CaillouCard");
                    if (item == null) return;

                    dbPlayer2.UpdateInventoryItems(item.Name, Convert.ToInt32(1), false);
                    dbPlayer.SendNotification("Der Spieler " + name + " kann nun in das Casino", 3000, "lightblue", "CASINO");
                    dbPlayer2.SendNotification("Du Kannst in das Casino!", 3000, "lightblue", "CASINO");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CASINO] " + ex.Message);
                    Logger.Print("[EXCEPTION CASINO] " + ex.StackTrace);
                }
            }, "casino", 97, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Notification.SendGlobalNotification("Der Server startet nun automatisch neu.", 8000, "red", Notification.icon.warn);
                Process.Start("C:\\Users\\Administrator\\Desktop\\GVMPc\\start.bat");
                Environment.Exit(0);

            }, "restart", 98, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Notification.SendGlobalNotification("discord.gg/gvmpc", 8000, "red", Notification.icon.bullhorn);
            }, "discord", 92, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Notification.SendGlobalNotification("Teamspeak GVMPc Crimelife", 8000, "red", Notification.icon.bullhorn);
            }, "teamspeak", 92, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Notification.SendGlobalNotification("Das Purge Event beginnt nun.", 8000, "red", Notification.icon.bullhorn);
                NAPI.World.SetWeather(Weather.THUNDER);
                NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", true));
                NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("sound:playPurge"));
                NAPI.Task.Run(delegate
                {
                    NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", false));
                }, 31800L);

            }, "purgeon", 96, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Notification.SendGlobalNotification("Das Purge Event endet nun.", 8000, "red", Notification.icon.bullhorn);
                NAPI.World.SetWeather(Weather.EXTRASUNNY);
                NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", false));
            }, "purgeoff", 96, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string ID1 = args[1];
                Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(ID1));
                string ID2 = args[2];
                Faction fraktion2 = FactionModule.getFactionById(Convert.ToInt32(ID2));
                Notification.SendGlobalNotification("Die Fraktion " + fraktion.Name + " hat den Kriegsvertrag gegen die Fraktion " + fraktion2.Name + " unterschrieben!", 8000, "orange", Notification.icon.bullhorn);
                NAPI.World.SetWeather(Weather.THUNDER);
                NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", true));
                NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("sound:playPurge"));
                NAPI.Task.Run(delegate
                {
                    NAPI.World.SetWeather(Weather.EXTRASUNNY);
                    NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", false));
                }, 31800L);
            }, "krieg", 97, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                Adminrank adminrank = dbPlayer.Adminrank;
                Adminrank adminranks = dbPlayer2.Adminrank;

                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                    if (adminrank.Permission <= adminranks.Permission)
                    {
                        dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                        return;
                    }
                    else
                    {
                        Client client = dbPlayer2.Client;
                        client.TriggerEvent("openWindow", new object[2]
{
                                   "Kick",
                                    "{\"name\":\""+ dbPlayer2.Name +"\",\"grund\":\"" + String.Join(" ", args).Replace("kick " + name, "") +"\"}"
});
                        dbPlayer2.Client.Kick();
                        dbPlayer.SendNotification("Spieler gekickt!", 3000, "red");
                        Notification.SendGlobalNotification("" + dbPlayer2.Name + " wurde von " + dbPlayer.Name + " gekickt. Grund: " + String.Join(" ", args).Replace("kick " + name, ""), 8000, "red", Notification.icon.warn);
                        // String.Join(" ", args).Replace("announce ", "")
                    }
            }, "kick", 1, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.DeathData.IsDead) return;

                Client client = dbPlayer.Client;

                try
                {
                    if (client.GetData("PBZone") != null || client.Dimension == 1 || client.Dimension == 2 || client.CurrentWeapon == WeaponHash.Unarmed || dbPlayer.IsFarming || (client.CurrentWeapon != WeaponHash.Unarmed && dbPlayer.Loadout.FirstOrDefault(w => w.Weapon == client.CurrentWeapon) == null))
                    {
                        dbPlayer.SendNotification("Du kannst gerade keine Waffen einpacken.", 3000, "red");
                        return;
                    }
                    if (!dbPlayer.IsFarming)
                    {
                        WeaponHash weapon = client.CurrentWeapon;

                        Item item = ItemModule.itemRegisterList.FirstOrDefault((Item x) => x.Whash == weapon);
                        if (item == null)
                        {
                            dbPlayer.BanPlayer();
                            return;
                        }

                        NAPI.Player.SetPlayerCurrentWeapon(client, WeaponHash.Unarmed);
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.RemoveWeapon(weapon, true);
                        dbPlayer.UpdateInventoryItems(item.Name, 1, false);
                        dbPlayer.SendNotification("Du hast deine aktuelle Waffe in dein Inventar verstaut!!", 3000, "green");
                        return;
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION UN] " + ex.Message);
                    Logger.Print("[EXCEPTION PACKGUN] " + ex.StackTrace);
                }
            }, "pkpackweaponadmink3", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Das Waffen packen ist nun im Inventar!", 3000, "black");
            }, "packgun", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {

                if (dbPlayer.DeathData.IsDead) return;

                Client client = dbPlayer.Client;

                if ((dbPlayer.Faction.Id == 0 ? false : dbPlayer.Factionrank > 9))
                {
                    List<BuyCar> list = new List<BuyCar>();
                    List<string> list1 = new List<string>();
                    FactionModule.VehicleList[dbPlayer.Faction.Id].ForEach((GarageVehicle garageVehicle) =>
                    {
                        list1.Add(garageVehicle.Name);
                    });

                    if (!list1.Contains("jugular"))
                    {
                        list.Add(new BuyCar("jugular", 1));
                    }
                    if (!list1.Contains("drafter"))
                    {
                        list.Add(new BuyCar("drafter", 1));
                    }
                    if (!list1.Contains("revolter"))
                    {
                        list.Add(new BuyCar("revolter", 1));
                    }
                    if (!list1.Contains("bf400"))
                    {
                        list.Add(new BuyCar("bf400", 1));
                    }
                    if (!list1.Contains("schafterg"))
                    {
                        list.Add(new BuyCar("schafterg", 1));
                    }
                    if (!list1.Contains("schafter6"))
                    {
                        list.Add(new BuyCar("schafter6", 1));
                    }
                    if (!list1.Contains("supervolito2"))
                    {
                        list.Add(new BuyCar("supervolito2", 1));
                    }
                    if (!list1.Contains("s63amg"))
                    {
                        list.Add(new BuyCar("s63amg", 30000000));
                    }
                    List<NativeItem> list2 = new List<NativeItem>();
                    foreach (BuyCar buyCar in list)
                    {
                        list2.Add(new NativeItem(string.Concat(buyCar.Vehicle_Name, " - ", buyCar.Price.ToDots(), "$"), string.Concat(buyCar.Vehicle_Name.ToLower(), "-", buyCar.Price.ToString())));
                    }
                    dbPlayer.ShowNativeMenu(new NativeMenu("Leadershop", dbPlayer.Faction.Short, list2));
                }
            }, "leadershop", 0, 0));









            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    dbPlayer.SendNotification("X: " + Math.Round(client.Position.X, 2) + " Y: " + Math.Round(client.Position.Y, 2) + " Z: " + Math.Round(client.Position.Z, 2) + " Heading: " + Math.Round(client.Heading, 2), 60000, "green", "");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "pos", 0, 0));

            commandList.Add(new Command((dbPlayer, args) => FactionBank.OpenFactionBank(dbPlayer), "frakbank", 0, 0));

            commandList.Add(new Command((dbPlayer, args) => BusinessBank.OpenBusinessBank(dbPlayer), "businessbank", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;
                Notification.SendGlobalNotification("Es wurden Administrativ alle Fahrzeuge eingeparkt.", 5000, "white", Notification.icon.bullhorn);
                NAPI.Pools.GetAllVehicles().ForEach((Vehicle veh) => veh.Delete());
                MySqlHandler.ExecuteSync(new MySqlQuery("UPDATE vehicles SET Parked = 1"));
            }, "parkall", 97, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;
                dbPlayer.SendNotification("Fahrzeuge: " + NAPI.Pools.GetAllVehicles().Count, 10000, "red", "ADMIN");
            }, "cars", 97, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;
                Notification.SendGlobalNotification(String.Join(" ", args).Replace("announce ", ""), 10000, "white", Notification.icon.bullhorn);
            }, "announce", 95, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;
                Notification.SendGlobalNotification("Das Casino hat nun Geöffnet, Tickets könnt ihr vorort kaufen!", 10000, "lightblue", Notification.icon.diamond);
            }, "casinoa", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;
                Notification.SendGlobalNotification("Das Casino schließt nun, vielen dank für ihr Besuch!", 10000, "lightblue", Notification.icon.diamond);
            }, "casinoc", 99, 0));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string text = args[1];
                Client client = dbPlayer.Client;
                Notification.SendGlobalNotification(String.Join("", "Die Fraktion " + text + " hat eine Offene Bewerbungsphase! (15 Minuten Safezone)").Replace("bwp ", ""), 10000, "white", Notification.icon.bullhorn);
            }, "bwp", 94, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;
                DbVehicle dbVehicle = client.Vehicle.GetVehicle();
                if (!client.IsInVehicle) return;
                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                {
                    dbPlayer.SendNotification("Fahrzeug gelöscht!", 3000, "red", "ADMIN");
                    client.Vehicle.Delete();
                }
                else
                {
                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET Parked = 1 WHERE Id = @id");
                    mySqlQuery.AddParameter("@id", dbVehicle.Id);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fahrzeug eingeparkt!", 3000, "red", "ADMIN");
                    client.Vehicle.Delete();
                }
            }, "dv", 1, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;
                float dist = 0;
                bool dist2 = float.TryParse(args[1], out dist);

                if (!dist2) return;

                foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                {
                    if (vehicle.Position.DistanceTo(dbPlayer.GetPosition()) <= dist)
                    {
                        vehicle.Delete();
                    }
                }
                dbPlayer.SendNotification("Fahrzeuge gelöscht!", 3000, "red", "ADMIN");
            }, "dvradius", 1, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    float x = 0;
                    float y = 0;
                    float z = 0;

                    bool x2 = float.TryParse(args[1], out x);
                    bool y2 = float.TryParse(args[2], out y);
                    bool z2 = float.TryParse(args[3], out z);
                    if (!x2 || !y2 || !z2) return;
                    dbPlayer.SetPosition(new Vector3(x, y, z));
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "tp", 1, 3));

            /*commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    dbPlayer.SetPosition();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "tpm", 1, 3));*/

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    string name = args[1];

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null) return;
                    dbPlayer.SetPosition(dbPlayer2.Client.Position);
                    dbPlayer.SendNotification("Du hast dich zu Spieler " + name + " teleportiert.");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION GOTO] " + ex.Message);
                    Logger.Print("[EXCEPTION GOTO] " + ex.StackTrace);
                }
            }, "goto", 1, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    string name = args[1];

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null) return;
                    dbPlayer2.SetPosition(new Vector3(112.21, -1075.84, 29.19));
                    dbPlayer.SendNotification("Du hast den Spieler " + name + " zu der Würfelpark Garage Teleportiert");
                    dbPlayer2.SendNotification("Der Administrator " + dbPlayer.Name + " hat dich zu der Würfelpark Garage Teleportiert");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION GOTO] " + ex.Message);
                    Logger.Print("[EXCEPTION GOTO] " + ex.StackTrace);
                }
            }, "tpgarage", 92, 1));



            /*  commandList.Add(new Command((dbPlayer, args) =>
              {
                  Client client = dbPlayer.Client;

                  try
                  {
                      string name = args[1];

                      DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                      if (dbPlayer2 == null) return;
                      dbPlayer2.SetPosition(dbPlayer.Client.Position);
                      dbPlayer.SendNotification("Du hast den Spieler " + name + " zu dir teleportiert.");
                      dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Adminrank.Name + " " + dbPlayer.Name + " teleportiert.");
                  }
                  catch (Exception ex)
                  {
                      Logger.Print("[EXCEPTION BRING] " + ex.Message);
                      Logger.Print("[EXCEPTION BRING] " + ex.StackTrace);
                  }
              }, "bring", 1, 1));*/

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    dbPlayer.SendNotification("Du hast den Adminshop refreshed.");
                    AdminShopModule.clothingList = ClothingManager.getClothingDataListAdmin();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.Message);
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.StackTrace);
                }
            }, "refreshadmin", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    dbPlayer.SendNotification("Du hast den DonatorShop refreshed.");
                    DonatorShopModule.clothingList = ClothingManager.getClothingDataListAdmin();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.Message);
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.StackTrace);
                }
            }, "refreshshop", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    string name = args[1];
                    int num = 0;
                    bool num2 = int.TryParse(args[2], out num);

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    {
                        dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                        return;
                    }
                    dbPlayer.SendNotification("Spieler " + name + " auf Dimension " + num + " gewechselt");
                    dbPlayer2.SendNotification("Deine Dimension wurde geändert (" + num + ")");
                    dbPlayer2.Dimension = num;
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION BRING] " + ex.Message);
                    Logger.Print("[EXCEPTION BRING] " + ex.StackTrace);
                }
            }, "dimension", 93, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    string name = args[1];

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    {
                        dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                        return;
                    }
                    dbPlayer2.SetPosition(dbPlayer.Client.Position);
                    dbPlayer.SendNotification("Du hast den Spieler " + name + " zu dir teleportiert.");
                    dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Adminrank.Name + " " + dbPlayer.Name + " teleportiert.");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION BRING] " + ex.Message);
                    Logger.Print("[EXCEPTION BRING] " + ex.StackTrace);
                }
            }, "bring", 1, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    if (!client.HasSharedData("PLAYER_INVISIBLE"))
                        return;

                    bool invisible = client.GetSharedData("PLAYER_INVISIBLE");
                    dbPlayer.SendNotification("Du hast dich " + (!invisible ? "unsichtbar" : "sichtbar") + " gemacht.", 3000, "red", "ADMIN");
                    client.SetSharedData("PLAYER_INVISIBLE", !invisible);

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION VANISH] " + ex.Message);
                    Logger.Print("[EXCEPTION VANISH] " + ex.StackTrace);
                }
            }, "v", 1, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    string car = args[1];
                    Vehicle vehicle = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(car), client.Position, 0.0f, 0, 0, "", 255, false, true, client.Dimension);
                    client.SetIntoVehicle(vehicle, -1);
                    vehicle.CustomPrimaryColor = dbPlayer.Adminrank.RGB;
                    vehicle.CustomSecondaryColor = dbPlayer.Adminrank.RGB;
                    vehicle.NumberPlate = ("ACL-" + dbPlayer.Adminrank.Permission);
                    dbPlayer.SendNotification("Du hast das Fahrzeug " + car + " erfolgreich gespawnt.", 3000, "red", "ADMINISTRATION");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION VEH] " + ex.Message);
                    Logger.Print("[EXCEPTION VEH] " + ex.StackTrace);
                }
            }, "veh", 94, 1));


            commandList.Add(new Command(delegate (DbPlayer dbPlayer, string[] args)
            {
                if (dbPlayer.HasData("IN_HOUSE"))
                {
                    int num5 = dbPlayer.GetData("IN_HOUSE");
                    if (num5 != 0)
                    {
                        House houseById = HouseModule.getHouseById(num5);
                        if (houseById.OwnerId != dbPlayer.Id && houseById.TenantsIds.Contains(dbPlayer.Id))
                        {
                            dbPlayer.Position = houseById.Entrance;
                            HouseModule.houses.Remove(houseById);
                            houseById.TenantsIds.Remove(dbPlayer.Id);
                            if (houseById.TenantPrices.ContainsKey(dbPlayer.Id))
                            {
                                houseById.TenantPrices.Remove(dbPlayer.Id);
                            }
                            HouseModule.houses.Add(houseById);
                            MySqlQuery mySqlQuery5 = new MySqlQuery("UPDATE houses SET TenantsId = @tenantsid, TenantPrices = @tenantprices WHERE Id = @id");
                            mySqlQuery5.AddParameter("@tenantsid", NAPI.Util.ToJson((object)houseById.TenantsIds));
                            mySqlQuery5.AddParameter("@tenantprices", NAPI.Util.ToJson((object)houseById.TenantPrices));
                            mySqlQuery5.AddParameter("@id", houseById.Id);
                            MySqlHandler.ExecuteSync(mySqlQuery5);
                            dbPlayer.SendNotification("Du hast den Mietvertrag verlassen!", 3000, "red");
                        }
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du befindest dich nicht in einem Haus!");
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Du befindest dich nicht in einem Haus!");
                }
            }, "cancelrental", 0, 0));
            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                if (args[1] == "cancel")
                {
                    TabletModule.Tickets.RemoveAll((Ticket ticket) => ticket.Creator == dbPlayer.Name);
                    dbPlayer.SendNotification("Du hast alle deine Tickets geschlossen.", 3000, "red");
                    return;
                }

                if (TabletModule.Tickets.Count >= 99)
                {
                    dbPlayer.SendNotification("Es sind bereits zu viele Tickets offen.", 3000, "yellow", "SUPPORT");
                    return;
                }


                if (TabletModule.Tickets.FirstOrDefault((Ticket ticket2) => ticket2.Creator == dbPlayer.Name) != null)
                {
                    dbPlayer.SendNotification("Du hast bereits ein offenes Ticket!", 3000, "yellow", "Support");
                    return;
                }
                if (String.Join(" ", args).Replace("support ", "").Length > 100)
                {
                    dbPlayer.SendNotification("Grund zu lang!", 3000, "yellow", "SUPPORT");
                    return;
                }

                var ticket = new Ticket
                {
                    Id = (int)new Random().Next(10000, 99999),
                    Created = DateTime.Now,
                    Creator = dbPlayer.Name,
                    Text = String.Join(" ", args).Replace("support ", "")
                };

                dbPlayer.SendNotification("Deine Support-Anfrage ist bei uns eingegangen, es wird sich bald ein Teammitglied um dich Kümmern!", 6000, "red", "SUPPORT");

                PlayerHandler.GetAdminPlayers().ForEach((DbPlayer dbPlayer2) =>
                {
                    if (dbPlayer2.HasData("disablenc")) return;
                    if (TabletModule.Tickets.Count >= 15)
                    {
                        dbPlayer2.SendNotification("Tickets machen sonst fick ich euch. LG!", 6000, "red", "ADMINDIENSTPFLICHT");
                        if (dbPlayer2.HasData("PLAYER_ADUTY") && dbPlayer2.GetData("PLAYER_ADUTY") == true)
                            dbPlayer2.SendNotification(dbPlayer.Name + ": " + String.Join(" ", args).Replace("support ", "") + "", 3000, "yellow", "NEUES TICKET");
                    }
                    else
                    {
                        if (dbPlayer2.HasData("PLAYER_ADUTY") && dbPlayer2.GetData("PLAYER_ADUTY") == true)
                            dbPlayer2.SendNotification(dbPlayer.Name + ": " + String.Join(" ", args).Replace("support ", "") + "", 3000, "yellow", "NEUES TICKET");
                    }
                });
                TabletModule.Tickets.Add(ticket);
            }, "support", 0, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.HasData("IN_HOUSE"))
                {
                    int houseId = dbPlayer.GetData("IN_HOUSE");
                    if (houseId != 0)
                    {
                        House house = HouseModule.getHouseById(houseId);
                        if (house.OwnerId != dbPlayer.Id)
                        {
                            dbPlayer.SendNotification("Das ist nicht dein Haus!", 3000, "red");
                            return;
                        }
                        dbPlayer.Position = house.Entrance;

                        HouseModule.houses.Remove(house);
                        house.OwnerId = 0;
                        dbPlayer.Dimension = 0;
                        HouseModule.houses.Add(house);
                        dbPlayer.addMoney(house.Price / 2);
                        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE houses SET OwnerId = @ownerid WHERE Id = @id");
                        mySqlQuery.AddParameter("@ownerid", 0);
                        mySqlQuery.AddParameter("@id", house.Id);
                        MySqlHandler.ExecuteSync(mySqlQuery);
                        dbPlayer.SendNotification("Du hast dein Haus verkauft! (" + house.Price / 2 + ")", 3000, "red", "");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du befindest dich nicht in einem Haus!");
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Du befindest dich nicht in einem Haus!");
                }
            }, "sellhouse", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (String.Join(" ", args).ToLower().Contains("aspectisteinhs"))
                {
                    dbPlayer.BanPlayer();
                }

                if (String.Join(" ", args).ToLower().Contains("kianistgeil"))
                {
                    dbPlayer.BanPlayer();
                }

                NAPI.ClientEvent.TriggerClientEventInRange(dbPlayer.Client.Position, 100.0f, "sendPlayerNotification", String.Join(" ", args).Replace("ooc ", ""), 3500, "darkgreen", "OOC - (" + dbPlayer.Name + ")", "");
            }, "ooc", 0, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                PlayerHandler.GetAdminPlayers().ForEach((DbPlayer dbPlayer2) =>
                {
                    if (dbPlayer2.HasData("disablenc")) return;

                    Adminrank adminranks = dbPlayer2.Adminrank;

                    if (adminranks.Permission >= 91)
                        dbPlayer2.SendNotification(String.Join(" ", args).Replace("tc", ""), 6000, "red", "TEAMCHAT - (" + dbPlayer.Name + ")");
                });
            }, "tc", 91, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.HasData("disablenc"))
                {
                    dbPlayer.SendNotification("Team Chat aktiviert", 3000, "green");
                    dbPlayer.SendNotification("Reports aktiviert", 3000, "green");
                    dbPlayer.ResetData("disablenc");
                }
                else
                {
                    dbPlayer.SendNotification("Team Chat deaktivert", 3000, "red");
                    dbPlayer.SendNotification("Reports deaktivert", 3000, "red");
                    dbPlayer.SetData("disablenc", true);
                }
            }, "notify", 94, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                Adminrank adminrank = dbPlayer.Adminrank;
                Adminrank adminranks = dbPlayer2.Adminrank;

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                    if (adminrank.Permission <= adminranks.Permission)
                    {
                        dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                        return;
                    }
                    else
                    {
                        BanModule.BanIdentifier(name, String.Join(" ", args).Replace("xcm " + name + " ", ""), name);

                        MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @username");
                        mySqlQuery.AddParameter("@username", name);
                        MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                        MySqlDataReader reader = mySqlResult.Reader;

                        if (reader.HasRows)
                        {
                            reader.Read();
                            BanModule.BanIdentifier(reader.GetString("Social"), String.Join(" ", args).Replace("xcm " + name + " ", ""), name);
                            BanModule.BanIdentifier(dbPlayer2.Client.Serial, String.Join("", args).Replace("xcm " + name + " ", ""), dbPlayer2.Name);
                            BanModule.BanIdentifier(dbPlayer2.Client.Address, String.Join("", args).Replace("xcm " + name + " ", ""), dbPlayer2.Name);
                        }

                        reader.Dispose();
                        mySqlResult.Connection.Dispose();

                        BanModule.BanIdentifier(name, String.Join(" ", args).Replace("xcm " + name + " ", ""), name);
                        dbPlayer.SendNotification("Spieler gebannt!", 3000, "red");
                        dbPlayer2.TriggerEvent("openWindow", new object[2]
{
                                    "Bann",
                                    "{\"name\":\"" + dbPlayer2.Name + "\"}"
});
                        dbPlayer2.Client.Kick();
                        Notification.SendGlobalNotification($"Der Spieler " + name + " wurde von " + dbPlayer.Name + " gebannt.", 8000, "red", Notification.icon.warn);
                        WebhookSender.SendMessage("Spieler wird gebannt", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + name + " offlinegebannt. Grund: " + String.Join(" ", args).Replace("xcm " + name + " ", ""), Webhooks.banlogs, "Ban");
                    }
                else
                {
                    Client client = dbPlayer2.Client;
                    dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Name, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                    dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Name, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                    dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Client.Serial, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                    dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Client.Address, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                    dbPlayer.SendNotification("Spieler gebannt!", 3000, "red");
                    client.TriggerEvent("openWindow", new object[2]
{
                                    "Bann",
                                    "{\"name\":\"" + dbPlayer2.Name + "\"}"
});
                    dbPlayer2.Client.Kick();
                    Notification.SendGlobalNotification($"Der Spieler " + name + " wurde von " + dbPlayer.Name + " gebannt. Grund: " + String.Join(" ", args).Replace("xcm " + name + " ", ""), 8000, "red", Notification.icon.warn);
                    WebhookSender.SendMessage("Spieler wird gebannt", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + name + " gebannt. Grund: " + String.Join(" ", args).Replace("xcm " + name + " ", ""), Webhooks.banlogs, "Ban");
                }
            }, "xcm", 94, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string frak = args[2];
                string rang = args[3];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;

                }
                if (Int64.Parse(rang) > 13)
                {
                    dbPlayer.SendNotification("Du kannst nur Ränge bis 13 vergeben!", 7000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Client client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(frak));
                    dbPlayer2.SendNotification("Deine Fraktion wurde Administrativ geändert! (" + fraktion.Name + ")", 3000, "red");
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " die Fraktion " + fraktion.Name + " und den Rang " + rang + " gesetzt!", 3000, "red");

                    dbPlayer2.SetAttribute("Fraktion", frak);
                    dbPlayer2.SetAttribute("Fraktionrank", rang);

                    dbPlayer2.TriggerEvent("updateTeamId", frak);
                    dbPlayer2.TriggerEvent("updateTeamRank", rang);
                    dbPlayer2.TriggerEvent("updateJob", fraktion.Name);
                    dbPlayer2.Faction = fraktion;
                    dbPlayer2.Factionrank = 0;
                    dbPlayer2.RefreshData(dbPlayer2);
                }
            }, "setfrak", 95, 3));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string fahrzeug = args[2];
                string nummernschild = args[3];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    int id = new Random().Next(10000, 99999999);
                    Adminrank adminrank = dbPlayer.Adminrank;
                    dbPlayer2.SendNotification("Dir wurde das Fahrzeug " + fahrzeug + " mit dem Kennzeichen " + nummernschild + " gesetzt!", 3000, "red", "ADMINISTRATION");

                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " das Fahrzeug " + fahrzeug + " mit dem Kennzeichen " + nummernschild + " gesetzt!", 3000, "red");
                    List<int> list = new List<int>();
                    list.Add(dbPlayer2.Id);
                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO `vehicles` (`Id`, `Vehiclehash`, `Parked`, `OwnerId`, `Carkeys`, `Plate`) VALUES (@id, @vehiclehash, @parked, @ownerid, @carkeys, @plate)");
                    mySqlQuery.AddParameter("@vehiclehash", fahrzeug);
                    mySqlQuery.AddParameter("@parked", 1);
                    mySqlQuery.AddParameter("@ownerid", dbPlayer2.Id);
                    mySqlQuery.AddParameter("@carkeys", NAPI.Util.ToJson(list));
                    mySqlQuery.AddParameter("@plate", nummernschild);
                    mySqlQuery.AddParameter("@id", id);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                }
            }, "givecar", 97, 3));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string reason = "XCM";

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    int id = new Random().Next(10000, 99999999);
                    Client client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;

                    dbPlayer.SendNotification("Du hast den Spieler " + name + " Offline Gebannt!", 3000, "red");
                    List<int> list = new List<int>();
                    list.Add(dbPlayer2.Id);
                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO `bans` (`Id`, `Identifier`, `Reason`, `Account`) VALUES (@id, @identifier, @reason, @account)");
                    mySqlQuery.AddParameter("@identifier", name);
                    mySqlQuery.AddParameter("@reason", reason);
                    mySqlQuery.AddParameter("@account", name);
                    mySqlQuery.AddParameter("@id", id);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                }
            }, "offxcm", 95, 2));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string kat = args[2];
                string com = args[3];
                string draw = args[4];
                string tex = args[5];


                int id = new Random().Next(10000, 99999999);
                dbPlayer.SendNotification("Hinzugefügt!", 3000, "red");
                MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO `adminclothes` (`Name`, `Category`, `Component`, `Drawable`, `Texture`, `Id`) VALUES (@name, @category, @component, @drawable, @texture, @id)");
                mySqlQuery.AddParameter("@name", name);
                mySqlQuery.AddParameter("@category", kat);
                mySqlQuery.AddParameter("@component", com);
                mySqlQuery.AddParameter("@drawable", draw);
                mySqlQuery.AddParameter("@texture", tex);
                mySqlQuery.AddParameter("@id", id);
                MySqlHandler.ExecuteSync(mySqlQuery);
            }, "addcloth", 96, 5));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string frakid = args[1];
                {
                    Adminrank adminrank = dbPlayer.Adminrank;
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(frakid));
                    dbPlayer.SendNotification("Die ID " + frakid + " gehört zu der Fraktion: " + fraktion.Name + "", 5000, "red");
                }
            }, "frakinfo", 92, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    if ((int)dbPlayer2.GetAttributeInt("warns") == 0)
                    {
                        dbPlayer2.SendNotification("Das geht nicht, da der Spieler keine Warns hat!", 6000, "red", "Administration");
                        return;
                    }
                    else
                        dbPlayer2.SetAttribute("warns", (int)dbPlayer2.GetAttributeInt("warns") - 1);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler " + name + " einen Warn entfernt!", 3000, "red");
                    dbPlayer2.SendNotification("Dir wurde ein Warn entfernt!", 6000, "red", "VERWARNUNG");
                }
            }, "delwarn", 91, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Client client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;



                    dbPlayer2.disableAllPlayerActions(true);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler gefrezzed", 8000, "red", "ADMINISTRATION");
                }
            }, "freeze", 92, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Client client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;


                    dbPlayer2.disableAllPlayerActions(false);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler ungefrezzed", 8000, "red", "ADMINISTRATION");
                }
            }, "unfreeze", 92, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                else
                {
                    dbPlayer2.PlayAnimation(33, "combat@damage@rb_writhe", "rb_writhe_loop", 8f);
                }


            }, "trolldeath", 92, 1));




            commandList.Add(new Command((dbPlayer, args) =>
            {
                int fraktion1 = FactionModule.factionList.Count;

                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion1));
                    if (fraktion.Name == "Zivilist")
                    {
                        return;
                    }
                    if (fraktion.Name != "Zivilist")
                    {
                        dbPlayer.SendNotification("Fraktionen: " + FactionModule.factionList.Count, 5000, "red");

                    }
                }
            }, "fraks", 91, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string rang = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;

                    Adminrank adminrank = dbPlayer.Adminrank;
                    Adminrank adminranks = dbPlayer2.Adminrank;
                    if (adminrank.Permission <= adminranks.Permission)
                    {
                        dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                        return;
                    }
                    else
                        dbPlayer2.SetAttribute("Adminrank", rang);
                    dbPlayer2.RefreshData(dbPlayer2);
                    adminranks = dbPlayer2.Adminrank;
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Rang " + adminranks.Name + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Team Rang wurde verändert (" + adminranks.Name + ")", 3000, "red");
                }
            }, "setperm", 98, 2));


            commandList.Add(new Command(delegate (DbPlayer dbPlayer, string[] args)
            {
                Client client2 = dbPlayer.Client;
                StringBuilder stringBuilder = new StringBuilder();
                try
                {
                    {
                        string name = args[1];
                        DbPlayer player2 = PlayerHandler.GetPlayer(name);
                        if (player2 == null || !player2.IsValid(ignorelogin: true))
                        {
                            dbPlayer.SendNotification("Spieler nicht gefunden.");
                        }
                        else
                        {
                            player2.RefreshData(player2);
                            player2.SetAttribute("Donator", 1);
                            dbPlayer.SendNotification("Donator gesetzt!");
                        }
                    }
                }
                catch (Exception ex2)
                {
                    Logger.Print("[EXCEPTION setmedic] " + ex2.Message);
                    Logger.Print("[EXCEPTION setmedic] " + ex2.StackTrace);
                }
            }, "setdonator", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string telefonnrm = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET OwnerId = @neued WHERE OwnerId = @username");
                    mySqlQuery.AddParameter("@username", name);
                    mySqlQuery.AddParameter("@neued", telefonnrm);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                    dbPlayer2.SetAttribute("Id", telefonnrm);

                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " die Telefonnummer " + telefonnrm + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Deine Telefonnummer wurde geändert! (" + telefonnrm + ")", 3000, "red");
                }
            }, "changenumber", 99, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string name2 = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("Username", name2);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Namen " + name2 + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Name wurde geändert! (" + name2 + ")", 3000, "red");
                }
            }, "rename", 96, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string name2 = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("Social", name2);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Social Name auf " + name2 + " gesetzt!", 3000, "red");
                }
            }, "changesocial", 96, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                if (name == null) return;
                string daysold = args[2];
                if (daysold == null) return;

                if (int.TryParse(daysold, out var days))
                {
                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null) return;
                    Adminrank adminrank = dbPlayer.Adminrank;
                    if (adminrank == null) return;
                    Adminrank adminranks = dbPlayer2.Adminrank;
                    if (adminranks == null) return;

                    if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                        if (adminrank.Permission <= adminranks.Permission)
                        {
                            dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                            return;
                        }
                        else
                        {
                            BanExternal.TimeBanPlayer(dbPlayer2, days, dbPlayer.Name);
                            dbPlayer.SendNotification("Spieler gebannt!", 3000, "red");
                        }
                }
            }, "timeban", 94, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];//

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.RemoveAllWeapons();
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " das Waffenrad gelöscht!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Waffenrad wurde gelöscht! ", 3000, "red", "ADMINISTRATION");
                }
            }, "clearload", 96, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Char neugeladen!", 3000, "red");
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer2.SpawnPlayer(dbPlayer.Client.Position);
                    dbPlayer2.disableAllPlayerActions(false);
                    dbPlayer2.SetAttribute("Death", 0);
                    dbPlayer2.StopAnimation();
                    dbPlayer2.SetInvincible(false);
                    dbPlayer2.DeathData = new DeathData
                    {
                        IsDead = false,
                        DeathTime = new DateTime(0)
                    };
                    dbPlayer2.StopScreenEffect("DeathFailOut");
                    dbPlayer2.SendNotification("Dein Char wurde neugeladen! ", 3000, "red", "ADMINISTRATION");
                }
            }, "reloadchar", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string str = args[1];
                if (str != "reset")
                {
                    dbPlayer.Name = str;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.Client.Name = (dbPlayer.Name);
                    dbPlayer.SendNotification("Fakename gesetzt! (" + str + ")", 3000, "gray", "");
                }
                else
                {
                    string altername = dbPlayer.GetAttributeString("Username");
                    dbPlayer.GetAttributeString("Username");
                    dbPlayer.Client.Name = (altername);
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.SendNotification("Fakename zurückgesetzt! (" + altername + ")", 3000, "red", "");
                }
            }, "fakename", 100, 1));

            commandList.Add(new Command((client, dbPlayer, args) =>
            {
                string name = args[1];

                if (client.HasData("OLD_CHAR"))
                {
                    dbPlayer.RefreshData(client.GetData("OLD_CHAR"));
                    dbPlayer.SendNotification("Du hast nun deinen alten Char", 3000, "red");
                    dbPlayer.ResetData("OLD_CHAR");
                }

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Du hast nun den Char von " + name, 3000, "red");
                    dbPlayer.Client.SetData("OLD_CHAR", dbPlayer);
                    dbPlayer.RefreshData(dbPlayer2);
                }
            }, "troll", 100, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string Id = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(Id);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM inventorys WHERE Id = @id");
                    mySqlQuery.AddParameter("@id", Id);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                    dbPlayer2.RefreshData(dbPlayer2);

                    dbPlayer.SendNotification("Du hast dem Spieler " + Id + " das Inventar zurückgesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Inventar wurde zurückgesetzt!! ", 3000, "red", "ADMINISTRATION");
                }
            }, "clearinv", 98, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string reason = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("warns", (int)dbPlayer.GetAttributeInt("warns") + 1);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler " + name + " verwarnt!", 3000, "red");
                    dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Name + "verwarnt! Grund: " + reason + "", 6000, "red", "VERWARNUNG");
                }
            }, "warn", 91, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Client client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;

                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.SendNotification("Warns von " + name + ": " + dbPlayer2.warns + "", 10000, "red");
                    dbPlayer.SendNotification("Social Name von " + name + ": " + client.SocialClubName + "", 10000, "red");
                    dbPlayer.SendNotification("Fraktion von " + name + ": " + dbPlayer2.Faction.Name + "", 10000, "red");
                    dbPlayer.SendNotification("Fraktion - Rang von " + name + ": " + dbPlayer2.Factionrank + "", 10000, "red");
                    dbPlayer.SendNotification("Business von " + name + ": " + dbPlayer2.Business.Name + "", 10000, "red");
                    dbPlayer.SendNotification("Geld von " + name + ": " + dbPlayer2.Money + "", 10000, "red");
                    dbPlayer.SendNotification("Level von " + name + ": " + dbPlayer2.Level + "", 10000, "red");
                    dbPlayer.SendNotification("ID von " + name + ": " + dbPlayer2.Id + "", 10000, "red");
                    dbPlayer.SendNotification("Online Seit: " + dbPlayer2.OnlineSince + "", 10000, "red");

                }
            }, "info", 91, 1));





            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string price = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.addMoney(Convert.ToInt32(price));
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " Geld gegeben! (" + price + ")", 5000, "red");
                    dbPlayer2.SendNotification("Dir wurde von " + dbPlayer.Name + " Geld gegeben (" + price + ")", 6000, "red", "ADMINISTRATION");
                    WebhookSender.SendMessage("addmoney", " Der Admin " + dbPlayer.Name + " hat dem Spieler " + dbPlayer2.Name + "  " + price + " gegeben ", Webhooks.addmoneylogs, "Addmoney Logs");
                }
            }, "addmoney", 100, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            //  NAPI.Pools.GetAllPlayers().ForEach(player => player.Client.addMoney(Convert.ToInt32(price)));
            //NAPI.Pools.GetAllPlayers().ForEach((Client client) =>
            {
                string price = args[1];
                dbPlayer.SendNotification("Du hast allen Spielern " + price + "$ gegeben! (" + NAPI.Pools.GetAllPlayers().Count + ")", 5000, "red");
                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {

                    DbPlayer dbPlayer2 = target.GetPlayer(); dbPlayer2.addMoney(Convert.ToInt32(price));
                    dbPlayer2.SendNotification("Von " + dbPlayer.Name + " +" + price + "$", 6000, "red", "ENTSCHÄDIGUNG/EVENT");




                }
            }, "eventmoney", 100, 1));



            commandList.Add(new Command((dbPlayer, args) =>

            {

                dbPlayer.SendNotification("Du hast alle Spieler zu dir Teleportiert ", 5000, "red");
                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {

                    DbPlayer dbPlayer2 = target.GetPlayer(); dbPlayer2.SetPosition(dbPlayer.Client.Position);
                    dbPlayer2.SendNotification("Alle Spieler wurden von " + dbPlayer.Name + " Administrativ Teleportiert ", 6000, "red", "Teleport");
                    Notification.SendGlobalNotification("Alle Spieler Wurden Administrativ von " + dbPlayer.Name + " Teleportiert ", 10000, "white", Notification.icon.warn);
                    WebhookSender.SendMessage("TPALL", " Der Admin " + dbPlayer.Name + " hat alle Spieler zu sich Teleportiert    ", Webhooks.tpalllogs, "Tpall Logs");






                }
            }, "tpall", 100, 1));

            commandList.Add(new Command((dbPlayer, args) =>

            {

                dbPlayer.SendNotification("Du hast alle Spieler Revived ", 5000, "red");
                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {

                    DbPlayer dbPlayer2 = target.GetPlayer(); dbPlayer2.SpawnPlayer(dbPlayer2.Client.Position);


                    dbPlayer2.SendNotification("Du Wurdest von " + dbPlayer.Name + " Administrativ Revived ", 6000, "red", "Revive");
                    WebhookSender.SendMessage("REVIVEALL", " Der Admin " + dbPlayer.Name + " hat alle Spieler Revived    ", Webhooks.revivealllogs, "Reviveall Logs");

                    dbPlayer2.SpawnPlayer(dbPlayer2.Client.Position);
                    Notification.SendGlobalNotification("Alle Spieler Wurden Administrativ von " + dbPlayer.Name + " Revived ", 10000, "white", Notification.icon.warn);
                    dbPlayer2.disableAllPlayerActions(false);
                    dbPlayer2.SetAttribute("Death", 0);
                    dbPlayer2.StopAnimation();
                    dbPlayer2.SetInvincible(false);
                    dbPlayer2.DeathData = new DeathData
                    {
                        IsDead = false,
                        DeathTime = new DateTime(0)
                    };
                    dbPlayer2.StopScreenEffect("DeathFailOut");






                }
            }, "reviveall", 100, 1));



            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string price = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.removeMoney(Convert.ToInt32(price));
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " Geld entfernt! (" + price + ")", 5000, "red");
                    dbPlayer2.SendNotification("Dir wurde von " + dbPlayer.Name + " Geld entfernt (" + price + ")", 6000, "red", "ADMINISTRATION");
                }
            }, "removemoney", 98, 2));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string level = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Client client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("Level", level);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " das Level " + level + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Level wurde geändert! (" + level + ")", 3000, "red");
                }
            }, "changelevel", 97, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string nachricht = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Client client = dbPlayer2.Client;
                    dbPlayer2.SendNotification(String.Join(" ", args).Replace("apn " + name, ""), 3000, "red", "ADMIN-PN - (" + dbPlayer.Name + ")");
                    dbPlayer.SendNotification("Privat Nachricht an " + name + " gesendet!", 3000, "red");

                }
            }, "apn", 91, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {


                    var random = new Random();
                    int id = random.Next(10000, 99999);

                    string factionid = args[1];

                    string model = args[2];

                    int primary = 0;
                    bool primary2 = int.TryParse(args[3], out primary);

                    int secondary = 0;
                    bool secondary2 = int.TryParse(args[4], out secondary);

                    int headlight = 0;
                    bool headlight2 = int.TryParse(args[5], out headlight);



                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(factionid));
                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO fraktion_vehicles (Id, FactionId, Model, PrimaryColor, SecondaryColor, HeadlightColor) VALUES (@id, @factionid, @model, @primary, @secondary, @headlight)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@factionid", factionid);
                    mySqlQuery.AddParameter("@model", model);
                    mySqlQuery.AddParameter("@primary", primary);
                    mySqlQuery.AddParameter("@secondary", secondary);
                    mySqlQuery.AddParameter("@headlight", headlight);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktions Fahrzeug für " + fraktion.Name + " Erstellt!", 5000, "red");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "cfrakcar", 97, 4));



            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    string frakid = args[1]; //41142
                    int rgbfarbe = 0;
                    bool rgbfarbe22 = int.TryParse(args[2], out rgbfarbe);
                    int rgbfarbe2 = 0;
                    bool rgbfarbe33 = int.TryParse(args[3], out rgbfarbe2);
                    int rgbfarbe3 = 0;
                    bool rgbfarbe44 = int.TryParse(args[4], out rgbfarbe3);
                    string rgbf = NAPI.Util.ToJson(new Color(rgbfarbe, rgbfarbe2, rgbfarbe3));


                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET RGB = @rgb WHERE Id = @id");
                    mySqlQuery.AddParameter("@rgb", rgbf);
                    mySqlQuery.AddParameter("@id", frakid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgaragen Spawnpoint gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "changefrakcolor", 98, 3));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    string dieid = args[1];
                    string garage = NAPI.Util.ToJson(client.Position);



                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET Garage = @garage WHERE Id = @id");
                    mySqlQuery.AddParameter("@garage", garage);
                    mySqlQuery.AddParameter("@id", dieid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgarage gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "setfrakgarage", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    string dieid = args[1];
                    string garage = NAPI.Util.ToJson(client.Position);
                    string garage2 = NAPI.Util.ToJson(client.Heading);



                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET GarageSpawn = @garage, GarageSpawnRotation = @garagr2 WHERE Id = @id");
                    mySqlQuery.AddParameter("@garage", garage);
                    mySqlQuery.AddParameter("@id", dieid);
                    mySqlQuery.AddParameter("@garagr2", garage2);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgaragen Spawnpoint gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "setfrakgaragspawn", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    string frakid = args[1];
                    string logo = args[2];



                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET Logo = @logo WHERE Id = @id");
                    mySqlQuery.AddParameter("@logo", logo);
                    mySqlQuery.AddParameter("@id", frakid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgaragen Spawnpoint gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "changefraklogo", 98, 2));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM bans WHERE Account = @username");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);
                BanModule.bans.RemoveAll(ban => ban.Account == name);
                BanModule.TimeBanIdentifier(DateTime.Now, name);

                dbPlayer.SendNotification("Spieler entbannt!", 3000, "red");
            }, "unban", 95, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM accounts WHERE Username = @username");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Account gelöscht!", 3000, "red");
            }, "delacc", 97, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM accounts WHERE Social = @social");
                mySqlQuery.AddParameter("@social", name);
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Account durch Socialclub gelöscht!", 3000, "red");
            }, "delsacc", 97, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string frakid = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM fraktionen WHERE Id = @id");
                mySqlQuery.AddParameter("@id", frakid);
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Fraktion gelöscht!", 3000, "red");
            }, "delfrak", 99, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {

                NAPI.World.SetWeather(Weather.EXTRASUNNY);

                dbPlayer.SendNotification("Wetter gecleert!", 3000, "red");
            }, "clearweather", 92, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Password = @password WHERE Username = @username");
                mySqlQuery.AddParameter("@password", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);



                dbPlayer.SendNotification("Passwort geändert! (123)", 3000, "red");
            }, "resetpw", 96, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    if (dbPlayer2.Faction.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("Der Spieler ist in keiner Fraktion!", 3000, "red");
                    }
                    else
                    {
                        dbPlayer2.SetPosition(dbPlayer2.Faction.Storage);
                        dbPlayer.SendNotification("Spieler zu " + dbPlayer2.Faction.Name + " teleportiert!", 3000, "red");
                    }
                }
            }, "tpfrak", 92, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                int slot = 0;
                bool slot2 = int.TryParse(args[2], out slot);
                int drawable = 0;
                bool drawable2 = int.TryParse(args[3], out drawable);
                int texture = 0;
                bool texture2 = int.TryParse(args[4], out texture);

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer2.SetClothes(slot, drawable, texture);
                    dbPlayer.SendNotification("Kleidungsstück geändert zu " + slot + " " + drawable + " " + texture + " ", 3000, "red");
                }
            }, "aclothes", 92, 4));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string collection = args[1];
                string overlay = args[2];
                Client p;
                Decoration data = new Decoration();
                data.Collection = NAPI.Util.GetHashKey(collection);//"mpchristmas2_overlays"
                data.Overlay = NAPI.Util.GetHashKey(overlay);//"MP_Xmas2_M_Tat_005"
                dbPlayer.SendNotification("" + data, 20000, "red");
                dbPlayer.Client.SetDecoration(data);
            }, "atattoos", 92, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string hash = args[1];
                string hash2 = args[2];
                Client p;
                dbPlayer.SendNotification("1: " + NAPI.Util.GetHashKey(hash), 30000, "red");
                dbPlayer.SendNotification("2: " + NAPI.Util.GetHashKey(hash2), 30000, "red");
            }, "tattoo", 92, 2));






            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET Parked = 1 WHERE OwnerId = @username");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Du hast alle Fahrzeuge von " + name + " eingeparkt!", 3000, "red");
            }, "parkcars", 95, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (int.TryParse(args[1], out var result))
                {
                    if (dbPlayer.OwnVehicles.ContainsKey(result))
                    {
                        VehicleKeyHandler.Instance.DeleteAllVehicleKeys(result);
                        dbPlayer.SendNotification("Schlüssel von dem Fahrzeug wurden gecleart.", 3500, "green");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Fahrzeug nicht in deinem Besitz.", 3500, "red");
                    }
                }

            }, "clearvehiclekeys", 0, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.GetHouse() != null && dbPlayer.GetHouse().OwnerId == dbPlayer.Id)
                {
                    HouseKeyHandler.Instance.DeleteAllHouseKeys(dbPlayer.GetHouse());
                    dbPlayer.SendNotification("Schlüssel von dem Haus wurden gecleart.", 3500, "green");
                }
                else
                {
                    dbPlayer.SendNotification("Haus nicht in deinem Besitz.", 3500, "red");
                }

            }, "clearhousekeys", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.Client.IsInVehicle)
                {
                    dbPlayer.Client.Vehicle.Repair();
                }
            }, "repair", 92, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (args[1] == "1")
                {
                    dbPlayer.SendNotification("test", 3500, "dodgerblue");
                    dbPlayer.PlayAnimation(33, "mp_arresting", "a_uncuff");
                }
                else if (args[1] == "2")
                {
                    dbPlayer.PlayAnimation(33, "mp_arresting", "b_uncuff");
                }
                else if (args[1] == "3")
                {
                    dbPlayer.PlayAnimation(33, "mp_arrest_paired", "crook_p2_back_right");
                }
            }, "an", 97, 0));

            // dbPlayer.PlayAnimation(2, "mp_arresting", "b_uncuff");
            //  dbPlayer2.PlayAnimation(2, "mp_arrest_paired", "crook_p2_back_right");
            commandList.Add(new Command((dbPlayer, args) =>
            {
                BanModule.Instance.Load(true);

                dbPlayer.SendNotification("Bans neu geladen!", 3000, "red");
            }, "reloadbans", 94, 0));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                Client client = dbPlayer2.Client;

                dbPlayer2.SpawnPlayer(dbPlayer2.Client.Position);
                dbPlayer2.disableAllPlayerActions(false);
                dbPlayer2.SetAttribute("Death", 0);
                dbPlayer2.StopAnimation();
                dbPlayer2.SetInvincible(false);
                dbPlayer2.DeathData = new DeathData
                {
                    IsDead = false,
                    DeathTime = new DateTime(0)
                };
                dbPlayer2.StopScreenEffect("DeathFailOut");

                dbPlayer.SendNotification("Du hast den Spieler " + dbPlayer2.Name + " revived!", 3000, "red", "Support");
                dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Adminrank.Name + " " + dbPlayer.Name + " revived!", 3000, "red", "Support");
                WebhookSender.SendMessage("Spieler wird revived", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + dbPlayer2.Name + " revived.", Webhooks.revivelogs, "Revive");
            }, "revive", 92, 0));

            commandList.Add(new Command((dbPlayer, args) => PaintballModule.leavePaintball(dbPlayer.Client), "quitffa", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    var random = new Random();
                    int id = random.Next(10000, 99999);
                    int price = 0;
                    bool price2 = int.TryParse(args[1], out price);
                    string entrance = NAPI.Util.ToJson(client.Position);
                    int classid = 0;
                    bool classid2 = int.TryParse(args[2], out classid);

                    if (!classid2) return;

                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO houses (Id, Price, Entrance, ClassId) VALUES (@id, @price, @entrance, @classid)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@price", price);
                    mySqlQuery.AddParameter("@entrance", entrance);
                    mySqlQuery.AddParameter("@classid", classid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("An deiner Position wurde erfolgreich ein Haus gesetzt. ID: " + id);

                    NAPI.Marker.CreateMarker(1, client.Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, 0);
                    NAPI.Blip.CreateBlip(40, client.Position, 1f, 0, "Haus " + id, 255, 0.0f, true, 0, 0);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "sethouse", 100, 2));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    var random = new Random();
                    int id = random.Next(10000, 99999);

                    int price = 15000;

                    int minprice = 15000;

                    int maxprice = 350000;

                    int pricestep = 5000;

                    int maxmultiple = 3;

                    int radius = 3;

                    string pos_x = NAPI.Util.ToJson(client.Position.X);

                    string pos_y = NAPI.Util.ToJson(client.Position.Y);

                    string pos_z = NAPI.Util.ToJson(client.Position.Z);

                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO kasino_devices (id, price, minprice, maxprice, pricestep, maxmultiple, pos_x, pos_y, pos_z, radius) VALUES (@id, @price, @minprice, @maxprice, @pricestep, @maxmultiple, @pos_x, @pos_y, @pos_z, @radius)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@price", price);
                    mySqlQuery.AddParameter("@minprice", minprice);
                    mySqlQuery.AddParameter("@maxprice", maxprice);
                    mySqlQuery.AddParameter("@pricestep", pricestep);
                    mySqlQuery.AddParameter("@maxmultiple", maxmultiple);
                    mySqlQuery.AddParameter("@pos_x", pos_x);
                    mySqlQuery.AddParameter("@pos_y", pos_y);
                    mySqlQuery.AddParameter("@pos_z", pos_z);
                    mySqlQuery.AddParameter("@radius", radius);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("An deiner Position wurde erfolgreich ein Casino Automat gesetzt. ID: " + id);

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "setcasino", 100, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {


                    var random = new Random();
                    int id = random.Next(10000, 99999);

                    string name = args[1];

                    string shortname = args[2];

                    string spawn = NAPI.Util.ToJson(client.Position);

                    int dimension = random.Next(10000, 99999);

                    string blip = args[3];


                    //  string rgbfarbe = args[4];

                    int rgbfarbe = 0;
                    bool rgbfarbe22 = int.TryParse(args[4], out rgbfarbe);
                    int rgbfarbe2 = 0;
                    bool rgbfarbe33 = int.TryParse(args[5], out rgbfarbe2);
                    int rgbfarbe3 = 0;
                    bool rgbfarbe44 = int.TryParse(args[6], out rgbfarbe3);
                    string rgbf = NAPI.Util.ToJson(new Color(rgbfarbe, rgbfarbe2, rgbfarbe3));




                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO fraktionen (Id, Name, Short, Spawn, Storage, Dimension, Blip, RGB) VALUES (@id, @name, @short, @spawn, @storage, @dimension, @blip, @rgb)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@name", name);
                    mySqlQuery.AddParameter("@short", shortname);
                    mySqlQuery.AddParameter("@spawn", spawn);
                    mySqlQuery.AddParameter("@storage", spawn);
                    mySqlQuery.AddParameter("@dimension", dimension);
                    mySqlQuery.AddParameter("@blip", blip);
                    mySqlQuery.AddParameter("@rgb", rgbf);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktion Erstellt! Fraktions-ID " + id, 10000, "red");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "cfrak", 98, 6));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {


                    var random = new Random();
                    int id = random.Next(10000, 99999);

                    string name = args[1];

                    string hash = args[2];

                    string collection = args[3];

                    string zone = args[4];

                    string price = args[5];







                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO assets_tattoo (Id, name, hash_male, hash_female, collection, zone_id, price) VALUES (@id, @name, @hash_male, @hash_female, @collection, @zone_id, @price)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@name", name);
                    mySqlQuery.AddParameter("@hash_male", hash);
                    mySqlQuery.AddParameter("@hash_female", hash);
                    mySqlQuery.AddParameter("@collection", collection);
                    mySqlQuery.AddParameter("@zone_id", zone);
                    mySqlQuery.AddParameter("@price", price);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Tattoo hinzugefügt", 3000, "red");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "addtattoo", 92, 5));

            /* commandList.Add(new Command((dbPlayer, args) =>
             {
                 Client client = dbPlayer.Client;

                 try
                 {


                     var random = new Random();
                     int id = random.Next(10000, 99999);

                     string name = args[1];

                     string shortname = args[2];

                     string spawn = NAPI.Util.ToJson(client.Position);

                     int dimension = random.Next(10000, 99999);

                     string blip = args[3];

                     int rgbfarbe = 0;
                     bool rgbfarbe22 = int.TryParse(args[4], out rgbfarbe);
                     int rgbfarbe2 = 0;
                     bool rgbfarbe33 = int.TryParse(args[5], out rgbfarbe2);
                     int rgbfarbe3 = 0;
                     bool rgbfarbe44 = int.TryParse(args[6], out rgbfarbe3);
                     string rgbf = NAPI.Util.ToJson(new Color(rgbfarbe, rgbfarbe2, rgbfarbe3));


                     MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO fraktionen (Id, Name, Short, Spawn, Storage, Dimension, Blip, RGB) VALUES (@id, @name, @short, @spawn, @storage, @dimension, @rgb)");
                     mySqlQuery.AddParameter("@id", id);
                     mySqlQuery.AddParameter("@name", name);
                     mySqlQuery.AddParameter("@short", shortname);
                     mySqlQuery.AddParameter("@spawn", spawn);
                     mySqlQuery.AddParameter("@storage", spawn);
                     mySqlQuery.AddParameter("@dimension", dimension);
                     mySqlQuery.AddParameter("@rgb", rgbf);
                     MySqlHandler.ExecuteSync(mySqlQuery);
                 }
                 catch (Exception ex)
                 {
                     Logger.Print("[EXCEPTION POS] " + ex.Message);
                     Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                 }
             }, "cfrak", 97, 6));*/


            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {


                    var random = new Random();
                    int id = random.Next(10000, 99999);

                    string factionid = args[1];

                    string model = args[2];

                    int primary = 0;
                    bool primary2 = int.TryParse(args[3], out primary);

                    int secondary = 0;
                    bool secondary2 = int.TryParse(args[4], out secondary);

                    int headlight = 0;
                    bool headlight2 = int.TryParse(args[5], out headlight);



                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(factionid));
                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO fraktion_vehicles (Id, FactionId, Model, PrimaryColor, SecondaryColor, HeadlightColor) VALUES (@id, @factionid, @model, @primary, @secondary, @headlight)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@factionid", factionid);
                    mySqlQuery.AddParameter("@model", model);
                    mySqlQuery.AddParameter("@primary", primary);
                    mySqlQuery.AddParameter("@secondary", secondary);
                    mySqlQuery.AddParameter("@headlight", headlight);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktions Fahrzeug für " + fraktion.Name + " Erstellt!", 5000, "red");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "cfrakcar", 97, 4));



            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    string frakid = args[1]; //41142
                    int rgbfarbe = 0;
                    bool rgbfarbe22 = int.TryParse(args[2], out rgbfarbe);
                    int rgbfarbe2 = 0;
                    bool rgbfarbe33 = int.TryParse(args[3], out rgbfarbe2);
                    int rgbfarbe3 = 0;
                    bool rgbfarbe44 = int.TryParse(args[4], out rgbfarbe3);
                    string rgbf = NAPI.Util.ToJson(new Color(rgbfarbe, rgbfarbe2, rgbfarbe3));


                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET RGB = @rgb WHERE Id = @id");
                    mySqlQuery.AddParameter("@rgb", rgbf);
                    mySqlQuery.AddParameter("@id", frakid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgaragen Spawnpoint gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "changefrakcolor", 98, 3));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    string dieid = args[1];
                    string garage = NAPI.Util.ToJson(client.Position);



                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET Garage = @garage WHERE Id = @id");
                    mySqlQuery.AddParameter("@garage", garage);
                    mySqlQuery.AddParameter("@id", dieid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgarage gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "setfrakgarage", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    string dieid = args[1];
                    string garage = NAPI.Util.ToJson(client.Position);
                    string garage2 = NAPI.Util.ToJson(client.Heading);



                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET GarageSpawn = @garage, GarageSpawnRotation = @garagr2 WHERE Id = @id");
                    mySqlQuery.AddParameter("@garage", garage);
                    mySqlQuery.AddParameter("@id", dieid);
                    mySqlQuery.AddParameter("@garagr2", garage2);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgaragen Spawnpoint gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "setfrakgaragspawn", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {

                    string frakid = args[1];
                    string logo = args[2];



                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET Logo = @logo WHERE Id = @id");
                    mySqlQuery.AddParameter("@logo", logo);
                    mySqlQuery.AddParameter("@id", frakid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Fraktionsgaragen Spawnpoint gesetzt!");

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "changefraklogo", 98, 2));



            /*  commandList.Add(new Command((dbPlayer, args) =>
              {
                  Client client = dbPlayer.Client;

                  try
                  {

                      string labor = NAPI.Util.ToJson(client.Position);



                      MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET Logo = @logo WHERE Id = @id");
                      mySqlQuery.AddParameter("@logo", logo);
                      MySqlHandler.ExecuteSync(mySqlQuery);

                      dbPlayer.SendNotification("Labor gesetzt!");

                  }
                  catch (Exception ex)
                  {
                      Logger.Print("[EXCEPTION POS] " + ex.Message);
                      Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                  }
              }, "setfraklabor", 98, 1));*/



            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string str = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                    return;
                }
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer2.Client.SetSkin(NAPI.Util.GetHashKey(str));
                    dbPlayer.SendNotification("Skin geändert!", 3000, "red");
                }
            }, "setped", 98, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                TabletModule.AcceptedTickets.Clear();
                TabletModule.Tickets.Clear();
                dbPlayer.SendNotification("Tickets gecleart!", 3000, "gray", "");
                Notification.SendGlobalNotification("Alle Support Tickets wurden aufgrund von technischen Problemen geschlossen!", 8000, "orange", Notification.icon.bullhorn);
            }, "cleartickets", 97, 0));






            /*List<Command> list49 = CommandModule.commandList;
            Action<DbPlayer, string[]> u003cu003e9_146 = CommandModule.<>c.<>9__1_46;
            if (u003cu003e9_146 == null)
            {
                u003cu003e9_146 = new Action<DbPlayer, string[]>(CommandModule.<>c.<>9, (DbPlayer dbPlayer, string[] args) => {
                    CommandModule.<>c__DisplayClass1_7 variable = null;
                    Client client = dbPlayer.Client;
                    try
                    {
                        if (dbPlayer.Faction.Id != 0)
                        {
                            if (dbPlayer.Factionrank == 12)
                            {
                                DbPlayer dbPlayer1 = PlayerHandler.GetPlayer(args[1]);
                                if ((dbPlayer1 == null ? true : !dbPlayer1.IsValid(true)))
                                {
                                    dbPlayer.SendNotification("Spieler nicht gefunden.", 3000, "gray", "");
                                }
                                else if (dbPlayer.Faction.Id != dbPlayer1.Faction.Id)
                                {
                                    dbPlayer.SendNotification("Ihr seit nicht in der gleichen Fraktion!", 3000, "gray", "");
                                }
                                else if (!dbPlayer1.Medic)
                                {
                                    List<DbPlayer> factionPlayers = dbPlayer.Faction.GetFactionPlayers();
                                    Predicate<DbPlayer> u003cu003e9_158 = CommandModule.<>c.<>9__1_58;
                                    if (u003cu003e9_158 == null)
                                    {
                                        u003cu003e9_158 = new Predicate<DbPlayer>(CommandModule.<>c.<>9, (DbPlayer player) => player.Medic);
                                        CommandModule.<>c.<>9__1_58 = u003cu003e9_158;
                                    }
                                    List<DbPlayer> list = factionPlayers.FindAll(u003cu003e9_158);
                                    if (list.get_Count() < 2)
                                    {
                                        dbPlayer1.Medic = true;
                                        dbPlayer1.RefreshData(dbPlayer1);
                                        dbPlayer1.SetAttribute("Medic", 1);
                                        dbPlayer.SendNotification("Medic gesetzt!", 3000, "gray", "");
                                    }
                                    else
                                    {
                                        StringBuilder stringBuilder = new StringBuilder();
                                        list.ForEach(new Action<DbPlayer>(CommandModule.<>c__DisplayClass1_7.<OnLoad>b__59));
                                        dbPlayer.SendNotification(string.Concat("Es sind bereits 2 Medics in deiner Fraktion. ", stringBuilder.ToString()), 3000, "gray", "");
                                        return;
                                    }
                                }
                                else
                                {
                                    dbPlayer1.Medic = false;
                                    dbPlayer1.RefreshData(dbPlayer1);
                                    dbPlayer1.SetAttribute("Medic", 0);
                                    dbPlayer.SendNotification("Medic entfernt!", 3000, "gray", "");
                                }
                            }
                        }
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        Logger.Print(string.Concat("[EXCEPTION setmedic] ", exception.get_Message()));
                        Logger.Print(string.Concat("[EXCEPTION setmedic] ", exception.StackTrace));
                    }
                });
                CommandModule.<>c.


          9__1_46 = u003cu003e9_146;
            }
            list49.Add(new Command(u003cu003e9_146, "setmedic", 0, 1));*/

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery($"SELECT * FROM accounts WHERE Social = '{name}'");
                MySqlResult result = MySqlHandler.GetQuery(mySqlQuery);

                if (result.Reader.HasRows)
                {
                    result.Reader.Read();
                    dbPlayer.SendNotification("Ingame Name: " + result.Reader.GetString("Username"));
                }
                result.Connection.Dispose();
            }, "name", 0, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                dbPlayer.TriggerEvent("updateCuffed", false);
                dbPlayer.IsCuffed = false;
                dbPlayer.IsFarming = false;
                dbPlayer.RefreshData(dbPlayer);
                dbPlayer.SendNotification("Du wurdest von " + dbPlayer.Name + " entfesselt.", 3000);
                dbPlayer.StopAnimation();
                dbPlayer.disableAllPlayerActions(false);
            }, "uncuff", 93, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Client client = dbPlayer.Client;

                try
                {
                    int id = 0;
                    bool id2 = int.TryParse(args[1], out id);

                    if (!id2) return;

                    MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM houses WHERE Id = @id");
                    mySqlQuery.AddParameter("@id", id);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Das Haus wurde erfolgreich entfernt.");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "delhouse", 100, 1));

            return true;
        }

        [RemoteEvent("PlayerChat")]
        public static async void onPlayerCommand(Client player, string input)
        {
            try
            {
                SqlModule.Instance.CheckSymbols(input);

                if (player == null) return;
                DbPlayer dbPlayer = player.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (!dbPlayer.CanInteractAntiFlood(1)) return;

                Logger.Print(player.Name + " " + input);
                if (input == "" && input == " ")
                {
                    return;
                }

                string[] array = input.Split(" ");
                foreach (Command command in CommandModule.commandList)
                {
                    if (array[0] == command.Name)
                    {
                        Adminrank adminranks = dbPlayer.Adminrank;

                        if (array.Length <= command.Args)
                        {
                            dbPlayer.SendNotification("Du hast zu wenig Argumente angegeben!", 3000, "red");
                            return;
                        }
                        if (command.Permission <= adminranks.Permission)
                        {
                            WebhookSender.SendMessage("Command", dbPlayer.Name + ": " + input, Webhooks.commandlogs,
                                "Command");
                            if (command.Callback2 != null) command.Callback2(player, dbPlayer, array);
                            else
                                command.Callback(dbPlayer, array);
                        }
                        else
                        {
                            dbPlayer.SendNotification("Du besitzt dafür keine Berechtigung!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION onPlayerCommand] " + ex.Message);
                Logger.Print("[EXCEPTION onPlayerCommand] " + ex.StackTrace);
            }

        }

        [RemoteEvent("nM-Adminmenu")]
        public void Adminmenu(Client c, string selection)
        {
            if (selection == null)
                return;

            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            Adminrank adminranks = dbPlayer.Adminrank;

            if (adminranks.Permission <= 91)
                return;

            if (selection == "aduty")
            {
                try
                {
                    Client client = dbPlayer.Client;

                    if (!dbPlayer.HasData("PLAYER_ADUTY"))
                    {
                        dbPlayer.SetData("PLAYER_ADUTY", false);
                    }

                    dbPlayer.ACWait();

                    WebhookSender.SendMessage("Spieler wechselt Aduty", "Der Spieler " + dbPlayer.Name + " hat den Adminmodus " + (dbPlayer.GetData("PLAYER_ADUTY") ? "betreten" : "verlassen") + ".", Webhooks.adminlogs, "Admin");

                    client.TriggerEvent("setPlayerAduty", !dbPlayer.GetData("PLAYER_ADUTY"));
                    client.TriggerEvent("updateAduty", !dbPlayer.GetData("PLAYER_ADUTY"));
                    dbPlayer.SetData("PLAYER_ADUTY", !dbPlayer.GetData("PLAYER_ADUTY"));
                    dbPlayer.SpawnPlayer(new Vector3(client.Position.X, client.Position.Y, client.Position.Z + 0.52f));
                    if (dbPlayer.GetData("PLAYER_ADUTY"))
                    {
                        dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", true);
                        dbPlayer.SendNotification("Du hast den Admin-Dienst betreten.", 3000, "red", "ADMIN");
                        Adminrank adminrank = dbPlayer.Adminrank;
                        int num = (int)adminrank.ClothingId;
                        dbPlayer.SetClothes(3, 9, 0);
                        PlayerClothes.setAdmin(dbPlayer, num);
                        dbPlayer.SetClothes(5, 0, 0);
                        return;
                    }
                    else
                    {
                        dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", false);
                        dbPlayer.SendNotification("Du hast den Admin-Dienst verlassen.", 3000, "red", "ADMIN");
                    }
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @user LIMIT 1");
                    mySqlQuery.AddParameter("@user", client.Name);

                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PlayerClothes playerClothes = NAPI.Util.FromJson<PlayerClothes>(reader.GetString("Clothes"));

                                    //  dbPlayer.SetClothes(2, playerClothes.Haare.drawable, playerClothes.Haare.texture);
                                    client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                                    client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                                    dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                                    dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                                    dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                                    dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                                    dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                                    dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                                }
                            }
                        }
                        finally
                        {
                            reader.Dispose();
                        }
                    }
                    finally
                    {
                        mySqlReaderCon.Connection.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION ADUTY] " + ex.Message);
                    Logger.Print("[EXCEPTION ADUTY] " + ex.StackTrace);
                }
            }
            else if (selection == "vanish")
            {

                Client client = dbPlayer.Client;

                if (!client.HasSharedData("PLAYER_INVISIBLE"))
                    return;

                bool invisible = client.GetSharedData("PLAYER_INVISIBLE");
                dbPlayer.SendNotification("Du hast dich " + (!invisible ? "unsichtbar" : "sichtbar") + " gemacht.", 3000, "red", "ADMIN");
                client.SetSharedData("PLAYER_INVISIBLE", !invisible);

            }
            else if (selection == "revivemenu")
            {
                if (adminranks.Permission <= 91)
                    return;

                if (dbPlayer == null || !dbPlayer.IsValid(true))
                    return;

                Client client = dbPlayer.Client;

                dbPlayer.SpawnPlayer(dbPlayer.Client.Position);
                dbPlayer.disableAllPlayerActions(false);
                dbPlayer.SetAttribute("Death", 0);
                dbPlayer.StopAnimation();
                dbPlayer.SetInvincible(false);
                dbPlayer.DeathData = new DeathData
                {
                    IsDead = false,
                    DeathTime = new DateTime(0)
                };
                dbPlayer.StopScreenEffect("DeathFailOut");

                dbPlayer.SendNotification("Du hast dich selber revived!", 3000, "red", "Support");
                WebhookSender.SendMessage("Spieler hat sich selber revived", "Der Spieler " + dbPlayer.Name + " hat sich selber revived!", Webhooks.revivelogs, "Revive");
            }
            else if (selection == "kickplayer")
            {
                dbPlayer.CloseNativeMenu();
                if (adminranks.Permission <= 91)
                    return;
                TextInputBoxObject textInputBoxObject = new TextInputBoxObject
                {
                    Callback = "kickplayer"
                };
            }
            else if (selection == "banplayer")
            {
                dbPlayer.CloseNativeMenu();
                if (adminranks.Permission >= 93)
                    return;

                TextInputBoxObject textInputBoxObject = new TextInputBoxObject
                {
                    Callback = "banplayer"
                };
            }
            else if (selection == "bringmenu")
            {
                dbPlayer.CloseNativeMenu();
                if (adminranks.Permission >= 91)
                    return;

                TextInputBoxObject textInputBoxObject = new TextInputBoxObject
                {
                    Callback = "bringmenu"
                };
            }
            else if (selection == "gotomenu")
            {
                dbPlayer.CloseNativeMenu();
                if (adminranks.Permission >= 91)
                    return;

                TextInputBoxObject textInputBoxObject = new TextInputBoxObject
                {
                    Callback = "gotomenu"
                };
            }
        }

    

        [RemoteEvent("kickplayer")]
        public static void kickplayer(DbPlayer dbPlayer, string[] args)
        {
            string name = args[1];

            DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

            Adminrank adminrank = dbPlayer.Adminrank;
            Adminrank adminranks = dbPlayer2.Adminrank;

            if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
            {
                dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                return;
            }
            if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                if (adminrank.Permission <= adminranks.Permission)
                {
                    dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                    return;
                }
                else
                {
                    Client client = dbPlayer2.Client;
                    client.TriggerEvent("openWindow", new object[2]
{
                                   "Kick",
                                    "{\"name\":\""+ dbPlayer2.Name +"\",\"grund\":\"" + String.Join(" ", args).Replace("kick " + name, "") +"\"}"
});
                    dbPlayer2.Client.Kick();
                    dbPlayer.SendNotification("Spieler gekickt!", 3000, "red");
                    Notification.SendGlobalNotification("" + dbPlayer2.Name + " wurde von " + dbPlayer.Name + " gekickt. Grund: " + String.Join(" ", args).Replace("kick " + name, ""), 8000, "red", Notification.icon.warn);
                    // String.Join(" ", args).Replace("announce ", "")
                }


        }

        [RemoteEvent("banplayer")]
        public static void banplayer(DbPlayer dbPlayer, string[] args)
        {
            string name = args[1];

            DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
            Adminrank adminrank = dbPlayer.Adminrank;
            Adminrank adminranks = dbPlayer2.Adminrank;

            if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                if (adminrank.Permission <= adminranks.Permission)
                {
                    dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                    return;
                }
                else
                {
                    BanModule.BanIdentifier(name, String.Join(" ", args).Replace("xcm " + name + " ", ""), name);

                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @username");
                    mySqlQuery.AddParameter("@username", name);
                    MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                    MySqlDataReader reader = mySqlResult.Reader;

                    if (reader.HasRows)
                    {
                        reader.Read();
                        BanModule.BanIdentifier(reader.GetString("Social"), String.Join(" ", args).Replace("xcm " + name + " ", ""), name);
                        BanModule.BanIdentifier(dbPlayer2.Client.Serial, String.Join("", args).Replace("xcm " + name + " ", ""), dbPlayer2.Name);
                        BanModule.BanIdentifier(dbPlayer2.Client.Address, String.Join("", args).Replace("xcm " + name + " ", ""), dbPlayer2.Name);
                    }

                    reader.Dispose();
                    mySqlResult.Connection.Dispose();

                    BanModule.BanIdentifier(name, String.Join(" ", args).Replace("xcm " + name + " ", ""), name);
                    dbPlayer.SendNotification("Spieler gebannt!", 3000, "red");
                    dbPlayer2.TriggerEvent("openWindow", new object[2]
{
                                    "Bann",
                                    "{\"name\":\"" + dbPlayer2.Name + "\"}"
});
                    dbPlayer2.Client.Kick();
                    Notification.SendGlobalNotification($"Der Spieler " + name + " wurde von " + dbPlayer.Name + " gebannt.", 8000, "red", Notification.icon.warn);
                    WebhookSender.SendMessage("Spieler wird gebannt", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + name + " offlinegebannt. Grund: " + String.Join(" ", args).Replace("xcm " + name + " ", ""), Webhooks.banlogs, "Ban");
                }
            else
            {
                Client client = dbPlayer2.Client;
                dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Name, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Name, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Client.Serial, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Client.Address, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                dbPlayer.SendNotification("Spieler gebannt!", 3000, "red");
                client.TriggerEvent("openWindow", new object[2]
{
                                    "Bann",
                                    "{\"name\":\"" + dbPlayer2.Name + "\"}"
});
                dbPlayer2.Client.Kick();
                Notification.SendGlobalNotification($"Der Spieler " + name + " wurde von " + dbPlayer.Name + " gebannt. Grund: ", 8000, "red", Notification.icon.warn);
                WebhookSender.SendMessage("Spieler wird gebannt", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + name + " gebannt. Grund: ", Webhooks.banlogs, "Ban");
            }
        }

        [RemoteEvent("bringmenu")]
        public static void bringmenu(DbPlayer dbPlayer, string[] args)
        {
            string name = args[1];

            DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
            if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
            {
                dbPlayer.SendNotification("Der Spieler ist nicht online.", 3000, "red");
                return;
            }
            dbPlayer2.SetPosition(dbPlayer.Client.Position);
            dbPlayer.SendNotification("Du hast den Spieler " + name + " zu dir teleportiert.");
            dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Adminrank.Name + " " + dbPlayer.Name + " teleportiert.");
        }

        [RemoteEvent("gotomenu")]
        public static void gotomenu(DbPlayer dbPlayer, string[] args)
        {
            string name = args[1];

            DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
            if (dbPlayer2 == null) return;
            dbPlayer.SetPosition(dbPlayer2.Client.Position);
            dbPlayer.SendNotification("Du hast dich zu Spieler " + name + " teleportiert.");
        }

    }


}