using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace GVMP
{
    public class AdminShopModule : GVMP.Module.Module<AdminShopModule>
    {
        public static List<ClothingModel> clothingList = new List<ClothingModel>();

        protected override bool OnLoad()
        {
            Vector3 Position = new Vector3(-74.47, - 817.9, 326.16);
            
            ColShape val = NAPI.ColShape.CreateCylinderColShape(Position, 1.4f, 2.4f, 0);
            val.SetData("FUNCTION_MODEL", new FunctionModel("openAdminShop"));
            val.SetData("MESSAGE", new Message("Benutze E um den Kleidungsladen zu öffnen.", "ADMINSHOP", "red", 3000));

            NAPI.Marker.CreateMarker(1, Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 0, 0), false, 0);

            clothingList = ClothingManager.getClothingDataListAdmin();

            return true;
        }

        [RemoteEvent("openAdminShop")]
        public static void openAdminShop(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.Adminrank.Permission > 93)
                {
                    dbPlayer.ShowNativeMenu(new NativeMenu("Adminshop", "", new List<NativeItem>()
                    {
                        new NativeItem("Maske", "Maske"),
                        new NativeItem("Hüte", "Hüte"),
                        new NativeItem("Oberteil", "Oberteil"),
                        new NativeItem("Unterteil", "Unterteil"),
                        new NativeItem("Koerper", "Koerper"),
                        new NativeItem("Hose", "Hose"),
                        new NativeItem("Schuhe", "Schuhe")
                    }));
                }
                else
                {
                    dbPlayer.SendNotification("Kein Zugriff!", 3000, "red");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openAdminShop] " + ex.Message);
                Logger.Print("[EXCEPTION openAdminShop] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Adminshop")]
        public static void Adminshop(Client client, string selection)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (selection == null)
                    return;

                if (dbPlayer.Adminrank.Permission > 93)
                {
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

                        foreach (ClothingModel clothing in clothingList)
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

                        dbPlayer.CloseNativeMenu();
                        dbPlayer.ShowNativeMenu(new NativeMenu("Kleidungsauswahl", selection, Items));
                    }
                    catch (Exception ex)
                    {
                        Logger.Print("[EXCEPTION nM-Adminshop] " + ex.Message);
                        Logger.Print("[EXCEPTION nM-Adminshop] " + ex.StackTrace);
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Kein Zugriff!", 3000, "red");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Adminshop] " + ex.Message);
                Logger.Print("[EXCEPTION Adminshop] " + ex.StackTrace);
            }
        }

    }
}