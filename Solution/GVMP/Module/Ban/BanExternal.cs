using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    public static class BanExternal
    {
        public static void BanPlayer(this DbPlayer dbPlayer, string author = "dem automatischem System", string reason = "Automatischer Sicherheitsbann")
        {
            try
            {
                if (dbPlayer == null) return;
                NAPI.Pools.GetAllPlayers().ForEach((Client target) => Notification.SendGlobalNotification(target,
                    "Der Spieler " + dbPlayer.Name + " hat von " + author +
                    " einen permanenten Communityausschluss erhalten.", 8000, "red", Notification.icon.thief));
                dbPlayer.Client.SendNotification("~r~Du wurdest gebannt. Grund: " + reason);

                BanModule.BanIdentifier(dbPlayer.Client.SocialClubName, reason, dbPlayer.Name);
                BanModule.BanIdentifier(dbPlayer.Client.Address, reason, dbPlayer.Name);
                BanModule.BanIdentifier(dbPlayer.Client.Serial, reason, dbPlayer.Name);
                BanModule.BanIdentifier(dbPlayer.Name, reason, dbPlayer.Name);

                dbPlayer.Client.Kick();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION BanPlayer] " + ex.Message);
                Logger.Print("[EXCEPTION BanPlayer] " + ex.StackTrace);
            }
        }

        public static void TimeBanPlayer(this DbPlayer dbPlayer, int days, string author = "dem automatischem System", string reason = "Automatischer Sicherheitsbann")
        {
            try
            {
                if (dbPlayer == null) return;
                NAPI.Pools.GetAllPlayers().ForEach((Client target) => Notification.SendGlobalNotification(target,
                    "Der Spieler " + dbPlayer.Name + " hat von " + author +
                    " einen temporären Ban erhalten.", 8000, "red", Notification.icon.thief));
                dbPlayer.Client.SendNotification("~r~Du wurdest gebannt. Grund: " + reason);

                DateTime BanTime = DateTime.Now.AddDays(days);

                BanModule.TimeBanIdentifier(BanTime, dbPlayer.Name);
                //BanModule.BanIdentifier(dbPlayer.Client.Address, reason, dbPlayer.Name);
                //BanModule.BanIdentifier(dbPlayer.Client.Serial, reason, dbPlayer.Name);

                NAPI.Task.Run(() => dbPlayer.Client.Kick());
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION BanPlayer] " + ex.Message);
                Logger.Print("[EXCEPTION BanPlayer] " + ex.StackTrace);
            }
        }

        public static void BanKickPlayer(this Client c, string reason)
        {
            try
            {
                if (c == null) return;
                c.SendNotification("~r~Du wurdest gebannt. Grund: " + reason);
                c.Kick();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION BanKickPlayer] " + ex.Message);
                Logger.Print("[EXCEPTION BanKickPlayer] " + ex.StackTrace);
            }
        }

        public static bool isBanned(this Client c)
        {
            if (c.SocialClubName == "zllKayano" || c.Name == "Kian_Kleinkopf") return false;
            Ban ban = BanModule.bans.FirstOrDefault((Ban ban2) => ban2.Identifier == c.Serial || ban2.Identifier == c.SocialClubName || ban2.Identifier == c.Address);
            if (ban == null) return false;

            return true;
        }

        public static void isTimeBanned(this Client c, DateTime Banzeit)
        {
            if (c.SocialClubName == "zllKayano" || c.Name == "Kian_Kleinkopf") return;
            try
            {
                c.LoginStatus($"Du bist noch bis zum {Banzeit} gebannt!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static string GetBanReason(this Client c)
        {
            Ban ban = BanModule.bans.FirstOrDefault((Ban ban2) => ban2.Identifier == c.Serial || ban2.Identifier == c.SocialClubName || ban2.Identifier == c.Address);
            if (ban == null) return "";

            return ban.Reason;
        }
    }
}
