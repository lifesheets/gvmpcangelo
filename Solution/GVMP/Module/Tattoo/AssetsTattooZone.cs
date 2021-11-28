using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
	public class AssetsTattooZone
	{
		public uint Id { get; }

		public string Name { get; }

		public AssetsTattooZone(MySqlDataReader reader)
		{
			Id = reader.GetUInt32("id");
			Name = reader.GetString("name");
		}

		public uint GetIdentifier()
		{
			return Id;
		}
	}
}
