using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
	public class AssetsTattoo
	{
		public uint Id { get; set; }

		public string Name { get; set; }

		public string HashMale { get; set; }

		public string HashFemale { get; set; }

		public string Collection { get; set; }

		public int ZoneId { get; set; }

		public int Price { get; set; }

		public AssetsTattoo() { }

		public AssetsTattoo(MySqlDataReader reader)
		{
			Id = reader.GetUInt32("id");
			Name = reader.GetString("name");
			HashMale = reader.GetString("hash_male");
			HashFemale = reader.GetString("hash_female");
			Collection = reader.GetString("collection");
			ZoneId = reader.GetInt32("zone_id");
			Price = reader.GetInt32("price");
		}

		public uint GetIdentifier()
		{
			return Id;
		}

		public string GetHashForPlayer(DbPlayer dbPlayer)
		{
			if (!ClothingManager.isMale(dbPlayer.Client))
			{
				return HashFemale;
			}
			return HashMale;
		}
	}
}
