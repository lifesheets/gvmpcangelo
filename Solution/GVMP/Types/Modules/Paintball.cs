using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace GVMP
{
    public static class Paintball
    {
        public static void initializePaintball(this DbPlayer dbPlayer)
        {
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

            dbPlayer.Client.TriggerEvent("initializePaintball");
        }

        public static void finishPaintball(this DbPlayer dbPlayer)
        {
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

            dbPlayer.Client.TriggerEvent("finishPaintball");
        }

        public static void updatePaintballScore(this DbPlayer dbPlayer, int kills, int deaths)
        {
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;
            float kd = 0;
            if (kills > 0 && deaths > 0) kd = kills / deaths;
            dbPlayer.Client.TriggerEvent("updatePaintballScore", kills, deaths, kd);
        }
    }
}
