using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class AnticheatModule : GVMP.Module.Module<AnticheatModule>
    {
        //    public static List<WeaponHash> blacklist = new List<WeaponHash>() { WeaponHash.RPG, WeaponHash.Railgun, WeaponHash.GrenadeLauncher, WeaponHash.HomingLauncher, WeaponHash.Minigun, WeaponHash.Molotov, WeaponHash.StickyBomb };


        [RemoteEvent("checkWeaponHashes")]
        public void checkWeaponHashes(Client c, WeaponHash weaponHash)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

                if (dbPlayer.HasData("IN_GANGWAR") ||
                    (dbPlayer.HasData("PBZone") && dbPlayer.GetData("PBZone") != null))
                    return;


                if (weaponHash == WeaponHash.Unarmed) return;

                var weaponloadout = dbPlayer.Loadout.Find(x => x.Weapon == weaponHash);
                if (weaponloadout == null)
                {
                    c.RemoveWeapon(weaponHash);
                    Item item = ItemModule.itemRegisterList.Find((Item item2) => item2.Whash == weaponHash);
                    if (item == null) dbPlayer.BanPlayer();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION checkWeaponHashes] " + ex.Message);
                Logger.Print("[EXCEPTION checkWeaponHashes] " + ex.StackTrace);
            }
        }
        [ServerEvent(Event.PlayerWeaponSwitch)]
        public void playerWeaponChange(Client p, WeaponHash oldweapon, WeaponHash newweapon)
        {
            try
            {
                checkWeaponHashes(p, newweapon);
                p.TriggerEvent("client:weaponSwap");
            }
            catch (Exception ex) { Logger.Print(ex.Message); }
        }
        /*
                public static void Wait(Client p)
                {
                    try
                    {
                        if (p == null) return;
                        DbPlayer dbPlayer = p.GetPlayer();
                        if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                            return;

                        if (dbPlayer.HasData("IN_GANGWAR") ||
                            (dbPlayer.HasData("PBZone") && dbPlayer.GetData("PBZone") != null))
                            return;

                        if (p != null && p.Exists)
                            p.TriggerEvent("client:respawning");

                        if (blacklist.Contains(p.CurrentWeapon))

                        {
                            dbPlayer.BanPlayer();
                        }
                    }
                    catch (Exception ex) { Logger.Print(ex.Message); }
                }

                [ServerEvent(Event.PlayerWeaponSwitch)]
                public void onAntiCheatDetectBlacklistedWeapon(Client p, WeaponHash oldWeapon, WeaponHash newweapon)
                {
                    try
                    {
                        if (p == null) return;
                        DbPlayer dbPlayer = p.GetPlayer();
                        if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                            return;

                        if (dbPlayer.HasData("IN_GANGWAR") ||
                            (dbPlayer.HasData("PBZone") && dbPlayer.GetData("PBZone") != null))
                            return;
                        if (blacklist.Contains(p.CurrentWeapon))
                        {
                            NAPI.Task.Run(delegate
                            {
                                dbPlayer.BanPlayer();
                            }, 3000);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.Print(ex.Message);
                    }
                }
        */

        [ServerEvent(Event.PlayerDamage)]
        public void onAnticheatDetectHealkey(Client p, float healloss, float armorloss)
        {

            if (p == null) return;
            DbPlayer dbPlayer = p.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;
            if (dbPlayer.acsleep) return;
            if (dbPlayer.HasData("IN_GANGWAR") ||
                (dbPlayer.HasData("PBZone") && dbPlayer.GetData("PBZone") != null))
                return;
            if (healloss == 0 || armorloss == 0 || healloss == 0 && armorloss == 0)
            {
                dbPlayer.BanPlayer();
                p.Kick("Healkey|Godmode");
            }
        }



        [RemoteEvent("server:CheatDetection")]
        public void CheatDetection(Client c, string reason)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.HasData("PLAYER_ADUTY") && dbPlayer.GetData("PLAYER_ADUTY") == true) return;


                PlayerHandler.GetAdminPlayers().ForEach((DbPlayer dbPlayer2) =>
                {
                    if (dbPlayer2.HasData("disablenc")) return;

                    Adminrank adminranks = dbPlayer2.Adminrank;

                    if (adminranks.Permission >= 91)
                        dbPlayer2.SendNotification(("Der Spieler " + dbPlayer.Name + " wurde gerade von dem Anticheat geflaggt. Verdacht: " + reason), 6000, "red", "Anticheat");
                });

                WebhookSender.SendMessage("Neuer Flag",
                    "Der Spieler " + dbPlayer.Name + " wurde gerade von dem Anticheat geflaggt. Verdacht: " + reason,
                    Webhooks.aclogs, "Flag");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheatDetection] " + ex.Message);
                Logger.Print("[EXCEPTION CheatDetection] " + ex.StackTrace);
            }
        }
    }
}
