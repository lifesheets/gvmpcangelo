using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
   public class DonatorShopModule : GVMP.Module.Module<DonatorShopModule>
    {
        public static List<ClothingModel> clothingList;

        static DonatorShopModule()
        {
            DonatorShopModule.clothingList = new List<ClothingModel>();
        }

        public DonatorShopModule()
        {
        }

        [RemoteEvent("nM-Donatorshop")]
        public static void Donatorshop(Client client, string selection)
        {
            try
            {
                if (client != null)
                {
                    DbPlayer player = client.GetPlayer();
                    if ((player == null || !player.IsValid(true) ? false : player.Client != null))
                    {
                        if (player.CanInteractAntiFlood(1))
                        {
                            if (selection != null)
                            {
                                if ((dynamic)player.GetAttributeInt("Donator") != 1)
                                {
                                    player.SendNotification("Kein Zugriff!", 3000, "red", "");
                                }
                                else
                                {
                                    try
                                    {
                                        List<NativeItem> list = new List<NativeItem>();
                                        if (selection == "Maske")
                                        {
                                            List<NativeItem> list1 = list;
                                            string str = "Keine Maske";
                                            string[] strArray = new string[] { selection, "-", 1.ToString(), "-", 0.ToString(), "-", 0.ToString() };
                                            list1.Add(new NativeItem(str, string.Concat(strArray)));
                                        }
                                        if (selection == "Hüte")
                                        {
                                            List<NativeItem> list2 = list;
                                            string str1 = "Kein Hut";
                                            string[] strArray1 = new string[] { selection, "-", "1", "-", "500", "-", "0" };
                                            list2.Add(new NativeItem(str1, string.Concat(strArray1)));
                                        }
                                        foreach (ClothingModel clothingModel in DonatorShopModule.clothingList)
                                        {
                                            if (clothingModel.category == selection)
                                            {
                                                List<NativeItem> list3 = list;
                                                string str2 = clothingModel.name;
                                                string[] strArray2 = new string[] { selection, "-", default(string), default(string), default(string), default(string), default(string) };
                                                int num = clothingModel.component;
                                                strArray2[2] = num.ToString();
                                                strArray2[3] = "-";
                                                num = clothingModel.drawable;
                                                strArray2[4] = num.ToString();
                                                strArray2[5] = "-";
                                                num = clothingModel.texture;
                                                strArray2[6] = num.ToString();
                                                list3.Add(new NativeItem(str2, string.Concat(strArray2)));
                                            }
                                        }
                                        player.CloseNativeMenu();
                                        player.ShowNativeMenu(new NativeMenu("Kleidungsauswahl", selection, list));
                                    }
                                    catch (Exception exception1)
                                    {
                                        Exception exception = exception1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
            }
        }

        protected override bool OnLoad()
        {
            Vector3 vector3 = new Vector3(448.96, - 979.21, 42.73);
            ColShape colShape = NAPI.ColShape.CreateCylinderColShape(vector3, 1.4f, 2.4f, 0);
            colShape.SetData("FUNCTION_MODEL", new FunctionModel("openDonatorShop"));
            colShape.SetData("MESSAGE", new Message("Benutze E um den Kleidungsladen zu öffnen.", "DONATORSHOP", "lightblue", 3000));
            NAPI.Marker.CreateMarker(1, vector3, new Vector3(), new Vector3(), 1f, new Color(173, 216, 230), false, 0);
            DonatorShopModule.clothingList = ClothingManager.getClothingDataListAdmin();
            return true;
        }

        [RemoteEvent("openDonatorShop")]
        public static void openDonatorShop(Client client)
        {
            try
            {
                if (client != null)
                {
                    DbPlayer player = client.GetPlayer();
                    if ((player == null || !player.IsValid(true) ? false : player.Client != null))
                    {
                        if (player.CanInteractAntiFlood(1))
                        {
                            if ((dynamic)player.GetAttributeInt("Donator") != 1)
                            {
                                player.SendNotification("Kein Zugriff!", 3000, "red", "");
                            }
                            else
                            {
                                List<NativeItem> list = new List<NativeItem>();
                                list.Add(new NativeItem("Maske", "Maske"));
                                list.Add(new NativeItem("Hüte", "Hüte"));
                                list.Add(new NativeItem("Oberteil", "Oberteil"));
                                list.Add(new NativeItem("Unterteil", "Unterteil"));
                                list.Add(new NativeItem("Koerper", "Koerper"));
                                list.Add(new NativeItem("Hose", "Hose"));
                                list.Add(new NativeItem("Schuhe", "Schuhe"));
                                player.ShowNativeMenu(new NativeMenu("Donatorshop", "", list));
                            }
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
            }
        }
    }
}