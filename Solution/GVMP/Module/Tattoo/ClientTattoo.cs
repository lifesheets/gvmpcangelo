using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
	public class ClientTattoo
	{
		public uint Id { get; set; }
		public string TattooHash { get; }

		public int ZoneId { get; }

		public int Price { get; }

		public string Name { get; }

		public string Collection { get; set; }

		public ClientTattoo(string tattooHash, int zoneId, int price, string name, string collection, uint id)
		{
			TattooHash = tattooHash;
			ZoneId = zoneId;
			Price = price;
			Name = name;
			Collection = collection;
			Id = id;
		}
	}
}
