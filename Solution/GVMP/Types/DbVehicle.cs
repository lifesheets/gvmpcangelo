using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class DbVehicle
    {
        public Vehicle Vehicle { get; set; }

        public string Model { get; set; }

        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Plate { get; set; }

        public List<int> Keys { get; set; }

        public int PrimaryColor { get; set; }

        public int SecondaryColor { get; set; }

        public int PearlescentColor { get; set; }

        public int WindowTint { get; set; }

        public Dictionary<int, int> Tuning { get; set; }

        public bool Parked { get; set; }

        public Faction Fraktion { get; set; } = null;

        public bool HasData(string key) => this.Vehicle.HasData(key);

        public void ResetData(string key) => this.Vehicle.ResetData(key);

        public object GetData(string key) => this.Vehicle.GetData(key);

        public void SetData(string key, object value) => this.Vehicle.SetData(key, value);

        public void RefreshData(DbVehicle dbVehicle)
        {
            this.Vehicle.ResetData("vehicle");
            this.Vehicle.SetData("vehicle", dbVehicle);
        }

        public DbVehicle() { }
    }
}
