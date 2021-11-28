using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class GarageWindow
    {
        public static void OpenGarage(this DbPlayer dbPlayer, int Id, string Name, bool Fraktion)
        {
            dbPlayer.TriggerEvent("openWindow", "Garage", "{\"id\":" + Id + ", \"name\": \"" + Name + "\", \"fraktion\":" + Fraktion.ToString().ToLower() + "}");
        }

        public static void responseVehicleList(this DbPlayer dbPlayer, string Json)
        {
            dbPlayer.TriggerEvent("componentServerEvent", "Garage", "responseVehicleList", Json);
        }
    }
}
