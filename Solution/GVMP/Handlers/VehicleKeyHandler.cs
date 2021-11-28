using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace GVMP.Handlers
{
    public class VehicleKeyHandler
    {
		public static VehicleKeyHandler Instance { get; } = new VehicleKeyHandler();


		private VehicleKeyHandler()
		{
		}

		public void DeletePlayerKey(DbPlayer iPlayer, int vehicleId)
		{
			if (iPlayer.VehicleKeys.ContainsKey(vehicleId))
			{
				iPlayer.VehicleKeys.Remove(vehicleId);
				MySqlQuery mySqlQuery = new MySqlQuery($"DELETE FROM `player_to_vehicle` WHERE `playerID` = '{iPlayer.Id}' AND `vehicleID` = '{vehicleId}';");
				MySqlHandler.ExecuteSync(mySqlQuery);
				iPlayer.RefreshData(iPlayer);
			}
		}

		public void DeleteAllVehicleKeys(int vehicleId)
		{
			foreach (DbPlayer validPlayer in PlayerHandler.GetPlayers())
			{
				if (validPlayer?.VehicleKeys != null && validPlayer.VehicleKeys.ContainsKey(vehicleId))
				{
					validPlayer.VehicleKeys.Remove(vehicleId);
					validPlayer.RefreshData(validPlayer);
				}
			}
			MySqlQuery mySqlQuery = new MySqlQuery($"DELETE FROM `player_to_vehicle` WHERE `vehicleID` = '{vehicleId}';");
			MySqlHandler.ExecuteSync(mySqlQuery);
		}

		public int GetVehicleKeyCount(int vehicleId)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			if (vehicleId == 0)
			{
				return 0;
			}
			int result = 0;
			MySqlConnection val = new MySqlConnection(Configuration.connectionString);
			try
			{
				MySqlCommand val2 = val.CreateCommand();
				try
				{
					((DbConnection)(object)val).Open();
					((DbCommand)(object)val2).CommandText = $"SELECT COUNT(*) FROM `player_to_vehicle` WHERE vehicleID = '{vehicleId}'";
					MySqlDataReader val3 = val2.ExecuteReader();
					try
					{
						if ((val3).HasRows)
						{
							if ((val3).Read())
							{
								return (val3).GetInt32(0);
							}
							return result;
						}
						return result;
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public void AddPlayerKey(DbPlayer iPlayer, int vehicleId, string vehicleName)
		{
			if (vehicleId != 0 && !iPlayer.VehicleKeys.ContainsKey(vehicleId))
			{
				iPlayer.VehicleKeys.Add(vehicleId, vehicleName);
				MySqlQuery mySqlQuery = new MySqlQuery($"INSERT INTO `player_to_vehicle` (`playerID`, `vehicleID`) VALUES ('{iPlayer.Id}', '{vehicleId}');");
				MySqlHandler.ExecuteSync(mySqlQuery);
				iPlayer.RefreshData(iPlayer);
			}
		}

		public async Task LoadPlayerVehicleKeys(DbPlayer iPlayer)
		{
			MySqlConnection keyConn = new MySqlConnection(Configuration.connectionString);
			try
			{
				MySqlCommand keyCmd = keyConn.CreateCommand();
				try
				{
					await ((DbConnection)(object)keyConn).OpenAsync();
					((DbCommand)(object)keyCmd).CommandText = $"SELECT player_to_vehicle.vehicleID, vehicles.Vehiclehash FROM player_to_vehicle INNER JOIN vehicles ON player_to_vehicle.vehicleID = vehicles.Id WHERE playerID = '{iPlayer.Id}';";
					MySqlDataReader val = keyCmd.ExecuteReader();
					try
					{
						if ((val).HasRows)
						{
							while ((val).Read())
							{
								int uInt = val.GetInt32(0);
								string @string = (val).GetString(1);
								if (!iPlayer.VehicleKeys.ContainsKey(uInt))
								{
									iPlayer.VehicleKeys.Add(uInt, @string);
								}
							}
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					((DbCommand)(object)keyCmd).CommandText = $"SELECT Id, Vehiclehash FROM `vehicles` WHERE OwnerId = '{iPlayer.Id}';";
					MySqlDataReader val2 = keyCmd.ExecuteReader();
					try
					{
						if ((val2).HasRows)
						{
							while ((val2).Read())
							{
								int uInt2 = val2.GetInt32(0);
								string string2 = (val2).GetString(1);
								if (!iPlayer.OwnVehicles.ContainsKey(uInt2))
								{
									iPlayer.OwnVehicles.Add(uInt2, string2);
								}
							}
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					await keyConn.CloseAsync();
				}
				finally
				{
					((IDisposable)keyCmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)keyConn)?.Dispose();
			}
			iPlayer.RefreshData(iPlayer);
		}

		public List<VHKey> GetAllKeysPlayerHas(DbPlayer iPlayer)
		{
			List<VHKey> list = new List<VHKey>();
			foreach (KeyValuePair<int, string> vehicleKey in iPlayer.VehicleKeys)
			{
				list.Add(new VHKey(vehicleKey.Value, vehicleKey.Key));
			}
			foreach (KeyValuePair<int, string> ownVehicle in iPlayer.OwnVehicles)
			{
				list.Add(new VHKey(ownVehicle.Value, ownVehicle.Key));
			}
			return list;
		}

		public List<VHKey> GetOwnVehicleKeys(DbPlayer iPlayer)
		{
			List<VHKey> list = new List<VHKey>();
			foreach (KeyValuePair<int, string> ownVehicle in iPlayer.OwnVehicles)
			{
				list.Add(new VHKey(ownVehicle.Value, ownVehicle.Key));
			}
			return list;
		}
	}
}
