using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    public class FahrzeugShopModule : GVMP.Module.Module<FahrzeugShopModule>
    {
        public static List<FahrzeugShop> autoshopList = new List<FahrzeugShop>();

        protected override bool OnLoad()
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM fahrzeugshops";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            autoshopList.Add(new FahrzeugShop
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

            foreach (FahrzeugShop autoshop in autoshopList)
            {
                ColShape val = NAPI.ColShape.CreateCylinderColShape(autoshop.Position, 1.4f, 1.4f, 0);
                val.SetData("FUNCTION_MODEL", new FunctionModel("openVehicleShop", autoshop.Name, NAPI.Util.ToJson(autoshop.BuyItems)));
                val.SetData("MESSAGE", new Message("Benutze E um den Autohandel zu öffnen.", autoshop.Name, "white"));

                NAPI.Marker.CreateMarker(1, autoshop.Position.Subtract(new Vector3(0, 0, 0.3)), new Vector3(), new Vector3(), 0.8f, new Color(255, 165, 0), false, 0);
                NAPI.Blip.CreateBlip(225, autoshop.Position, 1f, 0, autoshop.Name, byte.MaxValue, 0f, true, 0, 0);
            }
            return true;
        }

        [RemoteEvent("openVehicleShop")]
        public static void openVehicleShop(Client client, string name, string items)
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

                NativeMenu nativeMenu = new NativeMenu("Autohandel", name, list2);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openVehicleShop] " + ex.Message);
                Logger.Print("[EXCEPTION openVehicleShop] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Autohandel")]
        public static void Autohandel(Client client, string selection)
        {
            if (client == null) return;
            
            DbPlayer dbPlayer = client.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                string[] args = selection.Split("-");
                FahrzeugShop autoShop = autoshopList.FirstOrDefault((FahrzeugShop a) => a.Name == args[1]);
                if (autoShop == null) return;
                BuyCar buyCar = autoShop.BuyItems.FirstOrDefault((BuyCar i) => i.Vehicle_Name == args[0]);
                if (buyCar == null) return;
                if (dbPlayer.Money >= buyCar.Price)
                {
                    Vehicle val = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(buyCar.Vehicle_Name.ToLower()), autoShop.CarSpawn, autoShop.CarSpawnRotation, 0, 0, "", 255, false, true, client.Dimension);
                    
                    dbPlayer.removeMoney(Convert.ToInt32(buyCar.Price));
                    client.TriggerEvent("componentServerEvent", "NativeMenu", "hide");
                    dbPlayer.SendNotification("Du hast das Fahrzeug " + buyCar.Vehicle_Name + " für " + buyCar.Price + "$ erfolgreich gekauft.", 3000, "green", "Autohandel");

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
