using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace GVMP
{
    public static class MySqlManager
    {
		public static Vector3 GetDbLocation(this DbPlayer dbPlayer)
		{
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Id = @id LIMIT 1");
			mySqlQuery.AddParameter("@id", dbPlayer.Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			try
			{
				MySqlDataReader reader = query.Reader;
				try
				{
					if ((reader).HasRows && (reader).Read())
					{
						return NAPI.Util.FromJson<Vector3>(reader.GetString("Location"));
					}
				}
				finally
				{
					if (reader != null)
					{
						reader.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetDbLocation] " + ex.Message);
				Logger.Print("[EXCEPTION GetDbLocation] " + ex.StackTrace);
			}
			finally
			{
				query.Connection.Dispose();
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static List<ItemModel> GetFactionStorageItems(this Faction faction)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			List<ItemModel> list = new List<ItemModel>();
			try
			{
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "SELECT * FROM fraktionen WHERE Id = @id LIMIT 1";
						val2.Parameters.AddWithValue("@id", (object)faction.Id);
						MySqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if ((val3).HasRows)
							{
								while ((val3).Read())
								{
									list = NAPI.Util.FromJson<List<ItemModel>>(val3.GetString("StorageData"));
									int Slot = 0;
									list.ForEach(delegate (ItemModel item)
									{
										item.Slot = Slot;
										Slot++;
									});
								}
							}
						}
						finally
						{
							val3.Dispose();
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetFactionStorageItems] " + ex.Message);
				Logger.Print("[EXCEPTION GetFactionStorageItems] " + ex.StackTrace);
			}
			return list;
		}

		public static List<ItemModel> GetHouseItems(this House house)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			List<ItemModel> list = new List<ItemModel>();
			try
			{
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "SELECT * FROM houses WHERE Id = @id LIMIT 1";
						val2.Parameters.AddWithValue("@id", (object)house.Id);
						MySqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if ((val3).HasRows)
							{
								while ((val3).Read())
								{
									list = NAPI.Util.FromJson<List<ItemModel>>(val3.GetString("Inventory"));
									int Slot = 0;
									list.ForEach(delegate (ItemModel item)
									{
										item.Slot = Slot;
										Slot++;
									});
								}
							}
						}
						finally
						{
							val3.Dispose();
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetHouseItems] " + ex.Message);
				Logger.Print("[EXCEPTION GetHouseItems] " + ex.StackTrace);
			}
			return list;
		}

		public static List<ItemModel> GetVehicleItems(this DbVehicle dbVehicle)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			List<ItemModel> list = new List<ItemModel>();
			try
			{
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "SELECT * FROM vehicles WHERE Id = @id LIMIT 1";
						val2.Parameters.AddWithValue("@id", (object)dbVehicle.Id);
						MySqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if ((val3).HasRows)
							{
								while ((val3).Read())
								{
									list = NAPI.Util.FromJson<List<ItemModel>>(val3.GetString("Items"));
									int Slot = 0;
									list.ForEach(delegate (ItemModel item)
									{
										item.Slot = Slot;
										Slot++;
									});
								}
							}
						}
						finally
						{
							val3.Dispose();
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetVehicleItems] " + ex.Message);
				Logger.Print("[EXCEPTION GetVehicleItems] " + ex.StackTrace);
			}
			return list;
		}

		public static List<ItemModel> GetInventoryItems(this DbPlayer dbPlayer)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			List<ItemModel> list = new List<ItemModel>();
			try
			{
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "SELECT * FROM inventorys WHERE Id = @id LIMIT 1";
						val2.Parameters.AddWithValue("@id", (object)dbPlayer.Id);
						MySqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if (!(val3).HasRows)
							{
								val3.Dispose();
								MySqlCommand val4 = val.CreateCommand();
								((DbCommand)(object)val4).CommandText = "INSERT INTO `inventorys` (`Id`, `Items`) VALUES(@id, @inv)";
								val4.Parameters.AddWithValue("@id", (object)dbPlayer.Id);
								val4.Parameters.AddWithValue("@inv", (object)"[]");
								((DbCommand)(object)val4).ExecuteNonQueryAsync();
								dbPlayer.SendNotification("Inventar erstellt. Bitte erneut Inventar öffnen", 3000, "green", "Inventar");
							}
							else
							{
								while ((val3).Read())
								{
									list = NAPI.Util.FromJson<List<ItemModel>>(val3.GetString("Items"));
									int Slot = 0;
									list.ForEach(delegate (ItemModel item)
									{
										item.Slot = Slot;
										Slot++;
									});
								}
							}
						}
						finally
						{
							val3.Dispose();
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetInventoryItems] " + ex.Message);
				Logger.Print("[EXCEPTION GetInventoryItems] " + ex.StackTrace);
			}
			return list;
		}

		public static List<ItemModel> GetBusinessItems(this Business business)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			List<ItemModel> list = new List<ItemModel>();
			try
			{
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "SELECT * FROM businesses WHERE Id = @id LIMIT 1";
						val2.Parameters.AddWithValue("@id", (object)business.Id);
						MySqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if ((val3).HasRows)
							{
								while ((val3).Read())
								{
									list = NAPI.Util.FromJson<List<ItemModel>>(val3.GetString("Storage"));
									int Slot = 0;
									list.ForEach(delegate (ItemModel item)
									{
										item.Slot = Slot;
										Slot++;
									});
								}
							}
						}
						finally
						{
							val3.Dispose();
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetBusinessItems] " + ex.Message);
				Logger.Print("[EXCEPTION GetBusinessItems] " + ex.StackTrace);
			}
			return list;
		}

		public static void UpdateBusinessItems(this Business business, ItemModel item, bool remove)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			try
			{
				Item item4 = ItemModule.itemRegisterList.FirstOrDefault((Item item3) => item3.Name == item.Name);
				if (item4 == null)
				{
					return;
				}
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						List<ItemModel> businessItems = business.GetBusinessItems();
						if (remove)
						{
							ItemModel itemModel = businessItems.FirstOrDefault((ItemModel item) => item.Name == item.Name);
							if (itemModel == null)
							{
								return;
							}
							if (itemModel.Amount > item.Amount)
							{
								businessItems.Remove(itemModel);
								itemModel.Amount -= item.Amount;
								businessItems.Add(itemModel);
							}
							goto IL_0100;
						}
						item.Weight = item4.WeightInG;
						businessItems.Add(item);
						goto IL_0100;
					IL_0100:
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "UPDATE businesses SET Storage = @val WHERE Id = @id";
						val2.Parameters.AddWithValue("@id", (object)business.Id);
						val2.Parameters.AddWithValue("@val", (object)NAPI.Util.ToJson((object)businessItems));
						((DbCommand)(object)val2).ExecuteNonQueryAsync();
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION UpdateBusinessItems] " + ex.Message);
				Logger.Print("[EXCEPTION UpdateBusinessItems] " + ex.StackTrace);
			}
		}

		public static List<GarageVehicle> GetParkedVehicles()
		{
			List<GarageVehicle> list = new List<GarageVehicle>();
			try
			{
				MySqlQuery query = new MySqlQuery("SELECT * FROM vehicles WHERE Parked = 1");
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
								list.Add(new GarageVehicle
								{
									Id = reader.GetInt32("Id"),
									OwnerID = reader.GetInt32("OwnerId"),
									Name = reader.GetString("Vehiclehash"),
									Plate = reader.GetString("Plate"),
									Keys = NAPI.Util.FromJson<List<int>>(reader.GetString("Carkeys"))
								});
							}
						}
					}
					finally
					{
						reader.Dispose();
					}
				}
				finally
				{
					query2.Connection.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetParkedVehicles] " + ex.Message);
				Logger.Print("[EXCEPTION GetParkedVehicles] " + ex.StackTrace);
			}
			return list;
		}

		public static void UpdateHouseItems(this House dbHouse, ItemModel item, bool remove)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			try
			{
				Item item4 = ItemModule.itemRegisterList.FirstOrDefault((Item item3) => item3.Name == item.Name);
				if (item4 == null)
				{
					return;
				}
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						List<ItemModel> houseItems = dbHouse.GetHouseItems();
						if (remove)
						{
							ItemModel itemModel = houseItems.FirstOrDefault((ItemModel item) => item.Name == item.Name);
							if (itemModel == null)
							{
								return;
							}
							if (itemModel.Amount > item.Amount)
							{
								houseItems.Remove(itemModel);
								itemModel.Amount -= item.Amount;
								houseItems.Add(itemModel);
							}
							goto IL_0100;
						}
						item.Weight = item4.WeightInG;
						houseItems.Add(item);
						goto IL_0100;
					IL_0100:
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "UPDATE houses SET Inventory = @val WHERE Id = @id";
						val2.Parameters.AddWithValue("@id", (object)dbHouse.Id);
						val2.Parameters.AddWithValue("@val", (object)NAPI.Util.ToJson((object)houseItems));
						((DbCommand)(object)val2).ExecuteNonQueryAsync();
						HouseModule.houses.Remove(dbHouse);
						dbHouse.Inventory = houseItems;
						HouseModule.houses.Add(dbHouse);
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION UpdateHouseItems] " + ex.Message);
				Logger.Print("[EXCEPTION UpdateHouseItems] " + ex.StackTrace);
			}
		}

		public static List<ItemModel> GetLaborStorageItems(this DbPlayer dbPlayer, int laboratoryId)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			List<ItemModel> result = new List<ItemModel>();
			try
			{
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "SELECT * FROM laboratorys WHERE Id = @id LIMIT 1";
						val2.Parameters.AddWithValue("@id", (object)laboratoryId);
						MySqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if ((val3).HasRows && (val3).Read())
							{
								Dictionary<int, List<ItemModel>> dictionary = NAPI.Util.FromJson<Dictionary<int, List<ItemModel>>>(val3.GetString("StorageInventorys"));
								if (dictionary == null)
								{
									return new List<ItemModel>();
								}
								if (!dictionary.ContainsKey(dbPlayer.Id))
								{
									return new List<ItemModel>();
								}
								List<ItemModel> itemModels = new List<ItemModel>();
								int Slot = 0;
								dictionary[dbPlayer.Id].ForEach(delegate (ItemModel item)
								{
									item.Slot = Slot;
									Slot++;
									itemModels.Add(item);
								});
								return itemModels;
							}
						}
						finally
						{
							val3.Dispose();
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetLaborStorageItems] " + ex.Message);
				Logger.Print("[EXCEPTION GetLaborStorageItems] " + ex.StackTrace);
			}
			return result;
		}

		public static List<ItemModel> GetLaborProductItems(this DbPlayer dbPlayer, int laboratoryId)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			List<ItemModel> result = new List<ItemModel>();
			try
			{
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "SELECT * FROM laboratorys WHERE Id = @id LIMIT 1";
						val2.Parameters.AddWithValue("@id", (object)laboratoryId);
						MySqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if ((val3).HasRows && (val3).Read())
							{
								Dictionary<int, List<ItemModel>> dictionary = NAPI.Util.FromJson<Dictionary<int, List<ItemModel>>>(val3.GetString("ProductInventorys"));
								if (dictionary == null)
								{
									return new List<ItemModel>();
								}
								if (!dictionary.ContainsKey(dbPlayer.Id))
								{
									return new List<ItemModel>();
								}
								List<ItemModel> itemModels = new List<ItemModel>();
								int Slot = 0;
								dictionary[dbPlayer.Id].ForEach(delegate (ItemModel item)
								{
									item.Slot = Slot;
									Slot++;
									itemModels.Add(item);
								});
								return itemModels;
							}
						}
						finally
						{
							val3.Dispose();
						}
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION GetLaborProductItems] " + ex.Message);
				Logger.Print("[EXCEPTION GetLaborProductItems] " + ex.StackTrace);
			}
			return result;
		}

		public static void UpdateLaborStorageItems(this Laboratory laboratory, int Id, ItemModel item, bool remove)
		{
			try
			{
				Item item4 = ItemModule.itemRegisterList.FirstOrDefault((Item item3) => item3.Name == item.Name);
				if (item4 == null)
				{
					return;
				}
				Dictionary<int, List<ItemModel>> dictionary = NAPI.Util.FromJson<Dictionary<int, List<ItemModel>>>(laboratory.GetAttributeString("StorageInventorys"));
				if (!dictionary.ContainsKey(Id))
				{
					dictionary.Add(Id, new List<ItemModel>());
				}
				List<ItemModel> list = dictionary[Id];
				if (list == null)
				{
					return;
				}
				if (remove)
				{
					ItemModel itemModel = list.FirstOrDefault((ItemModel item) => item.Name == item.Name);
					if (itemModel == null)
					{
						return;
					}
					if (itemModel.Amount > item.Amount)
					{
						list.Remove(itemModel);
						itemModel.Amount -= item.Amount;
						list.Add(itemModel);
					}
					goto IL_01d2;
				}
				item.Weight = item4.WeightInG;
				list.Add(item);
				goto IL_01d2;
			IL_01d2:
				dictionary[Id] = list;
				laboratory.SetAttribute("StorageInventorys", NAPI.Util.ToJson((object)dictionary));
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION UpdateLaborStorageItems] " + ex.Message);
				Logger.Print("[EXCEPTION UpdateLaborStorageItems] " + ex.StackTrace);
			}
		}

		public static void UpdateLaborProductItems(this Laboratory laboratory, int Id, ItemModel item, bool remove)
		{
			try
			{
				Item item4 = ItemModule.itemRegisterList.FirstOrDefault((Item item3) => item3.Name == item.Name);
				if (item4 == null)
				{
					return;
				}
				Dictionary<int, List<ItemModel>> dictionary = NAPI.Util.FromJson<Dictionary<int, List<ItemModel>>>(laboratory.GetAttributeString("ProductInventorys"));
				if (!dictionary.ContainsKey(Id))
				{
					dictionary.Add(Id, new List<ItemModel>());
				}
				List<ItemModel> list = dictionary[Id];
				if (list == null)
				{
					return;
				}
				if (remove)
				{
					ItemModel itemModel = list.FirstOrDefault((ItemModel item) => item.Name == item.Name);
					if (itemModel == null)
					{
						return;
					}
					if (itemModel.Amount > item.Amount)
					{
						list.Remove(itemModel);
						itemModel.Amount -= item.Amount;
						list.Add(itemModel);
					}
					goto IL_01d2;
				}
				item.Weight = item4.WeightInG;
				list.Add(item);
				goto IL_01d2;
			IL_01d2:
				dictionary[Id] = list;
				laboratory.SetAttribute("ProductInventorys", NAPI.Util.ToJson((object)dictionary));
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION UpdateLaborProductItems] " + ex.Message);
				Logger.Print("[EXCEPTION UpdateLaborProductItems] " + ex.StackTrace);
			}
		}

		public static void UpdateFraklagerItems(this Faction faction, ItemModel item, bool remove)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			try
			{
				Item item4 = ItemModule.itemRegisterList.FirstOrDefault((Item item3) => item3.Name == item.Name);
				if (item4 == null)
				{
					return;
				}
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						List<ItemModel> factionStorageItems = faction.GetFactionStorageItems();
						if (remove)
						{
							ItemModel itemModel = factionStorageItems.FirstOrDefault((ItemModel item) => item.Name == item.Name);
							if (itemModel == null)
							{
								return;
							}
							if (itemModel.Amount > item.Amount)
							{
								factionStorageItems.Remove(itemModel);
								itemModel.Amount -= item.Amount;
								factionStorageItems.Add(itemModel);
							}
							goto IL_0100;
						}
						item.Weight = item4.WeightInG;
						factionStorageItems.Add(item);
						goto IL_0100;
					IL_0100:
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "UPDATE fraktionen SET StorageData = @val WHERE Id = @id";
						val2.Parameters.AddWithValue("@id", (object)faction.Id);
						val2.Parameters.AddWithValue("@val", (object)NAPI.Util.ToJson((object)factionStorageItems));
						((DbCommand)(object)val2).ExecuteNonQueryAsync();
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION UpdateFraklagerItems] " + ex.Message);
				Logger.Print("[EXCEPTION UpdateFraklagerItems] " + ex.StackTrace);
			}
		}

		public static void UpdateVehicleItems(this DbVehicle dbVehicle, ItemModel item, bool remove)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			try
			{
				Item item4 = ItemModule.itemRegisterList.FirstOrDefault((Item item3) => item3.Name == item.Name);
				if (item4 == null)
				{
					return;
				}
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						List<ItemModel> vehicleItems = dbVehicle.GetVehicleItems();
						item.Weight = item4.WeightInG;
						vehicleItems.Add(item);
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "UPDATE vehicles SET Items = @val WHERE Id = @id";
						val2.Parameters.AddWithValue("@id", (object)dbVehicle.Id);
						val2.Parameters.AddWithValue("@val", (object)NAPI.Util.ToJson((object)vehicleItems));
						((DbCommand)(object)val2).ExecuteNonQueryAsync();
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION UpdateVehicleItems] " + ex.Message);
				Logger.Print("[EXCEPTION UpdateVehicleItems] " + ex.StackTrace);
			}
		}

		public static void UpdateInventoryItems(this DbPlayer dbPlayer, string name, int amount, bool remove)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			try
			{
				Item item4 = ItemModule.itemRegisterList.FirstOrDefault((Item item3) => item3.Name == name);
				if (item4 == null)
				{
					return;
				}
				MySqlConnection val = new MySqlConnection(Configuration.connectionString);
				try
				{
					try
					{
						((DbConnection)(object)val).Open();
						List<ItemModel> inventoryItems = dbPlayer.GetInventoryItems();
						ItemModel itemModel = inventoryItems.FirstOrDefault((ItemModel model2) => model2.Name == name);
						bool flag = false;
						if (itemModel == null)
						{
							if (remove)
							{
								return;
							}
							itemModel = new ItemModel
							{
								Id = item4.Id,
								Weight = 0,
								Slot = inventoryItems.Count + 1,
								Name = item4.Name,
								Amount = amount,
								ImagePath = item4.ImagePath
							};
							flag = true;
							goto IL_0122;
						}
						inventoryItems.Remove(itemModel);
						if (remove)
						{
							itemModel.Amount -= amount;
						}
						else
						{
							itemModel.Amount += amount;
						}
						if (itemModel.Amount > 0)
						{
							flag = true;
						}
						goto IL_0122;
					IL_0122:
						itemModel.Slot = inventoryItems.Count + 1;
						if (flag)
						{
							inventoryItems.Add(itemModel);
						}
						MySqlCommand val2 = val.CreateCommand();
						((DbCommand)(object)val2).CommandText = "UPDATE inventorys SET Items = @val WHERE Id = @id";
						val2.Parameters.AddWithValue("@id", (object)dbPlayer.Id);
						val2.Parameters.AddWithValue("@val", (object)NAPI.Util.ToJson((object)inventoryItems));
						((DbCommand)(object)val2).ExecuteNonQueryAsync();
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION UpdateInventoryItems] " + ex.Message);
				Logger.Print("[EXCEPTION UpdateInventoryItems] " + ex.StackTrace);
			}
		}

		public static int GetItemAmount(this DbPlayer dbPlayer, string ItemName)
		{
			return dbPlayer.GetInventoryItems().FirstOrDefault((ItemModel itemModel) => itemModel.Name == ItemName)?.Amount ?? 0;
		}

	}
}
