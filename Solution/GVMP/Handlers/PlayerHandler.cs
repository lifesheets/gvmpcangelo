using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    public static class PlayerHandler
    {
        public static List<DbPlayer> GetPlayers()
        {
            List<DbPlayer> dbPlayers = new List<DbPlayer>();
            List<Client> clients = NAPI.Pools.GetAllPlayers();

            foreach (Client c in clients)
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.Client.IsNull)
                    continue;

                dbPlayers.Add(dbPlayer);
            }

            return dbPlayers;
        }

        public static List<DbPlayer> GetAdminPlayers()
        {
            List<DbPlayer> dbPlayers = new List<DbPlayer>();
            List<Client> clients = NAPI.Pools.GetAllPlayers();

            foreach (Client c in clients)
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.Client.IsNull || dbPlayer.Adminrank.Permission <= 0)
                    continue;

                dbPlayers.Add(dbPlayer);
            }

            return dbPlayers;
        }

/**/
        public static List<DbPlayer> GetFactionPlayers(this Faction faction)
        {
            List<DbPlayer> dbPlayers = new List<DbPlayer>();
            List<Client> clients = NAPI.Pools.GetAllPlayers();

            foreach (Client c in clients)
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.Client.IsNull || dbPlayer.Faction.Id != faction.Id)
                    continue;

                dbPlayers.Add(dbPlayer);
            }

            return dbPlayers;
        }

        public static List<DbPlayer> GetBusinessPlayers(this Business business)
        {
            List<DbPlayer> dbPlayers = new List<DbPlayer>();
            List<Client> clients = NAPI.Pools.GetAllPlayers();

            foreach (Client c in clients)
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.Client.IsNull || dbPlayer.Business.Id != business.Id)
                    continue;

                dbPlayers.Add(dbPlayer);
            }

            return dbPlayers;
        }

        public static DbPlayer GetPlayer(string Name)
        {
            DbPlayer dbPlayer = GetPlayers().FirstOrDefault((DbPlayer dbPlayer) => dbPlayer.Name == Name);

            return dbPlayer;
        }


        public static DbPlayer GetPlayer(int Id)
        {
            DbPlayer dbPlayer = GetPlayers().FirstOrDefault((DbPlayer dbPlayer) => dbPlayer.Id == Id);

            return dbPlayer;
        }
    }
}
