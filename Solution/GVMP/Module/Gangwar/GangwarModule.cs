using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GVMP
{
    class GangwarModule : GVMP.Module.Module<GangwarModule>
    {
        public static List<Gangwar> GWZones = new List<Gangwar>();
        public static List<Gangwar> BlockedZones = new List<Gangwar>();
        public static Gangwar RunningGangwar = null;

        public override Type[] RequiredModules() => new Type[1]
        {
            typeof (FactionModule)
        };
        
        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM gangwars");
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
            MySqlDataReader reader = mySqlResult.Reader;

            while (reader.Read())
            {
                Gangwar gangwar = new Gangwar
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Faction = FactionModule.getFactionById(reader.GetInt32("Faction")),
                    Zone = NAPI.Util.FromJson<Vector3>(reader.GetString("Zone")),
                    Flag1 = new Flag(NAPI.Util.FromJson<Vector3>(reader.GetString("Flag1"))),
                    Flag2 = new Flag(NAPI.Util.FromJson<Vector3>(reader.GetString("Flag2"))),
                    Flag3 = new Flag(NAPI.Util.FromJson<Vector3>(reader.GetString("Flag3"))),
                    Flag4 = new Flag(NAPI.Util.FromJson<Vector3>(reader.GetString("Flag4"))),
                    Attacker = null,
                    AttackerPoints = 0,
                    FactionPoints = 0,
                    StopDate = DateTime.Now
                };
                GWZones.Add(gangwar);
                ColShape c = NAPI.ColShape.CreateCylinderColShape(gangwar.Zone, 1.4f, 1.4f, 0);
                c.SetData("FUNCTION_MODEL", new FunctionModel("StartGangwar", gangwar.Id));
                c.SetData("MESSAGE", new Message("Benutze E um einen Gangwar zu starten.", "GANGWAR", "orange", 3000));

                NAPI.Blip.CreateBlip(543, gangwar.Zone, 1.0f, (byte)gangwar.Faction.Blip, gangwar.Name + " - " + gangwar.Faction.Name, 255, 0, true, 0, uint.MaxValue);
                NAPI.Marker.CreateMarker(1, gangwar.Zone, new Vector3(), new Vector3(), 1.0f, gangwar.Faction.RGB, false, 0);
            }

            reader.Dispose();
            mySqlResult.Connection.Dispose();
            return true;
        }
        public static Gangwar FindGWById(int Id)
        {
            return GWZones.FirstOrDefault((Gangwar GWZone) => GWZone.Id == Id);
        }

        [RemoteEvent("StartGangwar")]
        public void StartGangwar(Client c, int id)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.Faction.Id == 0) return;

                List<NativeItem> nativeItems = new List<NativeItem>
                {
                    new NativeItem("Gangwar starten", id.ToString())
                };
                NativeMenu nativeMenu = new NativeMenu("Gangwar", "", nativeItems);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION StartGangwar] " + ex.Message);
                Logger.Print("[EXCEPTION StartGangwar] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Gangwar")]
        public void Gangwar(Client c, string selection)
        {
            try
            {
                if (c == null) return;
                int id = 0;
                bool id2 = int.TryParse(selection, out id);
                if (!id2) return;
                if (id == 0) return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

                if (dbPlayer.Faction.Id == 0) return;

                dbPlayer.CloseNativeMenu();

                Gangwar gw = FindGWById(id);

                if (gw == null) return;
                if (dbPlayer.Faction == gw.Faction) return;

                if (gw.Faction.GetFactionPlayers().Count < 3)
                {
                    dbPlayer.SendNotification("Es müssen mindestens 3 Personen aus der anderen Fraktion online sein.", 3000, "orange", "GANGWAR");
                    return;
                }
                if (BlockedZones.Contains(gw))
                {
                    dbPlayer.SendNotification("Dieses Gebiet wurde bereits angegriffen.", 3000, "orange", "GANGWAR");
                    return;
                }
                if (RunningGangwar != null)
                {
                    dbPlayer.SendNotification("Es läuft bereits ein Gangwar.", 3000, "orange", "GANGWAR");
                    return;
                }
                if (gw.Faction.Id == 0)
                {
                    dbPlayer.SendNotification("Du hast das Gebiet eingenommen.", 3000, "orange", "GANGWAR");
                    GWZones.Remove(gw);
                    gw.Faction = dbPlayer.Faction;
                    GWZones.Add(gw);
                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE gangwars SET Faction = @faction WHERE Id = @id");
                    mySqlQuery.AddParameter("@id", gw.Id);
                    mySqlQuery.AddParameter("@faction", dbPlayer.Faction.Id);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                    return;
                }
                gw.Attacker = dbPlayer.Faction;
                dbPlayer.SetDimension(FactionModule.GangwarDimension);

                foreach (DbPlayer dbTarget in gw.Faction.GetFactionPlayers())
                {
 
                        dbTarget.SendNotification($"Das Gebiet {gw.Name} wird von der Fraktion {gw.Attacker.Name} angegriffen.", 6000, "orange", "GANGWAR");
                }
                foreach (DbPlayer dbTarget in gw.Attacker.GetFactionPlayers())
                {
                    if (dbTarget != null && dbTarget.IsValid(true))
                        dbTarget.SendNotification($"Deine Fraktion greift das Gebiet {gw.Name} an.", 5000, "orange", "GANGWAR");
                }
                Notification.SendGlobalNotification($"Die Fraktion {gw.Attacker.Name} greift das Gebiet {gw.Name} von der Fraktion {gw.Faction.Name} an.", 8000, "orange", Notification.icon.warn);
               
                BlockedZones.Add(gw);
                RunningGangwar = gw;

                gw.StopDate = DateTime.Now.AddMinutes(30);

                ColShape col = NAPI.ColShape.CreateCylinderColShape(gw.Zone, 300, 25f, Convert.ToUInt32(FactionModule.GangwarDimension));
                col.SetData("GANGWAR", true);

                Marker m = NAPI.Marker.CreateMarker(1, gw.Zone, new Vector3(), new Vector3(), 600, new Color(240, 132, 0), false, Convert.ToUInt32(FactionModule.GangwarDimension));
                m.SetData("GANGWAR", true);

                ColShape c1 = NAPI.ColShape.CreateCylinderColShape(gw.Flag1.Position.Add(new Vector3(0, 0, 1)), 1.4f, 1.4f, Convert.ToUInt32(FactionModule.GangwarDimension));
                c1.SetData("GANGWAR_FLAG", 1);

                NAPI.Marker.CreateMarker(4, gw.Flag1.Position.Add(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, Convert.ToUInt32(FactionModule.GangwarDimension));
                
                ColShape c2 = NAPI.ColShape.CreateCylinderColShape(gw.Flag2.Position.Add(new Vector3(0, 0, 1)), 1.4f, 1.4f, Convert.ToUInt32(FactionModule.GangwarDimension));
                c2.SetData("GANGWAR_FLAG", 2);

                NAPI.Marker.CreateMarker(4, gw.Flag2.Position.Add(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, Convert.ToUInt32(FactionModule.GangwarDimension));
               
                ColShape c3 = NAPI.ColShape.CreateCylinderColShape(gw.Flag3.Position.Add(new Vector3(0, 0, 1)), 1.4f, 1.4f, Convert.ToUInt32(FactionModule.GangwarDimension));
                c3.SetData("GANGWAR_FLAG", 3);

                NAPI.Marker.CreateMarker(4, gw.Flag3.Position.Add(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, Convert.ToUInt32(FactionModule.GangwarDimension));
                
                ColShape c4 = NAPI.ColShape.CreateCylinderColShape(gw.Flag4.Position.Add(new Vector3(0, 0, 1)), 1.4f, 1.4f, Convert.ToUInt32(FactionModule.GangwarDimension));
                c4.SetData("GANGWAR_FLAG", 4);

                NAPI.Marker.CreateMarker(4, gw.Flag4.Position.Add(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, Convert.ToUInt32(FactionModule.GangwarDimension));
            }
            catch(Exception ex)
            {
                Logger.Print("[EXCEPTION Gangwar] " + ex.Message);
                Logger.Print("[EXCEPTION Gangwar] " + ex.StackTrace);
            }
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public void EnterGWZone(ColShape col, Client c)
        {
            try
            {
                if (c == null || col == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;
                if (RunningGangwar == null) return;
                if (dbPlayer.Faction != RunningGangwar.Faction && dbPlayer.Faction != RunningGangwar.Attacker) return;
                if (col.HasData("GANGWAR"))
                {
                    dbPlayer.SetData("IN_GANGWAR", true);
                    c.GiveWeapon(WeaponHash.Gusenberg, 9999);
                    c.GiveWeapon(WeaponHash.AdvancedRifle, 9999);
                    c.GiveWeapon(WeaponHash.AssaultRifle, 9999);
                    c.GiveWeapon(WeaponHash.Pistol50, 9999);
                    c.GiveWeapon(WeaponHash.Pistol, 9999);
                    c.TriggerEvent("initializeGangwar", RunningGangwar.Faction.Short, RunningGangwar.Attacker.Short,
                        RunningGangwar.Faction.Id, RunningGangwar.Attacker.Id,
                        (int) (RunningGangwar.StopDate - DateTime.Now).TotalSeconds, RunningGangwar.Faction.Logo,
                        RunningGangwar.Attacker.Logo, RunningGangwar.Faction.GetRGBStr(),
                        RunningGangwar.Attacker.GetRGBStr());
                }
                else if (col.HasData("GANGWAR_FLAG"))
                {
                    int data = (int)((dynamic)col.GetData("GANGWAR_FLAG"));
                    dbPlayer.SendNotification(string.Concat("Du hast die Flagge ", data.ToString(), " betreten."), 3000, "orange", "GANGWAR");
                    switch (data)
                    {
                        case 1:
                            {
                                RunningGangwar.Flag1.Faction = dbPlayer.Faction.Id;
                                break;
                            }
                        case 2:
                            {
                                RunningGangwar.Flag2.Faction = dbPlayer.Faction.Id;
                                break;
                            }
                        case 3:
                            {
                                RunningGangwar.Flag3.Faction = dbPlayer.Faction.Id;
                                break;
                            }
                        case 4:
                            {
                                RunningGangwar.Flag4.Faction = dbPlayer.Faction.Id;
                                break;
                            }
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION EnterGWZone] " + ex.Message);
                Logger.Print("[EXCEPTION EnterGWZone] " + ex.StackTrace);
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        public void ExitGWZone(ColShape col, Client c)
        {
            try
            {
                if (c == null || col == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;
                if (RunningGangwar == null) return;
                if (dbPlayer.Faction != RunningGangwar.Faction && dbPlayer.Faction != RunningGangwar.Attacker) return;
                if (col.HasData("GANGWAR"))
                {
                    dbPlayer.ResetData("IN_GANGWAR");
                    c.RemoveAllWeapons();
                    WeaponManager.loadWeapons(c);
                    c.TriggerEvent("finishGangwar");
                }
                else if (col.HasData("GANGWAR_FLAG"))
                {
                    int data = (int)((dynamic)col.GetData("GANGWAR_FLAG"));
                    dbPlayer.SendNotification("Du hast die Flagge " + data + " verlassen.", 3000, "orange", "GANGWAR");

                    switch (data)
                    {
                        case 1:
                            {
                                RunningGangwar.Flag1.Faction = 0;
                                break;
                            }
                        case 2:
                            {
                                RunningGangwar.Flag2.Faction = 0;
                                break;
                            }
                        case 3:
                            {
                                RunningGangwar.Flag3.Faction = 0;
                                break;
                            }
                        case 4:
                            {
                                RunningGangwar.Flag4.Faction = 0;
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION ExitGWZone] " + ex.Message);
                Logger.Print("[EXCEPTION ExitGWZone] " + ex.StackTrace);
            }
        }


        public static void handleKill(DbPlayer killer)
        {
            try
            {
                if (killer == null || killer.Client == null) return;
                if (!killer.HasData("IN_GANGWAR") || RunningGangwar == null) return;

                if (killer.Faction.Id == RunningGangwar.Faction.Id)
                {
                    RunningGangwar.FactionPoints += 5;
                }
                else if (killer.Faction.Id == RunningGangwar.Attacker.Id)
                {
                    RunningGangwar.AttackerPoints += 5;
                }

                killer.Faction.GetFactionPlayers()
                    .ForEach((DbPlayer dbPlayer) => dbPlayer.SendNotification("+5 Punkte für das Töten eines Gegners!"));

                RunningGangwar.Faction.GetFactionPlayers().ForEach(e =>
                {
                    if (e.Client.Position.DistanceTo(RunningGangwar.Zone) < 350)
                        e.Client.TriggerEvent("updateGangwarScore", RunningGangwar.FactionPoints,
                            RunningGangwar.AttackerPoints);
                });
                RunningGangwar.Attacker.GetFactionPlayers().ForEach(e =>
                {
                    if (e.Client.Position.DistanceTo(RunningGangwar.Zone) < 350)
                        e.Client.TriggerEvent("updateGangwarScore", RunningGangwar.FactionPoints,
                            RunningGangwar.AttackerPoints);
                });
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION handleKill] " + ex.Message);
                Logger.Print("[EXCEPTION handleKill] " + ex.StackTrace);
            }
        }

        public override void OnFiveSecUpdate()
        {
            try
            {
                if (RunningGangwar == null) return;

                if (RunningGangwar.StopDate < DateTime.Now)
                {
                    EndGangwar();
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Gangwar OnFiveSecUpdate] " + ex.Message);
                Logger.Print("[EXCEPTION Gangwar OnFiveSecUpdate] " + ex.StackTrace);
            }
        }

        public override void OnMinuteUpdate()
        {
            try
            {
                if (RunningGangwar == null) return;

                if (RunningGangwar.Flag1.Faction == RunningGangwar.Faction.Id)
                {
                    RunningGangwar.Faction.GetFactionPlayers().ForEach((DbPlayer dbPlayer) => dbPlayer.SendNotification("+5 Für das Besetzen einer Flagge!!")); //test
                    RunningGangwar.FactionPoints += 5;
                }
                else if (RunningGangwar.Flag1.Faction == RunningGangwar.Attacker.Id)
                {
                    RunningGangwar.AttackerPoints += 5;
                }
                if (RunningGangwar.Flag2.Faction == RunningGangwar.Faction.Id)
                {
                    RunningGangwar.FactionPoints += 5;
                }
                else if (RunningGangwar.Flag2.Faction == RunningGangwar.Attacker.Id)
                {
                    RunningGangwar.AttackerPoints += 5;
                }
                if (RunningGangwar.Flag3.Faction == RunningGangwar.Faction.Id)
                {
                    RunningGangwar.FactionPoints += 5;
                }
                else if (RunningGangwar.Flag3.Faction == RunningGangwar.Attacker.Id)
                {
                    RunningGangwar.AttackerPoints += 5;
                }
                if (RunningGangwar.Flag4.Faction == RunningGangwar.Faction.Id)
                {
                    RunningGangwar.FactionPoints += 3;
                }
                else if (RunningGangwar.Flag4.Faction == RunningGangwar.Attacker.Id)
                {
                    RunningGangwar.AttackerPoints += 5;
                }

                RunningGangwar.Faction.GetFactionPlayers().ForEach(e =>
                {
                    if (e.Client.Position.DistanceTo(RunningGangwar.Zone) < 300)
                        e.Client.TriggerEvent("updateGangwarScore", RunningGangwar.FactionPoints, RunningGangwar.AttackerPoints);
                });

                RunningGangwar.Attacker.GetFactionPlayers().ForEach(e =>
                {
                    if (e.Client.Position.DistanceTo(RunningGangwar.Zone) < 300)
                        e.Client.TriggerEvent("updateGangwarScore", RunningGangwar.FactionPoints, RunningGangwar.AttackerPoints);
                });
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION AddGWPoints] " + ex.Message);
                Logger.Print("[EXCEPTION AddGWPoints] " + ex.StackTrace);
            }
        }
        public static void EndGangwar()
        {
            try
            {
                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE gangwars SET Faction = @faction WHERE Id = @id");
                mySqlQuery.AddParameter("@id", RunningGangwar.Id);
                if (RunningGangwar.AttackerPoints > RunningGangwar.FactionPoints)
                {
                    RunningGangwar.Faction = RunningGangwar.Attacker;
                    mySqlQuery.AddParameter("@faction", RunningGangwar.Attacker.Id);
                    Notification.SendGlobalNotification(
                        $"Die Fraktion {RunningGangwar.Attacker.Name} hat den Kampf um das Gebiet {RunningGangwar.Name} gegen {RunningGangwar.Faction.Name} gewonnen.",
                        8000, "orange", Notification.icon.warn);
                }
                else
                {
                    mySqlQuery.AddParameter("@faction", RunningGangwar.Faction.Id);
                    Notification.SendGlobalNotification(
                        $"Die Fraktion {RunningGangwar.Faction.Name} hat den Kampf um das Gebiet {RunningGangwar.Name} gegen {RunningGangwar.Attacker.Name} gewonnen.",
                        8000, "orange", Notification.icon.warn);
                }

                MySqlHandler.ExecuteSync(mySqlQuery);
                RunningGangwar.Flag1.Faction = 0;
                RunningGangwar.Flag2.Faction = 0;
                RunningGangwar.Flag3.Faction = 0;
                RunningGangwar.Flag4.Faction = 0;
                
                RunningGangwar = null;

                foreach (DbPlayer player in PlayerHandler.GetPlayers())
                {
                    player.TriggerEvent("finishGangwar");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION EndGangwar] " + ex.Message);
                Logger.Print("[EXCEPTION EndGangwar] " + ex.StackTrace);
            }
        }
    }
}
