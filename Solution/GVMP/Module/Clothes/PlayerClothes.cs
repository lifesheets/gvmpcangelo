using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    public class PlayerClothes : GVMP.Module.Module<PlayerClothes>
    {
        public clothingPart Haare
        {
            get;
            set;
        }

        public clothingPart Hut
        {
            get;
            set;
        }

        public clothingPart Brille
        {
            get;
            set;
        }


        public clothingPart Maske
        {
            get;
            set;
        }

        public clothingPart Oberteil
        {
            get;
            set;
        }

        public clothingPart Unterteil
        {
            get;
            set;
        }

        public clothingPart Kette
        {
            get;
            set;
        }

        public clothingPart Koerper
        {
            get;
            set;
        }

        public clothingPart Hose
        {
            get;
            set;
        }

        public clothingPart Schuhe
        {
            get;
            set;
        }

        public static void SetCaillou(DbPlayer dbPlayer)
        {
            try
            {
                if (dbPlayer == null) return;
                PlayerClothes playerClothes = new PlayerClothes();
                playerClothes.Haare = new clothingPart()
                {
                    drawable = 0,
                    texture = 0
                };
                dbPlayer.SetClothes(2, playerClothes.Haare.drawable, playerClothes.Haare.texture);
                playerClothes.Hut = new clothingPart()
                {
                    drawable = -1,
                    texture = 0
                };
                dbPlayer.Client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                playerClothes.Brille = new clothingPart()
                {
                    drawable = 0,
                    texture = 0
                };
                dbPlayer.Client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                playerClothes.Maske = new clothingPart()
                {
                    drawable = 0,
                    texture = 0
                };
                dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                playerClothes.Oberteil = new clothingPart()
                {
                    drawable = 1,
                    texture = 0
                };
                dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                playerClothes.Unterteil = new clothingPart()
                {
                    drawable = 15,
                    texture = 0
                };
                dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                playerClothes.Kette = new clothingPart()
                {
                    drawable = 14,
                    texture = 0
                };
                dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                playerClothes.Koerper = new clothingPart()
                {
                    drawable = 0,
                    texture = 0
                };
                dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                playerClothes.Hose = new clothingPart()
                {
                    drawable = 5,
                    texture = 0
                };
                dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                playerClothes.Schuhe = new clothingPart()
                {
                    drawable = 1,
                    texture = 0
                };
                dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION setCaillou] " + ex.Message);
                Logger.Print("[EXCEPTION setCaillou] " + ex.StackTrace);
            }
        }

        public static void setAdmin(DbPlayer dbPlayer, int id)
        {
            if (dbPlayer == null) return;
            try
            {
                if (ClothingManager.isMale(dbPlayer.Client))
                {
                    PlayerClothes playerClothes = new PlayerClothes();
                    playerClothes.Maske = new clothingPart()
                    {
                        drawable = 135,
                        texture = id
                    };
                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                    playerClothes.Oberteil = new clothingPart()
                    {
                        drawable = 287,
                        texture = id
                    };
                    dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                    playerClothes.Unterteil = new clothingPart()
                    {
                        drawable = 15,
                        texture = 0
                    };
                    dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                    playerClothes.Kette = new clothingPart()
                    {
                        drawable = 14,
                        texture = 0
                    };
                    dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                    playerClothes.Koerper = new clothingPart()
                    {
                        drawable = 3,
                        texture = 0
                    };
                    dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                    playerClothes.Hose = new clothingPart()
                    {
                        drawable = 114,
                        texture = id
                    };
                    dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                    playerClothes.Schuhe = new clothingPart()
                    {
                        drawable = 78,
                        texture = id
                    };
                    dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                }
                else
                {
                    PlayerClothes playerClothes = new PlayerClothes();
                    playerClothes.Maske = new clothingPart()
                    {
                        drawable = 135,
                        texture = id
                    };
                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                    playerClothes.Oberteil = new clothingPart()
                    {
                        drawable = 300,
                        texture = id
                    };
                    dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                    playerClothes.Unterteil = new clothingPart()
                    {
                        drawable = 14,
                        texture = 0
                    };
                    dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                    playerClothes.Kette = new clothingPart()
                    {
                        drawable = 14,
                        texture = 0
                    };
                    dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                    playerClothes.Koerper = new clothingPart()
                    {
                        drawable = 10,
                        texture = 0
                    };
                    dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                    playerClothes.Hose = new clothingPart()
                    {
                        drawable = 121,
                        texture = id
                    };
                    dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                    playerClothes.Schuhe = new clothingPart()
                    {
                        drawable = 82,
                        texture = id
                    };
                    dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION setAdmin] " + ex.Message);
                Logger.Print("[EXCEPTION setAdmin] " + ex.StackTrace);
            }

        }

    }
}
