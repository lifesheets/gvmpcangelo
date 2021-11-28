using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Linq;

namespace GVMP
{
    class KeyHandler : Script
    {
        [RemoteEvent("Pressed_L")]
        public void Pressed_L(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (!dbPlayer.CanInteractAntiFlood(2)) return;

                HouseModule.PressedL(dbPlayer);
                XMenu.PressedL(dbPlayer);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Pressed_L] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_L] " + ex.StackTrace);
            }
        }

        [RemoteEvent("Pressed_K")]
        public static void ShowAnimMenu(Client c)
        {
            try
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;
                dbPlayer.ShowNativeMenu(new NativeMenu("Kleidung", "Menu", new List<NativeItem>()
                {
                    new NativeItem("Kleidung wieder anziehen", "anziehen"),
                    new NativeItem("Oberteil", "oberteil"),
                    new NativeItem("Hut", "hut"),
                    new NativeItem("Brille", "brille"),
                    new NativeItem("Hose", "hose"),
                    new NativeItem("Schuhe", "schuhe")
                }));
            }
            catch (Exception ex) { }
        }

        [RemoteEvent("nM-Kleidung")]
        public static void Kleidung(Client c, string arg)
        {
            try
            {

                DbPlayer dbPlayer = c.GetPlayer();
                if (!dbPlayer.CanInteractAntiFlood(2)) return;
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;
                if (arg == "hut")
                {
                    c.SetAccessories(0, -1, 0);
                    return;
                }

                if (arg == "oberteil")
                { 
                dbPlayer.SetClothes(11, 15, 0);
                dbPlayer.SetClothes(3, 15, 0);
                    dbPlayer.SetClothes(8, 15, 0);
                    return;
                }

                if (arg == "hose")
                {
                    dbPlayer.SetClothes(4, 21, 0);
                return;
            }

                if (arg == "brille")
                {
                    c.SetAccessories(1, 0, 0);
                    return;
                }

                if (arg == "schuhe")
                {
                    dbPlayer.SetClothes(6, 34, 0);
            return;
        }

                if (arg == "anziehen")
                {
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @user LIMIT 1");
                    mySqlQuery.AddParameter("@user", c.Name);

                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    MySqlDataReader reader = mySqlReaderCon.Reader;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PlayerClothes playerClothes = NAPI.Util.FromJson<PlayerClothes>(reader.GetString("Clothes"));

                   // dbPlayer.SetClothes(2, playerClothes.Haare.drawable, playerClothes.Haare.texture);
                    c.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                    c.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                    dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                    dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                    dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                    dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                    dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                    dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                    return;

                        }
                    }
                }
            }
            catch (Exception ex) { }
        }



        [RemoteEvent("Pressed_M")]
        public void Pressed_M(Client c)
        {
            if (c == null) return;
            try
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;
                if (dbPlayer.Client.IsInVehicle) return;
                if (!dbPlayer.CanInteractAntiFlood(2)) return;

                PlayerClothes playerClothes = dbPlayer.PlayerClothes;

                if (dbPlayer.GetData("MASK") == true)
                {
                    dbPlayer.PlayAnimation(49, "missfbi4", "takeoff_mask", 8f);
                    dbPlayer.SetClothes(1, 0, 0);
                    dbPlayer.SetData("MASK", false);
                    NAPI.Task.Run(() => { dbPlayer.StopAnimation(); }, 1000);
                }
                else
                {
                    dbPlayer.PlayAnimation(49, "missfbi4", "takeoff_mask", 8f);
                    dbPlayer.SetData("MASK", true);
                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                    NAPI.Task.Run(() => { dbPlayer.StopAnimation(); }, 1000);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Pressed_M] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_M] " + ex.StackTrace);
            }
        }

        [RemoteEvent("Pressed_E")]
        public void PressedE(Client c)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            KasinoModule.Instance.PressedE(dbPlayer);

            try
            {

                ColShape val = NAPI.Pools.GetAllColShapes().FirstOrDefault((ColShape col) => col.IsPointWithin(c.Position));
                if (!(val != null) || (val.Dimension != uint.MaxValue) && (c.Dimension != val.Dimension))
                {
                    return;
                }

                FunctionModel functionModel = val.GetData("FUNCTION_MODEL");
                if (functionModel != null)
                {
                    if (functionModel.Arg1 != null && functionModel.Arg2 != null)
                    {
                        c.Eval("mp.events.callRemote('" + functionModel.Function + "', '" + functionModel.Arg1 + "', '" + functionModel.Arg2 + "');");
                    }
                    else if (functionModel.Arg2 == null && functionModel.Arg1 != null)
                    {
                        c.Eval("mp.events.callRemote('" + functionModel.Function + "', '" + functionModel.Arg1 + "');");
                    }
                    else
                    {
                        c.Eval("mp.events.callRemote('" + functionModel.Function + "');");
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION pressedE] " + ex.Message);
                Logger.Print("[EXCEPTION pressedE] " + ex.StackTrace);
            }
        }


        [ServerEvent(Event.PlayerEnterColshape)]
        public void onEnterColshape(ColShape colShape, Client player)
        {
            if (player == null || colShape == null) return;
            DbPlayer dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                if (colShape.HasData("MESSAGE"))
                {
                    Message message = colShape.GetData("MESSAGE");

                    if (message.Color == "frak")
                        message.Color = dbPlayer.Faction.GetRGBStr();

                    if (message.Title == "frak")
                        message.Title = dbPlayer.Faction.Name;

                    dbPlayer.SendNotification(message.Text, message.Duration, message.Color, message.Title);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION enterColshape] " + ex.Message);
                Logger.Print("[EXCEPTION enterColshape] " + ex.StackTrace);
            }
        }

        [RemoteEvent("Pressed_H")]
        public static void handsUp(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.CanInteractAntiDeath())
                {

                    if (client.IsInVehicle) return;

                    if (dbPlayer.HasData("handsup"))
                    {
                        dbPlayer.PlayAnimation(49, "missfbi5ig_21", "hand_up_scientist");
                        dbPlayer.ResetData("handsup");
                    }
                    else
                    {
                        dbPlayer.StopAnimation();
                        dbPlayer.SetData("handsup", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Pressed_H] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_H] " + ex.StackTrace);
            }
        }
    }
}
