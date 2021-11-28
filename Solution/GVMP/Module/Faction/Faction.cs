using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Faction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Short { get; set; }
        public Vector3 Spawn { get; set; }
        public Vector3 Storage { get; set; }
        public int Dimension { get; set; }
        public Vector3 Garage { get; set; }
        public Vector3 GarageSpawn { get; set; }
        public float GarageSpawnRotation { get; set; }
        public int Blip { get; set; }
        public int CustomCarColor
        {
            get;
            set;
        }
        public Color RGB { get; set; }
        public List<ClothingModel> ClothesMale { get; set; } = new List<ClothingModel>
        {
            new ClothingModel("Keine", "Maske", 1, 0, 0),
            new ClothingModel("Standart Shirt", "Oberteil", 11, 0, 0),
            new ClothingModel("Keine", "Unterteil", 8, 15, 0),
            new ClothingModel("Koerper 1", "Koerper", 3, 0, 0),
            new ClothingModel("Jogginghose Weiss", "Hose", 4, 5, 0),
            new ClothingModel("Sneaker Schwarz", "Schuhe", 6, 1, 0)
        };
        public List<ClothingModel> ClothesFemale { get; set; } = new List<ClothingModel>
        {
            new ClothingModel("Keine", "Maske", 1, 0, 0),
            new ClothingModel("Standart Shirt", "Oberteil", 11, 0, 0),
            new ClothingModel("Keine", "Unterteil", 8, 15, 0),
            new ClothingModel("Koerper 1", "Koerper", 3, 0, 0),
            new ClothingModel("Jogginghose Weiss", "Hose", 4, 5, 0),
            new ClothingModel("Sneaker Schwarz", "Schuhe", 6, 1, 0)
        };
        public bool BadFraktion { get; set; } = true;
        public int Money { get; set; } = 0;
        public string Logo { get; set; }

        public Faction() { }
    }
}
