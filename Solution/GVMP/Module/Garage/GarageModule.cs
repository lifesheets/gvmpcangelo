using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class GarageModule : GVMP.Module.Module<GarageModule>
    {
        public static List<Garage> garages = new List<Garage>();
        public static List<FraktionsGarage> fraktionsGarages = new List<FraktionsGarage>();

        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM garages");
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
                            loadGarage(reader);
                        }
                    }
                }
                finally
                {
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadGarages] " + ex.Message);
                Logger.Print("[EXCEPTION loadGarages] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            return true;
        }

        public static void loadGarage(MySqlDataReader reader)
        {
            try {
                Garage garage = new Garage
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Position = NAPI.Util.FromJson<Vector3>(reader.GetString("Position")),
                    CarPoint = NAPI.Util.FromJson<Vector3>(reader.GetString("CarPoint")),
                    Rotation = reader.GetFloat("Rotation")
                };

                garages.Add(garage);

                ColShape val = NAPI.ColShape.CreateCylinderColShape(garage.Position, 1.4f, 1.4f, 0);
                val.SetData("FUNCTION_MODEL", new FunctionModel("openGarage", reader.GetInt32("id"), garage.Name));
                val.SetData("MESSAGE", new Message("Benutze E um die Garage zu öffnen.", garage.Name, "orange", 5000));

                NAPI.Marker.CreateMarker(1, garage.Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, 0);
                NAPI.Blip.CreateBlip(473, garage.Position, 1f, 0, garage.Name, 255, 0.0f, true, 0, 0);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openGarage] " + ex.Message);
                Logger.Print("[EXCEPTION openGarage] " + ex.StackTrace);
            }
        }

        [RemoteEvent("openGarage")]
        public void openGarage(Client c, int id, string name)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    if (name == null) return;
                    if (id == 0) return;

                    dbPlayer.OpenGarage(id, name, false);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION openGarage] " + ex.Message);
                    Logger.Print("[EXCEPTION openGarage] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openGarage] " + ex.Message);
                Logger.Print("[EXCEPTION openGarage] " + ex.StackTrace);
            }

        }

        [RemoteEvent("openFraktionsGarage")]
        public void openFraktionsGarage(Client c, int id, string name)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    if (name == null) return;
                    if (id == 0) return;
                    if (dbPlayer.Faction.Id != id) return;

                    c.TriggerEvent("openWindow", "Garage",
                        "{\"id\":" + id + ", \"name\": \"" + name + "\", \"fraktion\":true}");
                    dbPlayer.OpenGarage(id, name, true);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.Message);
                    Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.Message);
                Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.StackTrace);
            }

        }

        [RemoteEvent("requestVehicleList")]
        public void requestVehicleList(Client c, int id, string val, bool fraktion)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                int num = (val == "takeout") ? 1 : 0;
                List<GarageVehicle> list = new List<GarageVehicle>();
                switch (num)
                {
                    case 1:
                        if (fraktion)
                        {
                            list = FactionModule.VehicleList[dbPlayer.Faction.Id];
                        }
                        else
                        {
                            foreach (GarageVehicle garageVehicle in MySqlManager.GetParkedVehicles())
                            {
                                if (garageVehicle.Keys.Contains(dbPlayer.Id) || dbPlayer.VehicleKeys.ContainsKey(garageVehicle.Id))
                                    list.Add(garageVehicle);
                            }
                        }
                        break;
                    case 0:
                        if (fraktion)
                        {
                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                DbVehicle dbVehicle = vehicle.GetVehicle();
                                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                    continue;

                                if (dbVehicle.Fraktion != null && dbVehicle.Fraktion.Id == dbPlayer.Faction.Id && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                {
                                    list.Add(new GarageVehicle
                                    {
                                        Id = dbVehicle.Id,
                                        OwnerID = dbVehicle.OwnerId,
                                        Name = dbVehicle.Model,
                                        Plate = dbVehicle.Plate,
                                        Keys = dbVehicle.Keys
                                    });
                                }
                            }
                        }
                        else
                        {
                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                DbVehicle dbVehicle = vehicle.GetVehicle();
                                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                    continue;

                                if ((dbVehicle.OwnerId == dbPlayer.Id || dbVehicle.Keys.Contains(dbPlayer.Id) || dbPlayer.VehicleKeys.ContainsKey(dbVehicle.Id)) && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                {
                                    list.Add(new GarageVehicle
                                    {
                                        Id = dbVehicle.Id,
                                        OwnerID = dbVehicle.OwnerId,
                                        Name = dbVehicle.Model,
                                        Plate = dbVehicle.Plate,
                                        Keys = dbVehicle.Keys
                                    });
                                }
                            }
                        }
                        break;
                }

                dbPlayer.responseVehicleList(NAPI.Util.ToJson(list));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION requestVehicleList] " + ex.Message);
                Logger.Print("[EXCEPTION requestVehicleList] " + ex.StackTrace);
            }
        }

        /*  [RemoteEvent("requestVehicle")]
          public void requestVehicle(Client c, string state, int garageid, int vehicleid, bool fraktion)
          {
              if (c == null) return;
              DbPlayer dbPlayer = c.GetPlayer();
              if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                  return;

              if (!dbPlayer.CanInteractAntiFlood(2)) return;

              try
              {
                  int num = (state == "takeout") ? 1 : 0;
                  switch (num)
                  {
                      case 1:
                          if (fraktion)
                          {
                              List<GarageVehicle> garageVehicles = FactionModule.VehicleList[dbPlayer.Faction.Id];
                              GarageVehicle garageVehicle = garageVehicles.FirstOrDefault((GarageVehicle gv) => gv.Id == vehicleid);
                              MySqlQuery mySqlQuery5 = new MySqlQuery("SELECT * FROM fraktion_vehicles WHERE FactionId = @factionid AND Model = @model LIMIT 1");
                              mySqlQuery5.AddParameter("@factionid", dbPlayer.Faction.Id);
                              mySqlQuery5.AddParameter("@model", garageVehicle.Name);
                              MySqlResult mySqlResult2 = MySqlHandler.GetQuery(mySqlQuery5);
                              MySqlDataReader mySqlDataReader = MySqlHandler.GetQuery(mySqlQuery5).Reader;
                              mySqlDataReader.Read();

                              if (garageVehicle == null) return;

                              Vehicle val = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(garageVehicle.Name.ToLower()), dbPlayer.Faction.GarageSpawn, dbPlayer.Faction.GarageSpawnRotation, 0, 0, "", 255, false, true, c.Dimension);



                              DbVehicle dbVehicle = new DbVehicle
                              {
                                  Id = garageVehicle.Id,
                                  Keys = new List<int>(),
                                  Model = garageVehicle.Name.ToLower(),
                                  OwnerId = garageVehicle.OwnerID,
                                  Parked = false,
                                  Plate = dbPlayer.Faction.Short.ToUpper(),
                                  PrimaryColor = 0,
                                  SecondaryColor = 0,
                                  PearlescentColor = 0,
                                  WindowTint = 0,
                                  Vehicle = val,
                                  Fraktion = dbPlayer.Faction
                              };

                              val.SetSharedData("headlightColor", mySqlDataReader.GetInt32("HeadlightColor"));

                              val.CustomPrimaryColor = dbPlayer.Faction.RGB;
                              val.CustomSecondaryColor = dbPlayer.Faction.RGB;

                              val.NumberPlate = dbPlayer.Faction.Short.ToUpper();

                              if(garageVehicle.Name == "drafter")
                              { 
                              val.SetMod(11, 3);
                              val.SetMod(12, 2);
                              val.SetMod(13, 2);
                              val.SetMod(15, 2);
                              val.SetMod(0, 5);
                              val.SetMod(46, 1);
                              val.SetMod(6, 2);
                                  val.SetMod(3, 2);
                                  val.SetMod(2, 2);
                                  val.SetMod(48, 4);
                                  val.SetMod(1, 2);
                              val.SetMod(23, 55);
                              val.SetMod(18, 0);
                              val.WindowTint = 2;
                              }
                              if (garageVehicle.Name == "fibd2")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 5);
                                  val.SetMod(46, 1);
                                  val.SetMod(6, 2);
                                  val.SetMod(3, 2);
                                  val.SetMod(2, 2);
                                  val.SetMod(48, 4);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 55);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "jugular")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 0);
                                  val.SetMod(10, -1);
                                  val.SetMod(4, 1);
                                  val.SetMod(3, 2);
                                  val.SetMod(7, 3);
                                  val.SetMod(46, 1);
                                  val.SetMod(48, 1);
                                  val.SetMod(6, 1);
                                  val.SetMod(2, 0);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 59);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "fibj")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 0);
                                  val.SetMod(10, -1);
                                  val.SetMod(4, 1);
                                  val.SetMod(3, 2);
                                  val.SetMod(7, 3);
                                  val.SetMod(46, 1);
                                  val.SetMod(48, 1);
                                  val.SetMod(6, 1);
                                  val.SetMod(2, 0);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 59);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "schafterg")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 1);
                                  val.SetMod(5, -1);
                                  val.SetMod(4, 1);
                                  val.SetMod(46, 1);
                                  val.SetMod(48, 1);
                                  val.SetMod(6, 1);
                                  val.SetMod(10, 1);
                                  val.SetMod(2, 0);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 59);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;

                              }
                              else if (garageVehicle.Name == "umkbuffals")
                                  {
                                      val.WindowTint = 2;
                                  }
                              else if (garageVehicle.Name == "gt63samgf")
                              {
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "fibn")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 1);
                                  val.SetMod(5, -1);
                                  val.SetMod(4, 1);
                                  val.SetMod(46, 1);
                                  val.SetMod(48, 1);
                                  val.SetMod(6, 1);
                                  val.SetMod(2, 0);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 59);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "fibs")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 1);
                                  val.SetMod(5, -1);
                                  val.SetMod(4, 1);
                                  val.SetMod(46, 1);
                                  val.SetMod(48, 1);
                                  val.SetMod(6, 1);
                                  val.SetMod(2, 0);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 59);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "schafter6")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 1);
                                  val.SetMod(5, -1);
                                  val.SetMod(10, 1);
                                  val.SetMod(4, 1);
                                  val.SetMod(46, 1);
                                  val.SetMod(48, 1);
                                  val.SetMod(6, 1);
                                  val.SetMod(2, 0);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 59);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "schafter7")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(13, 2);
                                  val.SetMod(15, 2);
                                  val.SetMod(0, 1);
                                  val.SetMod(5, -1);
                                  val.SetMod(4, 1);
                                  val.SetMod(46, 1);
                                  val.SetMod(48, 1);
                                  val.SetMod(6, 1);
                                  val.SetMod(2, 0);
                                  val.SetMod(1, 2);
                                  val.SetMod(23, 59);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "bf400")
                              {
                                  val.SetMod(11, 3);
                                  val.SetMod(12, 2);
                                  val.SetMod(18, 0);
                                  val.WindowTint = 2;
                              }
                              dbPlayer.SendNotification("Fahrzeug ausgeparkt!", 3000, "green", "Garage");

                              val.SetSharedData("lockedStatus", true);
                              val.SetSharedData("kofferraumStatus", true);
                              val.SetSharedData("engineStatus", true);
                              val.Locked = true;

                              val.NumberPlate = dbPlayer.Faction.Short.ToUpper();

                              val.SetData("vehicle", dbVehicle);

                          }
                          else
                          {
                              MySqlQuery mySqlQuery2 = new MySqlQuery("UPDATE vehicles SET Parked = 0 WHERE Id = @id");
                              mySqlQuery2.AddParameter("@id", vehicleid);
                              MySqlHandler.ExecuteSync(mySqlQuery2);

                              MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id LIMIT 1");
                              mySqlQuery.AddParameter("@id", vehicleid);
                              MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                              MySqlDataReader reader = mySqlResult.Reader;

                              if (reader.HasRows)
                              {
                                  reader.Read();

                                  Garage garage = garages.FirstOrDefault((Garage garage2) => garage2.Id == garageid);

                                  if (garage == null) return;

                                      Vehicle val = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(reader.GetString("Vehiclehash")), garage.CarPoint, garage.Rotation, 0, 0, "", 255, false, true, c.Dimension);




                                  Dictionary<int, int> tuningDict = new Dictionary<int, int>();
                                  string savedTuning = reader.GetString("Tuning");
                                  if (savedTuning != null && savedTuning != "[]") tuningDict = NAPI.Util.FromJson<Dictionary<int, int>>(savedTuning);

                                  DbVehicle dbVehicle = new DbVehicle
                                  {
                                      Id = reader.GetInt32("Id"),
                                      Keys = NAPI.Util.FromJson<List<int>>(reader.GetString("Carkeys")),
                                      Model = reader.GetString("Vehiclehash"),
                                      OwnerId = reader.GetInt32("OwnerId"),
                                      Parked = Convert.ToBoolean(reader.GetInt32("Parked")),
                                      Plate = reader.GetString("Plate"),
                                      PrimaryColor = reader.GetInt32("PrimaryColor"),
                                      SecondaryColor = reader.GetInt32("SecondaryColor"),
                                      PearlescentColor = reader.GetInt32("PearlescentColor"),
                                      WindowTint = reader.GetInt32("WindowTint"),
                                      Tuning = tuningDict,
                                      Vehicle = val
                                  };

                                  foreach (KeyValuePair<int, int> keyValuePair in tuningDict)
                                  {
                                      val.SetMod(keyValuePair.Key, keyValuePair.Value);
                                  }

                                  val.Neons = Convert.ToBoolean(dbVehicle.GetAttributeInt("Neons"));
                                  val.NeonColor = NAPI.Util.FromJson<Color>(dbVehicle.GetAttributeString("NeonColor"));

                                  val.SetSharedData("headlightColor", dbVehicle.GetAttributeInt("HeadlightColor"));

                                  val.PrimaryColor = dbVehicle.PrimaryColor;
                                  val.SecondaryColor = dbVehicle.SecondaryColor;
                                  val.PearlescentColor = dbVehicle.PearlescentColor;
                                  val.WindowTint = dbVehicle.WindowTint;

                                  dbPlayer.SendNotification("Fahrzeug ausgeparkt!", 3000, "green", "Garage");

                                  val.NumberPlate = dbVehicle.Plate;

                                  val.SetData("vehicle", dbVehicle);
                                  val.SetSharedData("lockedStatus", true);
                                  val.SetSharedData("kofferraumStatus", true);
                                  val.SetSharedData("engineStatus", true);
                                  val.Locked = true;
                              }
                          }



                          break;
                      case 0:
                          if (fraktion)
                          {
                              foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                              {
                                  DbVehicle dbVehicle = vehicle.GetVehicle();
                                  if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                      return;

                                  if (dbVehicle.Fraktion != null && dbVehicle.Fraktion.Id == dbPlayer.Faction.Id && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                  {
                                      dbPlayer.SendNotification("Du hast das Fahrzeug " + dbVehicle.Model + " erfolgreich eingeparkt.", 3000, "green", "Garage");
                                      vehicle.Delete();
                                      break;
                                  }
                              }
                          }
                          else
                          {

                              foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                              {
                                  DbVehicle dbVehicle = vehicle.GetVehicle();
                                  if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                      return;

                                  if (dbVehicle.Id == vehicleid && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                  {
                                      MySqlQuery mySqlQuery3 =
                                          new MySqlQuery("UPDATE vehicles SET Parked = 1 WHERE Id = @id");
                                      mySqlQuery3.AddParameter("@id", vehicleid);
                                      MySqlHandler.ExecuteSync(mySqlQuery3);

                                      dbPlayer.SendNotification(
                                          "Du hast das Fahrzeug " + dbVehicle.Model + " erfolgreich eingeparkt.",
                                          3000, "green", "Garage");
                                      vehicle.Delete();
                                      break;
                                  }
                              }
                          }
                          break;
                  }
              }
              catch (Exception ex)
              {
                  Logger.Print("[EXCEPTION requestVehicle] " + ex.Message);
                  Logger.Print("[EXCEPTION requestVehicle] " + ex.StackTrace);
              }
          }*/

        [RemoteEvent("requestVehicle")]
        public Vehicle? requestVehicle(Client c, string state, int garageid, int vehicleid, bool fraktion)
        {
            Predicate<Garage> predicate = null;
            if (c != null)
            {
                DbPlayer player = c.GetPlayer();
                if ((player == null || !player.IsValid(true) ? false : player.Client != null))
                {
                    if (player.CanInteractAntiFlood(1))
                    {
                        try
                        {
                            int num = (state == "takeout" ? 1 : 0);
                            if (num != 0)
                            {
                                if (num == 1)
                                {
                                    if (!fraktion)
                                    {
                                        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET Parked = 0 WHERE Id = @id");
                                        mySqlQuery.AddParameter("@id", vehicleid);
                                        MySqlHandler.ExecuteSync(mySqlQuery);
                                        MySqlQuery mySqlQuery1 = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id LIMIT 1");
                                        mySqlQuery1.AddParameter("@id", vehicleid);
                                        MySqlDataReader reader = MySqlHandler.GetQuery(mySqlQuery1).Reader;
                                        if (reader.HasRows)
                                        {
                                            while (reader.Read())
                                            {
                                                List<Garage> list = GarageModule.garages;
                                                Predicate<Garage> predicate1 = predicate;
                                                if (predicate1 == null)
                                                {
                                                    Predicate<Garage> predicate2 = new Predicate<Garage>((Garage garage2) => garage2.Id == garageid);
                                                    Predicate<Garage> predicate3 = predicate2;
                                                    predicate = predicate2;
                                                    predicate1 = predicate3;
                                                }
                                                Garage garage = list.Find(predicate1);
                                                if (garage != null)
                                                {
                                                    Vehicle vehicle1 = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(reader.GetString("Vehiclehash")), garage.CarPoint, garage.Rotation, 0, 0, "", 255, false, true, c.Dimension);
                                                    Dictionary<int, int> dictionary = new Dictionary<int, int>();
                                                    string str = reader.GetString("Tuning");
                                                    if ((str == null ? false : str != "[]"))
                                                    {
                                                        dictionary = NAPI.Util.FromJson<Dictionary<int, int>>(str);
                                                    }
                                                    DbVehicle dbVehicle = new DbVehicle()
                                                    {
                                                        Id = reader.GetInt32("Id"),
                                                        Keys = NAPI.Util.FromJson<List<int>>(reader.GetString("Carkeys")),
                                                        Model = reader.GetString("Vehiclehash"),
                                                        OwnerId = reader.GetInt32("OwnerId"),
                                                        Parked = Convert.ToBoolean(reader.GetInt32("Parked")),
                                                        Plate = reader.GetString("Plate"),
                                                        PrimaryColor = reader.GetInt32("PrimaryColor"),
                                                        SecondaryColor = reader.GetInt32("SecondaryColor"),
                                                        PearlescentColor = reader.GetInt32("PearlescentColor"),
                                                        WindowTint = reader.GetInt32("WindowTint"),
                                                        Tuning = dictionary,
                                                        Vehicle = vehicle1
                                                    };
                                                    foreach (KeyValuePair<int, int> keyValuePair in dictionary)
                                                    {
                                                        vehicle1.SetMod(keyValuePair.Key, keyValuePair.Value);
                                                    }
                                                    vehicle1.Neons = (bool)Convert.ToBoolean((dynamic)dbVehicle.GetAttributeInt("Neons"));
                                                    vehicle1.NeonColor = (Color)NAPI.Util.FromJson<Color>((dynamic)dbVehicle.GetAttributeString("NeonColor"));
                                                    vehicle1.SetSharedData("headlightColor", (dynamic)dbVehicle.GetAttributeInt("HeadlightColor"));
                                                    vehicle1.PrimaryColor = dbVehicle.PrimaryColor;
                                                    vehicle1.SecondaryColor = dbVehicle.SecondaryColor;
                                                    vehicle1.PearlescentColor = dbVehicle.PearlescentColor;
                                                    vehicle1.WindowTint = dbVehicle.WindowTint;
                                                    player.SendNotification("Fahrzeug ausgeparkt!", 3000, "green", "Garage");
                                                    vehicle1.NumberPlate = dbVehicle.Plate;
                                                    vehicle1.SetData("vehicle", dbVehicle);
                                                    vehicle1.SetSharedData("lockedStatus", true);
                                                    vehicle1.SetSharedData("kofferraumStatus", true);
                                                    vehicle1.SetSharedData("engineStatus", true);
                                                    vehicle1.Locked = true;
                                                    return vehicle1;
                                                }
                                                else
                                                {
                                                    return null;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        List<GarageVehicle> item = FactionModule.VehicleList[player.Faction.Id];
                                        GarageVehicle garageVehicle = item.Find(new Predicate<GarageVehicle>((GarageVehicle gv) => gv.Id == vehicleid));
                                        if (player.Dimension == FactionModule.GangwarDimension)
                                        {
                                            garageVehicle = new GarageVehicle()
                                            {
                                                Id = (new Random()).Next(10000, 99999999),
                                                Keys = new List<int>(),
                                                Name = "revolter",
                                                Plate = player.Faction.Short,
                                                OwnerID = 0
                                            };
                                        }
                                        if (garageVehicle != null)
                                        {
                                            MySqlQuery mySqlQuery2 = new MySqlQuery("SELECT * FROM fraktion_vehicles WHERE FactionId = @factionid AND Model = @model LIMIT 1");
                                            mySqlQuery2.AddParameter("@factionid", player.Faction.Id);
                                            mySqlQuery2.AddParameter("@model", garageVehicle.Name);
                                            MySqlDataReader mySqlDataReader = MySqlHandler.GetQuery(mySqlQuery2).Reader;
                                            Vehicle vehicle2 = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(garageVehicle.Name.ToLower()), player.Faction.GarageSpawn, player.Faction.GarageSpawnRotation, 0, 0, "", 255, false, true, c.Dimension);
                                            DbVehicle dbVehicle1 = new DbVehicle()
                                            {
                                                Id = garageVehicle.Id,
                                                Keys = new List<int>(),
                                                Model = garageVehicle.Name.ToLower(),
                                                OwnerId = garageVehicle.OwnerID,
                                                Parked = false,
                                                Plate = player.Faction.Short.ToUpper(),
                                                PrimaryColor = 0,
                                                SecondaryColor = 0,
                                                PearlescentColor = 0,
                                                WindowTint = 0,
                                                Vehicle = vehicle2,
                                                Fraktion = player.Faction
                                            };
                                            if (player.Faction.CustomCarColor != 0)
                                            {
                                                vehicle2.PrimaryColor = player.Faction.CustomCarColor;
                                                vehicle2.SecondaryColor = player.Faction.CustomCarColor;
                                            }
                                            else
                                            {
                                                vehicle2.CustomPrimaryColor = player.Faction.RGB;
                                                vehicle2.CustomSecondaryColor = player.Faction.RGB;
                                            }
                                            vehicle2.NumberPlate = player.Faction.Short.ToUpper();

                                            
                              if(garageVehicle.Name == "drafter")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 5);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(6, 2);
                                                vehicle2.SetMod(3, 2);
                                                vehicle2.SetMod(2, 2);
                                                vehicle2.SetMod(48, 4);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 55);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              if (garageVehicle.Name == "fibd2")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 5);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(6, 2);
                                                vehicle2.SetMod(3, 2);
                                                vehicle2.SetMod(2, 2);
                                                vehicle2.SetMod(48, 4);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 55);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "jugular")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 0);
                                                vehicle2.SetMod(10, -1);
                                                vehicle2.SetMod(4, 1);
                                                vehicle2.SetMod(3, 2);
                                                vehicle2.SetMod(7, 3);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(48, 1);
                                                vehicle2.SetMod(6, 1);
                                                vehicle2.SetMod(2, 0);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 59);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "fibj")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 0);
                                                vehicle2.SetMod(10, -1);
                                                vehicle2.SetMod(4, 1);
                                                vehicle2.SetMod(3, 2);
                                                vehicle2.SetMod(7, 3);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(48, 1);
                                                vehicle2.SetMod(6, 1);
                                                vehicle2.SetMod(2, 0);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 59);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "schafterg")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 1);
                                                vehicle2.SetMod(5, -1);
                                                vehicle2.SetMod(4, 1);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(48, 1);
                                                vehicle2.SetMod(6, 1);
                                                vehicle2.SetMod(10, 1);
                                                vehicle2.SetMod(2, 0);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 59);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;

                              }
                              else if (garageVehicle.Name == "umkbuffals")
                                  {
                                                vehicle2.WindowTint = 2;
                                  }
                              else if (garageVehicle.Name == "gt63samgf")
                              {
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "fibn")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 1);
                                                vehicle2.SetMod(5, -1);
                                                vehicle2.SetMod(4, 1);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(48, 1);
                                                vehicle2.SetMod(6, 1);
                                                vehicle2.SetMod(2, 0);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 59);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "fibs")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 1);
                                                vehicle2.SetMod(5, -1);
                                                vehicle2.SetMod(4, 1);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(48, 1);
                                                vehicle2.SetMod(6, 1);
                                                vehicle2.SetMod(2, 0);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 59);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "schafter6")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 1);
                                                vehicle2.SetMod(5, -1);
                                                vehicle2.SetMod(10, 1);
                                                vehicle2.SetMod(4, 1);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(48, 1);
                                                vehicle2.SetMod(6, 1);
                                                vehicle2.SetMod(2, 0);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 59);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "schafter7")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(13, 2);
                                                vehicle2.SetMod(15, 2);
                                                vehicle2.SetMod(0, 1);
                                                vehicle2.SetMod(5, -1);
                                                vehicle2.SetMod(4, 1);
                                                vehicle2.SetMod(46, 1);
                                                vehicle2.SetMod(48, 1);
                                                vehicle2.SetMod(6, 1);
                                                vehicle2.SetMod(2, 0);
                                                vehicle2.SetMod(1, 2);
                                                vehicle2.SetMod(23, 59);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                              else if (garageVehicle.Name == "bf400")
                              {
                                                vehicle2.SetMod(11, 3);
                                                vehicle2.SetMod(12, 2);
                                                vehicle2.SetMod(18, 0);
                                                vehicle2.WindowTint = 2;
                              }
                                            vehicle2.WindowTint = 2;
                                            player.SendNotification("Fahrzeug ausgeparkt!", 3000, "green", "Garage");
                                            vehicle2.SetSharedData("lockedStatus", true);
                                            vehicle2.SetSharedData("kofferraumStatus", true);
                                            vehicle2.SetSharedData("engineStatus", true);
                                            vehicle2.Locked = true;
                                            vehicle2.NumberPlate = player.Faction.Short.ToUpper();
                                            vehicle2.SetData("vehicle", dbVehicle1);
                                            if (mySqlDataReader.HasRows)
                                            {
                                                while (mySqlDataReader.Read())
                                                {
                                                    vehicle2.Neons = Convert.ToBoolean(mySqlDataReader.GetInt32("Neons"));
                                                    vehicle2.NeonColor = NAPI.Util.FromJson<Color>(mySqlDataReader.GetString("NeonColor"));
                                                    vehicle2.SetSharedData("headlightColor", mySqlDataReader.GetInt32("HeadlightColor"));
                                                    if (mySqlDataReader.GetInt32("PrimaryColor") != 0)
                                                    {
                                                        dbVehicle1.PrimaryColor = mySqlDataReader.GetInt32("PrimaryColor");
                                                    }
                                                    if (mySqlDataReader.GetInt32("SecondaryColor") != 0)
                                                    {
                                                        dbVehicle1.SecondaryColor = mySqlDataReader.GetInt32("SecondaryColor");
                                                    }
                                                    if (mySqlDataReader.GetInt32("PearlescentColor") != 0)
                                                    {
                                                        dbVehicle1.PearlescentColor = mySqlDataReader.GetInt32("PearlescentColor");
                                                    }
                                                    if (mySqlDataReader.GetInt32("WindowTint") != 0)
                                                    {
                                                        dbVehicle1.WindowTint = mySqlDataReader.GetInt32("WindowTint");
                                                    }
                                                    dbVehicle1.RefreshData(dbVehicle1);
                                                    if (mySqlDataReader.GetInt32("PrimaryColor") != 0)
                                                    {
                                                        vehicle2.PrimaryColor = dbVehicle1.PrimaryColor;
                                                    }
                                                    if (mySqlDataReader.GetInt32("SecondaryColor") != 0)
                                                    {
                                                        vehicle2.SecondaryColor = dbVehicle1.SecondaryColor;
                                                    }
                                                    if (mySqlDataReader.GetInt32("PearlescentColor") != 0)
                                                    {
                                                        vehicle2.PearlescentColor = dbVehicle1.PearlescentColor;
                                                    }
                                                    if (mySqlDataReader.GetInt32("WindowTint") != 0)
                                                    {
                                                        vehicle2.WindowTint = dbVehicle1.WindowTint;
                                                    }
                                                    vehicle2.NumberPlate = player.Faction.Short.ToUpper();
                                                    Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
                                                    string str1 = mySqlDataReader.GetString("Tuning");
                                                    if ((str1 == null ? false : str1 != "[]"))
                                                    {
                                                        dictionary1 = NAPI.Util.FromJson<Dictionary<int, int>>(str1);
                                                    }
                                                    foreach (KeyValuePair<int, int> keyValuePair1 in dictionary1)
                                                    {
                                                        vehicle2.SetMod(keyValuePair1.Key, keyValuePair1.Value);
                                                    }
                                                }
                                            }

                                            return vehicle2;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                }
                            }
                            else if (!fraktion)
                            {
                                Vehicle vehicle3 = NAPI.Pools.GetAllVehicles().Find(new Predicate<Vehicle>((Vehicle veh) => (veh.GetVehicle() == null ? false : veh.GetVehicle().Id == vehicleid)));
                                DbVehicle dbVehicle2 = vehicle3.GetVehicle();
                                if (vehicle3 == null)
                                {
                                    return null;
                                }
                                else if (dbVehicle2 == null)
                                {
                                    return null;
                                }
                                else if (vehicle3.Position.DistanceTo(player.Position) < 30f)
                                {
                                    MySqlQuery mySqlQuery3 = new MySqlQuery("UPDATE vehicles SET Parked = 1 WHERE Id = @id");
                                    mySqlQuery3.AddParameter("@id", vehicleid);
                                    MySqlHandler.ExecuteSync(mySqlQuery3);
                                    player.SendNotification(string.Concat("Du hast das Fahrzeug ", vehicle3.GetVehicle().Model, " erfolgreich eingeparkt."), 3000, "green", "Garage");
                                    vehicle3.Delete();
                                }
                            }
                            else
                            {
                                List<Vehicle> list1 = NAPI.Pools.GetAllVehicles().FindAll(new Predicate<Vehicle>((Vehicle veh) => (veh.GetVehicle() == null || veh.GetVehicle().Fraktion == null || veh.GetVehicle().Fraktion.Id != player.Faction.Id ? false : veh.Position.DistanceTo(player.Position) < 30f)));
                                list1.ForEach(new Action<Vehicle>((Vehicle veh) => {
                                    DbVehicle vehicle = veh.GetVehicle();
                                    if (vehicle != null)
                                    {
                                        player.SendNotification(string.Concat("Du hast das Fahrzeug ", vehicle.Model, " erfolgreich eingeparkt."), 3000, player.Faction.GetRGBStr(), player.Faction.Name);
                                        veh.Delete();
                                    }
                                }));
                            }
                        }
                        catch (Exception exception1)
                        {
                            Exception exception = exception1;
                            Logger.Print(string.Concat("[EXCEPTION requestVehicle] ", exception.Message));
                            Logger.Print(string.Concat("[EXCEPTION requestVehicle] ", exception.StackTrace));
                        }
                    }
                }
                return null;
            }
            return null;
        }
    }
}
