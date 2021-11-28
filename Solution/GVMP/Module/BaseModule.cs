using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GVMP
{
    public abstract class BaseModule : Script
    {
        
        private bool loaded;
        private DateTime loadTime = DateTime.Now;
        private StringBuilder currentLog;

        public void Log(string log) => this.currentLog?.AppendLine(log);

        public virtual bool OnClientConnected(Client client) => true;

        public virtual void OnVehicleSpawn(DbVehicle dbVehicle)
        {
        }

        public virtual void OnPlayerSpawn(DbPlayer dbPlayer)
        {
        }

        public virtual void OnPlayerLoggedIn(DbPlayer dbPlayer)
        {
        }

        public virtual void 
            isconnected(DbPlayer dbPlayer, string reason)
        {
        }

        public virtual bool OnPlayerDeathBefore(DbPlayer dbPlayer, NetHandle killer, int weapon) => false;

        public virtual void OnPlayerDeath(DbPlayer dbPlayer, NetHandle killer, int weapon)
        {
        }

        public virtual void OnPlayerEnterVehicle(DbPlayer dbPlayer, Vehicle vehicle, sbyte seat)
        {
        }

        public virtual void OnPlayerExitVehicle(DbPlayer dbPlayer, Vehicle vehicle)
        {
        }

        public virtual void OnPlayerWeaponSwitch(
          DbPlayer dbPlayer,
          WeaponHash oldgun,
          WeaponHash newgun)
        {
        }

        protected virtual bool OnLoad()
        {
            return true;
        }

        public bool IsLoaded() => this.loaded;

        public virtual void OnMinuteUpdate()
        {
        }

        public virtual void OnTwoMinutesUpdate()
        {
        }

        public virtual void OnFiveMinuteUpdate()
        {
        }

        public virtual void OnTenSecUpdate()
        {
        }
        
        public virtual void OnFiveSecUpdate()
        {
        }

        public virtual int GetOrder() => 0;

        public virtual bool Load(bool reload = false)
        {
            if (this.loaded && !reload)
                return true;
            Type[] typeArray = this.RequiredModules();
            if (typeArray != null)
            {
                foreach (Type moduleType in typeArray)
                    Modules.Instance.Load(moduleType, reload);
            }
            this.currentLog = new StringBuilder();
            this.loaded = this.OnLoad();
            Logger.Print("Loading Module " + this.ToString());
            return this.loaded;
        }

        public virtual Type[] RequiredModules() => (Type[])null;
    }
}
