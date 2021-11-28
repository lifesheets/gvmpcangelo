using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace GVMP
{
    class ClothingShopModule : GVMP.Module.Module<ClothingShopModule>
    {
        public static List<ClothingShop> clothingshopList = new List<ClothingShop>();
        public static List<ClothingModel> clothingListMale = new List<ClothingModel>();
        public static List<ClothingModel> clothingListFemale = new List<ClothingModel>();

        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM clothingshops");
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
            try
            {
                MySqlDataReader reader = mySqlResult.Reader;
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string Name = reader.GetString("Name");
                            Vector3 Position = NAPI.Util.FromJson<Vector3>(reader.GetString("Position"));
                            clothingshopList.Add(new ClothingShop
                            {
                                Id = reader.GetInt32("Id"),
                                Name = Name,
                                Position = Position
                            });

                            ColShape val = NAPI.ColShape.CreateCylinderColShape(Position, 1.4f, 2.4f, 0);
                            val.SetData("FUNCTION_MODEL", new FunctionModel("openClothingShop", Name));
                            val.SetData("MESSAGE", new Message("Benutze E um den Kleidungsladen zu öffnen.", Name, "orange", 3000));

                            NAPI.Marker.CreateMarker(1, Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, 0);
                            NAPI.Blip.CreateBlip(73, Position, 1f, 0, Name, 255, 0.0f, true, 0, 0);
                        }
                    }
                }
                finally
                {
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadClothingShops] " + ex.Message);
                Logger.Print("[EXCEPTION loadClothingShops] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            clothingListMale = ClothingManager.getClothingDataListMale();
            clothingListFemale = ClothingManager.getClothingDataListFemale();

            return true;
        }

        [RemoteEvent("openClothingShop")]
        public static void openClothingShop(Client client, string name)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (name == null)
                    return;

                dbPlayer.ShowNativeMenu(new NativeMenu("Kleiderladen", name.Replace("Kleiderladen ", ""),
                    new List<NativeItem>()
                    {
                        new NativeItem("Maske", "Maske"),
                        new NativeItem("Hüte", "Hüte"),
                        new NativeItem("Brillen", "Brillen"),
                        new NativeItem("Oberteil", "Oberteil"),
                        new NativeItem("Unterteil", "Unterteil"),
                        new NativeItem("Kette", "Kette"),
                        new NativeItem("Koerper", "Koerper"),
                        new NativeItem("Hose", "Hose"),
                        new NativeItem("Schuhe", "Schuhe")
                    }));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openClothingShop] " + ex.Message);
                Logger.Print("[EXCEPTION openClothingShop] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Kleiderladen")]
        public static void Kleiderladen(Client client, string selection)
        {
            if (client == null) return;
            DbPlayer dbPlayer = client.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            if (selection == null)
                return;

            try
            {
                List<NativeItem> Items = new List<NativeItem>();

                if (selection == "Maske")
                {
                    List<NativeItem> nativeItemList = Items;
                    string name = "Keine Maske";
                    string[] strArray = new string[7];
                    strArray[0] = selection;
                    strArray[1] = "-";
                    int num = 1;
                    strArray[2] = num.ToString();
                    strArray[3] = "-";
                    num = 0;
                    strArray[4] = num.ToString();
                    strArray[5] = "-";
                    num = 0;
                    strArray[6] = num.ToString();
                    string selectionName = string.Concat(strArray);
                    NativeItem nativeItem = new NativeItem(name, selectionName);
                    nativeItemList.Add(nativeItem);
                }

                if (selection == "Hüte")
                {
                    List<NativeItem> nativeItemList = Items;
                    string name = "Kein Hut";
                    string[] strArray = new string[7];
                    strArray[0] = selection;
                    strArray[1] = "-";
                    strArray[2] = "1";
                    strArray[3] = "-";
                    strArray[4] = "500";
                    strArray[5] = "-";
                    strArray[6] = "0";
                    string selectionName = string.Concat(strArray);
                    NativeItem nativeItem = new NativeItem(name, selectionName);
                    nativeItemList.Add(nativeItem);
                }
                if (client.Model == 1885233650)
                {
                    foreach (ClothingModel clothing in clothingListMale)
                    {
                        if (clothing.category == selection)
                        {
                            List<NativeItem> nativeItemList = Items;
                            string name = clothing.name;
                            string[] strArray = new string[7];
                            strArray[0] = selection;
                            strArray[1] = "-";
                            int num = clothing.component;
                            strArray[2] = num.ToString();
                            strArray[3] = "-";
                            num = clothing.drawable;
                            strArray[4] = num.ToString();
                            strArray[5] = "-";
                            num = clothing.texture;
                            strArray[6] = num.ToString();
                            string selectionName = string.Concat(strArray);
                            NativeItem nativeItem = new NativeItem(name, selectionName);
                            nativeItemList.Add(nativeItem);
                        }
                    }
                }
                else
                {
                    foreach (ClothingModel clothing in clothingListFemale)
                    {
                        if (clothing.category == selection)
                        {
                            List<NativeItem> nativeItemList = Items;
                            string name = clothing.name;
                            string[] strArray = new string[7];
                            strArray[0] = selection;
                            strArray[1] = "-";
                            int num = clothing.component;
                            strArray[2] = num.ToString();
                            strArray[3] = "-";
                            num = clothing.drawable;
                            strArray[4] = num.ToString();
                            strArray[5] = "-";
                            num = clothing.texture;
                            strArray[6] = num.ToString();
                            string selectionName = string.Concat(strArray);
                            NativeItem nativeItem = new NativeItem(name, selectionName);
                            nativeItemList.Add(nativeItem);
                        }
                    }
                }
                dbPlayer.CloseNativeMenu();
                dbPlayer.ShowNativeMenu(new NativeMenu("Kleidungsauswahl", selection, Items));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION nM-Kleiderladen] " + ex.Message);
                Logger.Print("[EXCEPTION nM-Kleiderladen] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Kleidungsauswahl")]
        public static void Kleidungsauswahl(Client client, string selection)
        {
            if (client == null) return;
            DbPlayer dbPlayer = client.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            if (selection == null)
                return;

            try
            {
                string[] strArray = selection.Split("-");

                PlayerClothes playerClothes = dbPlayer.PlayerClothes;

                if (strArray[0] == "Maske")
                {
                    playerClothes.Maske = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                }
                else if (strArray[0] == "Hüte")
                {
                    if (Convert.ToInt32(strArray[2]) == 500)
                    {
                        playerClothes.Hut = new clothingPart()
                        {
                            drawable = -1,
                            texture = 0
                        };
                        client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                    }
                    else
                    {
                        playerClothes.Hut = new clothingPart()
                        {
                            drawable = Convert.ToInt32(strArray[2]),
                            texture = Convert.ToInt32(strArray[3])
                        };
                        client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                    }
                }
                 else if (strArray[0] == "Brillen")
                {
                    if (Convert.ToInt32(strArray[2]) == 500)
                    {
                        playerClothes.Brille = new clothingPart()
                        {
                            drawable = -1,
                            texture = 0
                        };
                        client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                    }
                    else
                    {
                        playerClothes.Brille = new clothingPart()
                        {
                            drawable = Convert.ToInt32(strArray[2]),
                            texture = Convert.ToInt32(strArray[3])
                        };
                        client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                    }
                }
                else if (strArray[0] == "Oberteil")
                {
                    playerClothes.Oberteil = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                }
                else if (strArray[0] == "Unterteil")
                {
                    playerClothes.Unterteil = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                }
                else if (strArray[0] == "Kette")
                {
                    playerClothes.Kette = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                }
                else if (strArray[0] == "Koerper")
                {
                    playerClothes.Koerper = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                }
                else if (strArray[0] == "Hose")
                {
                    playerClothes.Hose = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                }
                else if (strArray[0] == "Schuhe")
                {
                    playerClothes.Schuhe = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                }

                dbPlayer.PlayerClothes = playerClothes;
                dbPlayer.RefreshData(dbPlayer);

                ClothingManager.setClothes(dbPlayer.Client, playerClothes);

            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION nM-Kleidungsauswahl] " + ex.Message);
                Logger.Print("[EXCEPTION nM-Kleidungsauswahl] " + ex.StackTrace);
            }
        }
    }
}
