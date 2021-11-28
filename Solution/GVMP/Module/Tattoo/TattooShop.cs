using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
	public class TattooShop
	{
		public uint Id { get; }

		public string Name { get; }

		public Vector3 Position { get; }

		public float Heading { get; }

		public ColShape ColShape { get; set; }

		public TattooShop(MySqlDataReader reader)
		{
			Id = reader.GetUInt32("id");
			Name = reader.GetString("name");
			Position = new Vector3(reader.GetFloat("pos_x"), reader.GetFloat("pos_y"), reader.GetFloat("pos_z"));
			Heading = reader.GetFloat("heading");
			ColShape = NAPI.ColShape.CreateCylinderColShape(Position, 5f, 2f, 0);
			NAPI.Blip.CreateBlip(75, Position, 1f, 0, Name, 255, 0, true, 0, 0);
			ColShape.SetData("FUNCTION_MODEL", new FunctionModel("openTattooShop", Id));
			ColShape.SetData("MESSAGE", new Message("Benutze E um dir ein Tattoo stechen zu lassen.", Name, "green", 3000));
			ColShape.SetData("TATTOO", 1);
		}

		public uint GetIdentifier()
		{
			return Id;
		}
	}
}
