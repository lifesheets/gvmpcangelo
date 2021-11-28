
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GVMP
{
	public class TattooShopModule : GVMP.Module.Module<TattooShopModule>
	{
		public static Dictionary<uint, TattooShop> TattooShops = new Dictionary<uint, TattooShop>();

		protected override bool OnLoad()
		{
			MySqlQuery query = new MySqlQuery("SELECT * FROM tattoo_shops");
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
							var tattooShop = new TattooShop(reader);
							TattooShops.Add(tattooShop.GetIdentifier(), tattooShop);
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
				Logger.Print("[EXCEPTION loadtattooshop] " + ex.Message);
				Logger.Print("[EXCEPTION loadtattooshop] " + ex.StackTrace);
			}
			finally
			{
				query2.Connection.Dispose();
			}
			return true;
		}
	}
}
