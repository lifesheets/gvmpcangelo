using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class HouseClassModule : GVMP.Module.Module<HouseClassModule>
    {
        public static List<HouseClass> houseClasses = new List<HouseClass>();

        protected override bool OnLoad()
        {
            houseClasses.Add(new HouseClass
            {
                Id = 4,
                Name = "High-End",
                HouseLocation = new Vector3(-785, 324, 212),
                KleiderschrankLocation = new Vector3(-793, 327, 211),
                LagerLocation = new Vector3(-765, 331, 211),
                MaxTenants = 3
            });

            houseClasses.Add(new HouseClass
            {
                Id = 3,
                Name = "High",
                HouseLocation = new Vector3(346, -1013, -99),
                KleiderschrankLocation = new Vector3(351, -994, -99),
                LagerLocation = new Vector3(345, -995, -99),
                MaxTenants = 2
            });

            houseClasses.Add(new HouseClass
            {
                Id = 2,
                Name = "Normal",
                HouseLocation = new Vector3(266, -1007, -101),
                KleiderschrankLocation = new Vector3(260, -1004, -99),
                LagerLocation = new Vector3(265, -999, -99),
                MaxTenants = 1
            });

            houseClasses.Add(new HouseClass
            {
                Id = 1,
                Name = "Low",
                HouseLocation = new Vector3(151, -1008, -99),
                KleiderschrankLocation = new Vector3(152, -1002, -99),
                LagerLocation = new Vector3(153, -1001, -99),
                MaxTenants = 0
            });

            foreach(HouseClass houseClass in houseClasses)
            {
                ColShape val = NAPI.ColShape.CreateCylinderColShape(houseClass.HouseLocation, 1.4f, 1.4f, uint.MaxValue);
                val.SetData("FUNCTION_MODEL", new FunctionModel("leaveHouse"));
                val.SetData("MESSAGE", new Message("Drücke E um die Immobilie zu verlassen.", "Immobilie", "green", 3000));
                NAPI.Marker.CreateMarker(1, houseClass.HouseLocation.Subtract(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, uint.MaxValue);

                ColShape val2 = NAPI.ColShape.CreateCylinderColShape(houseClass.KleiderschrankLocation, 1.4f, 1.4f, uint.MaxValue);
                val2.SetData("FUNCTION_MODEL", new FunctionModel("openHouseWardrobe"));
                val2.SetData("MESSAGE", new Message("Drücke E um den Kleiderschrank zu öffnen.", "Immobilie", "green", 3000));
                NAPI.Marker.CreateMarker(1, houseClass.KleiderschrankLocation.Subtract(new Vector3(0, 0, 1)), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, uint.MaxValue);
            }

            return true;
        }
    }
}
