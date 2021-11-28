using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
	internal class BusinessMember
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("owner")]
		public bool Owner { get; set; }

		[JsonProperty("manage")]
		public bool Manage { get; set; }

		[JsonProperty("number")]
		public int Number { get; set; }
	}

}
