using GTANetworkAPI;
using GVMP;
using GVMP.Module;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    internal class ItemModule : Module<ItemModule>
    {
        public static List<Item> itemRegisterList = new List<Item>();
        public static List<string> equipItems = new List<string>();
        protected override bool OnLoad()
        {
            equipItems.Add("revolter");
            equipItems.Add("cyclone");
            equipItems.Add("revolter");
            equipItems.Add("schafterg");
            equipItems.Add("bati");
            equipItems.Add("bati");
            equipItems.Add("t20");
            equipItems.Add("schafterg");
            equipItems.Add("schafterg");
            equipItems.Add("cyclone");
            equipItems.Add("t20");
            equipItems.Add("bati");
            equipItems.Add("gtr");
            equipItems.Add("bati");
            equipItems.Add("revolter");
            equipItems.Add("bati");
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM items";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Item item = new Item
                            {
                                Id = reader.GetInt32("Id"),
                                ImagePath = reader.GetString("Image"),
                                MaxStackSize = reader.GetInt32("Stack"),
                                Name = reader.GetString("Name"),
                                WeightInG = reader.GetInt32("Weight"),
                                Whash = (WeaponHash)NAPI.Util.GetHashKey(reader.GetString("Whash"))
                            };

                            itemRegisterList.Add(item);
                        }
                    }
                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadItems] " + ex.Message);
                Logger.Print("[EXCEPTION loadItems] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }

            return true;
        }

        [RemoteEvent("Pressed_KOMMA")]
        public static void komma(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                if (dbPlayer.IsFarming)
                {
                    return;
                }

                MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                try
                {
                    MySqlDataReader reader = mySqlReaderCon.Reader;
                    try
                    {
                        if (!reader.HasRows)
                        {
                            return;
                        }

                        reader.Read();
                        List<ItemModel> list = new List<ItemModel>();
                        string @string = reader.GetString("Items");
                        list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                        ItemModel itemToUse = list.FirstOrDefault((ItemModel x) => x.Name == "Verbandskasten");
                        if (itemToUse == null)
                        {
                            return;
                        }

                        int index = list.IndexOf(itemToUse);
                        if (itemToUse.Amount == 1)
                        {
                            list.Remove(itemToUse);
                        }
                        else
                        {
                            itemToUse.Amount--;
                            list[index] = itemToUse;
                        }

                        Item item = itemRegisterList.FirstOrDefault((Item x) => x.Name == itemToUse.Name);
                        reader.Close();
                        if (reader.IsClosed)
                        {
                            mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                            mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                            MySqlHandler.ExecuteSync(mySqlQuery);
                            dbPlayer.disableAllPlayerActions(true);
                            dbPlayer.SendProgressbar(4000);
                            dbPlayer.IsFarming = true;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.PlayAnimation(33, "amb@medic@standing@tendtodead@idle_a", "idle_a", 8f);
                            NAPI.Task.Run(delegate
                            {
                                dbPlayer.SetHealth(100);
                                dbPlayer.TriggerEvent("client:respawning");
                                dbPlayer.StopProgressbar();
                                dbPlayer.IsFarming = false;
                                dbPlayer.RefreshData(dbPlayer);
                                dbPlayer.StopAnimation();
                                dbPlayer.disableAllPlayerActions(false);
                                dbPlayer.SendNotification("Du hast einen Verbandskasten benutzt.", 3000, "green");
                            }, 4000);
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
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.StackTrace);
            }
        }

        [RemoteEvent("Pressed_PUNKT")]
        public static void punkt(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                if (dbPlayer.IsFarming)
                {
                    return;
                }


                if (dbPlayer.Faction.Name == "FIB" || dbPlayer.Faction.Name == "PoliceDepartment")
                {
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                    mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (!reader.HasRows)
                            {
                                return;
                            }

                            reader.Read();
                            List<ItemModel> list = new List<ItemModel>();
                            string @string = reader.GetString("Items");
                            list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                            ItemModel itemToUse = list.FirstOrDefault((ItemModel x) => x.Name == "BeamtenSchutzweste");
                            if (itemToUse == null)
                            {
                                return;
                            }

                            int index = list.IndexOf(itemToUse);
                            if (itemToUse.Amount == 1)
                            {
                                list.Remove(itemToUse);
                            }
                            else
                            {
                                itemToUse.Amount--;
                                list[index] = itemToUse;
                            }

                            Item item = itemRegisterList.FirstOrDefault((Item x) => x.Name == itemToUse.Name);
                            reader.Close();
                            if (reader.IsClosed)
                            {
                                mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                                mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                                MySqlHandler.ExecuteSync(mySqlQuery);
                                dbPlayer.disableAllPlayerActions(true);
                                dbPlayer.SendProgressbar(4000);
                                dbPlayer.IsFarming = true;
                                dbPlayer.RefreshData(dbPlayer);
                                dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle",
                                    "gang_chatting_idle01", 8f);
                                if (dbPlayer.Faction.Name == "FIB")
                                {


                                    NAPI.Task.Run(delegate
                                    {
                                        dbPlayer.SetArmor(100);
                                        dbPlayer.TriggerEvent("client:respawning");
                                        dbPlayer.StopProgressbar();
                                        dbPlayer.IsFarming = false;
                                        dbPlayer.RefreshData(dbPlayer);
                                        dbPlayer.disableAllPlayerActions(false);
                                        dbPlayer.SendNotification("Du hast eine FIB-Schutzweste benutzt.", 3000, "green");
                                        if (dbPlayer.Factionrank <= 8)
                                        {
                                            dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 3 : 3, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 0);
                                        }
                                        else
                                        {
                                            dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 7 : 7, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 2 : 2);
                                        }
                                        dbPlayer.StopAnimation();
                                    }, 4000);
                                }
                                else if (dbPlayer.Faction.Name == "PoliceDepartment")
                                {
                                    NAPI.Task.Run(delegate
                                    {
                                        dbPlayer.SetArmor(100);
                                        dbPlayer.TriggerEvent("client:respawning");
                                        dbPlayer.StopProgressbar();
                                        dbPlayer.IsFarming = false;
                                        dbPlayer.RefreshData(dbPlayer);
                                        dbPlayer.disableAllPlayerActions(false);
                                        dbPlayer.SendNotification("Du hast eine LSPD-Schutzweste benutzt.", 3000, "green");
                                        dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 7 : 7, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 0);
                                        dbPlayer.StopAnimation();
                                    }, 4000);
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
                else
                {
                    if (client == null) return;
                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                        return;

                    if (dbPlayer.DeathData.IsDead) return;

                    if (dbPlayer.IsFarming)
                    {
                        return;
                    }
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                    mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);

                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (!reader.HasRows)
                            {
                                return;
                            }

                            reader.Read();
                            List<ItemModel> list = new List<ItemModel>();
                            string @string = reader.GetString("Items");
                            list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                            ItemModel itemToUse = list.FirstOrDefault((ItemModel x) => x.Name == "Schutzweste");
                            if (itemToUse == null)
                            {
                                return;
                            }

                            int index = list.IndexOf(itemToUse);
                            if (itemToUse.Amount == 1)
                            {
                                list.Remove(itemToUse);
                            }
                            else
                            {
                                itemToUse.Amount--;
                                list[index] = itemToUse;
                            }

                            Item item = itemRegisterList.FirstOrDefault((Item x) => x.Name == itemToUse.Name);
                            reader.Close();
                            if (reader.IsClosed)
                            {
                                mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                                mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                                MySqlHandler.ExecuteSync(mySqlQuery);
                                dbPlayer.disableAllPlayerActions(true);
                                dbPlayer.SendProgressbar(4000);
                                dbPlayer.IsFarming = true;
                                dbPlayer.RefreshData(dbPlayer);
                                dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle",
                                    "gang_chatting_idle01", 8f);
                                NAPI.Task.Run(delegate
                                {
                                    dbPlayer.SetArmor(100);
                                    dbPlayer.TriggerEvent("client:respawning");
                                    dbPlayer.StopProgressbar();
                                    dbPlayer.IsFarming = false;
                                    dbPlayer.RefreshData(dbPlayer);
                                    dbPlayer.disableAllPlayerActions(false);
                                    dbPlayer.SendNotification("Du hast eine Schutzweste benutzt.", 3000, "green");
                                    dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 16 : 15, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 2 : 2);
                                    dbPlayer.StopAnimation();
                                }, 4000);
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
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Pressed_PUNKT] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_PUNKT] " + ex.StackTrace);
            }
        }


        public static bool getItemFunction(Client c, int id)
        {
            if (c == null) return false;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return false;

            var r = new Random();
            string itemm = equipItems[r.Next(0, equipItems.Count)];
            int id3 = new Random().Next(10000, 99999999);
            Item item = itemRegisterList.FirstOrDefault((Item item2) => item2.Id == id);
            if (item == null) return false;

            PaintballModel paintballModel = dbPlayer.GetData("PBZone");


            if (item.Whash != WeaponHash.Unarmed)
            {
                if (paintballModel != null)
                {
                    dbPlayer.SendNotification("Du kannst im Paintball keine Waffen ausrüsten! ", 5000, "red", "");
                }
                else
                {
                 WeaponManager.addWeapon(c, item.Whash);
                dbPlayer.SendNotification("Du hast die Waffe ausgerüstet.", 3000, "green", "Inventar");
                }
                return true;
            }
            else if (item.Name == "Schutzweste")
            {
                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.SetArmor(100);
                        dbPlayer.TriggerEvent("client:respawning");
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.StopProgressbar();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer.SendNotification("Du hast eine Schutzweste benutzt.", 3000, "green");
                        dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 16 : 15, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 2 : 2);
                        dbPlayer.StopAnimation();
                    }, 4000);
                    return true;
                }
            }
            else if (item.Name == "BeamtenSchutzweste")
            {
                if (dbPlayer.Faction.Name == "FIB" || dbPlayer.Faction.Name == "PoliceDepartment" && !dbPlayer.IsFarming)
                {
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    if (dbPlayer.Faction.Name == "FIB")
                    {

                        NAPI.Task.Run(delegate
                        {
                            dbPlayer.SetArmor(100);
                            dbPlayer.TriggerEvent("client:respawning");
                            dbPlayer.StopProgressbar();
                            dbPlayer.IsFarming = false;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.disableAllPlayerActions(false);
                            dbPlayer.SendNotification("Du hast eine FIB-Schutzweste benutzt.", 3000, "green");
                            if (dbPlayer.Factionrank <= 8)
                            {
                            dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 3 : 3, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 0);
                            }
                            else
                            {
                                dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 7 : 7, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 2 : 2);
                            }
                            dbPlayer.StopAnimation();
                        }, 4000);
                    }
                    else if (dbPlayer.Faction.Name == "PoliceDepartment")
                    {
                        NAPI.Task.Run(delegate
                        {
                            dbPlayer.SetArmor(100);
                            dbPlayer.TriggerEvent("client:respawning");
                            dbPlayer.StopProgressbar();
                            dbPlayer.IsFarming = false;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.disableAllPlayerActions(false);
                            dbPlayer.SendNotification("Du hast eine LSPD-Schutzweste benutzt.", 3000, "green");
                            dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 7 : 7, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 0);
                            dbPlayer.StopAnimation();
                        }, 4000);
                    }
                    return true;
                }
            }

            else if (item.Name == "Underarmor")
            {
                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.SetArmor(dbPlayer.GetArmor() + 100);
                        dbPlayer.TriggerEvent("client:respawning");
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.StopProgressbar();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer.SendNotification("Du hast eine Underarmor benutzt.", 3000, "green");
                        // dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 16 : 15, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 2);
                        dbPlayer.StopAnimation();
                    }, 4000);
                    return true;
                }
            }
            else if (item.Name == "Verbandskasten")
            {
                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.SetHealth(100);
                        dbPlayer.TriggerEvent("client:respawning");
                        dbPlayer.StopAnimation();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer.StopProgressbar();
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.SendNotification("Du hast einen Verbandskasten benutzt.", 3000, "green");
                    }, 4000);
                    return true;
                }
            }
            else if (item.Name == "Mietvertrag")
            {
                House house = HouseModule.houses.FirstOrDefault((House house2) => house2.OwnerId == dbPlayer.Id);
                if (house == null)
                {
                    dbPlayer.SendNotification("Du besitzt kein Haus!", 3000, "red");
                    return false;
                }

                dbPlayer.OpenTextInputBox(new TextInputBoxObject
                {
                    Title = "Mietvertrag",
                    Message = "Gebe bitte den Namen des Spielers ein, dem du gerne einen Mietvertrag machen möchtest.",
                    Callback = "sendMietvertrag"
                });
                return true;
            }

            else if (item.Name == "Magazin")
            {
                Client client = dbPlayer.Client;
                if (NAPI.Player.GetPlayerCurrentWeapon(client) != WeaponHash.Unarmed)
                {
                    WeaponHash weapon = client.CurrentWeapon;
                    dbPlayer.TriggerEvent("sendProgressbar", new object[1]
                    {
                    5000
                    });
                    dbPlayer.TriggerEvent("disableAllPlayerActions", new object[1]
                    {
                    true
                    });
                    //NAPI.Player.PlayPlayerAnimation(p, 33, "ac_ig_3_p3_b-0", "w_ar_assaultrifle_mag1-0", 8);
                    dbPlayer.PlayAnimation(33, "weapons@submg@micro_smg_str", "reload_aim", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.GiveWeapon(weapon, 9999);
                        dbPlayer.SendNotification("Du hast 1 Magazin benutzt, deine Waffe ist nun wieder voll!", 3000, "green");
                        dbPlayer.TriggerEvent("disableAllPlayerActions", new object[1]
                        {
                            false
                        });
                        dbPlayer.StopAnimation();
                        dbPlayer.ResetData("IS_FARMING");

                    }, 5000);
                }
                else
                {
                    dbPlayer.SendNotification("Du musst eine Waffe in der Hand halten!", 3000, "red");
                }
            }

            else if (item.Name == "Wishreq")
            {
                Client client = dbPlayer.Client;
                {
                    dbPlayer.OpenTextInputBox(new TextInputBoxObject
                    {
                        Title = "Wunsch-Kennzeichen",
                        Message = "Bitte gebe dein Wunsch Kennzeichen ein!",
                        Callback = "sendwishreq"
                    });
                }
            }

            else if (item.Name == "AspectsDildo")
            {
                Client client = dbPlayer.Client;
                {
                    //NAPI.Player.PlayPlayerAnimation(p, 33, "ac_ig_3_p3_b-0", "w_ar_assaultrifle_mag1-0", 8);
                    dbPlayer.PlayAnimation(33, "rcmpaparazzo_2", "shag_loop_poppy", 8f);
                }
            }
            else if (item.Name == "Geschenk")
            {
                try
                {
                    if (c == null) return true;
                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                        return true;

                    if (dbPlayer.DeathData.IsDead) return true;

                    if (dbPlayer.IsFarming)
                    {
                        return true;
                    }

                    MySqlQuery mySqlQuery2 = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                    mySqlQuery2.AddParameter("@userId", dbPlayer.Id);
                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery2);
                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (!reader.HasRows)
                            {
                                return true;
                            }

                            reader.Read();
                            List<ItemModel> list2 = new List<ItemModel>();
                            string @string = reader.GetString("Items");
                            list2 = NAPI.Util.FromJson<List<ItemModel>>(@string);
                            ItemModel itemToUse = list2.FirstOrDefault((ItemModel x) => x.Name == "Geschenk");
                            if (itemToUse == null)
                            {
                                return true;
                            }

                            int index = list2.IndexOf(itemToUse);
                            if (itemToUse.Amount == 1)
                            {
                                list2.Remove(itemToUse);
                            }
                            else
                            {
                                itemToUse.Amount--;
                                list2[index] = itemToUse;
                            }


                            reader.Close();
                            if (reader.IsClosed)
                            {
                                mySqlQuery2.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                                mySqlQuery2.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list2)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                                MySqlHandler.ExecuteSync(mySqlQuery2);
                                List<int> list = new List<int>();
                                list.Add(dbPlayer.Id);
                                MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO `vehicles` (`Id`, `Vehiclehash`, `Parked`, `OwnerId`, `Carkeys`, `Plate`) VALUES (@id, @vehiclehash, @parked, @ownerid, @carkeys, @plate)");
                                mySqlQuery.AddParameter("@vehiclehash", itemm);
                                mySqlQuery.AddParameter("@parked", 1);
                                mySqlQuery.AddParameter("@ownerid", dbPlayer.Id);
                                mySqlQuery.AddParameter("@carkeys", NAPI.Util.ToJson(list));
                                mySqlQuery.AddParameter("@plate", id3);
                                mySqlQuery.AddParameter("@id", id3);
                                MySqlHandler.ExecuteSync(mySqlQuery);
                                dbPlayer.SendNotification("Du hast aus deinem Geschenk das Fahrzeug " + itemm + " bekommen!", 3000, "red");


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
                    Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.Message);
                    Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.StackTrace);
                }
            }

            else if (item.Name == "Wuerfel")
            {
                int nrm = new Random().Next(1, 200);
                dbPlayer.disableAllPlayerActions(true);
                dbPlayer.IsFarming = true;
                dbPlayer.RefreshData(dbPlayer);
                dbPlayer.PlayAnimation(33, "mp_player_int_upperwank", "mp_player_int_wank_01");
                //Task.Delay(2000);
                NAPI.Task.Run(delegate
                {
                    dbPlayer.IsFarming = false;
                    dbPlayer.StopAnimation();
                    dbPlayer.disableAllPlayerActions(false);
                    dbPlayer.RefreshData(dbPlayer);
                    NAPI.ClientEvent.TriggerClientEventInRange(dbPlayer.Client.Position, 100.0f, "sendPlayerNotification", "* " + dbPlayer.Name + " rollt die Würfel und bekommt eine " + nrm + ".", 5500, "black", "", "");
                }, 2000);

            }

            else if (item.Name == "Kriegsvertrag")
            {
                try
                {
                    if (c == null) return true;
                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                        return true;

                    if (dbPlayer.DeathData.IsDead) return true;

                    if (dbPlayer.IsFarming)
                    {
                        return true;
                    }

                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                    mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (!reader.HasRows)
                            {
                                return true;
                            }

                            reader.Read();
                            List<ItemModel> list = new List<ItemModel>();
                            string @string = reader.GetString("Items");
                            list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                            ItemModel itemToUse = list.FirstOrDefault((ItemModel x) => x.Name == "Kriegsvertrag");
                            if (itemToUse == null)
                            {
                                return true;
                            }

                            int index = list.IndexOf(itemToUse);
                            if (itemToUse.Amount == 1)
                            {
                                list.Remove(itemToUse);
                            }
                            else
                            {
                                itemToUse.Amount--;
                                list[index] = itemToUse;
                            }


                            reader.Close();
                            if (reader.IsClosed)
                            {
                                mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                                mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                                MySqlHandler.ExecuteSync(mySqlQuery);
                                if (dbPlayer.Factionrank >= 12)
                                {
                                    return true;
                                }
                                dbPlayer.OpenTextInputBox(new TextInputBoxObject
                                {
                                    Title = "Kriegsvertrag",
                                    Message = "Bitte gebe den Namen des Leaders der anderen Fraktion an!",
                                    Callback = "sendvertrag"
                                });


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
                    Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.Message);
                    Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.StackTrace);
                }
            }


            else if (item.Name == "Schweissgeraet")
            {
                return Schweissgeraet.useSchweissgeraet(dbPlayer);
            }

            return false;
        }
    }
}
