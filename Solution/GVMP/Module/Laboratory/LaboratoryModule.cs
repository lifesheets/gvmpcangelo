using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GVMP
{
    class LaboratoryModule : GVMP.Module.Module<LaboratoryModule>
    {
		public static List<Laboratory> laboratories = new List<Laboratory>();

		public static Vector3 LaboratoryInteriorPoint = new Vector3(997.0, -3201.0, -37.8);

		public static Vector3 EphiDustProcessingPoint = new Vector3(1012f, -3195f, -40f);

		public static Vector3 BatteryProcessingPoint = new Vector3(1017f, -3195f, -40f);

		public static Vector3 MaterialStoragePoint = new Vector3(1008f, -3200f, -40f);

		public static Vector3 MaterialProductsPoint = new Vector3(1006f, -3200f, -40f);

		public static Vector3 LaboratorySetingsPoint = new Vector3(1011f, -3197f, -40f);

		protected override bool OnLoad()
		{
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Expected O, but got Unknown
			//IL_018a: Expected O, but got Unknown
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Expected O, but got Unknown
			//IL_0434: Expected O, but got Unknown
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Expected O, but got Unknown
			//IL_0466: Expected O, but got Unknown
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Expected O, but got Unknown
			//IL_0498: Expected O, but got Unknown
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Expected O, but got Unknown
			//IL_04ca: Expected O, but got Unknown
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Expected O, but got Unknown
			//IL_04fc: Expected O, but got Unknown
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Expected O, but got Unknown
			//IL_052e: Expected O, but got Unknown
			MySqlQuery query = new MySqlQuery("SELECT * FROM laboratorys");
			MySqlResult query2 = MySqlHandler.GetQuery(query);
			try
			{
				MySqlDataReader reader = query2.Reader;
				try
				{
					if ((reader).HasRows)
					{
						while ((reader).Read())
						{
							Laboratory laboratory = new Laboratory
							{
								Id = reader.GetInt32("Id"),
								FactionId = reader.GetInt32("FactionId"),
								Entrance = NAPI.Util.FromJson<Vector3>(reader.GetString("Entrance")),
								Status = true
							};
							laboratories.Add(laboratory);
							ColShape val = NAPI.ColShape.CreateCylinderColShape(laboratory.Entrance, 1.4f, 1.4f, 0u);
							((Entity)val).SetData("FUNCTION_MODEL", (object)new FunctionModel("enterLaboratory", laboratory.Id));
							((Entity)val).SetData("MESSAGE", (object)new Message("Drücke E um das Labor zu betreten.", FactionModule.getFactionById(laboratory.FactionId).Name, FactionModule.getFactionById(laboratory.FactionId).GetRGBStr()));
							NAPI.Blip.CreateBlip(499, laboratory.Entrance, 1f, (byte)FactionModule.getFactionById(laboratory.FactionId).Blip, "Labor - " + FactionModule.getFactionById(laboratory.FactionId).Name, 255, 0, true, 0);
							NAPI.Marker.CreateMarker(1, laboratory.Entrance, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, 0u);
						}
					}
				}
				finally
				{
					reader.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION loadLaboratorys] " + ex.Message);
				Logger.Print("[EXCEPTION loadLaboratorys] " + ex.StackTrace);
			}
			finally
			{
				query2.Connection.Dispose();
			}
			ColShape val2 = NAPI.ColShape.CreateCylinderColShape(LaboratoryInteriorPoint, 1.4f, 1.4f, uint.MaxValue);
			((Entity)val2).SetData("FUNCTION_MODEL", (object)new FunctionModel("leaveLaboratory"));
			((Entity)val2).SetData("MESSAGE", (object)new Message("Drücke E um das Labor zu verlassen.", "LABOR", "green"));
			ColShape val3 = NAPI.ColShape.CreateCylinderColShape(BatteryProcessingPoint, 1.4f, 1.4f, uint.MaxValue);
			((Entity)val3).SetData("FUNCTION_MODEL", (object)new FunctionModel("processBattery"));
			((Entity)val3).SetData("MESSAGE", (object)new Message("Drücke E um Batterien zu verarbeiten.", "LABOR", "green"));
			ColShape val4 = NAPI.ColShape.CreateCylinderColShape(MaterialStoragePoint, 1.4f, 1.4f, uint.MaxValue);
			((Entity)val4).SetData("FUNCTION_MODEL", (object)new FunctionModel("openMaterialStorage"));
			((Entity)val4).SetData("MESSAGE", (object)new Message("Drücke E um das Materiallager zu öffnen.", "LABOR", "green"));
			ColShape val5 = NAPI.ColShape.CreateCylinderColShape(MaterialProductsPoint, 1.4f, 1.4f, uint.MaxValue);
			((Entity)val5).SetData("FUNCTION_MODEL", (object)new FunctionModel("openMaterialProducts"));
			((Entity)val5).SetData("MESSAGE", (object)new Message("Drücke E um das Lager der Endprodukte zu öffnen.", "LABOR", "green"));
			ColShape val6 = NAPI.ColShape.CreateCylinderColShape(EphiDustProcessingPoint, 1.4f, 1.4f, uint.MaxValue);
			((Entity)val6).SetData("FUNCTION_MODEL", (object)new FunctionModel("processDust"));
			((Entity)val6).SetData("MESSAGE", (object)new Message("Drücke E um Ephidrinkonzentrat zu verarbeiten.", "LABOR", "green"));
			ColShape val7 = NAPI.ColShape.CreateCylinderColShape(LaboratorySetingsPoint, 1.4f, 1.4f, uint.MaxValue);
			((Entity)val7).SetData("FUNCTION_MODEL", (object)new FunctionModel("openLabor"));
			((Entity)val7).SetData("MESSAGE", (object)new Message("Drücke E um das Labor zu steuern", "LABOR", "green"));
			NAPI.Marker.CreateMarker(1, LaboratorySetingsPoint, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, uint.MaxValue);
			NAPI.Marker.CreateMarker(1, EphiDustProcessingPoint, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, uint.MaxValue);
			NAPI.Marker.CreateMarker(1, LaboratoryInteriorPoint, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, uint.MaxValue);
			NAPI.Marker.CreateMarker(1, BatteryProcessingPoint, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, uint.MaxValue);
			NAPI.Marker.CreateMarker(1, MaterialStoragePoint, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, uint.MaxValue);
			NAPI.Marker.CreateMarker(1, MaterialProductsPoint, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, uint.MaxValue);
			return true;
		}

		[RemoteEvent("enterLaboratory")]
		public void enterLaboratory(Client c, int Id)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null || player.Faction.Id == 0)
				{
					return;
				}
				Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.Id == Id);
				if (laboratory != null && player.Faction.Id == laboratory.FactionId && player.HasData("IN_LABOR"))
				{
					bool flag = player.GetData("IN_LABOR");
					if (!flag)
					{
						player.SetData("IN_LABOR", !flag);
						player.SendNotification("Du hast das Labor " + (flag ? "verlassen" : "betreten") + ".", 3000, player.Faction.GetRGBStr(), "LABOR");
						player.Dimension = ((!flag) ? (7500 + Id) : 0);
						player.Position = (flag ? laboratory.Entrance : LaboratoryInteriorPoint);
						int num = 2;
						int num2 = 1;
						int num3 = 1;
						player.Client.TriggerEvent("loadMethInterior", new object[3] { num2, num, num3 });
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION enterLaboratory] " + ex.Message);
				Logger.Print("[EXCEPTION enterLaboratory] " + ex.StackTrace);
			}
		}

		[RemoteEvent("leaveLaboratory")]
		public void leaveLaboratory(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null || dbPlayer.Faction.Id == 0)
				{
					return;
				}
				Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
				if (laboratory != null && dbPlayer.Faction.Id == laboratory.FactionId && dbPlayer.HasData("IN_LABOR"))
				{
					bool flag = dbPlayer.GetData("IN_LABOR");
					if (flag)
					{
						dbPlayer.SetData("IN_LABOR", !flag);
						dbPlayer.SendNotification("Du hast das Labor " + (flag ? "verlassen" : "betreten") + ".", 3000, dbPlayer.Faction.GetRGBStr(), "LABOR");
						dbPlayer.Dimension = ((!flag) ? (7500 + laboratory.Id) : 0);
						dbPlayer.Position = (flag ? laboratory.Entrance : LaboratoryInteriorPoint);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION leaveLaboratory] " + ex.Message);
				Logger.Print("[EXCEPTION leaveLaboratory] " + ex.StackTrace);
			}
		}

		[RemoteEvent("processBattery")]
		public void processBattery(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null || dbPlayer.Faction.Id == 0)
				{
					return;
				}
				Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
				if (laboratory == null || dbPlayer.Faction.Id != laboratory.FactionId || !dbPlayer.HasData("IN_LABOR") || !(bool)dbPlayer.GetData("IN_LABOR") || dbPlayer.IsFarming)
				{
					return;
				}
				if (dbPlayer.GetItemAmount("Batterie") >= 9)
				{
					dbPlayer.IsFarming = true;
					dbPlayer.RefreshData(dbPlayer);
					dbPlayer.SendNotification("Du verarbeitest nun 9 Batterien!", 3000, dbPlayer.Faction.GetRGBStr(), "LABOR");
					dbPlayer.AllActionsDisabled = true;
					dbPlayer.SendProgressbar(30000);
					dbPlayer.UpdateInventoryItems("Batterie", 9, remove: true);
					NAPI.Task.Run((Action)delegate
					{
						if (NAPI.Pools.GetAllPlayers().Contains(dbPlayer.Client))
						{
							dbPlayer.IsFarming = false;
							dbPlayer.RefreshData(dbPlayer);
							dbPlayer.AllActionsDisabled = false;
							dbPlayer.StopProgressbar();
							dbPlayer.SendNotification("Du hast 9 Batterien zu 45 Batteriezellen verarbeitet!", 3000, dbPlayer.Faction.GetRGBStr(), "LABOR");
							dbPlayer.UpdateInventoryItems("Batteriezelle", 45, remove: false);
						}
					}, 30000L);
				}
				else
				{
					dbPlayer.SendNotification("Du benötigst mindestens 9 Batterien!", 3000, dbPlayer.Faction.GetRGBStr(), "LABOR");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION processBattery] " + ex.Message);
				Logger.Print("[EXCEPTION processBattery] " + ex.StackTrace);
			}
		}

		[RemoteEvent("processDust")]
		public void processDust(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null || dbPlayer.Faction.Id == 0)
				{
					return;
				}
				Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
				if (laboratory == null || dbPlayer.Faction.Id != laboratory.FactionId || !dbPlayer.HasData("IN_LABOR") || !(bool)dbPlayer.GetData("IN_LABOR") || dbPlayer.IsFarming)
				{
					return;
				}
				if (dbPlayer.GetItemAmount("Ephidrinkonzentrat") >= 90)
				{
					dbPlayer.IsFarming = true;
					dbPlayer.RefreshData(dbPlayer);
					dbPlayer.SendNotification("Du verarbeitest nun 90 Ephidrinkonzentrat!", 3000, dbPlayer.Faction.GetRGBStr(), "LABOR");
					dbPlayer.AllActionsDisabled = true;
					dbPlayer.SendProgressbar(30000);
					dbPlayer.UpdateInventoryItems("Ephidrinkonzentrat", 90, remove: true);
					NAPI.Task.Run((Action)delegate
					{
						if (NAPI.Pools.GetAllPlayers().Contains(dbPlayer.Client))
						{
							dbPlayer.IsFarming = false;
							dbPlayer.RefreshData(dbPlayer);
							dbPlayer.AllActionsDisabled = false;
							dbPlayer.StopProgressbar();
							dbPlayer.SendNotification("Du hast 90 Ephidrinkonzentrat zu 45 Ephidrinpulver verarbeitet!", 3000, dbPlayer.Faction.GetRGBStr(), "LABOR");
							dbPlayer.UpdateInventoryItems("Ephidrinpulver", 45, remove: false);
						}
					}, 30000L);
				}
				else
				{
					dbPlayer.SendNotification("Du benötigst mindestens 90 Ephidrinkonzentrat!", 3000, dbPlayer.Faction.GetRGBStr(), "LABOR");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION processDust] " + ex.Message);
				Logger.Print("[EXCEPTION processDust] " + ex.StackTrace);
			}
		}

		[RemoteEvent("openMaterialStorage")]
		public void openMaterialStorage(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null || dbPlayer.Faction.Id == 0)
				{
					return;
				}
				Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
				if (laboratory != null && dbPlayer.Faction.Id == laboratory.FactionId && dbPlayer.HasData("IN_LABOR"))
				{
					dbPlayer.SetData("USING_STORAGE", true);
					if ((bool)dbPlayer.GetData("IN_LABOR"))
					{
						dbPlayer.TriggerEvent("openWindow", "Inventory", "{\"inventory\":[{\"Id\":" + dbPlayer.Id + ",\"Name\":\"Inventar\",\"Money\":" + dbPlayer.Money + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)dbPlayer.GetInventoryItems()) + "},{\"Id\":" + laboratory.Id + ",\"Name\":\"Rohstoffe Methlabor\",\"Money\":0,\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":6,\"Slots\":" + NAPI.Util.ToJson((object)dbPlayer.GetLaborStorageItems(laboratory.Id)) + "}]}");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION openMaterialStorage] " + ex.Message);
				Logger.Print("[EXCEPTION openMaterialStorage] " + ex.StackTrace);
			}
		}

		[RemoteEvent("openMaterialProducts")]
		public void openMaterialProducts(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null || dbPlayer.Faction.Id == 0)
				{
					return;
				}
				Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
				if (laboratory != null && dbPlayer.Faction.Id == laboratory.FactionId && dbPlayer.HasData("IN_LABOR"))
				{
					bool flag = dbPlayer.GetData("IN_LABOR");
					dbPlayer.SetData("USING_STORAGE", false);
					if (flag)
					{
						dbPlayer.TriggerEvent("openWindow", "Inventory", "{\"inventory\":[{\"Id\":" + dbPlayer.Id + ",\"Name\":\"Inventar\",\"Money\":" + dbPlayer.Money + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)dbPlayer.GetInventoryItems()) + "}, {\"Id\":" + laboratory.Id + ",\"Name\":\"Endprodukte Methlabor\",\"Money\":0,\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":6,\"Slots\":" + NAPI.Util.ToJson((object)dbPlayer.GetLaborProductItems(laboratory.Id)) + "}]}");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION openMaterialProducts] " + ex.Message);
				Logger.Print("[EXCEPTION openMaterialProducts] " + ex.StackTrace);
			}
		}

		[RemoteEvent("openLabor")]
		public void openLabor(Client c)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer != null && dbPlayer.IsValid(ignorelogin: true) && !((Entity)(object)dbPlayer.Client == (Entity)null) && dbPlayer.Faction.Id != 0)
				{
					Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
					if (laboratory != null && dbPlayer.Faction.Id == laboratory.FactionId && dbPlayer.HasData("IN_LABOR") && (bool)dbPlayer.GetData("IN_LABOR"))
					{
						dbPlayer.OpenLabor(laboratory.Status);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION openLabor] " + ex.Message);
				Logger.Print("[EXCEPTION openLabor] " + ex.StackTrace);
			}
		}

		[RemoteEvent("toggleMethLabor")]
		public void toggleMethLabor(Client c, bool status)
		{
			try
			{
				if ((Entity)(object)c == (Entity)null)
				{
					return;
				}
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer != null && dbPlayer.IsValid(ignorelogin: true) && !((Entity)(object)dbPlayer.Client == (Entity)null) && dbPlayer.Faction.Id != 0)
				{
					Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
					if (laboratory != null && dbPlayer.Faction.Id == laboratory.FactionId && dbPlayer.HasData("IN_LABOR") && (bool)dbPlayer.GetData("IN_LABOR"))
					{
						laboratories.Remove(laboratory);
						laboratory.Status = status;
						laboratories.Add(laboratory);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION toggleMethLabor] " + ex.Message);
				Logger.Print("[EXCEPTION toggleMethLabor] " + ex.StackTrace);
			}
		}

		public override void OnFiveMinuteUpdate()
		{
			try
			{
				foreach (DbPlayer dbPlayer in PlayerHandler.GetPlayers())
				{
					if (dbPlayer.Faction.Id == 0)
					{
						continue;
					}
					Laboratory laboratory = laboratories.FirstOrDefault((Laboratory labor) => labor.FactionId == dbPlayer.Faction.Id);
					if (laboratory == null)
					{
						continue;
					}
					List<ItemModel> laborStorageItems = dbPlayer.GetLaborStorageItems(laboratory.Id);
					ItemModel itemModel2 = laborStorageItems.FirstOrDefault((ItemModel itemModel) => itemModel.Name == "Ephidrinpulver");
					if (itemModel2 == null)
					{
						continue;
					}
					ItemModel itemModel3 = laborStorageItems.FirstOrDefault((ItemModel itemModel) => itemModel.Name == "Batteriezelle");
					if (itemModel3 == null)
					{
						continue;
					}
					if (itemModel3.Amount >= 5 && itemModel2.Amount >= 45)
					{
						itemModel2.Amount = 45;
						itemModel3.Amount = 5;
						laboratory.UpdateLaborStorageItems(dbPlayer.Id, itemModel2, remove: true);
						laboratory.UpdateLaborStorageItems(dbPlayer.Id, itemModel3, remove: true);
						Item item3 = ItemModule.itemRegisterList.FirstOrDefault((Item item2) => item2.Name == "Meth");
						ItemModel item4 = new ItemModel
						{
							Id = item3.Id,
							Amount = 500,
							ImagePath = item3.ImagePath,
							Slot = 0,
							Name = item3.Name,
							Weight = 0
						};
						laboratory.UpdateLaborProductItems(dbPlayer.Id, item4, remove: false);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION OnFiveMinuteUpdate] " + ex.Message);
				Logger.Print("[EXCEPTION OnFiveMinuteUpdate] " + ex.StackTrace);
			}
		}
    }
}
