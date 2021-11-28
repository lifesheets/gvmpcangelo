using System;
using GTANetworkAPI;

namespace GVMP
{
    public class WeaponSwitchModule : GVMP.Module.Module<WeaponSwitchModule>
    {
        [ServerEvent(Event.PlayerWeaponSwitch)]
        public void playerWeaponSwitch(Client c, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            try
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (oldWeapon == null) return;
                if(newWeapon == null) return;
                if (c == null) return;

                if (newWeapon == WeaponHash.Unarmed) return;
                
                /* if (!dbPlayer.HasData("IN_GANGWAR") &&
                    (dbPlayer.HasData("PBZone") && dbPlayer.GetData("PBZone") == null))
                {
                    if (!dbPlayer.Loadout.Contains(newWeapon))
                    {
                        c.RemoveWeapon(newWeapon);
                        return;
                    }   
                } */

                c.TriggerEvent("client:weaponSwap");
                NAPI.Player.SetPlayerCurrentWeapon(c, newWeapon);
                NAPI.Player.SetPlayerCurrentWeaponAmmo(c, 9999);
                c.Eval($"mp.game.invoke('0xDCD2A934D65CB497', mp.game.player.getPed(), {NAPI.Util.GetHashKey(newWeapon.ToString())}, 9999);");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION playerWeaponSwitch] " + ex.Message);
                Logger.Print("[EXCEPTION playerWeaponSwitch] " + ex.StackTrace);
            }
        }
    }
}