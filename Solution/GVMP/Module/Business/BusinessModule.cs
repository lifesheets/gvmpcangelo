using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GTANetworkAPI;
using GVMP;
using GVMP.Module;
using MySql.Data.MySqlClient;

namespace GVMP
{
	internal class BusinessModule : Module<BusinessModule>
	{
		public static List<Business> businesses = new List<Business>();

		public static Vector3 InteriorPosition = new Vector3(-77.84, -830.57, 243.39);

		public static Vector3 StoragePosition = new Vector3(-82.21, -801.23, 243.39);

		public static Vector3 EntrancePosition = new Vector3(-66.47, -802.39, 44.23);

		protected override bool OnLoad()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Expected O, but got Unknown
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Expected O, but got Unknown
			//IL_01e3: Expected O, but got Unknown
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Expected O, but got Unknown
			//IL_022e: Expected O, but got Unknown
			MySqlConnection val = new MySqlConnection(Configuration.connectionString);
			try
			{
				try
				{
					((DbConnection)(object)val).Open();
					MySqlCommand val2 = val.CreateCommand();
					((DbCommand)(object)val2).CommandText = "SELECT * FROM businesses";
					MySqlDataReader val3 = val2.ExecuteReader();
					try
					{
						if ((val3).HasRows)
						{
							while ((val3).Read())
							{
								Business item = new Business
								{
									Id = val3.GetInt32("Id"),
									Name = val3.GetString("Name"),
									Money = val3.GetInt32("Money")
								};
								businesses.Add(item);
							}
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				catch (Exception ex)
				{
					Logger.Print("[EXCEPTION loadBusinesses] " + ex.Message);
					Logger.Print("[EXCEPTION loadBusinesses] " + ex.StackTrace);
				}
				finally
				{
					val.Dispose();
				}
				ColShape val4 = NAPI.ColShape.CreateCylinderColShape(EntrancePosition, 1.4f, 1.4f, 0u);
				((Entity)val4).SetData("FUNCTION_MODEL", (object)new FunctionModel("enterBusinesstower"));
				((Entity)val4).SetData("MESSAGE", (object)new Message("Benutze E um den Businesstower zu betreten.", "BUSINESS", "orange"));
				ColShape val5 = NAPI.ColShape.CreateCylinderColShape(InteriorPosition, 1.4f, 1.4f, uint.MaxValue);
				((Entity)val5).SetData("FUNCTION_MODEL", (object)new FunctionModel("leaveBusinesstower"));
				((Entity)val5).SetData("MESSAGE", (object)new Message("Benutze E um den Businesstower zu verlassen.", "BUSINESS", "orange"));
				NAPI.Marker.CreateMarker(1, InteriorPosition.Subtract(new Vector3(0f, 0f, 1f)), new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, uint.MaxValue);
				NAPI.Marker.CreateMarker(1, EntrancePosition.Subtract(new Vector3(0f, 0f, 1f)), new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, 0u);
				NAPI.Blip.CreateBlip(106, EntrancePosition, 1f, (byte)0, "Businesstower", byte.MaxValue, 0f, true, (short)0, 0u);
				return true;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		[RemoteEvent("leaveBusinesstower")]
		public void leaveBusinesstower(Client c)
		{
			try
			{
				if (!((Entity)(object)c == (Entity)null))
				{
					DbPlayer player = c.GetPlayer();
					if (player != null && player.IsValid(ignorelogin: true) && !((Entity)(object)player.Client == (Entity)null))
					{
						NativeMenu nativeMenu = new NativeMenu("Businesstower", "", new List<NativeItem>
					{
						new NativeItem("Businesstower verlassen", "leave")
					});
						player.ShowNativeMenu(nativeMenu);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION leaveBusinesstower] " + ex.Message);
				Logger.Print("[EXCEPTION leaveBusinesstower] " + ex.StackTrace);
			}
		}

		[RemoteEvent("enterBusinesstower")]
		public void enterBusinesstower(Client c)
		{
			try
			{
				if (!((Entity)(object)c == (Entity)null))
				{
					DbPlayer player = c.GetPlayer();
					if (player != null && player.IsValid(ignorelogin: true) && !((Entity)(object)player.Client == (Entity)null))
					{
						NativeMenu nativeMenu = new NativeMenu("Businesstower", "", new List<NativeItem>
					{
						new NativeItem("Businesstower betreten", "enter"),
						new NativeItem("Business beantragen", "buy")
					});
						player.ShowNativeMenu(nativeMenu);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION enterBusinesstower] " + ex.Message);
				Logger.Print("[EXCEPTION enterBusinesstower] " + ex.StackTrace);
			}
		}
		[RemoteEvent]
		public void TestBusiness(string test) => Logger.Print(test);

	/*	[RemoteEvent]
		public void TestBusiness2() => Environment.Exit(0);*/

		[RemoteEvent("nM-Businesstower")]
		public void Businesstower(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null || string.IsNullOrEmpty(selection))
				{
					return;
				}
				player.CloseNativeMenu();
				switch (selection)
				{
					case "enter":
						if (player.Business.Id == 0)
						{
							player.SendNotification("Du bist aktuell nicht in einem Business!", 3000, "orange", "BUSINESS");
							break;
						}
						player.Position = InteriorPosition;
						player.Dimension = 1000 + player.Business.Id;
						player.SendNotification("Du hast den Businesstower betreten!", 3000, "orange", "BUSINESS");
						break;
					case "leave":
						if (player.Business.Id == 0)
						{
							player.SendNotification("Du bist aktuell nicht in einem Business!", 3000, "orange", "BUSINESS");
							break;
						}
						player.Position = EntrancePosition;
						player.Dimension = 0;
						player.SendNotification("Du hast den Businesstower verlassen!", 3000, "orange", "BUSINESS");
						break;
					case "buy":
						if (player.Business.Id != 0)
						{
							player.SendNotification("Du bereits in einem Business!", 3000, "orange", "BUSINESS");
							break;
						}
						player.OpenTextInputBox(new TextInputBoxObject
						{
							Title = "Business beantragen (8.000.000$)",
							Message = "Bitte gebe den Namen für dein neues Business an. Du kannst ihn danach nicht mehr ändern! Sobald du deinen Antrag bestätigt hast, wird dir 50.000.000$ abgezogen.",
							Callback = "createBusiness"
						});
						break;
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION nM-Businesstower] " + ex.Message);
				Logger.Print("[EXCEPTION nM-Businesstower] " + ex.StackTrace);
			}
		}

		[RemoteEvent("createBusiness")]
		public void createBusiness(Client c, string text)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null || string.IsNullOrEmpty(text))
				{
					return;
				}
				if (player.Money >= 10000000)
				{
					bool flag = true;
					if (text.Length > 32)
					{
						flag = false;
					}
					if (text != text.RemoveSpecialCharacters())
					{
						flag = false;
					}
					if (flag)
					{
						flag = businesses.FirstOrDefault((Business business2) => business2.Name.ToLower() == text.ToLower()) == null;
					}
					if (flag)
					{
						player.removeMoney(10000000);
						Business business3 = new Business
						{
							Id = new Random().Next(10000, int.MaxValue),
							Money = 0,
							Name = text
						};
						player.SetAttribute("Business", business3.Id);
						player.SetAttribute("Businessrank", 2);
						player.Business = business3;
						player.Businessrank = 2;
						player.RefreshData(player);
						MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO businesses (Id, Name) VALUES (@id, @name)");
						mySqlQuery.AddParameter("@id", business3.Id);
						mySqlQuery.AddParameter("@name", business3.Name);
						MySqlHandler.ExecuteSync(mySqlQuery);
						businesses.Add(business3);
						player.SendNotification("Business erfolgreich erstellt!", 3000, "orange", "BUSINESS");
						WebhookSender.SendMessage("TEXTINPUTBOX", "" + player.Name + " + " + business3.Name +" - BUSINESS", Webhooks.shoplogs, "Shoplogs");
					}
					else
					{
						player.Client.SendNotification("Entweder dieses Business ist bereits vergeben, der Name ist länger als 32 Zeichen oder es sind Sonderzeichen enthalten.", true);
					}

				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION createBusiness] " + ex.Message);
				Logger.Print("[EXCEPTION createBusiness] " + ex.StackTrace);
			}
		}

		[RemoteEvent("sendvertrag")]
		public void sendvertrag(Client c, string text)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null || string.IsNullOrEmpty(text))
				{
					return;
				}
				DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(text);
				if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
				{
					player.SendNotification("Der Spieler ist nicht online.", 3000, "red");
					return;
				}
				if (dbPlayer2.Factionrank >= 12)
				{
					player.SendNotification("Der Spieler ist nicht Leader.", 3000, "red");
					return;
				}
				dbPlayer2.TriggerEvent("openWindow", "Confirmation", "{\"confirmationObject\":{\"Title\":\"Kriegsvertrag\",\"Message\":\"Möchtest du den Kriegsvertrag von " + player.Faction.Name + " annehmen?\",\"Callback\":\"acceptkriegsvertrag\",\"Arg1\":" + player.Faction.Id + ",\"Arg2\":\"\"}}");
				player.SendNotification("Kriegsvertrag an " + dbPlayer2.Faction.Name + " gesendet!", 6000, player.Faction.GetRGBStr(), player.Faction.Name);
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION sendkrieg] " + ex.Message);
				Logger.Print("[EXCEPTION sendkrieg] " + ex.StackTrace);
			}
		}

		[RemoteEvent("acceptkriegsvertrag")]
		public void Acceptkrieg(Client c, string frakname)
		{
			if (c == null) return;
			DbPlayer dbPlayer = c.GetPlayer();
			if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
				return;

			Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(frakname));

			try
			{
				Notification.SendGlobalNotification("Die Fraktion " + dbPlayer.Faction.Name + " hat den Kriegsvertrag gegen die Fraktion " + fraktion.Name + " unterschrieben!", 8000, "orange", Notification.icon.bullhorn);
				NAPI.World.SetWeather(Weather.THUNDER);
				NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", true));
				NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("sound:playPurge"));
				NAPI.Task.Run(delegate
				{
					NAPI.World.SetWeather(Weather.EXTRASUNNY);
					NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", false));
				}, 31800L);
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION acceptInvite] " + ex.Message);
				Logger.Print("[EXCEPTION acceptInvite] " + ex.StackTrace);
			}
		}

		public static Business getBusinessById(int id)
		{
			Business business2 = businesses.FirstOrDefault((Business business) => business.Id == id);
			if (business2 == null)
			{
				return new Business
				{
					Name = "Zivilist",
					Id = 0,
					Money = 0
				};
			}
			return business2;
		}
	}

}