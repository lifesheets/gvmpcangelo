using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class BarberModule : GVMP.Module.Module<BarberModule>
    {
        public static List<BarberShop> barberShopList = new List<BarberShop>();

        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM barbershops");
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
                            barberShopList.Add(new BarberShop
                            {
                                Name = Name,
                                Position = Position
                            });

                            ColShape val = NAPI.ColShape.CreateCylinderColShape(Position, 1.4f, 2.4f, 0);
                            val.SetData("FUNCTION_MODEL", new FunctionModel("openBarberShop"));
                            val.SetData("MESSAGE", new Message("Benutze E um dir die Haare schneiden zu lassen.", Name, "orange", 3000));

                            NAPI.Marker.CreateMarker(1, Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, 0);
                            NAPI.Blip.CreateBlip(71, Position, 1f, 0, Name, 255, 0.0f, true, 0, 0);
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
                Logger.Print("[EXCEPTION loadBarberShops] " + ex.Message);
                Logger.Print("[EXCEPTION loadBarberShops] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            return true;
        }

        [RemoteEvent("openBarberShop")]
        public void openBarberShop(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                CustomizeModel customizeModel =
                    NAPI.Util.FromJson<CustomizeModel>(dbPlayer.GetAttributeString("Customization"));
                if (customizeModel == null) return;

                BarberPlayerObject barberPlayerObject = new BarberPlayerObject
                {
                    Hair = customizeModel.customization.Hair.Hair,
                    HairColor = customizeModel.customization.Hair.Color,
                    HairColor2 = customizeModel.customization.Hair.HighlightColor,
                    Beard = customizeModel.customization.Appearance[1].Value,
                    BeardColor = customizeModel.customization.BeardColor,
                    BeardOpacity = customizeModel.customization.Appearance[2].Opacity,
                    ChestHairColor = 0,
                    Chest = 0,
                    ChestHairOpacity = 0
                };

                BarberObject barberObject = new BarberObject
                {
                    Barber = new ListJsonBarberObject
                    {
                        Beards = new List<JsonBarberObject>
                        {
                            new JsonBarberObject
                            {
                                Id = 1,
                                CustomizationId = 0,
                                Name = "Rasierter Bart",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 2,
                                CustomizationId = 1,
                                Name = "Mundbart",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 3,
                                CustomizationId = 2,
                                Name = "Mundbart 2",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 4,
                                CustomizationId = 3,
                                Name = "Mundbart 3",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 5,
                                CustomizationId = 4,
                                Name = "Kinnbart",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 6,
                                CustomizationId = 7,
                                Name = "Leichter Bart",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 7,
                                CustomizationId = 8,
                                Name = "Italiener Bart",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 8,
                                CustomizationId = 9,
                                Name = "Mero Bart",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 9,
                                CustomizationId = 10,
                                Name = "Vollbart",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 10,
                                CustomizationId = 17,
                                Name = "Retard",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 11,
                                CustomizationId = 19,
                                Name = "Schnäutzer",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 12,
                                CustomizationId = 21,
                                Name = "Schnäutzer 2",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 13,
                                CustomizationId = 27,
                                Name = "Affenbart",
                                Price = 2000
                            }

                        },
                        Hairs = new List<JsonBarberObject>
                        {
                            new JsonBarberObject
                            {
                                Id = 1,
                                CustomizationId = 0,
                                Name = "Glatze",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 2,
                                CustomizationId = 1,
                                Name = "Kurz",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 3,
                                CustomizationId = 3,
                                Name = "Kurz Seiten auf 0",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 4,
                                CustomizationId = 2,
                                Name = "Oben Hoch",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 5,
                                CustomizationId = 10,
                                Name = "Mittellang",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 6,
                                CustomizationId = 12,
                                Name = "Seiten auf 10",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 7,
                                CustomizationId = 13,
                                Name = "Seiten auf 100",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 8,
                                CustomizationId = 14,
                                Name = "Dreadlocks",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 9,
                                CustomizationId = 15,
                                Name = "Mädchen",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 10,
                                CustomizationId = 16,
                                Name = "Wuschel",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 11,
                                CustomizationId = 18,
                                Name = "Sidecut",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 12,
                                CustomizationId = 19,
                                Name = "Gepflegt",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 13,
                                CustomizationId = 20,
                                Name = "Ungepflegt",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 14,
                                CustomizationId = 21,
                                Name = "Fresh",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 15,
                                CustomizationId = 22,
                                Name = "Rocker",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 16,
                                CustomizationId = 24,
                                Name = "Kurz",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 17,
                                CustomizationId = 30,
                                Name = "Retard",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 18,
                                CustomizationId = 33,
                                Name = "Seiten auf 0 Haare zur Seite",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 19,
                                CustomizationId = 34,
                                Name = "Irokese",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 20,
                                CustomizationId = 41,
                                Name = "Opa",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 21,
                                CustomizationId = 43,
                                Name = "Dutt",
                                Price = 2000
                            }
                        },
                        Colors = new List<JsonBarberObject>
                        {
                            new JsonBarberObject
                            {
                                Id = 1,
                                CustomizationId = 0,
                                Name = "Schwarz",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 2,
                                CustomizationId = 6,
                                Name = "Braun",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 3,
                                CustomizationId = 14,
                                Name = "Blond",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 4,
                                CustomizationId = 48,
                                Name = "Orange",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 5,
                                CustomizationId = 19,
                                Name = "Dunkelrot",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 6,
                                CustomizationId = 26,
                                Name = "Grau",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 7,
                                CustomizationId = 29,
                                Name = "Weiß",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 8,
                                CustomizationId = 32,
                                Name = "Lila",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 9,
                                CustomizationId = 33,
                                Name = "Pink",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 10,
                                CustomizationId = 35,
                                Name = "Rosa",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 11,
                                CustomizationId = 38,
                                Name = "Blau",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 12,
                                CustomizationId = 39,
                                Name = "Grün",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 13,
                                CustomizationId = 46,
                                Name = "Gelb",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 14,
                                CustomizationId = 53,
                                Name = "Rot",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 15,
                                CustomizationId = 28,
                                Name = "Dunkelweiß",
                                Price = 2000
                            },
                            new JsonBarberObject
                            {
                                Id = 16,
                                CustomizationId = 27,
                                Name = "Hellgrau",
                                Price = 2000
                            }
                        },
                        Chests = new List<JsonBarberObject>(),
                    },
                    Player = barberPlayerObject
                };

                dbPlayer.OpenBarberShop(barberObject);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openBarber] " + ex.Message);
                Logger.Print("[EXCEPTION openBarber] " + ex.StackTrace);
            }
        }

        [RemoteEvent("barberShopBuy")]
        public void barberShopBuy(Client c, int price, string json)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                BarberPlayerObject barberPlayerObject = NAPI.Util.FromJson<BarberPlayerObject>(json);
                if (barberPlayerObject == null) return;

                if (dbPlayer.Money <= price)
                {
                    dbPlayer.SendNotification("Du besitzt zu wenig Geld! (" + price.ToDots() + "$)", 3000, "red");
                    dbPlayer.ApplyCharacter();
                    return;
                }

                dbPlayer.removeMoney(price);

                PlayerClothes playerClothes = dbPlayer.PlayerClothes;

                CustomizeModel customizeModel =
                    NAPI.Util.FromJson<CustomizeModel>(dbPlayer.GetAttributeString("Customization"));
                if (customizeModel == null) return;

                playerClothes.Haare.drawable = barberPlayerObject.Hair;
                playerClothes.Haare.texture = barberPlayerObject.HairColor;
                dbPlayer.PlayerClothes = playerClothes;
                dbPlayer.RefreshData(dbPlayer);

                ClothingManager.setClothes(dbPlayer.Client, playerClothes);

                customizeModel.customization.Hair.Hair = barberPlayerObject.Hair;
                customizeModel.customization.Hair.Color = barberPlayerObject.HairColor;
                customizeModel.customization.Hair.HighlightColor = barberPlayerObject.HairColor2;

                customizeModel.customization.Appearance[1].Value = barberPlayerObject.Beard;
                customizeModel.customization.Appearance[9].Value = barberPlayerObject.BeardColor;
                customizeModel.customization.Appearance[1].Opacity = barberPlayerObject.BeardOpacity;
                customizeModel.customization.BeardColor = barberPlayerObject.BeardColor;

                dbPlayer.SetAttribute("Customization", NAPI.Util.ToJson(customizeModel));

                Dictionary<int, HeadOverlay> dictionary = new Dictionary<int, HeadOverlay>();
                dictionary.Add(1,
                    ClothingManager.CreateHeadOverlay((byte) customizeModel.customization.Appearance[1].Value,
                        (byte) customizeModel.customization.Appearance[9].Value, 0,
                        customizeModel.customization.Appearance[1].Opacity));
                dictionary.Add(2,
                    ClothingManager.CreateHeadOverlay(2, (byte) customizeModel.customization.Appearance[2].Value, 0,
                        customizeModel.customization.Appearance[2].Opacity));
                dictionary.Add(3,
                    ClothingManager.CreateHeadOverlay(3, (byte) customizeModel.customization.Appearance[3].Value, 0,
                        customizeModel.customization.Appearance[3].Opacity));
                dictionary.Add(4,
                    ClothingManager.CreateHeadOverlay(4, (byte) customizeModel.customization.Appearance[4].Value, 0,
                        customizeModel.customization.Appearance[4].Opacity));
                dictionary.Add(5,
                    ClothingManager.CreateHeadOverlay(5, (byte) customizeModel.customization.Appearance[5].Value, 0,
                        customizeModel.customization.Appearance[5].Opacity));
                dictionary.Add(8,
                    ClothingManager.CreateHeadOverlay(8, (byte) customizeModel.customization.Appearance[8].Value, 0,
                        customizeModel.customization.Appearance[8].Opacity));
                HeadBlend val = default(HeadBlend);
                val.ShapeFirst = (byte) customizeModel.customization.Parents.MotherShape;
                val.ShapeSecond = (byte) customizeModel.customization.Parents.FatherShape;
                val.ShapeThird = 0;
                val.SkinFirst = (byte) customizeModel.customization.Parents.MotherSkin;
                val.SkinSecond = (byte) customizeModel.customization.Parents.FatherSkin;
                val.SkinThird = 0;
                val.ShapeMix = customizeModel.customization.Parents.Similarity;
                val.SkinMix = customizeModel.customization.Parents.SkinSimilarity;
                val.ThirdMix = 0f;
                HeadBlend val2 = val;
                bool flag = customizeModel.customization.Gender == 0;
                List<Decoration> list = new List<Decoration>();
                foreach (uint tattoo in customizeModel.customization.Tattoos)
                {
                    if (AssetsTattooModule.AssetsTattoos.ContainsKey(tattoo))
                    {
                        AssetsTattoo assetsTattoo = AssetsTattooModule.AssetsTattoos[tattoo];
                        Decoration item = default(Decoration);
                        item.Collection = NAPI.Util.GetHashKey(assetsTattoo.Collection);
                        item.Overlay = flag ? NAPI.Util.GetHashKey(assetsTattoo.HashMale) : NAPI.Util.GetHashKey(assetsTattoo.HashFemale);
                        list.Add(item);
                    }
                }
                c.SetCustomization(flag, val2, (byte) customizeModel.customization.EyeColor,
                    (byte) customizeModel.customization.Hair.Color,
                    (byte) customizeModel.customization.Hair.HighlightColor,
                    customizeModel.customization.Features.ToArray(), dictionary,
                    list.ToArray());
                dbPlayer.SetClothes(2, customizeModel.customization.Hair.Hair, 0);
                dbPlayer.SendNotification("Du hast deine Haare geschnitten. -" + price.ToDots() + "$", 3000, "green");
                WeaponManager.loadWeapons(dbPlayer.Client);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION barberShopBuy] " + ex.Message);
                Logger.Print("[EXCEPTION barberShopBuy] " + ex.StackTrace);
            }
        }

        [RemoteEvent("barberShopExit")]
        public void barberShopExit(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                dbPlayer.ApplyCharacter();
                WeaponManager.loadWeapons(dbPlayer.Client);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION barberShopExit] " + ex.Message);
                Logger.Print("[EXCEPTION barberShopExit] " + ex.StackTrace);
            }
        }
    }
}
