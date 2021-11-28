using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP.Handlers
{
	public class HouseKeyHandler
	{
		public static HouseKeyHandler Instance { get; } = new HouseKeyHandler();


		private HouseKeyHandler()
		{
		}

		public void AddHouseKey(DbPlayer iPlayer, House house)
		{
			if (!iPlayer.HouseKeys.Contains(house.Id))
			{
				iPlayer.HouseKeys.Add(house.Id);
				MySqlQuery mySqlQuery = new MySqlQuery($"INSERT INTO `house_keys` (`player_id`, `house_id`) VALUES ('{iPlayer.Id}', '{house.Id}');");
				MySqlHandler.ExecuteSync(mySqlQuery);

				iPlayer.RefreshData(iPlayer);
			}
		}

		public void DeleteHouseKey(DbPlayer iPlayer, House house)
		{
			if (iPlayer != null && iPlayer.IsValid() && iPlayer.HouseKeys.Contains(house.Id))
			{
				iPlayer.HouseKeys.Remove(house.Id);
				MySqlQuery mySqlQuery = new MySqlQuery($"DELETE FROM `house_keys` WHERE `house_id` = '{house.Id}' AND `player_id` = '{iPlayer.Id}';");
				MySqlHandler.ExecuteSync(mySqlQuery);

				iPlayer.RefreshData(iPlayer);
			}
		}

		public void DeleteAllHouseKeys(House house)
		{
			foreach (DbPlayer validPlayer in PlayerHandler.GetPlayers())
			{
				if (validPlayer?.HouseKeys != null && validPlayer.HouseKeys.Contains(house.Id))
				{
					validPlayer.HouseKeys.Remove(house.Id);
					validPlayer.RefreshData(validPlayer);
				}
			}
			MySqlQuery mySqlQuery = new MySqlQuery($"DELETE FROM `house_keys` WHERE `house_id` = '{house.Id}';");
			MySqlHandler.ExecuteSync(mySqlQuery);
		}

		public async Task LoadHouseKeys(DbPlayer iPlayer)
		{
			MySqlConnection keyConn = new MySqlConnection(Configuration.connectionString);
			try
			{
				MySqlCommand keyCmd = keyConn.CreateCommand();
				try
				{
					await ((DbConnection)(object)keyConn).OpenAsync();
					((DbCommand)(object)keyCmd).CommandText = $"SELECT house_id FROM `house_keys` WHERE player_id = '{iPlayer.Id}';";
					MySqlDataReader val = keyCmd.ExecuteReader();
					try
					{
						if ((val).HasRows)
						{
							while ((val).Read())
							{
								int @int = (int)(val).GetInt32(0);
								if (!iPlayer.HouseKeys.Contains(@int))
								{
									iPlayer.HouseKeys.Add(@int);
								}
							}
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
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
			foreach (int item in iPlayer.HouseKeys.ToList())
			{
				list.Add(new VHKey(item.ToString() ?? "", item));
			}
			if (iPlayer.GetHouse() == null) return list;
			if (iPlayer.GetHouse().Id != 0)
			{
				list.Add(new VHKey("Haus " + iPlayer.GetHouse().Id.ToString(), iPlayer.GetHouse().Id));
			}
			return list;
		}

		public List<VHKey> GetOwnHouseKey(DbPlayer iPlayer)
		{
			List<VHKey> list = new List<VHKey>();
			if (iPlayer.GetHouse() == null) return list;
			if (iPlayer.GetHouse().Id != 0)
			{
				list.Add(new VHKey("Haus " + iPlayer.GetHouse().Id.ToString(), iPlayer.GetHouse().Id));
			}
			return list;
		}
	}
}
