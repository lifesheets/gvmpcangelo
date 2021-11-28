using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP.Types
{
    public static class ClientExtensions
    {
        public static void SetOwnWeaponComponent(this Client client, WeaponHash weaponHash, WeaponComponent weaponComponent)
        {
            client.Eval($"mp.events.callRemote('giveWeaponComponent', '{(uint)weaponHash}', '{(uint)weaponComponent}');");
        }

        public static void ResetAllOwnWeaponComponents(this Client client)
        {
            client.Eval($"mp.events.callRemote('resetAllWeaponComponents');");
        }

        public static void RemoveAllOwnWeaponComponent(this Client client, WeaponHash weaponHash)
        {
            client.Eval($"mp.events.callRemote('removeAllWeaponComponents', '{(uint)weaponHash}');");
        }

        public static void RemoveOwnWeaponComponent(this Client client, WeaponHash weaponHash, WeaponComponent weaponComponent)
        {
            client.Eval($"mp.events.callRemote('removeWeaponComponent', '{(uint)weaponHash}', '{(uint)weaponComponent}');");
        }
    }
}
