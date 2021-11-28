using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GVMP
{
	public class AssetsTattooZoneModule : GVMP.Module.Module<AssetsTattooZoneModule>
	{
		public Dictionary<uint, AssetsTattooZone> AssetsTattooZones = new Dictionary<uint, AssetsTattooZone>();

		protected override bool OnLoad()
		{
			MySqlQuery query = new MySqlQuery("SELECT * FROM assets_tattoo_zone");
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
							var tattooZone = new AssetsTattooZone(reader);
							AssetsTattooZones.Add(tattooZone.GetIdentifier(), tattooZone);
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
				Logger.Print("[EXCEPTION loadtattoozone] " + ex.Message);
				Logger.Print("[EXCEPTION loadtattoozone] " + ex.StackTrace);
			}
			finally
			{
				query2.Connection.Dispose();
			}
			return true;
		}
	}
}
