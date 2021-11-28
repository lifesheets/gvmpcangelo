using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace GVMP
{
    public class AssetsTattooModule : GVMP.Module.Module<AssetsTattooModule>
    {
        public static List<ClientTattoo> clientTattoosMale = new List<ClientTattoo>();
        public static List<ClientTattoo> clientTattoosFemale = new List<ClientTattoo>();
        public static Dictionary<uint, AssetsTattoo> AssetsTattoos = new Dictionary<uint, AssetsTattoo>();
        public static List<dynamic> AssetsTattoos3 = new List<dynamic>()
        {

  new {
    Name = "TAT_H3_000",
    LocalizedName = "Five Stars",
    HashNameMale = "mpHeist3_Tat_000_M",
    HashNameFemale = "mpHeist3_Tat_000_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 20750
  },
  new {
    Name = "TAT_H3_001",
    LocalizedName = "Ace of Spades",
    HashNameMale = "mpHeist3_Tat_001_M",
    HashNameFemale = "mpHeist3_Tat_001_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 15725
  },
  new {
    Name = "TAT_H3_002",
    LocalizedName = "Animal",
    HashNameMale = "mpHeist3_Tat_002_M",
    HashNameFemale = "mpHeist3_Tat_002_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 17200
  },
  new {
    Name = "TAT_H3_003",
    LocalizedName = "Assault Rifle",
    HashNameMale = "mpHeist3_Tat_003_M",
    HashNameFemale = "mpHeist3_Tat_003_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 16650
  },
  new {
    Name = "TAT_H3_004",
    LocalizedName = "Bandage",
    HashNameMale = "mpHeist3_Tat_004_M",
    HashNameFemale = "mpHeist3_Tat_004_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 18100
  },
  new {
    Name = "TAT_H3_005",
    LocalizedName = "Spades",
    HashNameMale = "mpHeist3_Tat_005_M",
    HashNameFemale = "mpHeist3_Tat_005_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 14500
  },
  new {
    Name = "TAT_H3_006",
    LocalizedName = "Crowned",
    HashNameMale = "mpHeist3_Tat_006_M",
    HashNameFemale = "mpHeist3_Tat_006_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 16950
  },
  new {
    Name = "TAT_H3_007",
    LocalizedName = "Two Horns",
    HashNameMale = "mpHeist3_Tat_007_M",
    HashNameFemale = "mpHeist3_Tat_007_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 24800
  },
  new {
    Name = "TAT_H3_008",
    LocalizedName = "Ice Cream",
    HashNameMale = "mpHeist3_Tat_008_M",
    HashNameFemale = "mpHeist3_Tat_008_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 21350
  },
  new {
    Name = "TAT_H3_009",
    LocalizedName = "Knifed",
    HashNameMale = "mpHeist3_Tat_009_M",
    HashNameFemale = "mpHeist3_Tat_009_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 18850
  },
  new {
    Name = "TAT_H3_010",
    LocalizedName = "Green Leaf",
    HashNameMale = "mpHeist3_Tat_010_M",
    HashNameFemale = "mpHeist3_Tat_010_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 14950
  },
  new {
    Name = "TAT_H3_011",
    LocalizedName = "Lipstick Kiss",
    HashNameMale = "mpHeist3_Tat_011_M",
    HashNameFemale = "mpHeist3_Tat_011_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 23900
  },
  new {
    Name = "TAT_H3_012",
    LocalizedName = "Razor Pop",
    HashNameMale = "mpHeist3_Tat_012_M",
    HashNameFemale = "mpHeist3_Tat_012_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 22850
  },
  new {
    Name = "TAT_H3_013",
    LocalizedName = "LS Star",
    HashNameMale = "mpHeist3_Tat_013_M",
    HashNameFemale = "mpHeist3_Tat_013_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 11200
  },
  new {
    Name = "TAT_H3_014",
    LocalizedName = "LS Wings",
    HashNameMale = "mpHeist3_Tat_014_M",
    HashNameFemale = "mpHeist3_Tat_014_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 11500
  },
  new {
    Name = "TAT_H3_015",
    LocalizedName = "On/Off",
    HashNameMale = "mpHeist3_Tat_015_M",
    HashNameFemale = "mpHeist3_Tat_015_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 15850
  },
  new {
    Name = "TAT_H3_016",
    LocalizedName = "Sleepy",
    HashNameMale = "mpHeist3_Tat_016_M",
    HashNameFemale = "mpHeist3_Tat_016_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 13900
  },
  new {
    Name = "TAT_H3_017",
    LocalizedName = "Space Monkey",
    HashNameMale = "mpHeist3_Tat_017_M",
    HashNameFemale = "mpHeist3_Tat_017_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 16050
  },
  new {
    Name = "TAT_H3_018",
    LocalizedName = "Stitches",
    HashNameMale = "mpHeist3_Tat_018_M",
    HashNameFemale = "mpHeist3_Tat_018_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 24600
  },
  new {
    Name = "TAT_H3_019",
    LocalizedName = "Teddy Bear",
    HashNameMale = "mpHeist3_Tat_019_M",
    HashNameFemale = "mpHeist3_Tat_019_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 20050
  },
  new {
    Name = "TAT_H3_020",
    LocalizedName = "UFO",
    HashNameMale = "mpHeist3_Tat_020_M",
    HashNameFemale = "mpHeist3_Tat_020_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 21200
  },
  new {
    Name = "TAT_H3_021",
    LocalizedName = "Wanted",
    HashNameMale = "mpHeist3_Tat_021_M",
    HashNameFemale = "mpHeist3_Tat_021_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 23700
  },
  new {
    Name = "TAT_H3_022",
    LocalizedName = "Thog's Sword",
    HashNameMale = "mpHeist3_Tat_022_M",
    HashNameFemale = "mpHeist3_Tat_022_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 20900
  },
  new {
    Name = "TAT_H3_023",
    LocalizedName = "Bigfoot",
    HashNameMale = "mpHeist3_Tat_023_M",
    HashNameFemale = "mpHeist3_Tat_023_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 40500
  },
  new {
    Name = "TAT_H3_024",
    LocalizedName = "Mount Chiliad",
    HashNameMale = "mpHeist3_Tat_024_M",
    HashNameFemale = "mpHeist3_Tat_024_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 75000
  },
  new {
    Name = "TAT_H3_025",
    LocalizedName = "Davis",
    HashNameMale = "mpHeist3_Tat_025_M",
    HashNameFemale = "mpHeist3_Tat_025_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 32500
  },
  new {
    Name = "TAT_H3_026",
    LocalizedName = "Dignity",
    HashNameMale = "mpHeist3_Tat_026_M",
    HashNameFemale = "mpHeist3_Tat_026_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 30300
  },
  new {
    Name = "TAT_H3_027",
    LocalizedName = "Epsilon",
    HashNameMale = "mpHeist3_Tat_027_M",
    HashNameFemale = "mpHeist3_Tat_027_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 64800
  },
  new {
    Name = "TAT_H3_028",
    LocalizedName = "Bananas Gone Bad",
    HashNameMale = "mpHeist3_Tat_028_M",
    HashNameFemale = "mpHeist3_Tat_028_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 52000
  },
  new {
    Name = "TAT_H3_029",
    LocalizedName = "Fatal Incursion",
    HashNameMale = "mpHeist3_Tat_029_M",
    HashNameFemale = "mpHeist3_Tat_029_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 33900
  },
  new {
    Name = "TAT_H3_030",
    LocalizedName = "Howitzer",
    HashNameMale = "mpHeist3_Tat_030_M",
    HashNameFemale = "mpHeist3_Tat_030_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 36000
  },
  new {
    Name = "TAT_H3_031",
    LocalizedName = "Kifflom",
    HashNameMale = "mpHeist3_Tat_031_M",
    HashNameFemale = "mpHeist3_Tat_031_F",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 39250
  },
  new {
    Name = "TAT_H3_032",
    LocalizedName = "Love Fist",
    HashNameMale = "mpHeist3_Tat_032_M",
    HashNameFemale = "mpHeist3_Tat_032_F",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 42600
  },
  new {
    Name = "TAT_H3_033",
    LocalizedName = "LS City",
    HashNameMale = "mpHeist3_Tat_033_M",
    HashNameFemale = "mpHeist3_Tat_033_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 28200
  },
  new {
    Name = "TAT_H3_034",
    LocalizedName = "LS Monogram",
    HashNameMale = "mpHeist3_Tat_034_M",
    HashNameFemale = "mpHeist3_Tat_034_F",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 15150
  },
  new {
    Name = "TAT_H3_035",
    LocalizedName = "LS Panic",
    HashNameMale = "mpHeist3_Tat_035_M",
    HashNameFemale = "mpHeist3_Tat_035_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 23900
  },
  new {
    Name = "TAT_H3_036",
    LocalizedName = "LS Shield",
    HashNameMale = "mpHeist3_Tat_036_M",
    HashNameFemale = "mpHeist3_Tat_036_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 30400
  },
  new {
    Name = "TAT_H3_037",
    LocalizedName = "Ladybug",
    HashNameMale = "mpHeist3_Tat_037_M",
    HashNameFemale = "mpHeist3_Tat_037_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 24100
  },
  new {
    Name = "TAT_H3_038",
    LocalizedName = "Robot Bubblegum",
    HashNameMale = "mpHeist3_Tat_038_M",
    HashNameFemale = "mpHeist3_Tat_038_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 52400
  },
  new {
    Name = "TAT_H3_039",
    LocalizedName = "Space Rangers",
    HashNameMale = "mpHeist3_Tat_039_M",
    HashNameFemale = "mpHeist3_Tat_039_F",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 49700
  },
  new {
    Name = "TAT_H3_040",
    LocalizedName = "Tiger Heart",
    HashNameMale = "mpHeist3_Tat_040_M",
    HashNameFemale = "mpHeist3_Tat_040_F",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 22600
  },
  new {
    Name = "TAT_H3_041",
    LocalizedName = "Mighty Thog",
    HashNameMale = "mpHeist3_Tat_041_M",
    HashNameFemale = "mpHeist3_Tat_041_F",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 37600
  },
  new {
    Name = "TAT_H3_042",
    LocalizedName = "Hearts",
    HashNameMale = "mpHeist3_Tat_042_M",
    HashNameFemale = "mpHeist3_Tat_042_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 14400
  },
  new {
    Name = "TAT_H3_043",
    LocalizedName = "Diamonds",
    HashNameMale = "mpHeist3_Tat_043_M",
    HashNameFemale = "mpHeist3_Tat_043_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 14300
  },
  new {
    Name = "TAT_H3_044",
    LocalizedName = "Clubs",
    HashNameMale = "mpHeist3_Tat_044_M",
    HashNameFemale = "mpHeist3_Tat_044_F",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 14200
  }

        };
        public static List<dynamic> AssetsTattoos2 = new List<dynamic>()
        {

  new {
    Name = "TAT_FM_008",
    LocalizedName = "Skull",
    HashNameMale = "FM_Tat_Award_M_000",
    HashNameFemale = "FM_Tat_Award_F_000",
    Zone = "ZONE_HEAD",
    ZoneID = 1,
    Price = 20000
  },
  new {
    Name = "TAT_FM_009",
    LocalizedName = "Burning Heart",
    HashNameMale = "FM_Tat_Award_M_001",
    HashNameFemale = "FM_Tat_Award_F_001",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 1400
  },
  new {
    Name = "TAT_FM_010",
    LocalizedName = "Grim Reaper Smoking Gun",
    HashNameMale = "FM_Tat_Award_M_002",
    HashNameFemale = "FM_Tat_Award_F_002",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 9750
  },
  new {
    Name = "TAT_FM_011",
    LocalizedName = "Blackjack",
    HashNameMale = "FM_Tat_Award_M_003",
    HashNameFemale = "FM_Tat_Award_F_003",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 2150
  },
  new {
    Name = "TAT_FM_012",
    LocalizedName = "Hustler",
    HashNameMale = "FM_Tat_Award_M_004",
    HashNameFemale = "FM_Tat_Award_F_004",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 10000
  },
  new {
    Name = "TAT_FM_013",
    LocalizedName = "Angel",
    HashNameMale = "FM_Tat_Award_M_005",
    HashNameFemale = "FM_Tat_Award_F_005",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 12400
  },
  new {
    Name = "TAT_FM_014",
    LocalizedName = "Skull and Sword",
    HashNameMale = "FM_Tat_Award_M_006",
    HashNameFemale = "FM_Tat_Award_F_006",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 3500
  },
  new {
    Name = "TAT_FM_015",
    LocalizedName = "Racing Blonde",
    HashNameMale = "FM_Tat_Award_M_007",
    HashNameFemale = "FM_Tat_Award_F_007",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 4950
  },
  new {
    Name = "TAT_FM_016",
    LocalizedName = "Los Santos Customs",
    HashNameMale = "FM_Tat_Award_M_008",
    HashNameFemale = "FM_Tat_Award_F_008",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 1350
  },
  new {
    Name = "TAT_FM_017",
    LocalizedName = "Dragon and Dagger",
    HashNameMale = "FM_Tat_Award_M_009",
    HashNameFemale = "FM_Tat_Award_F_009",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 1450
  },
  new {
    Name = "TAT_FM_018",
    LocalizedName = "Ride or Die",
    HashNameMale = "FM_Tat_Award_M_010",
    HashNameFemale = "FM_Tat_Award_F_010",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 2700
  },
  new {
    Name = "TAT_FM_019",
    LocalizedName = "Blank Scroll",
    HashNameMale = "FM_Tat_Award_M_011",
    HashNameFemale = "FM_Tat_Award_F_011",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 1200
  },
  new {
    Name = "TAT_FM_020",
    LocalizedName = "Embellished Scroll",
    HashNameMale = "FM_Tat_Award_M_012",
    HashNameFemale = "FM_Tat_Award_F_012",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 1500
  },
  new {
    Name = "TAT_FM_021",
    LocalizedName = "Seven Deadly Sins",
    HashNameMale = "FM_Tat_Award_M_013",
    HashNameFemale = "FM_Tat_Award_F_013",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 2650
  },
  new {
    Name = "TAT_FM_022",
    LocalizedName = "Trust No One",
    HashNameMale = "FM_Tat_Award_M_014",
    HashNameFemale = "FM_Tat_Award_F_014",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 1900
  },
  new {
    Name = "TAT_FM_023",
    LocalizedName = "Racing Brunette",
    HashNameMale = "FM_Tat_Award_M_015",
    HashNameFemale = "FM_Tat_Award_F_015",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 4950
  },
  new {
    Name = "TAT_FM_024",
    LocalizedName = "Clown",
    HashNameMale = "FM_Tat_Award_M_016",
    HashNameFemale = "FM_Tat_Award_F_016",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 2400
  },
  new {
    Name = "TAT_FM_025",
    LocalizedName = "Clown and Gun",
    HashNameMale = "FM_Tat_Award_M_017",
    HashNameFemale = "FM_Tat_Award_F_017",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 5100
  },
  new {
    Name = "TAT_FM_026",
    LocalizedName = "Clown Dual Wield",
    HashNameMale = "FM_Tat_Award_M_018",
    HashNameFemale = "FM_Tat_Award_F_018",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 7400
  },
  new {
    Name = "TAT_FM_027",
    LocalizedName = "Clown Dual Wield Dollars",
    HashNameMale = "FM_Tat_Award_M_019",
    HashNameFemale = "FM_Tat_Award_F_019",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 10000
  },
  new {
    Name = "TAT_FM_204",
    LocalizedName = "Brotherhood",
    HashNameMale = "FM_Tat_M_000",
    HashNameFemale = "FM_Tat_F_000",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 10000
  },
  new {
    Name = "TAT_FM_205",
    LocalizedName = "Dragons",
    HashNameMale = "FM_Tat_M_001",
    HashNameFemale = "FM_Tat_F_001",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 12500
  },
  new {
    Name = "TAT_FM_209",
    LocalizedName = "Melting Skull",
    HashNameMale = "FM_Tat_M_002",
    HashNameFemale = "FM_Tat_F_002",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 3750
  },
  new {
    Name = "TAT_FM_206",
    LocalizedName = "Dragons and Skull",
    HashNameMale = "FM_Tat_M_003",
    HashNameFemale = "FM_Tat_F_003",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 10000
  },
  new {
    Name = "TAT_FM_219",
    LocalizedName = "Faith",
    HashNameMale = "FM_Tat_M_004",
    HashNameFemale = "FM_Tat_F_004",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 10000
  },
  new {
    Name = "TAT_FM_201",
    LocalizedName = "Serpents",
    HashNameMale = "FM_Tat_M_005",
    HashNameFemale = "FM_Tat_F_005",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 2400
  },
  new {
    Name = "TAT_FM_202",
    LocalizedName = "Oriental Mural",
    HashNameMale = "FM_Tat_M_006",
    HashNameFemale = "FM_Tat_F_006",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 5100
  },
  new {
    Name = "TAT_FM_210",
    LocalizedName = "The Warrior",
    HashNameMale = "FM_Tat_M_007",
    HashNameFemale = "FM_Tat_F_007",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 3750
  },
  new {
    Name = "TAT_FM_211",
    LocalizedName = "Dragon Mural",
    HashNameMale = "FM_Tat_M_008",
    HashNameFemale = "FM_Tat_F_008",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 4800
  },
  new {
    Name = "TAT_FM_213",
    LocalizedName = "Skull on the Cross",
    HashNameMale = "FM_Tat_M_009",
    HashNameFemale = "FM_Tat_F_009",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 12350
  },
  new {
    Name = "TAT_FM_218",
    LocalizedName = "LS Flames",
    HashNameMale = "FM_Tat_M_010",
    HashNameFemale = "FM_Tat_F_010",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 2500
  },
  new {
    Name = "TAT_FM_214",
    LocalizedName = "LS Script",
    HashNameMale = "FM_Tat_M_011",
    HashNameFemale = "FM_Tat_F_011",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 1900
  },
  new {
    Name = "TAT_FM_220",
    LocalizedName = "Los Santos Bills",
    HashNameMale = "FM_Tat_M_012",
    HashNameFemale = "FM_Tat_F_012",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 10000
  },
  new {
    Name = "TAT_FM_215",
    LocalizedName = "Eagle and Serpent",
    HashNameMale = "FM_Tat_M_013",
    HashNameFemale = "FM_Tat_F_013",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 4500
  },
  new {
    Name = "TAT_FM_207",
    LocalizedName = "Flower Mural",
    HashNameMale = "FM_Tat_M_014",
    HashNameFemale = "FM_Tat_F_014",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 5000
  },
  new {
    Name = "TAT_FM_203",
    LocalizedName = "Zodiac Skull",
    HashNameMale = "FM_Tat_M_015",
    HashNameFemale = "FM_Tat_F_015",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 3600
  },
  new {
    Name = "TAT_FM_216",
    LocalizedName = "Evil Clown",
    HashNameMale = "FM_Tat_M_016",
    HashNameFemale = "FM_Tat_F_016",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 12250
  },
  new {
    Name = "TAT_FM_212",
    LocalizedName = "Tribal",
    HashNameMale = "FM_Tat_M_017",
    HashNameFemale = "FM_Tat_F_017",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 3500
  },
  new {
    Name = "TAT_FM_208",
    LocalizedName = "Serpent Skull",
    HashNameMale = "FM_Tat_M_018",
    HashNameFemale = "FM_Tat_F_018",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 7500
  },
  new {
    Name = "TAT_FM_217",
    LocalizedName = "The Wages of Sin",
    HashNameMale = "FM_Tat_M_019",
    HashNameFemale = "FM_Tat_F_019",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 12300
  },
  new {
    Name = "TAT_FM_221",
    LocalizedName = "Dragon",
    HashNameMale = "FM_Tat_M_020",
    HashNameFemale = "FM_Tat_F_020",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 7500
  },
  new {
    Name = "TAT_FM_222",
    LocalizedName = "Serpent Skull",
    HashNameMale = "FM_Tat_M_021",
    HashNameFemale = "FM_Tat_F_021",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 5000
  },
  new {
    Name = "TAT_FM_223",
    LocalizedName = "Fiery Dragon",
    HashNameMale = "FM_Tat_M_022",
    HashNameFemale = "FM_Tat_F_022",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 7300
  },
  new {
    Name = "TAT_FM_224",
    LocalizedName = "Hottie",
    HashNameMale = "FM_Tat_M_023",
    HashNameFemale = "FM_Tat_F_023",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 7250
  },
  new {
    Name = "TAT_FM_225",
    LocalizedName = "Flaming Cross",
    HashNameMale = "FM_Tat_M_024",
    HashNameFemale = "FM_Tat_F_024",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 11900
  },
  new {
    Name = "TAT_FM_226",
    LocalizedName = "LS Bold",
    HashNameMale = "FM_Tat_M_025",
    HashNameFemale = "FM_Tat_F_025",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 2750
  },
  new {
    Name = "TAT_FM_227",
    LocalizedName = "Smoking Dagger",
    HashNameMale = "FM_Tat_M_026",
    HashNameFemale = "FM_Tat_F_026",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 1750
  },
  new {
    Name = "TAT_FM_228",
    LocalizedName = "Virgin Mary",
    HashNameMale = "FM_Tat_M_027",
    HashNameFemale = "FM_Tat_F_027",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 7300
  },
  new {
    Name = "TAT_FM_229",
    LocalizedName = "Mermaid",
    HashNameMale = "FM_Tat_M_028",
    HashNameFemale = "FM_Tat_F_028",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 3250
  },
  new {
    Name = "TAT_FM_230",
    LocalizedName = "Trinity Knot",
    HashNameMale = "FM_Tat_M_029",
    HashNameFemale = "FM_Tat_F_029",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 1000
  },
  new {
    Name = "TAT_FM_231",
    LocalizedName = "Lucky Celtic Dogs",
    HashNameMale = "FM_Tat_M_030",
    HashNameFemale = "FM_Tat_F_030",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 5000
  },
  new {
    Name = "TAT_FM_232",
    LocalizedName = "Lady M",
    HashNameMale = "FM_Tat_M_031",
    HashNameFemale = "FM_Tat_F_031",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 7500
  },
  new {
    Name = "TAT_FM_233",
    LocalizedName = "Faith",
    HashNameMale = "FM_Tat_M_032",
    HashNameFemale = "FM_Tat_F_032",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 5100
  },
  new {
    Name = "TAT_FM_234",
    LocalizedName = "Chinese Dragon",
    HashNameMale = "FM_Tat_M_033",
    HashNameFemale = "FM_Tat_F_033",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 5050
  },
  new {
    Name = "TAT_FM_235",
    LocalizedName = "Flaming Shamrock",
    HashNameMale = "FM_Tat_M_034",
    HashNameFemale = "FM_Tat_F_034",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 2450
  },
  new {
    Name = "TAT_FM_236",
    LocalizedName = "Dragon",
    HashNameMale = "FM_Tat_M_035",
    HashNameFemale = "FM_Tat_F_035",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 4950
  },
  new {
    Name = "TAT_FM_237",
    LocalizedName = "Way of the Gun",
    HashNameMale = "FM_Tat_M_036",
    HashNameFemale = "FM_Tat_F_036",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 5100
  },
  new {
    Name = "TAT_FM_238",
    LocalizedName = "Grim Reaper",
    HashNameMale = "FM_Tat_M_037",
    HashNameFemale = "FM_Tat_F_037",
    Zone = "ZONE_LEFT_LEG",
    ZoneID = 4,
    Price = 12250
  },
  new {
    Name = "TAT_FM_239",
    LocalizedName = "Dagger",
    HashNameMale = "FM_Tat_M_038",
    HashNameFemale = "FM_Tat_F_038",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 1150
  },
  new {
    Name = "TAT_FM_240",
    LocalizedName = "Broken Skull",
    HashNameMale = "FM_Tat_M_039",
    HashNameFemale = "FM_Tat_F_039",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 7500
  },
  new {
    Name = "TAT_FM_241",
    LocalizedName = "Flaming Skull",
    HashNameMale = "FM_Tat_M_040",
    HashNameFemale = "FM_Tat_F_040",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 7600
  },
  new {
    Name = "TAT_FM_242",
    LocalizedName = "Dope Skull",
    HashNameMale = "FM_Tat_M_041",
    HashNameFemale = "FM_Tat_F_041",
    Zone = "ZONE_LEFT_ARM",
    ZoneID = 2,
    Price = 2600
  },
  new {
    Name = "TAT_FM_243",
    LocalizedName = "Flaming Scorpion",
    HashNameMale = "FM_Tat_M_042",
    HashNameFemale = "FM_Tat_F_042",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 2500
  },
  new {
    Name = "TAT_FM_244",
    LocalizedName = "Indian Ram",
    HashNameMale = "FM_Tat_M_043",
    HashNameFemale = "FM_Tat_F_043",
    Zone = "ZONE_RIGHT_LEG",
    ZoneID = 5,
    Price = 7450
  },
  new {
    Name = "TAT_FM_245",
    LocalizedName = "Stone Cross",
    HashNameMale = "FM_Tat_M_044",
    HashNameFemale = "FM_Tat_F_044",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 7500
  },
  new {
    Name = "TAT_FM_246",
    LocalizedName = "Skulls and Rose",
    HashNameMale = "FM_Tat_M_045",
    HashNameFemale = "FM_Tat_F_045",
    Zone = "ZONE_TORSO",
    ZoneID = 0,
    Price = 10000
  },
  new {
    Name = "TAT_FM_247",
    LocalizedName = "Lion",
    HashNameMale = "FM_Tat_M_047",
    HashNameFemale = "FM_Tat_F_047",
    Zone = "ZONE_RIGHT_ARM",
    ZoneID = 3,
    Price = 2500
  }

        };

        protected override bool OnLoad()
        {
            MySqlQuery query = new MySqlQuery("SELECT * FROM assets_tattoo");
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
                            var tattoo = new AssetsTattoo(reader);
                            AssetsTattoos.Add(tattoo.GetIdentifier(), tattoo);
                        }
                    }

                    foreach (dynamic obj in AssetsTattoos2)
                    {
                        if (obj.ZoneID == 1) continue;
                        uint id = (uint)new Random().Next(10000, 20000);
                        AssetsTattoo assetsTattoo = new AssetsTattoo
                        {
                            Id = id,
                            Name = obj.LocalizedName,
                            Collection = "mpairraces_overlays",
                            HashFemale = obj.HashNameFemale,
                            HashMale = obj.HashNameMale,
                            ZoneId = obj.ZoneID,
                            Price = obj.Price
                        };
                        if (AssetsTattoos.ContainsKey(id)) continue;
                        AssetsTattoos.Add(id, assetsTattoo);
                    }

                    foreach (dynamic obj in AssetsTattoos3)
                    {
                        if (obj.ZoneID == 1) continue;
                        uint id = (uint)new Random().Next(20000, 30000);
                        AssetsTattoo assetsTattoo = new AssetsTattoo
                        {
                            Id = id,
                            Name = obj.LocalizedName,
                            Collection = "mpheist3_overlays",
                            HashFemale = obj.HashNameFemale,
                            HashMale = obj.HashNameMale,
                            ZoneId = obj.ZoneID,
                            Price = obj.Price
                        };
                        if (AssetsTattoos.ContainsKey(id)) continue;
                        AssetsTattoos.Add(id, assetsTattoo);
                    }

                    foreach (AssetsTattoo assetsTattoo in AssetsTattooModule.AssetsTattoos.Values)
                    {
                        clientTattoosMale.Add(new ClientTattoo(assetsTattoo.HashMale.ToLower(), assetsTattoo.ZoneId, assetsTattoo.Price, assetsTattoo.Name.Replace(" ", "").Replace("/", "").Replace("-", ""), assetsTattoo.Collection, assetsTattoo.Id));
                        clientTattoosFemale.Add(new ClientTattoo(assetsTattoo.HashFemale.ToLower(), assetsTattoo.ZoneId, assetsTattoo.Price, assetsTattoo.Name.Replace(" ", "").Replace("/", "").Replace("-", ""), assetsTattoo.Collection, assetsTattoo.Id));
                    }
                }
                finally
                {
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadtattoo] " + ex.Message);
                Logger.Print("[EXCEPTION loadtattoo] " + ex.StackTrace);
            }
            finally
            {
                query2.Connection.Dispose();
            }
            return true;
        }
    }
}