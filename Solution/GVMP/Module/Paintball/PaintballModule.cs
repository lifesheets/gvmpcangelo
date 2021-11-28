using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class PaintballModule : GVMP.Module.Module<PaintballModule>
    {
        public static List<PaintballModel> Zones = new List<PaintballModel>();

        protected override bool OnLoad()
        {
            Zones.Add(new PaintballModel
            {
                Id = Zones.Count,
                Name = "Würfelpark",
                Spawns = new List<Vector3>
                {
                    new Vector3(170.8255, -915.5659, 30.69199),
                    new Vector3(211.5563, -944.5418, 30.68113),
                    new Vector3(241.9889, -886.0068, 30.48896),
                    new Vector3(159.5039, -969.2851, 30.09191)
                },
                MaxPlayer = 10,
                Weapons = new List<WeaponHash>()
                {
                    WeaponHash.AdvancedRifle,
                    WeaponHash.Gusenberg,
                    WeaponHash.HeavyPistol,
                    WeaponHash.AssaultRifle,
                    WeaponHash.BullpupRifle
                }
            });





            Zones.Add(new PaintballModel
            {
                Id = Zones.Count,
                Name = "Burrito Camp",
                Spawns = new List<Vector3>
                {
                    new Vector3(2316.73, 2523.17, 46.67),
                    new Vector3(2323.24, 2588.42, 46.65),
                    new Vector3(2371.18, 2623.24, 46.66),
                    new Vector3(2350.78, 2550.69, 46.67),
                    new Vector3(2352.86, 2523.49, 47.69)
                },
                MaxPlayer = 10,
                Weapons = new List<WeaponHash>()
                {
                    WeaponHash.AdvancedRifle,
                    WeaponHash.Gusenberg,
                    WeaponHash.HeavyPistol,
                    WeaponHash.AssaultRifle,
                    WeaponHash.BullpupRifle
                }
            });

            Zones.Add(new PaintballModel
            {
                Id = Zones.Count,
                Name = "Bratwa Dorf",
                Spawns = new List<Vector3>
                {
                    new Vector3(-1124.67, 4947.55, 220.1),
                    new Vector3(-1158.27, 4923.96, 222.46),
                    new Vector3(-1106.51, 4891.97, 215.48),
                    new Vector3(-1081.32, 4913.33, 214.15)
                },
                MaxPlayer = 10,
                Weapons = new List<WeaponHash>()
                {
                    WeaponHash.Revolver
                }
            });

            Zones.Add(new PaintballModel
            {
                Id = Zones.Count,
                Name = "LS Supply",
                Spawns = new List<Vector3>
                {
                    new Vector3(1216.89, -1270.16, 35.37),
                    new Vector3(1188.77, -1296.84, 34.92),
                    new Vector3(1214.44, -1364.21, 35.23),
                    new Vector3(1180.58, -1412.93, 34.86),
                    new Vector3(1137.9, -1358.51, 34.59),
                    new Vector3(1151.74, -1326.77, 34.69)
                },
                MaxPlayer = 10,
                Weapons = new List<WeaponHash>()
                {
                    WeaponHash.AdvancedRifle,
                    WeaponHash.Gusenberg,
                    WeaponHash.HeavyPistol,
                    WeaponHash.AssaultRifle,
                    WeaponHash.BullpupRifle
                }
            });







            NAPI.Blip.CreateBlip(432, new Vector3(758.5218, -815.93896, 25.292513), 1.0f, 0, "Paintball", 255, 0, true, 0, 0);
            NAPI.Marker.CreateMarker(1, new Vector3(758.5218, -815.93896, 25.292513), new Vector3(), new Vector3(), 1.0f, new Color(255, 165, 0), false, 0);
            //NAPI.Marker.CreateMarker(1, Position2.Subtract(new Vector3(0f, 0f, 1f)), new Vector3(), new Vector3(), 1f, new Color(255, 165, 0), false, 0u);


            ColShape cb = NAPI.ColShape.CreateCylinderColShape(new Vector3(758.5218, -815.93896, 25.292513), 1.4f, 1.4f, 0);
            cb.SetData("FUNCTION_MODEL", new FunctionModel("Paintball-Menu"));
            cb.SetData("MESSAGE", new Message("Benutze E um Paintball zu spielen.", "PAINTBALL", "orange", 3000));
            cb.SetData("ColShape", "Colshape1");


            return true;
        }





        [RemoteEvent("Paintball-Menu")]
        public void PaintballMenu(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;



                NAPI.ColShape.CreatCircleColShape((float)578.4797, (float)2792.4712, (float)41.119812);

                List<NativeItem> nativeItems = new List<NativeItem>();
                foreach (var t in Zones)
                {
                    nativeItems.Add(new NativeItem(t.Name + " (" + t.Players().Count + " / 10)", t.Name));
                }

                NativeMenu nativeMenu = new NativeMenu("Paintball", "", nativeItems);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Paintball-Menu] " + ex.Message);
                Logger.Print("[EXCEPTION Paintball-Menu] " + ex.StackTrace);
            }
        }







        [RemoteEvent("nM-Paintball")]
        public void PaintballEnter(Client c, string value)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

                foreach (var t in Zones)
                {
                    if (t.Players().Count >= 10)
                    {

                        dbPlayer.SendNotification("Die Lobby ist bereits voll!", 3000, "red");
                        return;
                    }
                }
                dbPlayer.CloseNativeMenu();

                PaintballModel zone = null;
                foreach (var t in Zones)
                    if (t.Name == value)
                        zone = t;
                if (zone != null)
                {
                    dbPlayer.RemoveAllWeapons();
                    Random r = new Random();
                    c.Dimension = Convert.ToUInt32(22750 + zone.Id);
                    c.Position = zone.Spawns[r.Next(0, zone.Spawns.Count)];
                    dbPlayer.SetData("PBZone", zone);
                    dbPlayer.SetData("PBKills", 0);
                    dbPlayer.SetData("PBDeaths", 0);
                    dbPlayer.SetData("PBZoneplayer", 0);
                    dbPlayer.RemoveAllWeapons();
                    dbPlayer.SetArmor(100);

                    dbPlayer.initializePaintball();

                    foreach (WeaponHash weaponHash in zone.Weapons)
                    {


                        dbPlayer.GiveWeapon(weaponHash, 9999);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION nM-Paintball] " + ex.Message);
                Logger.Print("[EXCEPTION nM-Paintball] " + ex.StackTrace);
            }
        }

        public static void leavePaintball(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                PaintballModel paintballModel = dbPlayer.GetData("PBZone");
                if (paintballModel == null) return;

                dbPlayer.ACWait();
                dbPlayer.SetPosition(new Vector3(758.5218, -815.93896, 25.292513));

                dbPlayer.SetDimension(0);
                dbPlayer.SetData("PBZone", null);
                dbPlayer.SetData("PBKills", 0);
                dbPlayer.SetData("PBDeaths", 0);

                dbPlayer.SetArmor(0);

                dbPlayer.finishPaintball();
                dbPlayer.RemoveAllWeapons();
                WeaponManager.loadWeapons(c);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION leavePaintball] " + ex.Message);
                Logger.Print("[EXCEPTION leavePaintball] " + ex.StackTrace);
            }
        }

        public static void PaintballDeath(DbPlayer dbPlayer, DbPlayer dbPlayer2)
        {
            try
            {
                if (dbPlayer == null || dbPlayer2 == null) return;

                Random r = new Random();

                if (!dbPlayer.HasData("PBZone") || !dbPlayer.HasData("PBDeaths") || !dbPlayer.HasData("PBKills") || dbPlayer.GetData("PBZone") == null && dbPlayer.GetData("PBDeaths") == null || dbPlayer.GetData("PBKills") == null) return;

                PaintballModel paintballModel = dbPlayer.GetData("PBZone");
                int newdeaths = dbPlayer.GetData("PBDeaths");
                newdeaths += 1;

                dbPlayer.SetData("PBKillstreak", 0);
                dbPlayer.SetData("PBDeaths", newdeaths);
                dbPlayer.SpawnPlayer(paintballModel.Spawns[r.Next(0, paintballModel.Spawns.Count)]);

                int newkills = dbPlayer2.GetData("PBKills");
                int killstreak = 0;

                if (dbPlayer2.HasData("PBKillstreak") && dbPlayer2.GetData("PBKillstreak") != null && dbPlayer2.GetData("PBKillstreak") is int)
                    killstreak = dbPlayer2.GetData("PBKillstreak");

                killstreak += 1;
                newkills += 1;

                if (killstreak == 3)
                {
                    paintballModel.Players().ForEach(p => Notification.SendGlobalNotification(p.Client, "Bei " + dbPlayer2.Name + " läuft!", 5000, "white", Notification.icon.bullhorn));
                }

                else if (killstreak == 5)
                {
                    paintballModel.Players().ForEach(p => Notification.SendGlobalNotification(p.Client, dbPlayer2.Name + " scheppert richtig!", 5000, "white", Notification.icon.bullhorn));
                }

                else if (killstreak == 10)
                {
                    paintballModel.Players().ForEach(p => Notification.SendGlobalNotification(p.Client, dbPlayer2.Name + " ist ein gott!", 5000, "white", Notification.icon.bullhorn));
                }

                dbPlayer2.SetData("PBKills", newkills);
                dbPlayer2.SetData("PBKillstreak", killstreak);

                foreach (WeaponHash weaponHash in paintballModel.Weapons)
                {
                    dbPlayer.GiveWeapon(weaponHash, 9999);
                }

                dbPlayer.updatePaintballScore((int)dbPlayer.GetData("PBKills"), (int)dbPlayer.GetData("PBDeaths"));
                dbPlayer2.updatePaintballScore((int)dbPlayer2.GetData("PBKills"), (int)dbPlayer2.GetData("PBDeaths"));

                dbPlayer.SendNotification("Du hast noch " + (10 - (int)dbPlayer2.GetData("PBDeaths")) + " Leben!", 3000, "black");

                if (dbPlayer.GetData("PBDeaths") >= 10) leavePaintball(dbPlayer.Client);

                dbPlayer.acsleep = true;
                dbPlayer.StopAnimation();
                dbPlayer.SetInvincible(false);
                dbPlayer.SetArmor(100);
                dbPlayer.disableAllPlayerActions(false);
                dbPlayer.acsleep = false;
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION PaintballDeath] " + ex.Message);
                Logger.Print("[EXCEPTION PaintballDeath] " + ex.StackTrace);
            }
        }
    }
}

/*using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class PaintballModule : GVMP.Module.Module<PaintballModule>
    {
        public static List<PaintballModel> Zones = new List<PaintballModel>();
        public static List<WeaponHash> PaintballWeapons = new List<WeaponHash>();
        public static List<WeaponHash> PaintballWeapons2 = new List<WeaponHash>();
        public static List<WeaponHash> PaintballWeapons3 = new List<WeaponHash>();

        protected override bool OnLoad()
        {
            PaintballWeapons.Add(WeaponHash.AdvancedRifle);
            PaintballWeapons.Add(WeaponHash.Gusenberg);
            PaintballWeapons.Add(WeaponHash.HeavyPistol);
            PaintballWeapons.Add(WeaponHash.AssaultRifle);
            PaintballWeapons.Add(WeaponHash.BullpupRifle);

            PaintballWeapons2.Add(WeaponHash.MarksmanRifle);

            PaintballWeapons3.Add(WeaponHash.SpecialCarbine);

            Zones.Add(new PaintballModel
            {
                Id = Zones.Count,
                Name = "Würfelpark",
                Spawns = new List<Vector3>
                {
                    new Vector3(170.8255, -915.5659, 30.69199),
                    new Vector3(211.5563, -944.5418, 30.68113),
                    new Vector3(241.9889, -886.0068, 30.48896),
                    new Vector3(159.5039, -969.2851, 30.09191)
                },
                MaxPlayer = 10
            });

            Zones.Add(new PaintballModel
            {
                Id = Zones.Count,
                Name = "Bratwa Dorf",
                Spawns = new List<Vector3>
                {
                    new Vector3(-1124.67, 4947.55, 220.1),
                    new Vector3(-1158.27, 4923.96, 222.46),
                    new Vector3(-1106.51, 4891.97, 215.48),
                    new Vector3(-1081.32, 4913.33, 214.15)
                },
                MaxPlayer = 10
            });

            Zones.Add(new PaintballModel
            {
                Id = Zones.Count,
                Name = "LS Supply",
                Spawns = new List<Vector3>
                {
                    new Vector3(1216.89, -1270.16, 35.37),
                    new Vector3(1188.77, -1296.84, 34.92),
                    new Vector3(1214.44, -1364.21, 35.23),
                    new Vector3(1180.58, -1412.93, 34.86),
                    new Vector3(1137.9, -1358.51, 34.59),
                    new Vector3(1151.74, -1326.77, 34.69)
                },
                MaxPlayer = 10
            });

            NAPI.Blip.CreateBlip(432, new Vector3(570.46, 2796.68, 41.01), 1.0f, 0, "Paintball", 255, 0, true, 0, 0);
            NAPI.Marker.CreateMarker(1, new Vector3(570.46, 2796.68, 41.01), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, 0);

            ColShape cb = NAPI.ColShape.CreateCylinderColShape(new Vector3(570.46, 2796.68, 41.01), 1.4f, 1.4f, 0);
            cb.SetData("FUNCTION_MODEL", new FunctionModel("Paintball-Menu"));
            cb.SetData("MESSAGE", new Message("Benutze E um Painball zu spielen.", "PAINTBALL", "orange", 3000));

            return true;
        }

        [RemoteEvent("Paintball-Menu")]
        public void PaintballMenu(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

                List<NativeItem> nativeItems = new List<NativeItem>();
                foreach (var t in Zones)
                {
                    nativeItems.Add(new NativeItem(t.Name, t.Name));
                }

                NativeMenu nativeMenu = new NativeMenu("Paintball", "", nativeItems);
                dbPlayer.ShowNativeMenu(nativeMenu);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Paintball-Menu] " + ex.Message);
                Logger.Print("[EXCEPTION Paintball-Menu] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Paintball")]
        public void PaintballEnter(Client c, string value)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

                PaintballModel paintballModel = dbPlayer.GetData("PBZone");
                if (paintballModel == null) return;

                dbPlayer.CloseNativeMenu();

                PaintballModel zone = null;
                foreach (var t in Zones)
                    if (t.Name == value)
                        zone = t;
                if (zone != null)
                {
                    Random r = new Random();
                    c.Dimension = Convert.ToUInt32(22750 + zone.Id);
                    c.Position = zone.Spawns[r.Next(0, zone.Spawns.Count)];
                    dbPlayer.SetData("PBZone", zone);
                    dbPlayer.SetData("PBKills", 0);
                    dbPlayer.SetData("PBDeaths", 0);

                    dbPlayer.SetArmor(100);

                    dbPlayer.initializePaintball();

                    if (paintballModel.Name == "Würfelpark")
                        foreach (WeaponHash weaponHash in PaintballWeapons)
                        {
                            dbPlayer.GiveWeapon(weaponHash, 9999);
                        }
                    else if (paintballModel.Name == "Bratwa Dorf")
                        foreach (WeaponHash weaponHash in PaintballWeapons2)
                        {
                            dbPlayer.GiveWeapon(weaponHash, 9999);
                        }
                    else if (paintballModel.Name == "LS Supply")
                        foreach (WeaponHash weaponHash in PaintballWeapons3)
                        {
                            dbPlayer.GiveWeapon(weaponHash, 9999);
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION nM-Paintball] " + ex.Message);
                Logger.Print("[EXCEPTION nM-Paintball] " + ex.StackTrace);
            }
        }

        public static void leavePaintball(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                PaintballModel paintballModel = dbPlayer.GetData("PBZone");
                if (paintballModel == null) return;

                dbPlayer.ACWait();
                dbPlayer.SetPosition(new Vector3(570.46, 2796.68, 41.01));

                dbPlayer.SetDimension(0);
                dbPlayer.SetData("PBZone", null);
                dbPlayer.SetData("PBKills", 0);
                dbPlayer.SetData("PBDeaths", 0);

                dbPlayer.SetArmor(0);

                dbPlayer.finishPaintball();
                dbPlayer.RemoveAllWeapons();
                WeaponManager.loadWeapons(c);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION leavePaintball] " + ex.Message);
                Logger.Print("[EXCEPTION leavePaintball] " + ex.StackTrace);
            }
        }

        public static void PaintballDeath(DbPlayer dbPlayer, DbPlayer dbPlayer2)
        {
            try
            {
                if (dbPlayer == null || dbPlayer2 == null) return;

                Random r = new Random();

                PaintballModel paintballModel = dbPlayer.GetData("PBZone");
                int newdeaths = dbPlayer.GetData("PBDeaths");
                newdeaths += 1;

                dbPlayer.SetData("PBDeaths", newdeaths);
                dbPlayer.SpawnPlayer(paintballModel.Spawns[r.Next(0, paintballModel.Spawns.Count)]);

                int newkills = dbPlayer2.GetData("PBKills");
                newkills += 1;

                dbPlayer2.SetData("PBKills", newkills);

                foreach (WeaponHash weaponHash in PaintballWeapons)
                {
                    dbPlayer.GiveWeapon(weaponHash, 9999);
                }

                dbPlayer.updatePaintballScore((int)dbPlayer.GetData("PBKills"), (int)dbPlayer.GetData("PBDeaths"));
                dbPlayer2.updatePaintballScore((int)dbPlayer2.GetData("PBKills"), (int)dbPlayer2.GetData("PBDeaths"));

                dbPlayer.SendNotification("Du hast noch " + (10 - (int)dbPlayer2.GetData("PBDeaths")) + " Leben!", 3000, "red");

                if (dbPlayer.GetData("PBDeaths") >= 10) leavePaintball(dbPlayer.Client);

                dbPlayer.StopAnimation();
                dbPlayer.SetInvincible(false);
                dbPlayer.SetArmor(100);
                dbPlayer.disableAllPlayerActions(false);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION PaintballDeath] " + ex.Message);
                Logger.Print("[EXCEPTION PaintballDeath] " + ex.StackTrace);
            }
        }
    }
}
*/
