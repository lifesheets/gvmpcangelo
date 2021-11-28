using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace GVMP
{
    public class ATM : GVMP.Module.Module<ATM>
    {
        public static List<ATMModel> Atm_ = new List<ATMModel>();


        protected override bool OnLoad()
        {

            Atm_.Add(new ATMModel
            {
                Position = new Vector3(4.5, -919.63, 29.56),
                robbed = false
            });

            Atm_.Add(new ATMModel
            {
                Position = new Vector3(24.21, -946.77, 29.36),
                robbed = false
            });
            Atm_.Add(new ATMModel
            {
                Position = new Vector3(47.5871, -1035.6813, 28.243156),
                robbed = false
            });
            Atm_.Add(new ATMModel
            {
                Position = new Vector3(147.28253, -1035.641, 29.370464),
                robbed = false
            });
            Atm_.Add(new ATMModel
            {
                Position = new Vector3(146.1826, -1034.9517, 29.370464),
                robbed = false
            });
            Atm_.Add(new ATMModel
            {
                Position = new Vector3(114.21235, -776.4816, 30.318945),
                robbed = false
            });
            Atm_.Add(new ATMModel
            {
                Position = new Vector3(295.3971, -895.7719, 29.030422),
                robbed = false
            });
            Atm_.Add(new ATMModel
            {
                Position = new Vector3(296.45862, -894.267, 29.030422),
                robbed = false
            }); Atm_.Add(new ATMModel
            {
                Position = new Vector3(289.01447, -1256.82, 28.340746),
                robbed = false
            }); Atm_.Add(new ATMModel
            {
                Position = new Vector3(288.7219, -1282.1271, 28.527348),
                robbed = false
            }); Atm_.Add(new ATMModel
            {
                Position = new Vector3(33.27779, -1347.8456, 29.037973),
                robbed = false
            }); Atm_.Add(new ATMModel
            {
                Position = new Vector3(-56.580833, -1752.3906, 28.958536),
                robbed = false
            });

            Atm_.ForEach(x => {

                ColShape val1 = NAPI.ColShape.CreateCylinderColShape(x.Position, 1.4f, 2.4f, 0);
                val1.SetData("FUNCTION_MODEL", new FunctionModel("robATM"));
                val1.SetData("MESSAGE", new Message("Drücke E um den ATM auszurauben", "", "red", 3000));
                NAPI.Marker.CreateMarker(29, x.Position, new Vector3(), new Vector3(), 1.0f, new Color(0, 239, 255), false, 0);

            });
            return true;
        }


        [RemoteEvent("robATM")]
        public static void robATM(Client client)
        {
            try
            {
                if (client == null) return;
                if (client.IsInVehicle)
                {
                    client.Kick();
                }
                else
                {


                    DbPlayer dbPlayer = client.GetPlayer();

                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                        return;


                    Atm_.ForEach(x =>
                    {
                        if (x.Position.DistanceTo(client.Position) <= 1.5f)
                        {
                            if (!dbPlayer.IsFarming)
                            {
                                if (!x.robbed)
                                {
                                    dbPlayer.disableAllPlayerActions(true);
                                    dbPlayer.AllActionsDisabled = true;
                                    dbPlayer.SendProgressbar(15000);
                                    dbPlayer.IsFarming = true;
                                    dbPlayer.RefreshData(dbPlayer);
                                    dbPlayer.PlayAnimation(33, "amb@world_human_welding@male@idle_a", "idle_a", 8f);
                                    dbPlayer.SendNotification("Du raubst nun diesen ATM aus!", 2000, "grey");
                                    x.robbed = true;



                                    NAPI.Task.Run(delegate
                                    {
                                        dbPlayer.TriggerEvent("client:respawning");
                                        dbPlayer.StopProgressbar();
                                        dbPlayer.addMoney(300000);
                                        dbPlayer.IsFarming = false;
                                        dbPlayer.RefreshData(dbPlayer);
                                        dbPlayer.disableAllPlayerActions(false);
                                        dbPlayer.StopAnimation();
                                        dbPlayer.SendNotification("Du hast diesen ATM erfolgreich ausgeraubt!!", 2000, "green");
                                    }, 15000);

                                }
                                else
                                {
                                    dbPlayer.SendNotification("Dieser ATM wurde bereits angegriffen.", 3000, "red", "ATM");
                                    return;
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION robATM] " + ex);
            }
        }
    }
}
