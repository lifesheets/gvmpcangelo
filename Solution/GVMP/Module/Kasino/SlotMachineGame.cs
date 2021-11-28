using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
	public class SlotMachineGame
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonIgnore]
		public int Einsatz { get; set; }

		[JsonProperty(PropertyName = "slot1")]
		public int Slot1 { get; set; }

		[JsonProperty(PropertyName = "slot2")]
		public int Slot2 { get; set; }

		[JsonProperty(PropertyName = "slot3")]
		public int Slot3 { get; set; }

		[JsonProperty(PropertyName = "winsum")]
		public int WinSum { get; set; }

		[JsonProperty(PropertyName = "status")]
		public Status Status { get; set; }

		[JsonIgnore]
		public uint KasinoDeviceId { get; set; }

		[JsonIgnore]
		public int Multiple { get; set; }
	}
}
