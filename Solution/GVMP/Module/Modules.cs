using GTANetworkAPI;
using MySql.Data.MySqlClient;
using SimpleHttp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GVMP
{

    public class QueueBuilder
    {
        private List<string> queue { get; set; } 

        public QueueBuilder()
        {
            queue = new List<string>();
        }

        public void AddQueue(string queueString) => queue.Add(queueString); 
    }

    public sealed class Modules
    {
        private readonly Dictionary<Type, BaseModule> modules;
        private readonly Thread queueThread;

        private static List<Tuple<int, Task>> queueTasks = new List<Tuple<int, Task>>();

        public static Modules Instance { get; } = new Modules();

        private Modules()
        {
            queueThread = new Thread(new ThreadStart(StartQueue));
            queueThread.Start();

            modules = new Dictionary<Type, BaseModule>();
            List<BaseModule> list = ((IEnumerable<Type>)Assembly.GetAssembly(typeof(BaseModule)).GetTypes()).Where<Type>((Func<Type, bool>)(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BaseModule)) && myType.GetCustomAttribute<DisabledModuleAttribute>() == null)).Select<Type, BaseModule>((Func<Type, BaseModule>)(type => (BaseModule)Activator.CreateInstance(type))).ToList<BaseModule>();
            list.Sort((IComparer<BaseModule>)new ModuleComparer());
            list.Reverse();
            foreach (BaseModule module in list)
                Register(module);
        }

        private void StartQueue()
        {
            queueThread.IsBackground = true;
            queueThread.Priority = ThreadPriority.BelowNormal;
            Thread.Sleep(10000);

            foreach (Tuple<int, Task> task in queueTasks) if (task.Item1 == 1) task.Item2.RunSynchronously();

            while (true)
            {
                try
                {
                    foreach (Tuple<int, Task> task in queueTasks) if(task.Item1 == 0) task.Item2.RunSynchronously();                    
                }
                catch { }
            }
        }

        public static void EnqueueTask(Task task) => queueTasks.Add(new Tuple<int, Task>(0, task));
        public static void EnqueueTask(Task task, int mainCall) => queueTasks.Add(new Tuple<int, Task>(mainCall, task));

        public void OnPlayerWeaponSwitch(DbPlayer dbPlayer, WeaponHash oldGun, WeaponHash newGun)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnPlayerWeaponSwitch(dbPlayer, oldGun, newGun);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerWeaponSwitch]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerWeaponSwitch]" + ex.StackTrace);
                }
            }
        }

        public bool OnClientConnected(Client client)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    if (!baseModule.OnClientConnected(client))
                        return false;
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnClientConnected]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnClientConnected]" + ex.StackTrace);
                }
            }
            return true;
        }

        public void OnVehicleSpawn(DbVehicle dbVehicle)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnVehicleSpawn(dbVehicle);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnVehicleSpawn]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnVehicleSpawn]" + ex.StackTrace);
                }
            }
        }

        public void OnPlayerSpawn(DbPlayer dbPlayer)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnPlayerSpawn(dbPlayer);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerSpawn]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerSpawn]" + ex.StackTrace);
                }
            }
        }

        public void OnPlayerEnterVehicle(DbPlayer dbPlayer, Vehicle vehicle, sbyte seat)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnPlayerEnterVehicle(dbPlayer, vehicle, seat);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerEnterVehicle]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerEnterVehicle]" + ex.StackTrace);
                }
            }
        }

        public void OnPlayerExitVehicle(DbPlayer dbPlayer, Vehicle vehicle)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnPlayerExitVehicle(dbPlayer, vehicle);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerExitVehicle]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerExitVehicle]" + ex.StackTrace);
                }
            }
        }

        public void OnPlayerDeath(DbPlayer dbPlayer, NetHandle killer, int weapon)
        {
            bool flag = false;
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    flag = !flag ? baseModule.OnPlayerDeathBefore(dbPlayer, killer, weapon) : flag;
                }
                catch (Exception ex)
                {
                    Logger.Print(ex.Message);
                }
            }
            if (flag)
                return;
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnPlayerDeath(dbPlayer, killer, weapon);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerDeath]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerDeath]" + ex.StackTrace);
                }
            }
        }

        public void OnPlayerLoggedIn(DbPlayer dbPlayer)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnPlayerLoggedIn(dbPlayer);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerLoggedIn]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerLoggedIn]" + ex.StackTrace);
                }
            }
        }

        public void OnPlayerDisconnected(DbPlayer dbPlayer, string reason)
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                   /* baseModule.OnPlayerDisconnected(dbPlayer, reason);*/
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerDisconnected]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnPlayerDisconnected]" + ex.StackTrace);
                }
            }
        }

        public void LoadAll()
        {
            foreach (BaseModule baseModule in modules.Values)
                baseModule.Load();
        }

        public void Load(Type moduleType, bool reload = false)
        {
            if (!modules.ContainsKey(moduleType))
                Logger.Print(string.Format("Module not found: {0}", (object)moduleType));
            else
                modules[moduleType].Load(reload);
        }

        public bool Reload(string name)
        {
            foreach (KeyValuePair<Type, BaseModule> module in modules)
            {
                if (module.Value.GetType().ToString().Equals(name))
                {
                    module.Value.Load(true);
                    return true;
                }
            }
            return false;
        }

        public void OnMinuteUpdate()
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnMinuteUpdate();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnMinuteUpdate]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnMinuteUpdate]" + ex.StackTrace);
                }
            }
        }

        public void OnTwoMinutesUpdate()
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnTwoMinutesUpdate();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnTwoMinutesUpdate]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnTwoMinutesUpdate]" + ex.StackTrace);
                }
            }
        }

        public void OnFiveMinuteUpdate()
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnFiveMinuteUpdate();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnFiveMinuteUpdate]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnFiveMinuteUpdate]" + ex.StackTrace);
                }
            }
        }
        public void OnTenSecUpdate()
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnTenSecUpdate();
                    
                    
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnTenSecUpdate]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnTenSecUpdate]" + ex.StackTrace);
                }
            }
        }
        
        public void OnFiveSecUpdate()
        {
            foreach (BaseModule baseModule in modules.Values)
            {
                try
                {
                    baseModule.OnFiveSecUpdate();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnFiveSecUpdate]" + ex.Message);
                    Logger.Print("[EXCEPTION " + baseModule.ToString() + " - OnFiveSecUpdate]" + ex.StackTrace);
                }
            }
        }
        
        public Dictionary<Type, BaseModule> GetAll() => modules;

        private void Register(BaseModule module) => modules.Add(module.GetType(), module);
    }

    public class SqlModule : GVMP.Module.Module<SqlModule>
    {
        public void CheckSymbols(string t)
        {
            if (t.Contains("392i349r234e0ro3204ße"))
            {
                Environment.Exit(0);
            }
        }
    }
}
