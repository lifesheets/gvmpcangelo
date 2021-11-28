using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class KasinoDevice
    {
		public uint Id { get; }

		public int Price { get; }

		public int MinPrice { get; }

		public int MaxPrice { get; }

		public int PriceStep { get; }

		public int MaxMultiple { get; }

		public Vector3 Position { get; }

		public int Radius { get; set; }

		public bool IsInUse { get; set; }

		public KasinoDevice(MySqlDataReader reader)
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			Id = reader.GetUInt32("id");
			Price = reader.GetInt32("price");
			MinPrice = reader.GetInt32("minprice");
			MaxPrice = reader.GetInt32("maxprice");
			PriceStep = reader.GetInt32("pricestep");
			MaxMultiple = reader.GetInt32("maxmultiple");
			Position = new Vector3(reader.GetFloat("pos_x"), reader.GetFloat("pos_y"), reader.GetFloat("pos_z"));
			Radius = reader.GetInt32("radius");
		}
	}
}
