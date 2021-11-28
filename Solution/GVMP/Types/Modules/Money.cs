using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class Money
    {
        public static void removeMoney(this DbPlayer dbPlayer, int amount)
        {
            dbPlayer.Money -= amount;
            dbPlayer.RefreshData(dbPlayer);

            MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Money = @money WHERE Id = @id");
            mySqlQuery.AddParameter("@money", dbPlayer.Money);
            mySqlQuery.AddParameter("@id", dbPlayer.Id);
            MySqlHandler.ExecuteSync(mySqlQuery);

            dbPlayer.TriggerEvent("updateMoney", dbPlayer.Money);
        }

        public static void addMoney(this DbPlayer dbPlayer, int amount)
        {
            dbPlayer.Money += amount;
            dbPlayer.RefreshData(dbPlayer);

            MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Money = @money WHERE Id = @id");
            mySqlQuery.AddParameter("@money", dbPlayer.Money);
            mySqlQuery.AddParameter("@id", dbPlayer.Id);
            MySqlHandler.ExecuteSync(mySqlQuery);

            dbPlayer.TriggerEvent("updateMoney", dbPlayer.Money);
        }

        public static void removeMoney(this Faction faction, int amount)
        {
            FactionModule.factionList.Remove(faction);
            faction.Money -= amount;
            FactionModule.factionList.Add(faction);

            MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET Money = @money WHERE Id = @id");
            mySqlQuery.AddParameter("@money", faction.Money);
            mySqlQuery.AddParameter("@id", faction.Id);
            MySqlHandler.ExecuteSync(mySqlQuery);
        }

        public static void addMoney(this Faction faction, int amount)
        {
            FactionModule.factionList.Remove(faction);
            faction.Money += amount;
            FactionModule.factionList.Add(faction);

            MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktionen SET Money = @money WHERE Id = @id");
            mySqlQuery.AddParameter("@money", faction.Money);
            mySqlQuery.AddParameter("@id", faction.Id);
            MySqlHandler.ExecuteSync(mySqlQuery);
        }
    }
}
