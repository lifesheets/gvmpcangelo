using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class PlayerDisconnect : Script
    {
		[ServerEvent(Event.PlayerDisconnected)]
		public void OnPlayerDisconnect(Client c, DisconnectionType type, string reason)
		{
			lock (c)
			{
				NAPI.Player.GetPlayersInRadiusOfPlayer(50.0, c).ForEach(delegate (Client player)
				{
					DbPlayer player3 = player.GetPlayer();
					if (player3 != null)
					{
						player3.SendNotification("Der Spieler " + c.Name + " hat sich ausgeloggt.", 4000, "yellow", "ANTI-OFFLINEFLUCHT");
					}
				});
			}
		}
	}
}
