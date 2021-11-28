using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace GVMP
{
    public class KasinoModule : GVMP.Module.Module<KasinoModule>
    {
		private readonly Random random = new Random();

		private int GameId;

		public float[,] Factors = new float[8, 3]
		{
			{ 1.5f, 3f, 250f },
			{ 1f, 1.5f, 30f },
			{ 1f, 1.5f, 15f },
			{ 0f, 1.5f, 10f },
			{ 0f, 1f, 5f },
			{ 0f, 1f, 2f },
			{ 0f, 0f, 1f },
			{ 0f, 0f, 0f }
		};

		public static List<KasinoDevice> KasinoDevices = new List<KasinoDevice>();

		public static Dictionary<int, SlotMachineGame> SlotMachineGames = new Dictionary<int, SlotMachineGame>();

		//public List<DbPlayer> CasinoGuests = new List<DbPlayer>();

        protected override bool OnLoad()
        {
			MySqlQuery query = new MySqlQuery("SELECT * FROM kasino_devices");
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
							KasinoDevices.Add(new KasinoDevice(reader));
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
				Logger.Print("[EXCEPTION loadkasino] " + ex.Message);
				Logger.Print("[EXCEPTION loadkasino] " + ex.StackTrace);
			}
			finally
			{
				query2.Connection.Dispose();
			}
			return true;
        }
		public bool PressedE(DbPlayer dbPlayer)
		{
			KasinoDevice closest = GetClosest(dbPlayer);
			if (closest == null)
			{
				return false;
			}
			ShowSlotMachine(dbPlayer, closest);
			return true;
		}

		private class ShowEvent
		{
			[JsonProperty(PropertyName = "machineId")]
			private uint Id { get; }

			[JsonProperty(PropertyName = "price")]
			private int Price { get; }

			[JsonProperty(PropertyName = "minprice")]
			private int MinPrice { get; }

			[JsonProperty(PropertyName = "maxprice")]
			private int MaxPrice { get; }

			[JsonProperty(PropertyName = "pricestep")]
			private int PriceStep { get; }

			[JsonProperty(PropertyName = "maxmultiple")]
			private int MaxMultiple { get; }

			public ShowEvent(KasinoDevice kasinoDevice)
			{
				Id = kasinoDevice.Id;
				Price = kasinoDevice.Price;
				MinPrice = kasinoDevice.MinPrice;
				MaxPrice = kasinoDevice.MaxPrice;
				PriceStep = kasinoDevice.PriceStep;
				MaxMultiple = kasinoDevice.MaxMultiple;
			}
		}

		private void ShowSlotMachine(DbPlayer dbPlayer, KasinoDevice kasinoDevice)
		{
			/*if (kasinoDevice.IsInUse)
			{
				dbPlayer.SendNotification("Diese Slot Machine wird bereits benutzt");
				return;
			}*/
			if (dbPlayer.Money < kasinoDevice.MinPrice * 5)
			{
				dbPlayer.SendNotification($"Sie benötigen mindestens ${kasinoDevice.MinPrice * 5} um hier zu spielen!");
			}
			dbPlayer.TriggerEvent("openWindow", "SlotMachine", JsonConvert.SerializeObject(new ShowEvent(kasinoDevice)));
			kasinoDevice.IsInUse = true;
		}

		public SlotMachineGame GenerateSlotMachineGame(DbPlayer dbPlayer, int moneyUsed)
		{
			int multiple = 1;
			int num = random.Next(1, 9);
			int num2 = random.Next(1, 101);
			if (num2 <= 27)
			{
				num2 = num;
			}
			else
			{
				for (num2 = random.Next(1, 9); num2 == num; num2 = random.Next(1, 9))
				{
				}
			}
			int num3 = random.Next(1, 101);
			if (num3 <= 2)
			{
				num3 = num2;
			}
			else
			{
				for (num3 = random.Next(1, 9); num3 == num2; num3 = random.Next(1, 9))
				{
				}
			}
			Status status = Status.LOSE;
			float num4 = calculateProfit(num, num2, num3, moneyUsed);
			if (num4 != 0f)
			{
				status = Status.WIN;
			}
			SlotMachineGame slotMachineGame = new SlotMachineGame
			{
				Id = GameId,
				Einsatz = moneyUsed,
				KasinoDeviceId = 1u,
				Slot1 = num,
				Slot2 = num2,
				Slot3 = num3,
				Status = status,
				WinSum = (int)num4,
				Multiple = multiple
			};
			SlotMachineGames.Add(GameId, slotMachineGame);
			GameId++;
			return slotMachineGame;
		}

		private float calculateProfit(int slot1, int slot2, int slot3, int einsatz)
		{
			float num = 0f;
			num = ((slot1 != slot2) ? Factors[slot1 - 1, 0] : ((slot1 != slot3) ? Factors[slot1 - 1, 1] : Factors[slot1 - 1, 2]));
			return num * einsatz;
		}

		public KasinoDevice GetClosest(DbPlayer dbPlayer)
		{
			return KasinoDevices.FirstOrDefault(kasinoDevice => kasinoDevice.Position.DistanceTo(dbPlayer.Client.Position) < kasinoDevice.Radius);
		}

		public void TriggerEvent(Client player, string eventName, params object[] args)
		{
			object[] array = new object[2 + args.Length];
			array[0] = "SlotMachine";
			array[1] = eventName;
			for (int i = 0; i < args.Length; i++)
			{
				array[i + 2] = args[i];
			}
			player.TriggerEvent("componentServerEvent", array);
		}

		[RemoteEvent]
		public void requestSlotInfo(Client player)
		{
			DbPlayer player2 = player.GetPlayer();
			if (player2 != null && player2.IsValid())
			{
				TriggerEvent(player2.Client, "responseSlotInfo", JsonConvert.SerializeObject(Factors));
			}
		}

		[RemoteEvent]
		public void newSlotRoll(Client player, int moneyUsed)
		{
			DbPlayer player2 = player.GetPlayer();
			if (player2 == null || !player2.IsValid())
			{
				return;
			}
			if (player2.Money >= moneyUsed)
			{
				player2.removeMoney(moneyUsed);
				SlotMachineGame slotMachineGame = GenerateSlotMachineGame(player2, moneyUsed);
				SendGameResultToPlayer(player2, slotMachineGame);
			}
			else
			{
				player2.SendNotification("Du hast nicht genug Geld dafür!");
			}
		}

		[RemoteEvent]
		public void leaveSlotMachine(Client player, int deviceId)
		{
			DbPlayer player2 = player.GetPlayer();
			if (player2 != null && player2.IsValid())
			{
				KasinoDevice kasinoDevice = KasinoDevices.FirstOrDefault(device => device.Id == (uint)deviceId);
				if (kasinoDevice != null)
				{
					kasinoDevice.IsInUse = false;
				}
			}
		}

		[RemoteEvent]
		public void cashoutSlotRoll(Client player, int id)
		{
			DbPlayer player2 = player.GetPlayer();
			if (player2 != null && SlotMachineGames.TryGetValue(id, out var value) && value.Status != Status.LOSE)
			{
				value.WinSum *= value.Multiple;
				player2.addMoney(value.WinSum);
				player2.SendNotification($"Du hast {value.WinSum}$ gewonnen!", 4000, "lightblue", "CASINO");
				SlotMachineGames.Remove(id);
			}
		}

		[RemoteEvent]
		public void risikoCard(Client player, int number, int id)
		{
			DbPlayer player2 = player.GetPlayer();
			if (player2 == null || !player2.IsValid())
			{
				return;
			}
			int num = random.Next(1, 3);
			if (SlotMachineGames.TryGetValue(id, out var value))
			{
				if (num == number)
				{
					value.Multiple++;
					TriggerEvent(player, "responseRisiko", num, 1);
				}
				else
				{
					value.Multiple = 0;
					value.WinSum = 0;
					value.Status = Status.LOSE;
					TriggerEvent(player, "responseRisiko", num, 0);
				}
			}
		}

		public void SendGameResultToPlayer(DbPlayer dbPlayer, SlotMachineGame slotMachineGame)
		{
			TriggerEvent(dbPlayer.Client, "rollSlot", JsonConvert.SerializeObject(slotMachineGame));
		}
	}
}
