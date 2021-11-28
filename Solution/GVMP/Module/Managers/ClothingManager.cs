using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class ClothingManager : Script
    {

        [RemoteEvent("StopCustomization")]
        public void StopCustomization(Client c)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            c.Position = dbPlayer.GetDbLocation();
            c.Rotation = new Vector3(0f, 0f, 327f);
            c.Dimension = 0;
        }

        [RemoteEvent("UpdateCharacterCustomization")]
        public static void UpdateCharacterCustomization(Client client, string json, int price)
        {
            DbPlayer player = client.GetPlayer();
            if ((player == null || !player.IsValid(true) ? false : player.Client != null))
            {
                MySqlConnection mySqlConnection = new MySqlConnection(Configuration.connectionString);
                try
                {
                    try
                    {
                        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Customization = @customization WHERE Id = @playerID");
                        mySqlQuery.AddParameter("@customization", string.Concat("{\"customization\":", json, ",\"level\":0}"));
                        mySqlQuery.AddParameter("@playerID", player.Id);
                        MySqlHandler.ExecuteSync(mySqlQuery);
                        mySqlQuery.Query = "UPDATE accounts SET Clothes = @clothes WHERE Id = @playerID";
                        customization _customization = NAPI.Util.FromJson<customization>(json);
                        PlayerClothes playerClothe = new PlayerClothes();
                        if ((dynamic)player.GetAttributeString("Clothes") == "")
                        {
                            if (!ClothingManager.isMale(client))
                            {
                                playerClothe.Hut = new clothingPart()
                                {
                                    drawable = -1,
                                    texture = 0
                                };
                                client.SetAccessories(0, playerClothe.Hut.drawable, playerClothe.Hut.texture);
                                playerClothe.Brille = new clothingPart()
                                {
                                    drawable = 0,
                                    texture = 0
                                };
                                client.SetAccessories(0, playerClothe.Brille.drawable, playerClothe.Brille.texture);
                                playerClothe.Haare = new clothingPart()
                                {
                                    drawable = _customization.Hair.Hair,
                                    texture = 0
                                };
                                player.SetClothes(2, playerClothe.Haare.drawable, playerClothe.Haare.texture);
                                playerClothe.Maske = new clothingPart()
                                {
                                    drawable = 0,
                                    texture = 0
                                };
                                player.SetClothes(1, playerClothe.Maske.drawable, playerClothe.Maske.texture);
                                playerClothe.Oberteil = new clothingPart()
                                {
                                    drawable = 3,
                                    texture = 0
                                };
                                player.SetClothes(11, playerClothe.Oberteil.drawable, playerClothe.Oberteil.texture);
                                playerClothe.Unterteil = new clothingPart()
                                {
                                    drawable = 14,
                                    texture = 0
                                };
                                player.SetClothes(8, playerClothe.Unterteil.drawable, playerClothe.Unterteil.texture);
                                playerClothe.Kette = new clothingPart()
                                {
                                    drawable = 14,
                                    texture = 0
                                };
                                player.SetClothes(7, playerClothe.Kette.drawable, playerClothe.Kette.texture);
                                playerClothe.Koerper = new clothingPart()
                                {
                                    drawable = 3,
                                    texture = 0
                                };
                                player.SetClothes(3, playerClothe.Koerper.drawable, playerClothe.Koerper.texture);
                                playerClothe.Hose = new clothingPart()
                                {
                                    drawable = 11,
                                    texture = 0
                                };
                                player.SetClothes(4, playerClothe.Hose.drawable, playerClothe.Hose.texture);
                                playerClothe.Schuhe = new clothingPart()
                                {
                                    drawable = 1,
                                    texture = 1
                                };
                                player.SetClothes(6, playerClothe.Schuhe.drawable, playerClothe.Schuhe.texture);
                            }
                            else
                            {
                                playerClothe.Hut = new clothingPart()
                                {
                                    drawable = -1,
                                    texture = 0
                                };
                                client.SetAccessories(0, playerClothe.Hut.drawable, playerClothe.Hut.texture);
                                playerClothe.Brille = new clothingPart()
                                {
                                    drawable = 0,
                                    texture = 0
                                };
                                client.SetAccessories(0, playerClothe.Brille.drawable, playerClothe.Brille.texture);
                                playerClothe.Haare = new clothingPart()
                                {
                                    drawable = _customization.Hair.Hair,
                                    texture = 0
                                };
                                player.SetClothes(2, playerClothe.Haare.drawable, playerClothe.Haare.texture);
                                playerClothe.Maske = new clothingPart()
                                {
                                    drawable = 0,
                                    texture = 0
                                };
                                player.SetClothes(1, playerClothe.Maske.drawable, playerClothe.Maske.texture);
                                playerClothe.Oberteil = new clothingPart()
                                {
                                    drawable = 1,
                                    texture = 0
                                };
                                player.SetClothes(11, playerClothe.Oberteil.drawable, playerClothe.Oberteil.texture);
                                playerClothe.Unterteil = new clothingPart()
                                {
                                    drawable = 15,
                                    texture = 0
                                };
                                player.SetClothes(8, playerClothe.Unterteil.drawable, playerClothe.Unterteil.texture);
                                playerClothe.Kette = new clothingPart()
                                {
                                    drawable = 15,
                                    texture = 0
                                };
                                player.SetClothes(7, playerClothe.Kette.drawable, playerClothe.Kette.texture);
                                playerClothe.Koerper = new clothingPart()
                                {
                                    drawable = 0,
                                    texture = 0
                                };
                                player.SetClothes(3, playerClothe.Koerper.drawable, playerClothe.Koerper.texture);
                                playerClothe.Hose = new clothingPart()
                                {
                                    drawable = 5,
                                    texture = 0
                                };
                                player.SetClothes(4, playerClothe.Hose.drawable, playerClothe.Hose.texture);
                                playerClothe.Schuhe = new clothingPart()
                                {
                                    drawable = 1,
                                    texture = 0
                                };
                                player.SetClothes(6, playerClothe.Schuhe.drawable, playerClothe.Schuhe.texture);
                            }
                            mySqlQuery.Parameters.Clear();
                            mySqlQuery.AddParameter("@clothes", NAPI.Util.ToJson(playerClothe));
                            mySqlQuery.AddParameter("@playerID", player.Id);
                            MySqlHandler.ExecuteSync(mySqlQuery);
                            player.PlayerClothes = playerClothe;
                        }
                        player.RefreshData(player);
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        Logger.Print(string.Concat("[EXCEPTION CharCreator] ", exception.Message));
                        Logger.Print(string.Concat("[EXCEPTION CharCreator] ", exception.StackTrace));
                    }
                }
                finally
                {
                    mySqlConnection.Dispose();
                }
                player.Position = player.GetDbLocation();
                client.Rotation = new Vector3(0f, 0f, 327f);
                player.Dimension = 0;
                WeaponManager.loadWeapons(player.Client);
                NAPI.Task.Run(new Action(() => {
                    if (NAPI.Pools.GetAllPlayers().Contains(client))
                    {
                        ClothingManager.loadCharacter(client);
                    }
                }), (long)1000);
            }
        }

        public static HeadOverlay CreateHeadOverlay(byte index, byte color, byte secondaryColor, float opacity)
        {
            HeadOverlay result = default(HeadOverlay);
            result.Index = index;
            result.Color = color;
            result.SecondaryColor = secondaryColor;
            result.Opacity = opacity;
            return result;
        }

        public static void loadCharacter(Client c)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Id = @userId LIMIT 1");
            mySqlQuery.AddParameter("@userId", dbPlayer.Id);
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
            try
            {
                PlayerClothes playerClothes = null;
                MySqlDataReader reader = mySqlResult.Reader;
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CustomizeModel customizeModel = NAPI.Util.FromJson<CustomizeModel>(reader.GetString("Customization"));
                            Dictionary<int, HeadOverlay> dictionary = new Dictionary<int, HeadOverlay>();
                            dictionary.Add(2, CreateHeadOverlay(2, (byte)customizeModel.customization.Appearance[2].Value, 0, customizeModel.customization.Appearance[2].Opacity));
                            dictionary.Add(3, CreateHeadOverlay(3, (byte)customizeModel.customization.Appearance[3].Value, 0, customizeModel.customization.Appearance[3].Opacity));
                            dictionary.Add(4, CreateHeadOverlay(4, (byte)customizeModel.customization.Appearance[4].Value, 0, customizeModel.customization.Appearance[4].Opacity));
                            dictionary.Add(5, CreateHeadOverlay(5, (byte)customizeModel.customization.Appearance[5].Value, 0, customizeModel.customization.Appearance[5].Opacity));
                            dictionary.Add(8, CreateHeadOverlay(8, (byte)customizeModel.customization.Appearance[8].Value, 0, customizeModel.customization.Appearance[8].Opacity));
                            HeadBlend val4 = default(HeadBlend);
                            val4.ShapeFirst = (byte)customizeModel.customization.Parents.MotherShape;
                            val4.ShapeSecond = (byte)customizeModel.customization.Parents.FatherShape;
                            val4.ShapeThird = 0;
                            val4.SkinFirst = (byte)customizeModel.customization.Parents.MotherSkin;
                            val4.SkinSecond = (byte)customizeModel.customization.Parents.FatherSkin;
                            val4.SkinThird = 0;
                            val4.ShapeMix = customizeModel.customization.Parents.Similarity;
                            val4.SkinMix = customizeModel.customization.Parents.SkinSimilarity;
                            val4.ThirdMix = 0f;
                            HeadBlend val5 = val4;
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

                            c.SetCustomization(isMale(c), val5, (byte)customizeModel.customization.EyeColor, (byte)customizeModel.customization.Hair.Color, (byte)customizeModel.customization.Hair.HighlightColor, customizeModel.customization.Features.ToArray(), dictionary, list.ToArray());
                            dbPlayer.SetClothes(2, customizeModel.customization.Hair.Hair, 0);
                            playerClothes = NAPI.Util.FromJson<PlayerClothes>(reader.GetString("Clothes"));
                            if (playerClothes == null) return;

                            if (playerClothes.Hut != null)
                                c.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);

                            if (playerClothes.Brille != null)
                                c.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);

                            if (playerClothes.Maske != null)
                                dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);

                            if (playerClothes.Oberteil != null)
                                dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);

                            if (playerClothes.Unterteil != null)
                                dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);

                            if (playerClothes.Kette != null)
                                dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);

                            if (playerClothes.Koerper != null)
                                dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);

                            if (playerClothes.Hose != null)
                                dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);

                            if (playerClothes.Schuhe != null)
                                dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);

                            dbPlayer.PlayerClothes = playerClothes;
                            c.SetData("player", dbPlayer);
                            
                            WeaponManager.loadWeapons(dbPlayer.Client);
                            dbPlayer.Client.RemoveAllWeapons();
                        }
                    }
                }
                finally
                {
                    reader.Dispose();
                }
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }
        }

        public static List<ClothingModel> getClothingDataListMale()
        {
            List<ClothingModel> list = new List<ClothingModel>();

            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM clothingsdatamale";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!list.Contains(new ClothingModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetInt32("Component"), reader.GetInt32("Drawable"), reader.GetInt32("Texture"))))
                                list.Add(new ClothingModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetInt32("Component"), reader.GetInt32("Drawable"), reader.GetInt32("Texture")));
                        }
                    }
                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION getClothingDataListMale] " + ex.Message);
                Logger.Print("[EXCEPTION getClothingDataListMale] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }
            return list;
        }

       /* public static List<TattooModel> getTattooDataListMale()
        {
            List<TattooModel> list = new List<TattooModel>();

            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM tattoodata";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!list.Contains(new TattooModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetString("Collection"), reader.GetString("Overlay"))))
                                list.Add(new TattooModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetString("Collection"), reader.GetString("Overlay")));
                        }
                    }
                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION getClothingDataListMale] " + ex.Message);
                Logger.Print("[EXCEPTION getClothingDataListMale] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }
            return list;
        }*/

        public static List<ClothingModel> getClothingDataListFemale()
        {
            List<ClothingModel> list = new List<ClothingModel>();

            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM clothingsdatafemale";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!list.Contains(new ClothingModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetInt32("Component"), reader.GetInt32("Drawable"), reader.GetInt32("Texture"))))
                                list.Add(new ClothingModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetInt32("Component"), reader.GetInt32("Drawable"), reader.GetInt32("Texture")));
                        }
                    }
                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION getClothingDataListFemale] " + ex.Message);
                Logger.Print("[EXCEPTION getClothingDataListFemale] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }
            return list;
        }
        
        public static List<ClothingModel> getClothingDataListAdmin()
        {
            List<ClothingModel> list = new List<ClothingModel>();

            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM adminclothes";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!list.Contains(new ClothingModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetInt32("Component"), reader.GetInt32("Drawable"), reader.GetInt32("Texture"))))
                                list.Add(new ClothingModel(reader.GetString("Name"), reader.GetString("Category"), reader.GetInt32("Component"), reader.GetInt32("Drawable"), reader.GetInt32("Texture")));
                        }
                    }
                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION getClothingDataListAdmin] " + ex.Message);
                Logger.Print("[EXCEPTION getClothingDataListAdmin] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }
            return list;
        }

        public static PlayerClothes getClothing(Client c)
        {
            PlayerClothes playerClothes = new PlayerClothes();

            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @user");
            mySqlQuery.AddParameter("@user", c.Name);
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
                            var clothes = reader.GetString("Clothes");

                            if (clothes != "[]")
                            {
                                playerClothes = NAPI.Util.FromJson<PlayerClothes>(clothes);
                                return playerClothes;
                            }
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
                Logger.Print("[EXCEPTION getClothing] " + ex.Message);
                Logger.Print("[EXCEPTION getClothing] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            return playerClothes;
        }

        public static void setClothes(Client c, PlayerClothes playerClothes)
        {
            try
            {
                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Clothes = @val WHERE Username = @username");
                mySqlQuery.AddParameter("@username", c.Name);
                mySqlQuery.AddParameter("@val", NAPI.Util.ToJson(playerClothes));
                MySqlHandler.ExecuteSync(mySqlQuery);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION setClothes] " + ex.Message);
                Logger.Print("[EXCEPTION setClothes] " + ex.StackTrace);
            }
        }

        [RemoteEvent("changeGender")]
        public static void changeGender(Client c, bool female)
        {
            try
            {
                string model = "male";

                if(female)
                {
                    model = "female";
                }

                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Gender = @val WHERE Username = @username");
                mySqlQuery.AddParameter("@username", c.Name);
                mySqlQuery.AddParameter("@val", model);
                MySqlHandler.ExecuteSync(mySqlQuery);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION changeGender] " + ex.Message);
                Logger.Print("[EXCEPTION changeGender] " + ex.StackTrace);
            }
        }

        public static bool isMale(Client c)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @user");
            mySqlQuery.AddParameter("@user", c.Name);
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
                            string gender = reader.GetString("Gender");
                            if (gender == "male")
                                return true;

                            if (gender == "female") {
                                return false;
                            }
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
                Logger.Print("[EXCEPTION isMale] " + ex.Message);
                Logger.Print("[EXCEPTION isMale] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            return true;
        }
    }
}



/*[RemoteEvent("UpdateCharacterCustomization")]
public static void UpdateCharacterCustomization(Client client, string json, int price)
{
    DbPlayer dbPlayer = client.GetPlayer();
    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
        return;

    MySqlConnection con = new MySqlConnection(Configuration.connectionString);
    try
    {
        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Customization = @customization WHERE Id = @playerID");
        mySqlQuery.AddParameter("@customization", "{\"customization\":" + json + ",\"level\":0}");
        mySqlQuery.AddParameter("@playerID", dbPlayer.Id);
        MySqlHandler.ExecuteSync(mySqlQuery);
        mySqlQuery.Query = "UPDATE accounts SET Clothes = @clothes WHERE Id = @playerID";

        customization customization = NAPI.Util.FromJson<customization>(json);
        PlayerClothes playerClothes = new PlayerClothes();
        if (dbPlayer.GetAttributeString("Clothes") == "")
        {
            if (isMale(client))
            {
                playerClothes.Hut = new clothingPart
                {
                    drawable = -1,
                    texture = 0
                };
                client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                playerClothes.Brille = new clothingPart
                {
                    drawable = -1,
                    texture = 0
                };
                client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                playerClothes.Haare = new clothingPart
                {
                    drawable = customization.Hair.Hair,
                    texture = 0
                };
                dbPlayer.SetClothes(2, playerClothes.Haare.drawable, playerClothes.Haare.texture);
                playerClothes.Maske = new clothingPart
                {
                    drawable = 0,
                    texture = 0
                };
                dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                playerClothes.Oberteil = new clothingPart
                {
                    drawable = 1,
                    texture = 0
                };
                dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                playerClothes.Unterteil = new clothingPart
                {
                    drawable = 15,
                    texture = 0
                };
                dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                playerClothes.Kette = new clothingPart
                {
                    drawable = 15,
                    texture = 0
                };
                dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                playerClothes.Koerper = new clothingPart
                {
                    drawable = 0,
                    texture = 0
                };
                dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                playerClothes.Hose = new clothingPart
                {
                    drawable = 5,
                    texture = 0
                };
                dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                playerClothes.Schuhe = new clothingPart
                {
                    drawable = 1,
                    texture = 0
                };
                dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
            }
            else
            {
                playerClothes.Hut = new clothingPart
                {
                    drawable = -1,
                    texture = 0
                };
                client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                playerClothes.Brille = new clothingPart
                {
                    drawable = -1,
                    texture = 0
                };
                client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                playerClothes.Haare = new clothingPart
                {
                    drawable = customization.Hair.Hair,
                    texture = 0
                };
                dbPlayer.SetClothes(2, playerClothes.Haare.drawable, playerClothes.Haare.texture);
                playerClothes.Maske = new clothingPart
                {
                    drawable = 0,
                    texture = 0
                };
                dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                playerClothes.Oberteil = new clothingPart
                {
                    drawable = 3,
                    texture = 0
                };
                dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                playerClothes.Unterteil = new clothingPart
                {
                    drawable = 14,
                    texture = 0
                };
                dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                playerClothes.Kette = new clothingPart
                {
                    drawable = 15,
                    texture = 0
                };
                dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                playerClothes.Koerper = new clothingPart
                {
                    drawable = 3,
                    texture = 0
                };
                dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                playerClothes.Hose = new clothingPart
                {
                    drawable = 11,
                    texture = 0
                };
                dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                playerClothes.Schuhe = new clothingPart
                {
                    drawable = 1,
                    texture = 1
                };
                dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
            }

            mySqlQuery.Parameters.Clear();
            mySqlQuery.AddParameter("@clothes", NAPI.Util.ToJson(playerClothes));
            mySqlQuery.AddParameter("@playerID", dbPlayer.Id);
            MySqlHandler.ExecuteSync(mySqlQuery);
            dbPlayer.PlayerClothes = playerClothes;
        }
        dbPlayer.RefreshData(dbPlayer);
    }
    catch (Exception ex)
    {
        Logger.Print("[EXCEPTION CharCreator] " + ex.Message);
        Logger.Print("[EXCEPTION CharCreator] " + ex.StackTrace);
    }
    finally
    {
        con.Dispose();
    }
    client.Position = dbPlayer.GetDbLocation();
    client.Rotation = new Vector3(0f, 0f, 327f);
    client.Dimension = 0;

    NAPI.Task.Run(() =>
    {
        loadCharacter(client);
    }, 1000);

}*/