using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class HouseModule : GVMP.Module.Module<HouseModule>
    {
        public static List<House> houses = new List<House>();

        public override Type[] RequiredModules() => new Type[1]
        {
            typeof (HouseClassModule)
        };
        
        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM houses");
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
            try
            {
                MySqlDataReader reader = mySqlResult.Reader;
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                loadHouse(reader);
                            }
                            catch (Exception ex)
                            {
                                Logger.Print("[EXCEPTION loadHouse " + reader.GetInt32("Id") + "] " + ex.Message);
                                Logger.Print("[EXCEPTION loadHouse " + reader.GetInt32("Id") + "] " + ex.StackTrace);
                                continue;
                            }
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
                mySqlResult.Connection.Dispose();
            }

            ColShape val = NAPI.ColShape.CreateCylinderColShape(new Vector3(1138, -3198, -40), 1.4f, 1.4f, uint.MaxValue);
            val.SetData("FUNCTION_MODEL", new FunctionModel("leaveKeller"));
            val.SetData("MESSAGE", new Message("Drücke E um den Keller zu verlassen.", "KELLER", "green", 3000)); 

            NAPI.Marker.CreateMarker(1, new Vector3(1138, -3198, -40).Subtract(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, uint.MaxValue);

            ColShape val2 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1130, -3194, -40), 1.4f, 1.4f, uint.MaxValue);
            val2.SetData("FUNCTION_MODEL", new FunctionModel("createBulletproofs"));
            val2.SetData("MESSAGE", new Message("Drücke E um Westen herzustellen.", "KELLER", "green", 3000));

            NAPI.Marker.CreateMarker(1, new Vector3(1130, -3194, -40).Subtract(new Vector3(0, 0, 1.4)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, uint.MaxValue);

            return true;
        }

        public static House getHouseById(int id)
        {
            return houses.FirstOrDefault((House house) => house.Id == id);
        }

        public static void loadHouse(MySqlDataReader reader)
        {
            try
            {
                Vector3 position = NAPI.Util.FromJson<Vector3>(reader.GetString("Entrance"));
                if (position == null) return;

                var houseClass = HouseClassModule.houseClasses.FirstOrDefault((HouseClass houseClass) => houseClass.Id == reader.GetInt32("ClassId"));
                if (houseClass == null) return;

                House house = new House
                {
                    Id = reader.GetInt32("Id"),
                    Price = reader.GetInt32("Price"),
                    OwnerId = reader.GetInt32("OwnerId"),
                    Entrance = position,
                    TenantsIds = NAPI.Util.FromJson<List<int>>(reader.GetString("TenantsId")),
                    Class = houseClass,
                    Locked = true,
                    SeeTel = Convert.ToBoolean(reader.GetInt32("SeeTel")),
                    Wardrobe = NAPI.Util.FromJson<List<HouseClothes>>(reader.GetString("Wardrobe")),
                    Inventory = NAPI.Util.FromJson<List<ItemModel>>(reader.GetString("Inventory")),
                    KellerBuilt = Convert.ToBoolean(reader.GetInt32("KellerBuilt")),
                    TenantPrices = reader.GetString("TenantPrices") == "[]"
                        ? new Dictionary<int, int>()
                        : NAPI.Util.FromJson<Dictionary<int, int>>(reader.GetString("TenantPrices"))
                };

                if (house.OwnerId != 0)
                {
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Id = @id");
                    mySqlQuery.AddParameter("@id", house.OwnerId);
                    MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                    MySqlDataReader mySqlDataReader = mySqlResult.Reader;

                    while (mySqlDataReader.Read())
                    {
                        house.PlayerName = mySqlDataReader.GetString("username");
                    }

                    mySqlResult.Reader.Dispose();
                    mySqlResult.Connection.Dispose();
                }

                houses.Add(house);

                ColShape val = NAPI.ColShape.CreateCylinderColShape(position, 1.4f, 1.4f, 0);
                val.SetData("FUNCTION_MODEL", new FunctionModel("enterHouse", house.Id));
                val.SetData("HOUSE", house.Id);

                NAPI.Marker.CreateMarker(1, position.Subtract(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f,
                    new Color(255, 140, 0), false, 0);
                // NAPI.Blip.CreateBlip(40, position, 1f, 0, "Haus " + house.Id, 255, 0.0f, true, 0, 0);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadHouse " + reader.GetInt32("Id") +  "] " + ex.Message);
                Logger.Print("[EXCEPTION loadHouse " + reader.GetInt32("Id") + "] " + ex.StackTrace);
            }
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public void ColEnter(ColShape shape, Client c)
        {
            try
            {
                if (c == null || !c.Exists || shape == null || !shape.Exists) return;

                if (shape == null) return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

                if (shape.HasData("HOUSE") && shape.GetData("HOUSE") != null)
                {
                    int houseId = shape.GetData("HOUSE");
                    if (houseId == null || houseId == 0) return;

                    var house = getHouseById(houseId);
                    if (house == null) return;

                    string msg = house.OwnerId == 0
                        ? "Preis: " + house.Price + "$ Mieterplätze: " + house.Class.MaxTenants
                        : "Besitzer: " + house.PlayerName != null ? house.PlayerName : "Unbekannt" + "         Freie Mietplätze: " +
                          (house.Class.MaxTenants - house.TenantsIds.Count) +
                          (house.SeeTel ? "      Tel: " + house.OwnerId : "");

                    dbPlayer.SendNotification(msg, 3500, house.Locked ? "red" : "green", "(" + house.Id + ") Immobilie");
                }
            } catch (Exception ex)
            {
                Logger.Print("colenter: " + ex.ToString());
            }
        }

        [RemoteEvent("requestTenants")]
        public void requestTenants(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<Tenant> tenants = new List<Tenant>();
                House house = HouseModule.houses.FirstOrDefault((House house2) => house2.OwnerId == dbPlayer.Id);
                if (house == null) return;

                foreach (int tenant in house.TenantsIds)
                {
                    MySqlQuery mySqlQuery = new MySqlQuery($"SELECT * FROM accounts WHERE Id = {tenant}");
                    MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                    mySqlResult.Reader.Read();

                    Tenant tenantObj = new Tenant
                    {
                        Id = mySqlResult.Reader.GetString("username"),
                        Player_Id = tenant,
                        Price = house.TenantPrices.ContainsKey(tenant) ? house.TenantPrices[tenant] : 0
                    };
                    tenants.Add(tenantObj);

                    mySqlResult.Reader.Dispose();
                    mySqlResult.Connection.Dispose();
                }

                dbPlayer.responseTenants(NAPI.Util.ToJson(tenants));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION requestTenants] " + ex.Message);
                Logger.Print("[EXCEPTION requestTenants] " + ex.StackTrace);
            }
        }

        [RemoteEvent("saverentprice")]
        public void saverentprice(Client c, int price, string name)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                MySqlQuery mySqlQuery = new MySqlQuery($"SELECT * FROM accounts WHERE Username = '{name}'");
                MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                mySqlResult.Reader.Read();

                int id = mySqlResult.Reader.GetInt32("id");

                mySqlResult.Reader.Dispose();
                mySqlResult.Connection.Dispose();

                House house = HouseModule.houses.FirstOrDefault((House house2) => house2.OwnerId == dbPlayer.Id);
                if (house == null) return;

                if (price > 10000)
                {
                    dbPlayer.SendNotification("Dieser Mietpreis ist zu hoch.", 3000, "red");
                    return;
                }

                houses.Remove(house);
                if (!house.TenantPrices.ContainsKey(id))
                    house.TenantPrices.Add(id, price);
                else
                    house.TenantPrices[id] = price;
                houses.Add(house);

                if (house.TenantPrices == null || house == null) return;

                mySqlQuery.Parameters.Clear();
                mySqlQuery = new MySqlQuery($"UPDATE houses SET TenantPrices = @val WHERE Id = {house.Id}");
                mySqlQuery.AddParameter("@val", NAPI.Util.ToJson(house.TenantPrices));
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Du hast den Mietpreis von dem Spieler erfolgreich geändert!", 3000, "green");

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(id);
                if (dbPlayer2 != null && dbPlayer.IsValid(true))
                {
                    dbPlayer2.SendNotification("Neuer Mietpreis: " + price.ToDots() + "$");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION saverentprice] " + ex.Message);
                Logger.Print("[EXCEPTION saverentprice] " + ex.StackTrace);
            }
        }

        [RemoteEvent("unrentTenant")]
        public void unrentTenant(Client c, string name)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                MySqlQuery mySqlQuery = new MySqlQuery($"SELECT * FROM accounts WHERE Username = '{name}'");
                MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                mySqlResult.Reader.Read();

                int id = mySqlResult.Reader.GetInt32("id");

                mySqlResult.Reader.Dispose();
                mySqlResult.Connection.Dispose();

                House house = HouseModule.houses.FirstOrDefault((House house2) => house2.OwnerId == dbPlayer.Id);
                if (house == null) return;

                houses.Remove(house);

                if (house.TenantPrices.ContainsKey(id))
                    house.TenantPrices.Remove(id);

                if (house.TenantsIds.Contains(id))
                    house.TenantsIds.Remove(id);

                houses.Add(house);

                mySqlQuery.Parameters.Clear();
                mySqlQuery = new MySqlQuery("UPDATE houses SET TenantPrices = @val WHERE Id = @id");
                mySqlQuery.AddParameter("@id", house.Id);
                mySqlQuery.AddParameter("@val", NAPI.Util.ToJson(house.TenantPrices));
                MySqlHandler.ExecuteSync(mySqlQuery);

                mySqlQuery.Parameters.Clear();
                mySqlQuery = new MySqlQuery("UPDATE houses SET TenantsId = @val WHERE Id = @id");
                mySqlQuery.AddParameter("@id", house.Id);
                mySqlQuery.AddParameter("@val", NAPI.Util.ToJson(house.TenantsIds));
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Du hast den Spieler erfolgreich aus deinem Haus rausgeschmissen!", 3000,
                    "green");

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(id);
                if (dbPlayer2 != null && dbPlayer.IsValid(true))
                {
                    dbPlayer2.SendNotification("Du wurdest aus deinem Haus rausgeschmissen!");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION unrentTenant] " + ex.Message);
                Logger.Print("[EXCEPTION unrentTenant] " + ex.StackTrace);
            }
        }

        [RemoteEvent("enterHouse")]
        public void enterHouse(Client c, int houseId)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                House house = houses.FirstOrDefault((House house) => house.Id == houseId);
                if (house == null) return;

                if (house.OwnerId == 0)
                {
                    NativeMenu nativeMenu = new NativeMenu("Hauskauf", "", new List<NativeItem>()
                    {
                        new NativeItem("Haus Id: " + house.Id, ""),
                        new NativeItem("Klasse: " + house.Class.Name, ""),
                        new NativeItem("Preis: " + house.Price.ToDots() + "$", ""),
                        new NativeItem("Haus kaufen", "Buy-" + houseId)
                    });
                    dbPlayer.ShowNativeMenu(nativeMenu);
                }
                else
                {
                    List<NativeItem> nativeItems = new List<NativeItem>()
                    {
                        new NativeItem("Haus betreten", "enter-" + houseId),
                        new NativeItem("Keller", "keller-" + houseId)
                    };
                    if (house.OwnerId == dbPlayer.Id)
                        nativeItems.Add(new NativeItem("Telefonnummer ein/ausblenden", "tele-" + houseId));
                    NativeMenu nativeMenu = new NativeMenu("Hausverwaltung",
                        house.Locked ? "Zugeschlossen" : "Aufgeschlossen", nativeItems);
                    dbPlayer.ShowNativeMenu(nativeMenu);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION enterHouse] " + ex.Message);
                Logger.Print("[EXCEPTION enterHouse] " + ex.StackTrace);
            }
        }

        public static void PressedL(DbPlayer dbPlayer)
        {
            if (dbPlayer == null) return;
            try
            {
                House house = dbPlayer.GetNearestHouse();

                if (house != null && (house.OwnerId == dbPlayer.Id || house.TenantsIds.Contains(dbPlayer.Id) || dbPlayer.HouseKeys.Contains(house.Id)))
                {
                    houses.Remove(house);
                    house.Locked = !house.Locked;
                    houses.Add(house);

                    dbPlayer.SendNotification(
                        "Du hast Haus " + house.Id + " " + (house.Locked ? " zugeschlossen." : "aufgeschlossen."), 3000,
                        house.Locked ? "red" : "green");
                }
                else
                {
                    if (!dbPlayer.HasData("IN_HOUSE")) return;

                    if (dbPlayer.GetData("IN_HOUSE") == 0) return;

                    House nearestInterior = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                    if (nearestInterior == null) return;

                    if (nearestInterior != null)
                    {
                        house = nearestInterior;

                        houses.Remove(house);
                        house.Locked = !house.Locked;
                        houses.Add(house);

                        dbPlayer.SendNotification(
                            "Du hast Haus " + house.Id + " " + (house.Locked ? "zugeschlossen." : "aufgeschlossen."),
                            3000, house.Locked ? "red" : "green");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION PressedL] " + ex.Message);
                Logger.Print("[EXCEPTION PressedL] " + ex.StackTrace);
            }
        }

        [RemoteEvent("leaveKeller")]
        public void leaveKeller(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (!dbPlayer.HasData("IN_HOUSE")) return;

                House house = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                if (house == null) return;

                if (house.Locked)
                {
                    dbPlayer.SendNotification("Das Haus ist abgeschlossen.", 3000, "red");
                }
                else
                {
                    dbPlayer.SendNotification("Du hast den Keller verlassen.", 3000, "green");
                    dbPlayer.SetDimension(0);
                    dbPlayer.SetPosition(house.Entrance);
                    dbPlayer.SetData("IN_HOUSE", 0);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION leaveKeller] " + ex.Message);
                Logger.Print("[EXCEPTION leaveKeller] " + ex.StackTrace);
            }
        }

        [RemoteEvent("createBulletproofs")]
        public void createBulletproofs(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (!dbPlayer.HasData("IN_HOUSE")) return;

                House house = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                if (house == null) return;

                List<NativeItem> nativeItems = new List<NativeItem>
                {
                    new NativeItem("Schutzweste (2.000$)", "bulletproof")
                };
                NativeMenu nativeMenu = new NativeMenu("Westenherstellung", "", nativeItems);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION createBulletproofs] " + ex.Message);
                Logger.Print("[EXCEPTION createBulletproofs] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Westenherstellung")]
        public void Westenherstellung(Client c, string selection)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (!dbPlayer.HasData("IN_HOUSE")) return;

                House house = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                if (house == null) return;

                dbPlayer.CloseNativeMenu();

                if (selection == "bulletproof" && dbPlayer.Money >= 2000)
                {
                    List<ItemModel> items = dbPlayer.GetInventoryItems();
                    ItemModel itemModel = items.FirstOrDefault((ItemModel itemModel2) => itemModel2.Name == "Kevlar");
                    if (itemModel != null && itemModel.Amount >= 5)
                    {
                        dbPlayer.removeMoney(2000);
                        dbPlayer.UpdateInventoryItems("Kevlar", 5, true);
                        dbPlayer.UpdateInventoryItems("Schutzweste", 1, false);
                        dbPlayer.SendNotification("Du hast eine Schutzweste hergestellt!", 3000, "green");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du benötigst mindestens 5 Kevlar um eine Schutzweste herzustellen.",
                            3000, "red");
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Westenherstellung] " + ex.Message);
                Logger.Print("[EXCEPTION Westenherstellung] " + ex.StackTrace);
            }
        }

        [RemoteEvent("leaveHouse")]
        public void leaveHouse(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (!dbPlayer.HasData("IN_HOUSE")) return;

                House house = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                if (house == null) return;

                if (house.Locked)
                {
                    dbPlayer.SendNotification("Das Haus ist abgeschlossen.", 3000, "red");
                }
                else
                {
                    dbPlayer.SendNotification("Du hast das Haus verlassen.", 3000, "green");
                    dbPlayer.SetDimension(0);
                    dbPlayer.SetPosition(house.Entrance);
                    dbPlayer.SetData("IN_HOUSE", 0);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION leaveHouse] " + ex.Message);
                Logger.Print("[EXCEPTION leaveHouse] " + ex.StackTrace);
            }
        }

        [RemoteEvent("openHouseWardrobe")]
        public void openHouseWardrobe(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (!dbPlayer.HasData("IN_HOUSE")) return;

                House house = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                if (house == null) return;

                if (house.OwnerId != dbPlayer.Id)
                {
                    dbPlayer.SendNotification("Nur der Hausbesitzer kann den Kleiderschrank benutzen!", 3000, "red");
                    return;
                }

                List<NativeItem> nativeItems = new List<NativeItem>
                {
                    new NativeItem("Aktuelles Outfit speichern", "save")
                };

                foreach (HouseClothes houseClothes in house.Wardrobe)
                {
                    nativeItems.Add(new NativeItem(houseClothes.name, houseClothes.name.ToLower()));
                }

                nativeItems.Add(new NativeItem("Outfits clearen", "clear"));

                NativeMenu nativeMenu = new NativeMenu("Outfits", "", nativeItems);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openHouseWardrobe] " + ex.Message);
                Logger.Print("[EXCEPTION openHouseWardrobe] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Outfits")]
        public void HouseOutfit(Client c, string selection)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (c.Dimension < 3500) return;

                if (!dbPlayer.HasData("IN_HOUSE")) return;

                House house = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                if (house == null) return;

                dbPlayer.CloseNativeMenu();

                if (selection == "save")
                {
                    dbPlayer.OpenTextInputBox(new TextInputBoxObject
                    {
                        Title = "Outfit speichern",
                        Message = "Wie soll das Outfit heißen?",
                        Callback = "SaveOutfit",
                        CloseCallback = ""
                    });
                }
                else if (selection == "clear")
                {
                    houses.Remove(house);
                    house.Wardrobe = new List<HouseClothes>();
                    houses.Add(house);

                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE houses SET Wardrobe = @clothes WHERE Id = @id");
                    mySqlQuery.AddParameter("@clothes", NAPI.Util.ToJson(house.Wardrobe));
                    mySqlQuery.AddParameter("@id", house.Id);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                }
                else
                {
                    foreach (HouseClothes houseClothes in house.Wardrobe)
                    {
                        if (selection == houseClothes.name.ToLower())
                        {
                            dbPlayer.PlayerClothes = houseClothes.playerClothes;
                            dbPlayer.RefreshData(dbPlayer);

                            ClothingManager.setClothes(dbPlayer.Client, houseClothes.playerClothes);
                            PlayerClothes playerClothes2 = houseClothes.playerClothes;
                            dbPlayer.SetClothes(1, playerClothes2.Maske.drawable, playerClothes2.Maske.texture);
                            dbPlayer.SetClothes(11, playerClothes2.Oberteil.drawable, playerClothes2.Oberteil.texture);
                            dbPlayer.SetClothes(8, playerClothes2.Unterteil.drawable, playerClothes2.Unterteil.texture);
                            dbPlayer.SetClothes(7, playerClothes2.Kette.drawable, playerClothes2.Kette.texture);
                            dbPlayer.SetClothes(3, playerClothes2.Koerper.drawable, playerClothes2.Koerper.texture);
                            dbPlayer.SetClothes(4, playerClothes2.Hose.drawable, playerClothes2.Hose.texture);
                            dbPlayer.SetClothes(6, playerClothes2.Schuhe.drawable, playerClothes2.Schuhe.texture);
                            break;
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openHouseWardrobe] " + ex.Message);
                Logger.Print("[EXCEPTION openHouseWardrobe] " + ex.StackTrace);
            }
        }

        [RemoteEvent("SaveOutfit")]
        public void SaveOutfit(Client c, string text)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (c.Dimension < 3500) return;

                if (!dbPlayer.HasData("IN_HOUSE")) return;

                House house = getHouseById(dbPlayer.GetData("IN_HOUSE"));

                if (house == null) return;

                houses.Remove(house);
                house.Wardrobe.Add(new HouseClothes
                {
                    name = text,
                    playerClothes = dbPlayer.PlayerClothes
                });
                houses.Add(house);

                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE houses SET Wardrobe = @clothes WHERE Id = @id");
                mySqlQuery.AddParameter("@clothes", NAPI.Util.ToJson(house.Wardrobe));
                mySqlQuery.AddParameter("@id", house.Id);
                MySqlHandler.ExecuteSync(mySqlQuery);

                WebhookSender.SendMessage("TEXTINPUTBOX", "" + dbPlayer.Name + " " + text + " - OUTFIT SPEICHERN", Webhooks.shoplogs, "Shop");
                dbPlayer.SendNotification("Outfit gespeichert!", 3000, "green");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION SaveOutfit] " + ex.Message);
                Logger.Print("[EXCEPTION SaveOutfit] " + ex.StackTrace);
            }
            
        }

        [RemoteEvent("nM-Hausverwaltung")]
        public void Hausverwaltung(Client c, string selection)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                dbPlayer.CloseNativeMenu();

                string action = selection.Split("-")[0];
                int houseId = 0;
                bool houseId2 = int.TryParse(selection.Split("-")[1], out houseId);
                
                if(!houseId2) return;

                House house = houses.FirstOrDefault((House house) => house.Id == houseId);
                if (house == null) return;

                if (action == "enter")
                {
                    if (house.Locked)
                    {
                        dbPlayer.SendNotification("Das Haus ist abgeschlossen.", 3000, "red");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du hast das Haus betreten.", 3000, "green");
                        dbPlayer.SetDimension(3500 + house.Id);
                        dbPlayer.SetPosition(house.Class.HouseLocation);
                        dbPlayer.SetData("IN_HOUSE", house.Id);
                    }
                }
                else if (action == "keller")
                {
                    List<NativeItem> nativeItems = new List<NativeItem>
                    {
                        new NativeItem("Keller betreten", "enter-" + house.Id),
                        new NativeItem("Keller ausbauen (500.000$)", "build-" + house.Id)
                    };
                    NativeMenu nativeMenu = new NativeMenu("Keller", "", nativeItems);
                    dbPlayer.ShowNativeMenu(nativeMenu);
                }
                else if (action == "tele")
                {
                    if (house.OwnerId == dbPlayer.Id)
                    {
                        houses.Remove(house);
                        house.SeeTel = !house.SeeTel;
                        houses.Add(house);

                        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE houses SET SeeTel = @see WHERE Id = @id");
                        mySqlQuery.AddParameter("@see", Convert.ToInt32(house.SeeTel));
                        mySqlQuery.AddParameter("@id", house.Id);
                        MySqlHandler.ExecuteSync(mySqlQuery);

                        dbPlayer.SendNotification(
                            "Du hast das anzeigen der Telefonnummer " +
                            (house.SeeTel ? "eingeschaltet" : "ausgeschaltet") + ".", 3000, "green");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Hausverwaltung] " + ex.Message);
                Logger.Print("[EXCEPTION Hausverwaltung] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Keller")]
        public void Keller(Client c, string selection)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                dbPlayer.CloseNativeMenu();

                string action = selection.Split("-")[0];

                int houseId = 0;
                bool houseId2 = int.TryParse(selection.Split("-")[1], out houseId);
                if(!houseId2) return;

                House house = houses.FirstOrDefault((House house) => house.Id == houseId);
                if (house == null) return;

                if (action == "enter")
                {
                    if (!house.KellerBuilt)
                    {
                        dbPlayer.SendNotification("Der Keller ist leider nicht ausgebaut!", 3000, "red");
                        return;
                    }

                    if (house.Locked)
                    {
                        dbPlayer.SendNotification("Das Haus ist abgeschlossen.", 3000, "red");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du hast den Keller betreten.", 3000, "green");
                        dbPlayer.SetDimension(3500 + house.Id);
                        dbPlayer.SetPosition(new Vector3(1138, -3198, -40));
                        dbPlayer.SetData("IN_HOUSE", house.Id);
                    }
                }
                else if (action == "build")
                {
                    if (house.KellerBuilt)
                    {
                        dbPlayer.SendNotification("Der Keller ist bereits ausgebaut!", 3000, "red");
                        return;
                    }

                    if (house.OwnerId == dbPlayer.Id)
                    {
                        if (dbPlayer.Money >= 500000)
                        {
                            dbPlayer.removeMoney(500000);

                            houses.Remove(house);
                            house.KellerBuilt = !house.KellerBuilt;
                            houses.Add(house);

                            MySqlQuery mySqlQuery =
                                new MySqlQuery("UPDATE houses SET KellerBuilt = @see WHERE Id = @id");
                            mySqlQuery.AddParameter("@see", Convert.ToInt32(house.KellerBuilt));
                            mySqlQuery.AddParameter("@id", house.Id);
                            MySqlHandler.ExecuteSync(mySqlQuery);

                            dbPlayer.SendNotification("Du hast den Keller ausgebaut.", 3000, "green");

                        }
                        else
                        {
                            dbPlayer.SendNotification("Du besitzt zu wenig Geld!", 3000, "red");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Keller] " + ex.Message);
                Logger.Print("[EXCEPTION Keller] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Hauskauf")]
        public void Hauskauf(Client c, string selection)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                dbPlayer.CloseNativeMenu();

                if (!selection.Contains("Buy")) return;

                int houseId = 0;
                bool houseId2 = int.TryParse(selection.Split("-")[1], out houseId);
                
                if (!houseId2) return;

                House house = houses.FirstOrDefault((House house) => house.Id == houseId);
                if (house == null) return;

                if (house.OwnerId == 0)
                {
                    if (dbPlayer.Money >= house.Price)
                    {
                        House house2 = HouseModule.houses.FirstOrDefault((House house2) =>
                            house2.OwnerId == dbPlayer.Id || house2.TenantsIds.Contains(dbPlayer.Id));
                        if (house2 != null)
                        {
                            dbPlayer.SendNotification("Du besitzt bereits ein Haus!", 3000, "red");
                            return;
                        }

                        dbPlayer.removeMoney(house.Price);
                        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE houses SET OwnerId = @ownerid WHERE Id = @id");
                        mySqlQuery.AddParameter("@ownerid", dbPlayer.Id);
                        mySqlQuery.AddParameter("@id", house.Id);
                        MySqlHandler.ExecuteSync(mySqlQuery);
                        dbPlayer.SendNotification("Du hast dieses Haus erfolgreich gekauft.", 3000, "green");

                        houses.Remove(house);
                        house.OwnerId = dbPlayer.Id;
                        houses.Add(house);
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du besitzt zu wenig Geld!", 3000, "red");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Hauskauf] " + ex.Message);
                Logger.Print("[EXCEPTION Hauskauf] " + ex.StackTrace);
            }
        }
    }
}
