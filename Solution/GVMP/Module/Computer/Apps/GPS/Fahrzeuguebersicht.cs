/*using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
	public class FahrzeugÜbersichtApp : Script
	{
		[RemoteEvent("requestVehicleOverviewByCategory")]
	public void requestVehicleOverviewByCategory(Client p, int id)
	{
		List<OwnVehicleModel> ownedVehicles = new List<OwnVehicleModel>();

		List<OwnVehicleModel> rentedCars = new List<OwnVehicleModel>();

		int parked = 1;

			DbVehicle dbVehicle = p.Vehicle.GetVehicle();
			foreach (Vehicles.VehicleModel veh in dbVehicle.p.Name)
		{
			Vehicle vehicle = Database.getVehicleFromPlate(veh.plate);
			string owner = veh.owner;

			if (Database.isVehicleParked((int)Database.getVehicleSQLId(veh.plate)) == true)
			{
				parked = 1;

			}
			else if (Database.isVehicleParked((int)Database.getVehicleSQLId(veh.plate)) == false)
			{
				parked = 0;
			}

			ownedVehicles.Add(new OwnVehicleModel(veh.name, (int)Database.getVehicleSQLId(veh.plate), parked, p.Name, Database.getVehicleGarage(owner)));
		}

		foreach (Vehicles.VehicleModel veh in Database.getRenterVehicles(p.Name))
		{
			int idCount = (int)Database.getVehicleSQLId(veh.plate);

			if (Database.isVehicleParked(idCount) == true)
			{
				parked = 1;
			}
			else
			{
				parked = 0;
			}

			rentedCars.Add(new OwnVehicleModel(veh.name, idCount, parked, veh.owner, Database.getVehicleGarage(p.Name)));
		}

		if (id == 0)
		{
			p.TriggerEvent("componentServerEvent", new object[3]
			{
					"FahrzeugUebersichtApp",
					"responseVehicleOverview",
					NAPI.Util.ToJson(ownedVehicles)
			});

		}
		else if (id == 1)
		{
			p.TriggerEvent("componentServerEvent", new object[3]
			{
					"FahrzeugUebersichtApp",
					"responseVehicleOverview",
					NAPI.Util.ToJson(rentedCars)
			});

		}
		else if (id == 2)
		{

		}
		else if (id == 3)
		{
		}
	}
}
}*/


