using GTANetworkAPI;
using GVMP.Handlers;
using GVMP.Module;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    public class KeyWindow : Script
    {
		private class ShowEvent
		{
			[JsonProperty(PropertyName = "Spielername")]
			private string PlayerName { get; }

			[JsonProperty(PropertyName = "Keys")]
			private Dictionary<string, List<VHKey>> Keys { get; }

			public ShowEvent(DbPlayer dbPlayer, string playerName, Dictionary<string, List<VHKey>> keys)
			{
				PlayerName = playerName;
				Keys = keys;
			}
		}

		public KeyWindow()
		{
		}

		public void Show(DbPlayer dbPlayer, string name, Dictionary<string, List<VHKey>> keys)
		{
			dbPlayer.TriggerEvent("openWindow", "Keys", JsonConvert.SerializeObject(new ShowEvent(dbPlayer, name, keys)));
		}

		[RemoteEvent]
		public void GivePlayerKey(Client player, string toPlayer, int id, string type)
		{
			if (player.Name.Equals(toPlayer))
			{
				return;
			}
			DbPlayer player2 = player.GetPlayer();
			if (player2 == null)
			{
				return;
			}
			DbPlayer dbPlayer = null;
			uint num = 0u;
			if (toPlayer != "0")
			{
				dbPlayer = PlayerHandler.GetPlayer(toPlayer);
				if (dbPlayer == null || !dbPlayer.IsValid())
				{
					return;
				}
				if (dbPlayer == null)
				{
					player2.SendNotification("Der Buerger ist nicht erreichbar.", 3500, "red");
					return;
				}
				if (((Entity)player2.Client).Position.DistanceTo(((Entity)dbPlayer.Client).Position) > 5f)
				{
					player2.SendNotification("Der Spieler ist nicht in deiner Nähe!", 3500, "red");
					return;
				}
			}
			else
			{
				if (player2.Business == null || player2.Business.Id == 0)
				{
					player2.SendNotification("Du bist nicht in einem Business!", 3500, "red");
					return;
				}
				num = (uint)player2.Business.Id;
			}
			switch (type)
			{
				case "Häuser":
					if (num != 0)
					{
						player2.SendNotification("Hausschlüssel können aktuell nicht im Business hinterlegt werden.", 3500, "red");
					}
					else if (player2.GetHouse() == null || player2.GetHouse().Id != id)
					{
						player2.SendNotification("Dieses Haus gehoert dir nicht!", 3500, "red");
					}
					else if (dbPlayer != null && dbPlayer.IsValid())
					{
						if (dbPlayer.HouseKeys.Contains(id))
						{
							player2.SendNotification("Der Buerger besitzt diesen Schluessel bereits!", 3500, "red");
							break;
						}
						var house = HouseModule.houses.FirstOrDefault(h => h.Id == id);
						if (house == null) return;
						HouseKeyHandler.Instance.AddHouseKey(dbPlayer, house);
						player2.SendNotification("Sie haben " + dbPlayer.Name + " einen Schluessel fuer Ihr Haus gegeben.", 3500, "green");
						dbPlayer.SendNotification(player2.Name + " hat ihnen einen Schluessel das Haus " + id + " gegeben.", 3500, "green");
					}
					break;
				case "Fahrzeuge":
					{
						DbVehicle byVehicleDatabaseId = NAPI.Pools.GetAllVehicles().FirstOrDefault(v => v.GetVehicle() != null && v.GetVehicle().Id == id).GetVehicle();
						if (byVehicleDatabaseId == null || byVehicleDatabaseId.OwnerId != player2.Id)
						{
							player2.SendNotification("Dieses Fahrzeug gehört ihnen nicht (darf nicht in der Garage sein!).", 3500, "red");
							break;
						}
						string model = byVehicleDatabaseId.Model;
						if (dbPlayer != null && dbPlayer.IsValid())
						{
							if (VehicleKeyHandler.Instance.GetVehicleKeyCount(id) >= 1)
							{
								player2.SendNotification("Es wurden bereits zu viele Schlüssel für dieses Fahrzeug vergeben!", 3500, "red");
								break;
							}
							if (dbPlayer.VehicleKeys.ContainsKey(id))
							{
								player2.SendNotification("Der Buerger besitzt diesen Schluessel bereits!", 3500, "red");
								break;
							}
							VehicleKeyHandler.Instance.AddPlayerKey(dbPlayer, id, model);
							player2.SendNotification("Sie haben " + dbPlayer.Name + " einen Schluessel fuer Fahrzeug " + model + " (" + id + ") gegeben.", 3500, "green");
							dbPlayer.SendNotification(player2.Name + " hat ihnen einen Schluessel fuer Fahrzeug " + model + " (" + id + ") gegeben.", 3500, "green");
						}
						break;
					}
			}
		}

		[RemoteEvent]
		public void DropPlayerKey(Client player, int id, string type)
		{
			DbPlayer player2 = player.GetPlayer();
			if (player2 == null || !player2.IsValid())
			{
				return;
			}
			switch (type)
			{
				case "Häuser":
					if (player2.GetHouse() == null) return;
					if (id != 0 && id == player2.GetHouse().Id)
					{
						player2.SendNotification("Du kannst den Hauptschlüssel nicht wegwerfen.", 3500, "red");
					}
					HouseKeyHandler.Instance.DeleteHouseKey(player2, HouseModule.houses[id]);
					break;
				case "Fahrzeuge":
					{
						DbVehicle byVehicleDatabaseId = NAPI.Pools.GetAllVehicles().FirstOrDefault(v => v.GetVehicle() != null && v.GetVehicle().Id == id).GetVehicle();
						if (byVehicleDatabaseId == null)
						{
							player2.SendNotification("Dieses Fahrzeug darf nicht in der Garage sein!", 3500, "red");
							break;
						}
						if (byVehicleDatabaseId.OwnerId == player2.Id)
						{
							player2.SendNotification("Du kannst den Hauptschlüssel nicht wegwerfen.", 3500, "red");
							break;
						}
						player2.SendNotification("Sie haben den Schluessel fuer das Fahrzeug " + player2.VehicleKeys.GetValueOrDefault(id) + " (" + id + ") fallen gelassen!", 3500, "red");
						VehicleKeyHandler.Instance.DeletePlayerKey(player2, id);
						break;
					}
			}
		}
	}
}
