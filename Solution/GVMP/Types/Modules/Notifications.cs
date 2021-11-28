using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class Notifications
    {
        public static void SendNotification(this DbPlayer dbPlayer, string msg, int duration = 3000, string color = "gray", string title = "")
        {
            dbPlayer.Client.TriggerEvent("sendPlayerNotification", msg, duration, color, title, "");
        }

        public static void SendNotification2(this DbPlayer dbPlayer, string msg, string color = "gray", int duration = 3000, string title = "")
        {
            dbPlayer.Client.TriggerEvent("sendPlayerNotification", msg, duration, color, title, "");
        }
    }
}
