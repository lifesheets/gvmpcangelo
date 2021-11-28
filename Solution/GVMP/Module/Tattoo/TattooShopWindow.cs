using GTANetworkAPI;
using GVMP.Module;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
	public class TattooShopWindow : Script
	{
		private class ShowEvent
		{
			[JsonProperty(PropertyName = "tattoos")]
			private List<ClientTattoo> Tattoos { get; }

			public ShowEvent(DbPlayer dbPlayer, List<ClientTattoo> tattoos)
			{
				tattoos = tattoos.OrderBy((ClientTattoo t) => t.Name).ToList();
				Tattoos = tattoos;
			}
		}

		public void ShowTattooShop(DbPlayer dbPlayer, List<ClientTattoo> tattoos)
		{
			dbPlayer.TriggerEvent("openWindow", "TattooShop", JsonConvert.SerializeObject(new ShowEvent(dbPlayer, tattoos)));
		}

		public TattooShopWindow()
		{
		}

		[RemoteEvent]
		public void requestTattooShopCategoryTattoos(Client c, uint id)
		{
			DbPlayer player = c.GetPlayer();
			if (player != null && player.IsValid())
			{
				IEnumerable<ClientTattoo> list = new List<ClientTattoo>();

				if (!ClothingManager.isMale(player.Client))
				{
					list = AssetsTattooModule.clientTattoosFemale.Where(x => x.ZoneId == id);
				}
				else list = AssetsTattooModule.clientTattoosMale.Where(x => x.ZoneId == id);

				player.TriggerEvent("componentServerEvent", "TattooShop", "responseTattooShopCategory", JsonConvert.SerializeObject(list.ToList()));
			}

		}

		[RemoteEvent]
		public void syncTattoo(Client client, string hash)
		{
			DbPlayer player = client.GetPlayer();
			if (player != null && player.IsValid())
			{
				CustomizeModel customizeModel =
				   NAPI.Util.FromJson<CustomizeModel>(player.GetAttributeString("Customization"));
				if (customizeModel == null) return;

				ClientTattoo assetsTattoo = null;

				if (!ClothingManager.isMale(player.Client))
				{
					assetsTattoo = AssetsTattooModule.clientTattoosFemale.FirstOrDefault(t => t.TattooHash == hash);
				}
				else assetsTattoo = AssetsTattooModule.clientTattoosMale.FirstOrDefault(t => t.TattooHash == hash);

				player.Client.ClearDecorations();
				if (assetsTattoo != null)
				{
					Decoration val = default(Decoration);
					val.Collection = NAPI.Util.GetHashKey(assetsTattoo.Collection);
					val.Overlay = NAPI.Util.GetHashKey(assetsTattoo.TattooHash);
					NAPI.Player.SetPlayerDecoration(client, val);
				}
			}
		}

		[ServerEvent(Event.PlayerExitColshape)]
		public void exitCol(ColShape shape, Client c)
		{
			try
			{
				if (shape == null || !shape.HasData("TATTOO")) return;
				DbPlayer player = c.GetPlayer();
				if (player == null) return;
				player.ApplyCharacter();
				WeaponManager.loadWeapons(c);
			}
			catch (Exception ex)
			{
				Logger.Print("leaveTattoo: " + ex.ToString());
			}
		}

		[RemoteEvent]
		public void tattooShopBuy(Client client, string hash)
		{
			try
			{
				DbPlayer player = client.GetPlayer();
				if (player == null || !player.IsValid())
				{
					return;
				}
				CustomizeModel customizeModel = NAPI.Util.FromJson<CustomizeModel>(player.GetAttributeString("Customization"));
				if (customizeModel == null) return;

				ClientTattoo assetsTattoo = null;

				if (!ClothingManager.isMale(player.Client))
				{
					assetsTattoo = AssetsTattooModule.clientTattoosFemale.FirstOrDefault(t => t.TattooHash == hash);
				}
				else assetsTattoo = AssetsTattooModule.clientTattoosMale.FirstOrDefault(t => t.TattooHash == hash);

				if (assetsTattoo == null)
				{
					return;
				}
				if (player.Money < assetsTattoo.Price)
				{
					player.SendNotification($"Sie haben nicht genug Geld! Benoetigt ({assetsTattoo.Price}$)");
					return;
				}
				player.removeMoney(assetsTattoo.Price);
				Decoration val = default(Decoration);
				val.Collection = NAPI.Util.GetHashKey(assetsTattoo.Collection);
				val.Overlay = NAPI.Util.GetHashKey(assetsTattoo.TattooHash);
				NAPI.Player.SetPlayerDecoration(client, val);
				player.AddTattoo(assetsTattoo.Id);
				player.SendNotification($"Tattoo {assetsTattoo.Name} fuer ${assetsTattoo.Price} gekauft!", 3500, "green", "TATTOO");
				player.ApplyDecorations();
			}
			catch
			{

			}
		}
	}
}
