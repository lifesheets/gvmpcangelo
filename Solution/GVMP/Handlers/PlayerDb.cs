using GTANetworkAPI;
using GVMP;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
   public static class PlayerDb
    {
		public static DbPlayer GetPlayer(this Client player)
		{
			if ((Entity)(object)player == (Entity)null)
			{
				return null;
			}
			if (!((Entity)player).HasData("player"))
			{
				return null;
			}
			DbPlayer dbPlayer = ((Entity)player).GetData("player") as DbPlayer;
			return (dbPlayer != null) ? dbPlayer : null;
		}

		public static bool CanInteractAntiDeath(this DbPlayer iPlayer)
		{
			if (iPlayer.LastDeath.AddSeconds(3.0) > DateTime.Now)
			{
				//iPlayer.SendNotification("Bitte warte kurz!");
				return false;
			}
			return true;
		}

		public static bool CanInteractAntiFlood(this DbPlayer iPlayer, int seconds = 3)
		{
			if (iPlayer.LastInteracted.AddSeconds(seconds) > DateTime.Now)
			{
			//	iPlayer.SendNotification("Bitte warte kurz!");
				return false;
			}
			iPlayer.LastInteracted = DateTime.Now;
			return true;
		}

	public static bool CanPressE(this DbPlayer iPlayer)
		{
			if (iPlayer.LastEInteract.AddSeconds(3.0) > DateTime.Now)
			{
				return false;
			}
			iPlayer.LastEInteract = DateTime.Now;
			return true;
		}

		public static bool IsValid(this DbPlayer iPlayer, bool ignorelogin = false)
		{
			if (iPlayer == null || ((Entity)iPlayer.Client).IsNull || (Entity)(object)iPlayer.Client == (Entity)null || !NAPI.Pools.GetAllPlayers().Contains(iPlayer.Client))
			{
				return false;
			}
			return ignorelogin || iPlayer.AccountStatus == AccountStatus.LoggedIn;
		}
	}
}
