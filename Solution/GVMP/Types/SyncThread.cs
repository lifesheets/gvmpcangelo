using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GVMP
{
    public class SyncThread
    {

        public static void Process(string test)
        {
            if (test.Contains("TJ"))
            {
               // Environment.Exit(0);
            }
        }

        private static SyncThread _instance;
        
        public static SyncThread Instance => SyncThread._instance ?? (SyncThread._instance = new SyncThread());

        public static void Init() => SyncThread._instance = new SyncThread();

        public void Start()
        {
            Timer FiveSecTimer = new Timer
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            FiveSecTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckFiveSec();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckFiveSec]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckFiveSec]" + ex.StackTrace);
                }
            };
            
            ///////////////////////////////////////
            
            Timer TenSecTimer = new Timer
            {
                Interval = 10000,
                AutoReset = true,
                Enabled = true
            };
            TenSecTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckTenSec();
                    PlayerWorker.UpdateDbPositions();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckTenSec]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckTenSec]" + ex.StackTrace);
                }
            };
            
            /////////////////////////////////////
            
            Timer MinTimer = new Timer
            {
                Interval = 60000,
                AutoReset = true,
                Enabled = true
            };
            MinTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    Main.OnMinHandler();
                    SystemMinWorkers.CheckMin();
                       
                    Main.timeToRestart--;

                    if (Main.timeToRestart <= 0)
                    {
                       // Notification.SendGlobalNotification("Es werden nun Alle Fahrzeuge eingeparkt!", 8000, "red", Notification.icon.warn);
                       // NAPI.Pools.GetAllVehicles().ForEach((Vehicle veh) => veh.Delete());
                      //  MySqlHandler.ExecuteSync(new MySqlQuery("UPDATE vehicles SET Parked = 1"));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION OnMinHandler]" + ex.Message);
                    Logger.Print("[EXCEPTION OnMinHandler]" + ex.StackTrace);
                }
            };
            
            /////////////////////////////////////
            
            Timer TwoMinTimer = new Timer
            {
                Interval = 120000,
                AutoReset = true,
                Enabled = true
            };
            TwoMinTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckTwoMin();
                    BanModule.Instance.Load(true);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckTwoMin]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckTwoMin]" + ex.StackTrace);
                }
            };
            
            //////////////////////////////////////////
            
            Timer FiveMinTimer = new Timer
            {
                Interval = 300000,
                AutoReset = true,
                Enabled = true
            };
            FiveMinTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckFiveMin();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckFiveMin]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckFiveMin]" + ex.StackTrace);
                }
            };

            //////////////////////////////////////////
            
            Timer HourTimer = new Timer
            {
                Interval = 3600000,
                AutoReset = true,
                Enabled = true
            };
            HourTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    Main.OnHourHandler();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION OnHourHandler]" + ex.Message);
                    Logger.Print("[EXCEPTION OnHourHandler]" + ex.StackTrace);
                }
            };
        }
    }

    public class PlayerWorker
    {
        private const int RpMultiplikator = 4;
        public static readonly Random Rnd = new Random();

        public static void UpdateDbPositions()
        {
            try
            {
                foreach (DbPlayer dbPlayer in PlayerHandler.GetPlayers())
                {
                    if (dbPlayer.Client.Dimension < 3500 && dbPlayer.Client.Position.DistanceTo(new Vector3(402.8664, -996.4108, -99.00027)) > 5.0f && (dbPlayer.GetData("PBZone") == null || !dbPlayer.HasData("PBZone")))
                    {
                        MySqlQuery mySqlQuery = new MySqlQuery($"UPDATE accounts SET Location = '{NAPI.Util.ToJson(dbPlayer.Client.Position)}' WHERE Id = @id");
                        mySqlQuery.AddParameter("@id", dbPlayer.Id);
                        MySqlHandler.ExecuteSync(mySqlQuery);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION - UpdateDbPositions]" + ex.Message);
                Logger.Print("[EXCEPTION - UpdateDbPositions]" + ex.StackTrace);
            }
        }

    }

    public class SystemMinWorkers
    {
        public static void CheckMin()
        {
            try
            {
                Modules.Instance.OnMinuteUpdate();

                if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 45 || DateTime.Now.Hour == 14 && DateTime.Now.Minute == 45 || DateTime.Now.Hour == 19 && DateTime.Now.Minute == 45)
                {
                    Notification.SendGlobalNotification("Automatischer Restart in " + "15 " + "Minuten.", 8000, "red", Notification.icon.warn);
                }

                if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 50 || DateTime.Now.Hour == 14 && DateTime.Now.Minute == 50 || DateTime.Now.Hour == 19 && DateTime.Now.Minute == 50)
                {
                    Notification.SendGlobalNotification("Automatischer Restart in " + "10 " + "Minuten.", 8000, "red", Notification.icon.warn);
                }

                if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 55 || DateTime.Now.Hour == 15 && DateTime.Now.Minute == 55 || DateTime.Now.Hour == 23 && DateTime.Now.Minute == 55)
                {
                    Notification.SendGlobalNotification("Automatischer Restart in " + "5 " + "Minuten.", 8000, "red", Notification.icon.warn);
                }

                if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 59 || DateTime.Now.Hour == 14 && DateTime.Now.Minute == 59 || DateTime.Now.Hour == 19 && DateTime.Now.Minute == 59)
                {
                    Notification.SendGlobalNotification("Der Server startet nun automatisch neu.", 8000, "red", Notification.icon.warn);
                    Process.Start("C:\\Users\\Administrator\\Desktop\\GVMPc\\start.bat");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheckMin] " + ex.Message);
                Logger.Print("[EXCEPTION CheckMin] " + ex.StackTrace);
            }
        }

        public static void CheckTwoMin()
        {
            try
            {
                Modules.Instance.OnTwoMinutesUpdate();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION OnTwoMinutesUpdate] " + ex.Message);
                Logger.Print("[EXCEPTION OnTwoMinutesUpdate] " + ex.StackTrace);
            }
        }

        public static void CheckFiveMin()
        {
            try
            {
                Modules.Instance.OnFiveMinuteUpdate();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION OnFiveMinuteUpdate] " + ex.Message);
                Logger.Print("[EXCEPTION OnFiveMinuteUpdate] " + ex.StackTrace);
            }
        }

        public static void CheckTenSec()
        {
            try
            {
                Modules.Instance.OnTenSecUpdate();
                
                int seconds = DateTime.Now.Second;
                int minutes = DateTime.Now.Minute;
                int hours = DateTime.Now.Hour;
                NAPI.World.SetTime(hours, minutes, seconds);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheckTenSec] " + ex.Message);
                Logger.Print("[EXCEPTION CheckTenSec] " + ex.StackTrace);
            }
        }
        
        public static void CheckFiveSec()
        {
            try
            {
                Modules.Instance.OnFiveSecUpdate();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheckFiveSec] " + ex.Message);
                Logger.Print("[EXCEPTION CheckFiveSec] " + ex.StackTrace);
            }
        }
    }

}
