using System.Collections.Generic;
using GTANetworkAPI;
using GVMP;

namespace GVMP
{
	internal class BusinessBank : Script
	{
		public static void OpenBusinessBank(DbPlayer dbPlayer)
		{
			if (dbPlayer.Business.Id != 0)
			{
				Bank bank = new Bank
				{
					Playername = dbPlayer.Name,
					Money = dbPlayer.Money,
					Balance = dbPlayer.Business.Money,
					History = new List<BankHistory>(),
					Bankaccount = dbPlayer.Business.Name
				};
				dbPlayer.ResetData("USING_FACTIONBANK");
				dbPlayer.SetData("USING_BUSINESSBANK", true);
				dbPlayer.OpenBank(bank);
			}
		}

		public static void bankDeposit(DbPlayer dbPlayer, int amount)
		{
			if (dbPlayer.Business.Id != 0 && dbPlayer.HasData("USING_BUSINESSBANK") && !((!dbPlayer.GetData("USING_BUSINESSBANK")) ? true : false))
			{
				if (dbPlayer.Money >= amount)
				{
					dbPlayer.removeMoney(amount);
					dbPlayer.Business.addMoney(amount);
					dbPlayer.SendNotification(amount.ToDots() + "$ erfolgreich eingezahlt!", 3000, "green");
				}
				else
				{
					dbPlayer.SendNotification("Zu wenig Geld!", 3000, "red");
				}
			}
		}

		public static void bankPayout(DbPlayer dbPlayer, int amount)
		{
			if (dbPlayer.Business.Id != 0 && dbPlayer.Businessrank == 2 && dbPlayer.HasData("USING_BUSINESSBANK") && !((!dbPlayer.GetData("USING_BUSINESSBANK")) ? true : false))
			{
				if (dbPlayer.Business.Money >= amount)
				{
					dbPlayer.addMoney(amount);
					dbPlayer.Business.removeMoney(amount);
					dbPlayer.SendNotification(amount.ToDots() + "$ erfolgreich ausgezahlt!", 3000, "green");
				}
				else
				{
					dbPlayer.SendNotification("Zu wenig Geld auf dem Businesskonto!", 3000, "red");
				}
			}
		}

		public BusinessBank()
		{
		}
	}
}