using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    public class HeliShopModule : GVMP.Module.Module<HeliShopModule>
    {
        public static List<HeliShop> helishopList = new List<HeliShop>();

        protected override bool OnLoad()
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM helishops";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            helishopList.Add(new HeliShop
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Position = NAPI.Util.FromJson<Vector3>(reader.GetString("Position")),
                                CarSpawn = NAPI.Util.FromJson<Vector3>(reader.GetString("CarSpawn")),
                                CarSpawnRotation = reader.GetFloat("CarSpawnRotation"),
                                BuyItems = NAPI.Util.FromJson<List<BuyCar>>(reader.GetString("BuyItems"))
                            });
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
                Logger.Print("[EXCEPTION loadFahrzeugShops] " + ex.Message);
                Logger.Print("[EXCEPTION loadFahrzeugShops] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }

            foreach (HeliShop helishop in helishopList)
            {
                ColShape val = NAPI.ColShape.CreateCylinderColShape(helishop.Position, 1.4f, 1.4f, 0);
                val.SetData("FUNCTION_MODEL", new FunctionModel("openHeliShop", helishop.Name, NAPI.Util.ToJson(helishop.BuyItems)));
                val.SetData("MESSAGE", new Message("Benutze E um den Helihandel zu öffnen.", helishop.Name, "white"));

                NAPI.Marker.CreateMarker(1, helishop.Position.Subtract(new Vector3(0, 0, 0.3)), new Vector3(), new Vector3(), 0.8f, new Color(255, 165, 0), false, 0);
                NAPI.Blip.CreateBlip(225, helishop.Position, 1f, 0, helishop.Name, byte.MaxValue, 0f, true, 0, 0);
            }
            return true;
        }

        [RemoteEvent("openHeliShop")]
        public static void openHeliShop(Client client, string name, string items)
        {
            try
            {
                if (client == null) return;

                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<BuyCar> list = NAPI.Util.FromJson<List<BuyCar>>(items);
                List<NativeItem> list2 = new List<NativeItem>();
                foreach (BuyCar item in list)
                {
                    list2.Add(new NativeItem(item.Vehicle_Name + " - " + item.Price + " $",
                        item.Vehicle_Name + "-" + name));
                }

                NativeMenu nativeMenu = new NativeMenu("Helihandel", name, list2);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openVehicleShop] " + ex.Message);
                Logger.Print("[EXCEPTION openVehicleShop] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Helihandel")]
        public static void Helihandel(Client client, string selection)
        {
            if (client == null) return;
            
            DbPlayer dbPlayer = client.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                string[] args = selection.Split("-");
                HeliShop heliShop = helishopList.FirstOrDefault((HeliShop a) => a.Name == args[1]);
                if (heliShop == null) return;
                BuyCar buyCar = heliShop.BuyItems.FirstOrDefault((BuyCar i) => i.Vehicle_Name == args[0]);
                if (buyCar == null) return;
                if (dbPlayer.Money >= buyCar.Price)
                {
                    Vehicle val = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(buyCar.Vehicle_Name.ToLower()), heliShop.CarSpawn, heliShop.CarSpawnRotation, 0, 0, "", 255, false, true, client.Dimension);
                    
                    dbPlayer.removeMoney(Convert.ToInt32(buyCar.Price));
                    client.TriggerEvent("componentServerEvent", "NativeMenu", "hide");
                    dbPlayer.SendNotification("Du hast den Heli " + buyCar.Vehicle_Name + " für " + buyCar.Price + "$ erfolgreich gekauft.", 3000, "green", "Helihandel");

                    int plate = new Random().Next(10000, 99999999);
                    int id = new Random().Next(10000, 99999999);
                    List<int> list = new List<int>();
                    list.Add(dbPlayer.Id);

                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO `vehicles` (`Id`, `Vehiclehash`, `Parked`, `OwnerId`, `Carkeys`, `Plate`) VALUES (@id, @vehiclehash, @parked, @ownerid, @carkeys, @plate)");
                    mySqlQuery.AddParameter("@vehiclehash", buyCar.Vehicle_Name.ToLower());
                    mySqlQuery.AddParameter("@parked", 0);
                    mySqlQuery.AddParameter("@ownerid", dbPlayer.Id);
                    mySqlQuery.AddParameter("@carkeys", NAPI.Util.ToJson(list));
                    mySqlQuery.AddParameter("@plate", plate.ToString());
                    mySqlQuery.AddParameter("@id", id);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    DbVehicle dbVehicle = new DbVehicle
                    {
                        Id = id,
                        Keys = list,
                        Model = buyCar.Vehicle_Name.ToLower(),
                        OwnerId = dbPlayer.Id,
                        Parked = false,
                        Plate = plate.ToString(),
                        PrimaryColor = 0,
                        SecondaryColor = 0,
                        PearlescentColor = 0,
                        Vehicle = val
                    };

                    val.SetSharedData("lockedStatus", true);
                    val.SetSharedData("kofferraumStatus", true);
                    val.SetSharedData("engineStatus", true);
                    val.Locked = true;

                    val.NumberPlate = plate.ToString();

                    val.SetData("vehicle", dbVehicle);

                    WebhookSender.SendMessage("Spieler kauft Fahrzeug", "Der Spieler " + dbPlayer.Name + " kauft das Fahrzeug " + dbVehicle.Model + " für " + buyCar.Price + "$.", Webhooks.autokauflogs, "Fahrzeugshop");
                }
                else
                {
                    client.TriggerEvent("componentServerEvent", "NativeMenu", "hide");
                    dbPlayer.SendNotification("Du besitzt nicht genügend Geld.", 3000, "red", "Autohandel");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Autohandel] " + ex.Message);
                Logger.Print("[EXCEPTION Autohandel] " + ex.StackTrace);
            }
        }
    }
}
