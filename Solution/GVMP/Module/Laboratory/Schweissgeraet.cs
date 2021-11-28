using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class Schweissgeraet : Script
    {
        public static List<int> blockedLaboratorys = new List<int>();

        public static bool useSchweissgeraet(DbPlayer dbPlayer)
        {
            Laboratory result = null;
            float distance = 99999;
            foreach (Laboratory labor in LaboratoryModule.laboratories)
            {
                Vector3 entityPosition = labor.Entrance;
                float num = dbPlayer.Client.Position.DistanceTo(entityPosition);
                if (num < distance)
                {
                    distance = num;
                    result = labor;
                }
            }

            if (result == null) return false;
            if (result.Entrance.DistanceTo2D(dbPlayer.Client.Position) > 5.0f) return false;
            if (result.FactionId == dbPlayer.Faction.Id) return false;
            if (blockedLaboratorys.Contains(result.Id))
            {
                dbPlayer.SendNotification("Dieses Labor wurde bereits ausgeraubt!", 3000, "green", "LABOR");
                return false;
            }

            Faction faction = FactionModule.getFactionById(result.FactionId);

            faction.GetFactionPlayers().ForEach((DbPlayer dbPlayer2) => dbPlayer2.SendNotification("Euer Labor wird angegriffen!", 300000, faction.GetRGBStr(), faction.Name)); 
            dbPlayer.SendNotification("Du greifst nun das Labor an!", 5000, "green", "LABOR");

            dbPlayer.disableAllPlayerActions(true);
            dbPlayer.IsFarming = true;
            dbPlayer.RefreshData(dbPlayer);
            dbPlayer.SendProgressbar(150000);
            dbPlayer.PlayAnimation(33, "amb@world_human_welding@male@base", "base", 8f);

            blockedLaboratorys.Add(result.Id);
            NAPI.Task.Run(() =>
            {
                dbPlayer.SendNotification("Labor erfolgreich angegriffen!", 3000, "green", "LABOR");
                dbPlayer.addMoney(new Random().Next(500000, 1500000));
                dbPlayer.UpdateInventoryItems("Advancedrifle", 6, false);
                dbPlayer.UpdateInventoryItems("Gusenberg", 7, false);
                dbPlayer.disableAllPlayerActions(false);
                dbPlayer.IsFarming = false;
                dbPlayer.RefreshData(dbPlayer);
                dbPlayer.StopProgressbar();
                dbPlayer.StopAnimation();
            }, 300000);

            return true;
        }
    }
}
