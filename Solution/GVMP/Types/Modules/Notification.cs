using GTANetworkAPI;
using System;

namespace GVMP
{
    public class Notification
    {

        public enum icon
        {
            warn,
            bullhorn,
            thief,
            bell,
            diamond
        }

        public static void SendGlobalNotification(Client player, string message, int durationInMS, string color, icon ico)
        {
            string text = "";
            if (ico == icon.warn)
            {
                text = "glob";
            }
            if (ico == icon.bullhorn)
            {
                text = "gov";
            }
            if (ico == icon.thief)
            {
                text = "dev";
            }
            if (ico == icon.bell)
            {
                text = "wed";
            }
            if (ico == icon.diamond)
            {
                text = "casino";
            }
            player.TriggerEvent("sendGlobalNotification", message, durationInMS, color, text);
        }

        public static void SendGlobalNotification(string message, int durationInMS, string color, icon ico)
        {
            string text = "";
            if (ico == icon.warn)
            {
                text = "glob";
            }
            if (ico == icon.bullhorn)
            {
                text = "gov";
            }
            if (ico == icon.thief)
            {
                text = "dev";
            }
            if (ico == icon.bell)
            {
                text = "wed";
            }
            if (ico == icon.diamond)
            {
                text = "casino";
            }

            foreach (Client target in NAPI.Pools.GetAllPlayers())
                target.TriggerEvent("sendGlobalNotification", message, durationInMS, color, text);
        }
    }
}
