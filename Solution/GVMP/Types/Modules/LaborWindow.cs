using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class LaborWindow
    {
        public static void OpenLabor(this DbPlayer dbPlayer, bool status)
        {
            object Laboratory = new
            {
                temperature = new {
                    min = 100,
                    max = 1500,
                    current = 563,
                    steps = 10
                },
                pressure = new
                {
                    min = 1,
                    max = 10,
                    current = 5
                },
                stirring = new
                {
                    min = 1,
                    max = 300,
                    curent = 239
                },
                amount = new
                {
                    min = 5,
                    max = 15,
                    current = 7
                },
                status = status
            };

            dbPlayer.TriggerEvent("openWindow", "MethLabor", NAPI.Util.ToJson(Laboratory));
        }
    }
}
