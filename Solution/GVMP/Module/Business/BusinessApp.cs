using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using GVMP;

namespace GVMP
{
	internal class BusinessApp : Script
	{
		[RemoteEvent("requestBusinessMembers")]
		public static void requestBusinessMembers(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null || !player.CanInteractAntiFlood(1) || player.Business.Id == 0)
				{
					return;
				}
				List<BusinessMember> list = new List<BusinessMember>();
				int managePermission = 0;
				if (player.Businessrank > 0 && player.Businessrank != 2)
				{
					managePermission = 1;
				}
				else if (player.Businessrank == 2)
				{
					managePermission = 2;
				}
				foreach (DbPlayer businessPlayer in player.Business.GetBusinessPlayers())
				{
					list.Add(new BusinessMember
					{
						Id = businessPlayer.Id,
						Name = businessPlayer.Name,
						Owner = (businessPlayer.Businessrank == 2),
						Manage = (businessPlayer.Businessrank > 0),
						Number = businessPlayer.Id
					});
				}
				list = list.OrderBy((BusinessMember obj) => obj.Manage ? 1 : (obj.Owner ? 2 : 0)).ToList();
				list.Reverse();
				object obj2 = new
				{
					BusinessMemberList = list,
					ManagePermission = managePermission
				};
				player.TriggerEvent("componentServerEvent", "BusinessListApp", "responseBusinessMembers", NAPI.Util.ToJson(obj2));
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION responseBusinessMembers] " + ex.Message);
				Logger.Print("[EXCEPTION responseBusinessMembers] " + ex.StackTrace);
			}
		}

		[RemoteEvent("requestBusinessMOTD")]
		public static void requestBusinessMOTD(Client c)
		{
			try
			{
				if (!((Entity)(object)c == (Entity)null))
				{
					DbPlayer player = c.GetPlayer();
					if (player != null && player.IsValid(ignorelogin: true) && !((Entity)(object)player.Client == (Entity)null) && player.Business.Id != 0)
					{
						player.TriggerEvent("componentServerEvent", "BusinessListApp", "responseBusinessMOTD", player.Business.Motd());
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION requestBusinessMOTD] " + ex.Message);
				Logger.Print("[EXCEPTION requestBusinessMOTD] " + ex.StackTrace);
			}
		}

		[RemoteEvent("saveBusinessMOTD")]
		public void saveBusinessMOTD(Client c, string motd)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player != null && player.IsValid(ignorelogin: true) && !((Entity)(object)player.Client == (Entity)null) && player.CanInteractAntiFlood(1) && player.Business.Id != 0 && player.Businessrank >= 1)
				{
					if (motd.Length > 32)
					{
						player.SendNotification("MOTD zu lang, maximal 32 Zeichen!", 3000, "orange", "Business");
						return;
					}
					MySqlQuery mySqlQuery = new MySqlQuery("UPDATE businesses SET Motd = @motd WHERE Id = @id");
					mySqlQuery.AddParameter("@id", player.Business.Id);
					mySqlQuery.AddParameter("@motd", motd);
					MySqlHandler.ExecuteSync(mySqlQuery);
					player.SendNotification("MOTD geändert!", 3000, "orange", player.Business.Name);
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION saveBusinessMOTD] " + ex.Message);
				Logger.Print("[EXCEPTION saveBusinessMOTD] " + ex.StackTrace);
			}
		}

		[RemoteEvent("leaveBusiness")]
		public void leaveBusiness(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null || !player.CanInteractAntiFlood(1) || player.Business.Id == 0)
				{
					return;
				}
				player.SetAttribute("Business", 0);
				player.SetAttribute("Businessrank", 0);
				foreach (DbPlayer businessPlayer in player.Business.GetBusinessPlayers())
				{
					businessPlayer.SendNotification("Der Spieler " + c.Name + " hat das Business verlassen.", 3000, "orange", businessPlayer.Business.Name);
				}
				player.Business = BusinessModule.getBusinessById(0);
				player.Businessrank = 0;
				player.RefreshData(player);
				player.SendNotification("Du hast das Business verlassen.", 3000, "orange", "Business");
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION leaveBusiness] " + ex.Message);
				Logger.Print("[EXCEPTION leaveBusiness] " + ex.StackTrace);
			}
		}

		[RemoteEvent("editBusinessMember")]
		public void editBusinessMember(Client c, int id, bool manage)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player != null && player.IsValid(ignorelogin: true) && !((Entity)(object)player.Client == (Entity)null) && player.CanInteractAntiFlood(1) && player.Business.Id != 0 && player.Businessrank >= 1)
				{
					DbPlayer player2 = PlayerHandler.GetPlayer(id);
					if (player2 == null || !player2.IsValid(ignorelogin: true))
					{
						player.SendNotification("Spieler nicht online!", 3000, "red");
					}
					else if (player2.Business.Id == player.Business.Id && player2.Businessrank <= player.Businessrank)
					{
						player2.SetAttribute("Businessrank", Convert.ToInt32(manage));
						player2.Businessrank = Convert.ToInt32(manage);
						player2.RefreshData(player2);
						player.SendNotification("Du hast den Spieler aktualisiert.", 3000, "orange", player.Business.Name);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION editBusinessMember] " + ex.Message);
				Logger.Print("[EXCEPTION editBusinessMember] " + ex.StackTrace);
			}
		}

		[RemoteEvent("kickBusinessMember")]
		public void kickBusinessMember(Client c, int id)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null || !player.CanInteractAntiFlood(1) || player.Business.Id == 0 || player.Businessrank < 1)
				{
					return;
				}
				DbPlayer player2 = PlayerHandler.GetPlayer(id);
				if (player2 == null || !player2.IsValid(ignorelogin: true))
				{
					player.SendNotification("Spieler nicht online!", 3000, "red");
				}
				else
				{
					if (player2.Business.Id != player.Business.Id)
					{
						return;
					}
					if (player.Businessrank <= player2.Businessrank)
					{
						player.SendNotification("Das kannst du nicht da der Spieler den/einen höheren Rang als du hat!", 3000, "orange", player.Business.Name);
						return;			
					}
					player2.SetAttribute("Business", 0);
					player2.SetAttribute("Businessrank", 0);
					player2.Business = BusinessModule.getBusinessById(0);
					player2.Businessrank = 0;
					player2.RefreshData(player2);
					foreach (DbPlayer businessPlayer in player.Business.GetBusinessPlayers())
					{
						businessPlayer.SendNotification("Der Spieler " + player2.Name + " hat das Business verlassen.", 3000, "orange", player.Business.Name);
					}
					player.SendNotification("Du hast den Spieler gekickt.", 3000, "orange", player.Business.Name);
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION kickBusinessMember] " + ex.Message);
				Logger.Print("[EXCEPTION kickBusinessMember] " + ex.StackTrace);
			}
		}

		[RemoteEvent("addPlayerToBusiness")]
		public void addPlayerToBusiness(Client c, string name)
		{
			if ((Entity)(object)c == (Entity)null)
			{
				return;
			}
			DbPlayer player = c.GetPlayer();
			if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null)
			{
				return;
			}
			try
			{
				if (!player.CanInteractAntiFlood(1) || player.Business == null || player.Business.Id == 0)
				{
					return;
				}
				if (player.Businessrank > 0)
				{
					DbPlayer player2 = PlayerHandler.GetPlayer(name);
					if (player2 == null || !player2.IsValid(ignorelogin: true))
					{
						player.SendNotification("Spieler nicht online!", 3000, "red");
					}
					else if (player2.Business.Id == player.Business.Id)
					{
						player.SendNotification("Der Spieler ist bereits in deinem Business.", 3000, "orange", "BUSINESS");
					}
					else if (player2.Business.Id == 0)
					{
						player2.TriggerEvent("openWindow", "Confirmation", "{\"confirmationObject\":{\"Title\":\"" + player.Business.Name + "\",\"Message\":\"Möchtest du die Einladung von " + c.Name + " annehmen?\",\"Callback\":\"acceptBusinessInvite\",\"Arg1\":" + player.Business.Id + ",\"Arg2\":\"\"}}");
						player.SendNotification("Du hast " + name + " eine Einladung gesendet.", 3000, "orange", "BUSINESS");
					}
					else
					{
						player.SendNotification("Dieser Spieler ist bereits in einem Business.", 3000, "orange", "BUSINESS");
					}
				}
				else
				{
					player.SendNotification("Du hast dazu keine Berechtigung.", 3000, "orange", "BUSINESS");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION addPlayerToBusiness] " + ex.Message);
				Logger.Print("[EXCEPTION addPlayerToBusiness] " + ex.StackTrace);
			}
		}

		[RemoteEvent("acceptBusinessInvite")]
		public void acceptBusinessInvite(Client c, string bus, object unused)
		{
			if ((Entity)(object)c == (Entity)null)
			{
				return;
			}
			DbPlayer player = c.GetPlayer();
			if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null)
			{
				return;
			}
			try
			{
				Business businessById = BusinessModule.getBusinessById(Convert.ToInt32(bus));
				if (businessById == null)
				{
					return;
				}
				foreach (DbPlayer businessPlayer in businessById.GetBusinessPlayers())
				{
					businessPlayer.SendNotification("Der Spieler " + c.Name + " hat das Business betreten.", 3000, "orange", businessPlayer.Business.Name);
				}
				player.Business = businessById;
				player.Businessrank = 0;
				player.RefreshData(player);
				player.SetAttribute("Business", businessById.Id);
				player.SetAttribute("Businessrank", 0);
				player.SendNotification("Du bist dem Business " + businessById.Name + " beigetreten.", 3000, "orange", "BUSINESS");
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION acceptBusinessInvite] " + ex.Message);
				Logger.Print("[EXCEPTION acceptBusinessInvite] " + ex.StackTrace);
			}
		}

		public BusinessApp()
		{
		}
	}

}