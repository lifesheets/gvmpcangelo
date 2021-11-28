using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
	class TuningModule : GVMP.Module.Module<TuningModule>
	{
		public static List<Tuner> tuningList = new List<Tuner>();

		public static List<TuningCategory> tuningCategories = new List<TuningCategory>();

		public static List<TuningColor> tuningColors = new List<TuningColor>();

		public static List<NeonColor> tuningNeons = new List<NeonColor>();

		public static List<TuningPart> tuningEngines = new List<TuningPart>();

		public static List<TuningPart> tuningTurbo = new List<TuningPart>();

		public static List<TuningPart> tuningBrakes = new List<TuningPart>();

		public static List<TuningPart> tuningHorns = new List<TuningPart>();

		public static List<TuningPart> tuningTrails = new List<TuningPart>();

		public static List<TuningColor> tuningLights = new List<TuningColor>();

		public static List<TuningPart> tuningSpoilers = new List<TuningPart>();

		public static List<TuningColor> tuningWindows = new List<TuningColor>();

		public static List<TuningPart> tuningFrontbumper = new List<TuningPart>();

		public static List<TuningPart> tuningRearbumper = new List<TuningPart>();

		public static List<TuningPart> tuningSideskirt = new List<TuningPart>();

		public static List<TuningPart> tuningFrame = new List<TuningPart>();

		public static List<TuningPart> tuningExhaust = new List<TuningPart>();

		public static List<TuningPart> tuningGrille = new List<TuningPart>();

		public static List<TuningPart> tuningHood = new List<TuningPart>();

		public static List<TuningPart> tuningFender = new List<TuningPart>();

		public static List<TuningPart> tuningRoof = new List<TuningPart>();

		public static List<TuningPart> tuningHydraulics = new List<TuningPart>();

		public static List<TuningPart> tuningDesigns = new List<TuningPart>();

		protected override bool OnLoad()
		{
			//IL_11c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_120e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1239: Unknown result type (might be due to invalid IL or missing references)
			//IL_1264: Unknown result type (might be due to invalid IL or missing references)
			//IL_128c: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_130e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1337: Unknown result type (might be due to invalid IL or missing references)
			//IL_1360: Unknown result type (might be due to invalid IL or missing references)
			//IL_138c: Unknown result type (might be due to invalid IL or missing references)
			//IL_13b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_13e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_142c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1436: Expected O, but got Unknown
			//IL_1477: Unknown result type (might be due to invalid IL or missing references)
			//IL_1481: Expected O, but got Unknown
			tuningCategories = new List<TuningCategory>
		{
			new TuningCategory("Motor - EMS", "ems"),
			new TuningCategory("Turbo", "turbo"),
			new TuningCategory("Bremsen", "brakes"),
			new TuningCategory("------------", "-"),
			new TuningCategory("Farbe", "color"),
			new TuningCategory("Sekundäre Farbe", "secondcolor"),
			new TuningCategory("Perleffekt", "pearl"),
			new TuningCategory("Neons", "neons"),
			new TuningCategory("Frontstoßstange", "frontbumper"),
			new TuningCategory("Rückstoßstange", "rearbumper"),
			new TuningCategory("Seitenverkleidung", "sideskirt"),
			new TuningCategory("Auspuff", "exhaust"),
			new TuningCategory("Rahmen", "frame"),
			new TuningCategory("Frontgrill", "grille"),
			new TuningCategory("Motorhaube", "hood"),
			new TuningCategory("Fender", "fender"),
			new TuningCategory("Dach", "roof"),
			new TuningCategory("Hydraulik", "hydraulics"),
			new TuningCategory("Felgen", "trails"),
			new TuningCategory("Fensterscheiben", "glasses"),
			new TuningCategory("Frontlichter", "lights"),
			new TuningCategory("Hupe", "horn"),
			new TuningCategory("Spoiler", "spoiler"),
			new TuningCategory("Designs", "designs")
		};
			for (int i = 0; i < 160; i++)
			{
				tuningColors.Add(new TuningColor("Farbe " + i, "color" + i, i, 0));
			}
			for (int j = 0; j < 160; j++)
			{
				tuningDesigns.Add(new TuningPart("Design " + j, "design" + j, 48, j, 0));
			}
			tuningEngines = new List<TuningPart>
		{
			new TuningPart("EMS Verbesserung 1", "ems1", 11, 0, 15000),
			new TuningPart("EMS Verbesserung 2", "ems2", 11, 1, 30000),
			new TuningPart("EMS Verbesserung 3", "ems3", 11, 2, 45000),
			new TuningPart("EMS Verbesserung 4", "ems4", 11, 3, 60000)
		};
			tuningTurbo = new List<TuningPart>
		{
			new TuningPart("EMS Verbesserung 1", "turbo1", 18, 0, 50000)
		};
			tuningBrakes = new List<TuningPart>
		{
			new TuningPart("Bremsen Upgrade 1", "brakes1", 12, 0, 8000),
			new TuningPart("Bremsen Upgrade 2", "brakes2", 12, 1, 16000),
			new TuningPart("Bremsen Upgrade 3", "brakes3", 12, 2, 24000)
		};
			tuningHorns = new List<TuningPart>
		{
			new TuningPart("Standart", "horn0", 14, -1, 0),
			new TuningPart("Hupe 1", "horn1", 14, 0, 0),
			new TuningPart("Hupe 2", "horn2", 14, 1, 0),
			new TuningPart("Hupe 3", "horn3", 14, 2, 0),
			new TuningPart("Hupe 4", "horn4", 14, 3, 0),
			new TuningPart("Hupe 5", "horn5", 14, 4, 0),
			new TuningPart("Hupe 6", "horn6", 14, 5, 0),
			new TuningPart("Hupe 7", "horn7", 14, 6, 0),
			new TuningPart("Hupe 8", "horn8", 14, 7, 0),
			new TuningPart("Hupe 9", "horn9", 14, 8, 0),
			new TuningPart("Hupe 10", "horn10", 14, 9, 0),
			new TuningPart("Hupe 11", "horn11", 14, 10, 0),
			new TuningPart("Hupe 12", "horn12", 14, 11, 0)
		};
			tuningLights = new List<TuningColor>
		{
			new TuningColor("Weiß", "light1", 0, 0),
			new TuningColor("Blau", "light2", 1, 5000),
			new TuningColor("Hellblau", "light3", 2, 5000),
			new TuningColor("Grün", "light4", 3, 5000),
			new TuningColor("Hellgrün", "light5", 4, 5000),
			new TuningColor("Helles Gelb", "light6", 5, 5000),
			new TuningColor("Gelb", "light7", 6, 5000),
			new TuningColor("Orange", "light8", 7, 5000),
			new TuningColor("Rot", "light9", 8, 5000),
			new TuningColor("Helles Pink", "light10", 9, 5000),
			new TuningColor("Pink", "light11", 10, 5000),
			new TuningColor("Lila", "light12", 11, 5000),
			new TuningColor("Helles Lila", "light13", 12, 5000)
		};
			tuningSpoilers = new List<TuningPart>
		{
			new TuningPart("Standart", "spoiler0", 0, -1, 0),
			new TuningPart("Spoiler 1", "spoiler1", 0, 0, 0),
			new TuningPart("Spoiler 2", "spoiler2", 0, 1, 0),
			new TuningPart("Spoiler 3", "spoiler3", 0, 2, 0),
			new TuningPart("Spoiler 4", "spoiler4", 0, 3, 0),
			new TuningPart("Spoiler 5", "spoiler5", 0, 4, 0),
			new TuningPart("Spoiler 6", "spoiler6", 0, 5, 0),
			new TuningPart("Spoiler 7", "spoiler7", 0, 6, 0),
			new TuningPart("Spoiler 8", "spoiler8", 0, 7, 0),
			new TuningPart("Spoiler 9", "spoiler9", 0, 8, 0),
			new TuningPart("Spoiler 10", "spoiler10", 0, 9, 0),
			new TuningPart("Spoiler 11", "spoiler11", 0, 10, 0),
			new TuningPart("Spoiler 12", "spoiler12", 0, 11, 0)
		};
			tuningWindows = new List<TuningColor>
		{
			new TuningColor("Standart", "window0", 0, 0),
			new TuningColor("Farbe 1", "window1", 1, 0),
			new TuningColor("Farbe 2", "window2", 2, 0),
			new TuningColor("Farbe 3", "window3", 3, 0),
			new TuningColor("Farbe 4", "window4", 4, 0),
			new TuningColor("Farbe 5", "window5", 5, 0),
			new TuningColor("Farbe 6", "window6", 6, 0)
		};
			tuningFrontbumper = new List<TuningPart>
		{
			new TuningPart("Standart", "fbumper0", 1, -1, 0),
			new TuningPart("Stoßstange 1", "fbumper1", 1, 0, 0),
			new TuningPart("Stoßstange 2", "fbumper2", 1, 1, 0),
			new TuningPart("Stoßstange 3", "fbumper3", 1, 2, 0),
			new TuningPart("Stoßstange 4", "fbumper4", 1, 3, 0),
			new TuningPart("Stoßstange 5", "fbumper5", 1, 4, 0),
			new TuningPart("Stoßstange 6", "fbumper6", 1, 5, 0),
			new TuningPart("Stoßstange 7", "fbumper7", 1, 6, 0),
			new TuningPart("Stoßstange 8", "fbumper8", 1, 7, 0)
		};
			tuningRearbumper = new List<TuningPart>
		{
			new TuningPart("Standart", "rbumper0", 2, -1, 0),
			new TuningPart("Stoßstange 1", "rbumper1", 2, 0, 0),
			new TuningPart("Stoßstange 2", "rbumper2", 2, 1, 0),
			new TuningPart("Stoßstange 3", "rbumper3", 2, 2, 0),
			new TuningPart("Stoßstange 4", "rbumper4", 2, 3, 0),
			new TuningPart("Stoßstange 5", "rbumper5", 2, 4, 0),
			new TuningPart("Stoßstange 6", "rbumper6", 2, 5, 0),
			new TuningPart("Stoßstange 7", "rbumper7", 2, 6, 0),
			new TuningPart("Stoßstange 8", "rbumper8", 2, 7, 0)
		};
			tuningSideskirt = new List<TuningPart>
		{
			new TuningPart("Standart", "skirt0", 3, -1, 0),
			new TuningPart("Seitenverkleidung 1", "skirt1", 3, 0, 0),
			new TuningPart("Seitenverkleidung 2", "skirt2", 3, 1, 0),
			new TuningPart("Seitenverkleidung 3", "skirt3", 3, 2, 0),
			new TuningPart("Seitenverkleidung 4", "skirt4", 3, 3, 0),
			new TuningPart("Seitenverkleidung 5", "skirt5", 3, 4, 0),
			new TuningPart("Seitenverkleidung 6", "skirt6", 3, 5, 0),
			new TuningPart("Seitenverkleidung 7", "skirt7", 3, 6, 0),
			new TuningPart("Seitenverkleidung 8", "skirt8", 3, 7, 0)
		};
			tuningFrame = new List<TuningPart>
		{
			new TuningPart("Standart", "frame0", 5, -1, 0),
			new TuningPart("Rahmen 1", "frame1", 5, 0, 0),
			new TuningPart("Rahmen 2", "frame2", 5, 1, 0),
			new TuningPart("Rahmen 3", "frame3", 5, 2, 0),
			new TuningPart("Rahmen 4", "frame4", 5, 3, 0),
			new TuningPart("Rahmen 5", "frame5", 5, 4, 0),
			new TuningPart("Rahmen 6", "frame6", 5, 5, 0),
			new TuningPart("Rahmen 7", "frame7", 5, 6, 0),
			new TuningPart("Rahmen 8", "frame8", 5, 7, 0)
		};
			tuningGrille = new List<TuningPart>
		{
			new TuningPart("Standart", "grill0", 6, -1, 0),
			new TuningPart("Grill 1", "grill1", 6, 0, 0),
			new TuningPart("Grill 2", "grill2", 6, 1, 0),
			new TuningPart("Grill 3", "grill3", 6, 2, 0),
			new TuningPart("Grill 4", "grill4", 6, 3, 0),
			new TuningPart("Grill 5", "grill5", 6, 4, 0),
			new TuningPart("Grill 6", "grill6", 6, 5, 0),
			new TuningPart("Grill 7", "grill7", 6, 6, 0),
			new TuningPart("Grill 8", "grill8", 6, 7, 0)
		};
			tuningExhaust = new List<TuningPart>
		{
			new TuningPart("Standart", "exhaust0", 4, -1, 0),
			new TuningPart("Auspuff 1", "exhaust1", 4, 0, 0),
			new TuningPart("Auspuff 2", "exhaust2", 4, 1, 0),
			new TuningPart("Auspuff 3", "exhaust3", 4, 2, 0),
			new TuningPart("Auspuff 4", "exhaust4", 4, 3, 0),
			new TuningPart("Auspuff 5", "exhaust5", 4, 4, 0),
			new TuningPart("Auspuff 6", "exhaust6", 4, 5, 0),
			new TuningPart("Auspuff 7", "exhaust7", 4, 6, 0),
			new TuningPart("Auspuff 8", "exhaust8", 4, 7, 0)
		};
			tuningHood = new List<TuningPart>
		{
			new TuningPart("Standart", "hood0", 7, -1, 0),
			new TuningPart("Motorhaube 1", "hood1", 7, 0, 0),
			new TuningPart("Motorhaube 2", "hood2", 7, 1, 0),
			new TuningPart("Motorhaube 3", "hood3", 7, 2, 0),
			new TuningPart("Motorhaube 4", "hood4", 7, 3, 0),
			new TuningPart("Motorhaube 5", "hood5", 7, 4, 0),
			new TuningPart("Motorhaube 6", "hood6", 7, 5, 0),
			new TuningPart("Motorhaube 7", "hood7", 7, 6, 0),
			new TuningPart("Motorhaube 8", "hood8", 7, 7, 0)
		};
			tuningFender = new List<TuningPart>
		{
			new TuningPart("Standart", "fender0", 8, -1, 0),
			new TuningPart("Fender 1", "fender1", 8, 0, 0),
			new TuningPart("Fender 2", "fender2", 8, 1, 0),
			new TuningPart("Fender 3", "fender3", 8, 2, 0),
			new TuningPart("Fender 4", "fender4", 8, 3, 0),
			new TuningPart("Fender 5", "fender5", 8, 4, 0),
			new TuningPart("Fender 6", "fender6", 8, 5, 0),
			new TuningPart("Fender 7", "fender7", 8, 6, 0),
			new TuningPart("Fender 8", "fender8", 8, 7, 0)
		};
			tuningRoof = new List<TuningPart>
		{
			new TuningPart("Standart", "roof0", 10, -1, 0),
			new TuningPart("Dach 1", "roof1", 10, 0, 0),
			new TuningPart("Dach 2", "roof2", 10, 1, 0),
			new TuningPart("Dach 3", "roof3", 10, 2, 0),
			new TuningPart("Dach 4", "roof4", 10, 3, 0),
			new TuningPart("Dach 5", "roof5", 10, 4, 0),
			new TuningPart("Dach 6", "roof6", 10, 5, 0),
			new TuningPart("Dach 7", "roof7", 10, 6, 0),
			new TuningPart("Dach 8", "roof8", 10, 7, 0)
		};
			tuningHydraulics = new List<TuningPart>
		{
			new TuningPart("Standart", "hydraulics0", 38, -1, 0),
			new TuningPart("Hydraulik 1", "hydraulics1", 38, 0, 10000),
			new TuningPart("Hydraulik 2", "hydraulics2", 38, 1, 10000),
			new TuningPart("Hydraulik 3", "hydraulics3", 38, 2, 10000)
		};
			for (int k = 0; k < 130; k++)
			{
				tuningTrails.Add(new TuningPart("Felge " + k, "felge" + k, 23, k, 0));
			}
			tuningNeons = new List<NeonColor>
		{
			new NeonColor("Aus", "off", new Color(0, 0, 0), 0),
			new NeonColor("Schwarz", "neon1", new Color(0, 0, 0), 10000),
			new NeonColor("Hell Grün", "neon2", new Color(0, 255, 0), 10000),
			new NeonColor("Helles Gelb", "neon3", new Color(255, 255, 0), 10000),
			new NeonColor("Lila", "neon4", new Color(136, 0, 255), 10000),
			new NeonColor("Hell Blau", "neon5", new Color(0, 102, 255), 10000),
			new NeonColor("Rot", "neon6", new Color(255, 0, 0), 10000),
			new NeonColor("Weiß", "neon7", new Color(255, 255, 255), 10000),
			new NeonColor("Grün", "neon8", new Color(48, 243, 214), 10000),
			new NeonColor("Orange", "neon9", new Color(242, 125, 32), 10000),
			new NeonColor("Blau", "neon10", new Color(49, 107, 184), 10000),
			new NeonColor("Gelb", "neon11", new Color(221, 197, 50), 10000),
			new NeonColor("Pink", "neon12", new Color(171, 125, 151), 10000),
			new NeonColor("Helles Pink", "neon13", new Color(171, 58, 163), 10000)
		};
			tuningList.Add(new Tuner
			{
				Id = 1,
				Name = "Los Santos Customs",
				Position = new Vector3(-340f, -137f, 39f)
			});
			tuningList.Add(new Tuner
			{
				Id = 2,
				Name = "Bennys Original Motorsports",
				Position = new Vector3(-211.65, -1324.27, 30.2)
			});
			foreach (Tuner tuning in tuningList)
			{
				ColShape val = NAPI.ColShape.CreateCylinderColShape(tuning.Position, 5f, 3f, 0u);
				((Entity)val).SetData("FUNCTION_MODEL", (object)new FunctionModel("openTuner"));
				((Entity)val).SetData("MESSAGE", (object)new Message("Drücke E um dein Fahrzeug zu tunen.", "TUNER", "red"));
				NAPI.Blip.CreateBlip(72, tuning.Position, 1f, (byte)0, tuning.Name, byte.MaxValue, 0f, true, (short)0, 0u);
			}
			return true;
		}

		[RemoteEvent("openTuner")]
		public void openTuner(Client c)
		{
			try
			{
				if (c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || player.Client == null || !c.IsInVehicle || c.Vehicle == null)
				{
					return;
				}
				DbVehicle vehicle = c.Vehicle.GetVehicle();
				if (vehicle == null || !vehicle.IsValid() || (Entity)(object)vehicle.Vehicle == null || (vehicle.OwnerId != player.Id && (player.Faction == null || vehicle.Fraktion.Id != player.Faction.Id || player.Factionrank <= 9)))
				{
					return;
				}
				List<NativeItem> list = new List<NativeItem>();
				foreach (TuningCategory tuningCategory in tuningCategories)
				{
					if (tuningCategory != null)
						list.Add(new NativeItem(tuningCategory.Label, tuningCategory.Name));
				}
				NativeMenu nativeMenu = new NativeMenu("Tuner", "Angebote", list);
				player.ShowNativeMenu(nativeMenu);
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION openTuner] " + ex.Message);
				Logger.Print("[EXCEPTION openTuner] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Tuner")]
		public void Tuner(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				switch (selection)
				{
					case "color":
						{
							List<NativeItem> list21 = new List<NativeItem>();
							foreach (TuningColor tuningColor in tuningColors)
							{
								list21.Add(new NativeItem(tuningColor.Label + " - " + tuningColor.Price + "$", tuningColor.Name));
							}
							NativeMenu nativeMenu21 = new NativeMenu("Farbe", "Angebote", list21);
							player.ShowNativeMenu(nativeMenu21);
							break;
						}
					case "secondcolor":
						{
							List<NativeItem> list12 = new List<NativeItem>();
							foreach (TuningColor tuningColor2 in tuningColors)
							{
								list12.Add(new NativeItem(tuningColor2.Label + " - " + tuningColor2.Price + "$", tuningColor2.Name));
							}
							NativeMenu nativeMenu12 = new NativeMenu("Sekundärfarbe", "Angebote", list12);
							player.ShowNativeMenu(nativeMenu12);
							break;
						}
					case "pearl":
						{
							List<NativeItem> list15 = new List<NativeItem>();
							foreach (TuningColor tuningColor3 in tuningColors)
							{
								list15.Add(new NativeItem(tuningColor3.Label + " - " + tuningColor3.Price + "$", tuningColor3.Name));
							}
							NativeMenu nativeMenu15 = new NativeMenu("Perleffekt", "Angebote", list15);
							player.ShowNativeMenu(nativeMenu15);
							break;
						}
					case "neons":
						{
							List<NativeItem> list4 = new List<NativeItem>();
							foreach (NeonColor tuningNeon in tuningNeons)
							{
								list4.Add(new NativeItem(tuningNeon.Label + " - " + tuningNeon.Price + "$", tuningNeon.Name));
							}
							NativeMenu nativeMenu4 = new NativeMenu("Neons", "Angebote", list4);
							player.ShowNativeMenu(nativeMenu4);
							break;
						}
					case "ems":
						{
							List<NativeItem> list19 = new List<NativeItem>();
							foreach (TuningPart tuningEngine in tuningEngines)
							{
								list19.Add(new NativeItem(tuningEngine.Label + " - " + tuningEngine.Price + "$", tuningEngine.Name));
							}
							NativeMenu nativeMenu19 = new NativeMenu("Tuning", "Angebote", list19);
							player.ShowNativeMenu(nativeMenu19);
							break;
						}
					case "glasses":
						{
							List<NativeItem> list16 = new List<NativeItem>();
							foreach (TuningColor tuningWindow in tuningWindows)
							{
								list16.Add(new NativeItem(tuningWindow.Label + " - " + tuningWindow.Price + "$", tuningWindow.Name));
							}
							NativeMenu nativeMenu16 = new NativeMenu("Fensterscheiben", "Angebote", list16);
							player.ShowNativeMenu(nativeMenu16);
							break;
						}
					case "turbo":
						{
							List<NativeItem> list8 = new List<NativeItem>();
							foreach (TuningPart item in tuningTurbo)
							{
								list8.Add(new NativeItem(item.Label + " - " + item.Price + "$", item.Name));
							}
							NativeMenu nativeMenu8 = new NativeMenu("Tuning", "Angebote", list8);
							player.ShowNativeMenu(nativeMenu8);
							break;
						}
					case "brakes":
						{
							List<NativeItem> list23 = new List<NativeItem>();
							foreach (TuningPart tuningBrake in tuningBrakes)
							{
								list23.Add(new NativeItem(tuningBrake.Label + " - " + tuningBrake.Price + "$", tuningBrake.Name));
							}
							NativeMenu nativeMenu23 = new NativeMenu("Tuning", "Angebote", list23);
							player.ShowNativeMenu(nativeMenu23);
							break;
						}
					case "frontbumper":
						{
							List<NativeItem> list20 = new List<NativeItem>();
							foreach (TuningPart item2 in tuningFrontbumper)
							{
								list20.Add(new NativeItem(item2.Label + " - " + item2.Price + "$", item2.Name));
							}
							NativeMenu nativeMenu20 = new NativeMenu("Tuning", "Angebote", list20);
							player.ShowNativeMenu(nativeMenu20);
							break;
						}
					case "rearbumper":
						{
							List<NativeItem> list18 = new List<NativeItem>();
							foreach (TuningPart item3 in tuningRearbumper)
							{
								list18.Add(new NativeItem(item3.Label + " - " + item3.Price + "$", item3.Name));
							}
							NativeMenu nativeMenu18 = new NativeMenu("Tuning", "Angebote", list18);
							player.ShowNativeMenu(nativeMenu18);
							break;
						}
					case "sideskirt":
						{
							List<NativeItem> list14 = new List<NativeItem>();
							foreach (TuningPart item4 in tuningSideskirt)
							{
								list14.Add(new NativeItem(item4.Label + " - " + item4.Price + "$", item4.Name));
							}
							NativeMenu nativeMenu14 = new NativeMenu("Tuning", "Angebote", list14);
							player.ShowNativeMenu(nativeMenu14);
							break;
						}
					case "exhaust":
						{
							List<NativeItem> list17 = new List<NativeItem>();
							foreach (TuningPart item5 in tuningExhaust)
							{
								list17.Add(new NativeItem(item5.Label + " - " + item5.Price + "$", item5.Name));
							}
							NativeMenu nativeMenu17 = new NativeMenu("Tuning", "Angebote", list17);
							player.ShowNativeMenu(nativeMenu17);
							break;
						}
					case "frame":
						{
							List<NativeItem> list10 = new List<NativeItem>();
							foreach (TuningPart item6 in tuningFrame)
							{
								list10.Add(new NativeItem(item6.Label + " - " + item6.Price + "$", item6.Name));
							}
							NativeMenu nativeMenu10 = new NativeMenu("Tuning", "Angebote", list10);
							player.ShowNativeMenu(nativeMenu10);
							break;
						}
					case "grille":
						{
							List<NativeItem> list6 = new List<NativeItem>();
							foreach (TuningPart item7 in tuningGrille)
							{
								list6.Add(new NativeItem(item7.Label + " - " + item7.Price + "$", item7.Name));
							}
							NativeMenu nativeMenu6 = new NativeMenu("Tuning", "Angebote", list6);
							player.ShowNativeMenu(nativeMenu6);
							break;
						}
					case "hood":
						{
							List<NativeItem> list2 = new List<NativeItem>();
							foreach (TuningPart item8 in tuningHood)
							{
								list2.Add(new NativeItem(item8.Label + " - " + item8.Price + "$", item8.Name));
							}
							NativeMenu nativeMenu2 = new NativeMenu("Tuning", "Angebote", list2);
							player.ShowNativeMenu(nativeMenu2);
							break;
						}
					case "fender":
						{
							List<NativeItem> list22 = new List<NativeItem>();
							foreach (TuningPart item9 in tuningFender)
							{
								list22.Add(new NativeItem(item9.Label + " - " + item9.Price + "$", item9.Name));
							}
							NativeMenu nativeMenu22 = new NativeMenu("Tuning", "Angebote", list22);
							player.ShowNativeMenu(nativeMenu22);
							break;
						}
					case "roof":
						{
							DbVehicle vehicle2 = vehicle.GetVehicle();
							if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
							{
								break;
							}
							if (vehicle2.Model.ToLower() == "revolter")
							{
								player.SendNotification("Du kannst bei dem Revolter nicht das Dach tunen.", 3000, "red", "TUNING");
								player.CloseNativeMenu();
								break;
							}
							List<NativeItem> list13 = new List<NativeItem>();
							foreach (TuningPart item10 in tuningRoof)
							{
								list13.Add(new NativeItem(item10.Label + " - " + item10.Price + "$", item10.Name));
							}
							NativeMenu nativeMenu13 = new NativeMenu("Tuning", "Angebote", list13);
							player.ShowNativeMenu(nativeMenu13);
							break;
						}
					case "hydraulics":
						{
							List<NativeItem> list11 = new List<NativeItem>();
							foreach (TuningPart tuningHydraulic in tuningHydraulics)
							{
								list11.Add(new NativeItem(tuningHydraulic.Label + " - " + tuningHydraulic.Price + "$", tuningHydraulic.Name));
							}
							NativeMenu nativeMenu11 = new NativeMenu("Tuning", "Angebote", list11);
							player.ShowNativeMenu(nativeMenu11);
							break;
						}
					case "horn":
						{
							List<NativeItem> list9 = new List<NativeItem>();
							foreach (TuningPart tuningHorn in tuningHorns)
							{
								list9.Add(new NativeItem(tuningHorn.Label + " - " + tuningHorn.Price + "$", tuningHorn.Name));
							}
							NativeMenu nativeMenu9 = new NativeMenu("Tuning", "Angebote", list9);
							player.ShowNativeMenu(nativeMenu9);
							break;
						}
					case "spoiler":
						{
							List<NativeItem> list7 = new List<NativeItem>();
							foreach (TuningPart tuningSpoiler in tuningSpoilers)
							{
								list7.Add(new NativeItem(tuningSpoiler.Label + " - " + tuningSpoiler.Price + "$", tuningSpoiler.Name));
							}
							NativeMenu nativeMenu7 = new NativeMenu("Tuning", "Angebote", list7);
							player.ShowNativeMenu(nativeMenu7);
							break;
						}
					case "designs":
						{
							List<NativeItem> list5 = new List<NativeItem>();
							foreach (TuningPart tuningDesign in tuningDesigns)
							{
								list5.Add(new NativeItem(tuningDesign.Label + " - " + tuningDesign.Price + "$", tuningDesign.Name));
							}
							NativeMenu nativeMenu5 = new NativeMenu("Tuning", "Angebote", list5);
							player.ShowNativeMenu(nativeMenu5);
							break;
						}
					case "trails":
						{
							List<NativeItem> list3 = new List<NativeItem>();
							foreach (TuningPart tuningTrail in tuningTrails)
							{
								list3.Add(new NativeItem(tuningTrail.Label + " - " + tuningTrail.Price + "$", tuningTrail.Name));
							}
							NativeMenu nativeMenu3 = new NativeMenu("Tuning", "Angebote", list3);
							player.ShowNativeMenu(nativeMenu3);
							break;
						}
					case "lights":
						{
							List<NativeItem> list = new List<NativeItem>();
							foreach (TuningColor tuningLight in tuningLights)
							{
								list.Add(new NativeItem(tuningLight.Label + " - " + tuningLight.Price + "$", tuningLight.Name));
							}
							NativeMenu nativeMenu = new NativeMenu("Frontlichter", "Angebote", list);
							player.ShowNativeMenu(nativeMenu);
							break;
						}
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION openTuner] " + ex.Message);
				Logger.Print("[EXCEPTION openTuner] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Frontlichter")]
		public void Frontlichter(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				DbVehicle vehicle2 = vehicle.GetVehicle();
				if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
				{
					return;
				}
				TuningColor tuningColor3 = tuningLights.FirstOrDefault((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor3 == null)
				{
					return;
				}
				if (player.Money >= tuningColor3.Price)
				{
					player.SendNotification("Du hast dein Fahrzeug für " + tuningColor3.Price.ToDots() + "$ getuned.", 3000, "red", "TUNER");
					player.removeMoney(tuningColor3.Price);
					if (vehicle2.Fraktion == null)
					{
						vehicle2.SetAttribute("HeadlightColor", tuningColor3.ColorId);
					}
					else
					{
						MySqlQuery mySqlQuery = new MySqlQuery($"UPDATE fraktion_vehicles SET HeadlightColor = @tuning WHERE FactionId = @factionid AND Model = '{vehicle2.Model}'");
						mySqlQuery.AddParameter("@tuning", tuningColor3.ColorId);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
						mySqlQuery.Parameters.Clear();
						mySqlQuery = new MySqlQuery($"UPDATE fraktion_vehicles SET Neons = @tuning WHERE FactionId = @factionid AND Model = '{vehicle2.Model}'");
						mySqlQuery.AddParameter("@tuning", 1);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
					}
					((Entity)vehicle).SetSharedData("headlightColor", (object)tuningColor3.ColorId);
				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION nM-Frontlichter] " + ex.Message);
				Logger.Print("[EXCEPTION nM-Frontlichter] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Neons")]
		public void Neons(Client c, string selection)
		{
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				DbVehicle vehicle2 = vehicle.GetVehicle();
				if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
				{
					return;
				}
				NeonColor neonColor3 = tuningNeons.FirstOrDefault((NeonColor neonColor2) => neonColor2.Name == selection);
				if (neonColor3 == null)
				{
					return;
				}
				if (selection == "off")
				{
					player.SendNotification("Du hast dein Fahrzeug für " + neonColor3.Price.ToDots() + "$ getuned.", 3000, "red", "TUNER");
					vehicle2.SetAttribute("NeonColor", NAPI.Util.ToJson((object)neonColor3.Color));
					vehicle2.SetAttribute("Neons", 0);
					vehicle.NeonColor = (neonColor3.Color);
					vehicle.Neons = (false);
				}
				else if (player.Money >= neonColor3.Price)
				{
					player.SendNotification("Du hast dein Fahrzeug für " + neonColor3.Price.ToDots() + "$ getuned.", 3000, "red", "TUNER");
					player.removeMoney(neonColor3.Price);
					if (vehicle2.Fraktion == null)
					{
						vehicle2.SetAttribute("NeonColor", NAPI.Util.ToJson((object)neonColor3.Color));
						vehicle2.SetAttribute("Neons", 1);
					}
					else
					{
						MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET NeonColor = @tuning WHERE FactionId = @factionid AND Model = @model");
						mySqlQuery.AddParameter("@tuning", NAPI.Util.ToJson((object)neonColor3.Color));
						mySqlQuery.AddParameter("@model", vehicle2.Model);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
						mySqlQuery.Parameters.Clear();
						mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET Neons = @tuning WHERE FactionId = @factionid AND Model = @model");
						mySqlQuery.AddParameter("@tuning", 1);
						mySqlQuery.AddParameter("@model", vehicle2.Model);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
					}
					vehicle.NeonColor = (neonColor3.Color);
					vehicle.Neons = (true);
				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION nM-Neons] " + ex.Message);
				Logger.Print("[EXCEPTION nM-Neons] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Fensterscheiben")]
		public void Fensterscheiben(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				DbVehicle vehicle2 = vehicle.GetVehicle();
				if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
				{
					return;
				}
				TuningColor tuningColor3 = tuningWindows.FirstOrDefault((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor3 == null)
				{
					return;
				}
				if (player.Money >= tuningColor3.Price)
				{
					player.SendNotification("Du hast dein Fahrzeug für " + tuningColor3.Price.ToDots() + "$ lackiert.", 3000, "red", "TUNER");
					player.removeMoney(tuningColor3.Price);
					if (vehicle2.Fraktion == null)
					{
						vehicle2.SetAttribute("WindowTint", tuningColor3.ColorId);
					}
					else
					{
						MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET WindowTint = @tuning WHERE FactionId = @factionid AND Model = @model");
						mySqlQuery.AddParameter("@tuning", tuningColor3.ColorId);
						mySqlQuery.AddParameter("@model", vehicle2.Model);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
					}
					vehicle2.WindowTint = tuningColor3.ColorId;
					vehicle2.RefreshData(vehicle2);
					vehicle.WindowTint = (tuningColor3.ColorId);
				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION nM-Fensterscheiben] " + ex.Message);
				Logger.Print("[EXCEPTION nM-Fensterscheiben] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Farbe")]
		public void FirstColor(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				DbVehicle vehicle2 = vehicle.GetVehicle();
				if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
				{
					return;
				}
				TuningColor tuningColor3 = tuningColors.FirstOrDefault((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor3 == null)// || vehicle2.Fraktion != null)
				{
					return;
				}

				if (player.Money >= tuningColor3.Price)
				{
					player.SendNotification("Du hast dein Fahrzeug für " + tuningColor3.Price.ToDots() + "$ lackiert.", 3000, "red", "TUNER");
					player.removeMoney(tuningColor3.Price);
					if (vehicle2.Fraktion == null)
					{
						vehicle2.SetAttribute("PrimaryColor", tuningColor3.ColorId);
					}
					else
					{
						MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET PrimaryColor = @tuning WHERE FactionId = @factionid AND Model = @model");
						mySqlQuery.AddParameter("@tuning", tuningColor3.ColorId);
						mySqlQuery.AddParameter("@model", vehicle2.Model);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
					}
					vehicle2.PrimaryColor = tuningColor3.ColorId;
					vehicle2.RefreshData(vehicle2);
					vehicle.PrimaryColor = (tuningColor3.ColorId);
					vehicle.SecondaryColor = vehicle2.SecondaryColor;

					/*if (vehicle2.Fraktion != null)
					{
						var rot = vehicle2.Vehicle.Rotation;
						var second = vehicle2.Vehicle.SecondaryColor;
						var model = vehicle2.Vehicle.Model;
						var pos = vehicle2.Vehicle.Position;
						var plate = vehicle2.Vehicle.NumberPlate;
						var data = vehicle2;

						NAPI.Task.Run(() =>
						{
							vehicle2.Vehicle.Delete();
						});

						NAPI.Task.Run(() =>
						{
							var car = GarageModule.Instance.requestVehicle(player.Client, "takeout", 1, data.Id, true);
							if (car == null) return;
							car.Position = pos;
							car.Rotation = rot;
							
							NAPI.Player.SetPlayerIntoVehicle(player.Client, car, -1);
						}, 500);
					}*/
				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION FirstColor] " + ex.Message);
				Logger.Print("[EXCEPTION FirstColor] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Sekundärfarbe")]
		public void SecondColor(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				DbVehicle vehicle2 = vehicle.GetVehicle();
				if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
				{
					return;
				}
				TuningColor tuningColor3 = tuningColors.FirstOrDefault((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor3 == null)// || vehicle2.Fraktion != null)
				{
					return;
				}
				if (player.Money >= tuningColor3.Price)
				{
					player.SendNotification("Du hast dein Fahrzeug für " + tuningColor3.Price.ToDots() + "$ lackiert.", 3000, "red", "TUNER");
					player.removeMoney(tuningColor3.Price);
					if (vehicle2.Fraktion == null)
					{
						vehicle2.SetAttribute("SecondaryColor", tuningColor3.ColorId);
					}
					else
					{
						MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET SecondaryColor = @tuning WHERE FactionId = @factionid AND Model = @model");
						mySqlQuery.AddParameter("@tuning", tuningColor3.ColorId);
						mySqlQuery.AddParameter("@model", vehicle2.Model);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
					}
					vehicle2.SecondaryColor = tuningColor3.ColorId;
					vehicle2.RefreshData(vehicle2);
					vehicle.SecondaryColor = (tuningColor3.ColorId);
				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION Sekundärfarbe] " + ex.Message);
				Logger.Print("[EXCEPTION Sekundärfarbe] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Perleffekt")]
		public void Pearlescent(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				DbVehicle vehicle2 = vehicle.GetVehicle();
				if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
				{
					return;
				}
				TuningColor tuningColor3 = tuningColors.FirstOrDefault((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor3 == null)
				{
					return;
				}
				if (player.Money >= tuningColor3.Price)
				{
					player.SendNotification("Du hast dein Fahrzeug für " + tuningColor3.Price.ToDots() + "$ lackiert.", 3000, "red", "TUNER");
					player.removeMoney(tuningColor3.Price);
					if (vehicle2.Fraktion == null)
					{
						vehicle2.SetAttribute("PearlescentColor", tuningColor3.ColorId);
					}
					else
					{
						MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET PearlescentColor = @tuning WHERE FactionId = @factionid AND Model = @model");
						mySqlQuery.AddParameter("@tuning", tuningColor3.ColorId);
						mySqlQuery.AddParameter("@model", vehicle2.Model);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
					}
					vehicle2.PearlescentColor = tuningColor3.ColorId;
					vehicle2.RefreshData(vehicle2);
					vehicle.PearlescentColor = (tuningColor3.ColorId);
				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION Perleffekt] " + ex.Message);
				Logger.Print("[EXCEPTION Perleffekt] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Tuning")]
		public void Tuning(Client c, string selection)
		{
			try
			{
				if ((Entity)(object)c == null)
				{
					return;
				}
				DbPlayer player = c.GetPlayer();
				if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == null)
				{
					return;
				}
				Vehicle vehicle = c.Vehicle;
				if ((Entity)(object)vehicle == null)
				{
					return;
				}
				DbVehicle vehicle2 = vehicle.GetVehicle();
				if (vehicle2 == null || !vehicle2.IsValid() || (Entity)(object)vehicle2.Vehicle == null)
				{
					return;
				}
				TuningPart tuningPart3 = tuningBrakes.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningEngines.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningHorns.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningTurbo.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningTrails.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningSpoilers.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningFrontbumper.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningRearbumper.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningSideskirt.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningFrame.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningExhaust.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningGrille.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningHood.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningFender.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningRoof.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningHydraulics.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					tuningPart3 = tuningDesigns.FirstOrDefault((TuningPart tuningPart2) => tuningPart2.Name == selection);
				}
				if (tuningPart3 == null)
				{
					return;
				}
				if (player.Money >= tuningPart3.Price)
				{
					player.SendNotification("Du hast dein Fahrzeug für " + tuningPart3.Price.ToDots() + "$ getuned.", 3000, "red", "TUNER");
					player.removeMoney(tuningPart3.Price);
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					string text = vehicle2.GetAttributeString("Tuning");
					if (text != null && text != "[]")
					{
						dictionary = NAPI.Util.FromJson<Dictionary<int, int>>(text);
					}
					if (!dictionary.ContainsKey(tuningPart3.Index))
					{
						dictionary.Add(tuningPart3.Index, tuningPart3.PartId);
					}
					else
					{
						dictionary[tuningPart3.Index] = tuningPart3.PartId;
					}
					if (vehicle2.Fraktion == null)
					{
						vehicle2.SetAttribute("Tuning", NAPI.Util.ToJson((object)dictionary));
					}
					else
					{
						MySqlQuery mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET Tuning = @tuning WHERE FactionId = @factionid AND Model = @model");
						mySqlQuery.AddParameter("@tuning", NAPI.Util.ToJson((object)dictionary));
						mySqlQuery.AddParameter("@model", vehicle2.Model);
						mySqlQuery.AddParameter("@factionid", vehicle2.Fraktion.Id);
						MySqlHandler.ExecuteSync(mySqlQuery);
					}
					vehicle2.Tuning = dictionary;
					vehicle2.RefreshData(vehicle2);
					vehicle.SetMod(tuningPart3.Index, tuningPart3.PartId);
				}
				else
				{
					player.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION Tuning] " + ex.Message);
				Logger.Print("[EXCEPTION Tuning] " + ex.StackTrace);
			}
		}
	}
}
