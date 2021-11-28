using GTANetworkAPI;
using GVMP.Module;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
	public class TattooModel
	{
		public string Name { get; set; }
		public string LocalizedName { get; set; }
		public string HashNameMale { get; set; }
		public string HashNameFemale { get; set; }
		public string Zone { get; set; }
		public int ZoneID { get; set; }
		public int Price { get; set; }

		public TattooModel() { }
	}

	public sealed class TattooModule : Module<TattooModule>
	{

		public override Type[] RequiredModules()
		{
			return new Type[1] { typeof(TattooShopModule) };
		}

		[RemoteEvent]
		public void openTattooShop(Client c, uint id)
		{
			if (c == null) return;
			DbPlayer dbPlayer = c.GetPlayer();
			if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
				return;

			if (!dbPlayer.CanInteractAntiFlood(3)) return;

			if (dbPlayer.Client.IsInVehicle)
			{
				//Logger.Print("3");
				return;
			}
			dbPlayer.SetTattooClothes();
			new TattooShopWindow().ShowTattooShop(dbPlayer, new List<ClientTattoo>());
		}
	}
}
