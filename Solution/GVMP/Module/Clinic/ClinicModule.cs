using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class ClinicModule : GVMP.Module.Module<ClinicModule>
    {
        public static List<Clinic> clinicList = new List<Clinic>();

        protected override bool OnLoad()
        {
            clinicList.Add(new Clinic
            {
                Id = 1,
                Position = new Vector3(356.1, -596.33, 27.77)
            });

            foreach (Clinic clinic in clinicList)
            {
                ColShape c = NAPI.ColShape.CreateCylinderColShape(clinic.Position, 2.4f, 2.4f, 0);
                c.SetData("FUNCTION_MODEL", new FunctionModel("openClinic"));
                c.SetData("MESSAGE", new Message("Benutze E um die Schönheitsklinik zu betreten. (5.000$)", "KLINIK", "green", 3000));

                NAPI.Marker.CreateMarker(1, clinic.Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, 0);
                NAPI.Blip.CreateBlip(468, clinic.Position, 1f, 0, "Schönheitsklinik", 255, 0, true, 0, 0);
            }

            return true;
        }

        [RemoteEvent("openClinic")]
        public void openClinic(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.Money >= 5000)
                {
                    dbPlayer.SendNotification("Du hast die Schönheitsklinik betreten!", 3000, "green");
                    dbPlayer.removeMoney(5000);
                    dbPlayer.OpenCharacterCreator();
                    dbPlayer.SetPosition(new Vector3(402.8664, -996.4108, -99.00027));
                    dbPlayer.Client.Eval("mp.players.local.setHeading(-185);");
                    dbPlayer.SetDimension( new Random().Next(10000, 99999999));
                }
                else
                {
                    dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openClinic] " + ex.Message);
                Logger.Print("[EXCEPTION openClinic] " + ex.StackTrace);
            }
        }
    }
}
