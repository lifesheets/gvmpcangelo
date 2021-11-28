using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    class VoiceModule : GVMP.Module.Module<VoiceModule>
    {
        [RemoteEvent("changeVoiceRange")]
        public static void changeVoiceRange(Client c)
        {
            try
            {
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;
                
                if(!dbPlayer.HasSharedData("CLIENT_RANGE") || dbPlayer.GetSharedData("CLIENT_RANGE") == null)
                    dbPlayer.SetSharedData("CLIENT_RANGE", 2);
                
                if(dbPlayer.GetSharedData("CLIENT_RANGE") is int) { } else
                {
                    dbPlayer.SetSharedData("CLIENT_RANGE", 2);
                }

                int range = dbPlayer.GetSharedData("CLIENT_RANGE");

                if (range >= 3)
                {
                    range = 1;
                }
                else
                {
                    range++;
                }
                
                dbPlayer.SetSharedData("CLIENT_RANGE", range);
                dbPlayer.Client.TriggerEvent("setVoiceType", range.ToString());
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION changeVoiceRange] " + ex.Message);
                Logger.Print("[EXCEPTION changeVoiceRange] " + ex.StackTrace);
            }
        }

        [RemoteEvent("changeSettings")]
        public static void changeSettings(Client c, string val2)
        {
            try
            {
                int val = 0;
                if (int.TryParse(val2, out val))
                {
                    DbPlayer player = c.GetPlayer();
                    if ((player == null || !player.IsValid(true) ? false : player.Client != null))
                    {
                        if (player.CanInteractAntiDeath())
                        {
                            if (val == 0)
                            {
                                c.SetSharedData("FUNK_TALKING", false);
                                c.SetSharedData("FUNK_CHANNEL", 0);
                            }
                            else if (val == 1)
                            {
                                if ((dynamic)c.GetSharedData("FUNK_CHANNEL") != 0)
                                {
                                    c.SetSharedData("FUNK_TALKING", false);
                                    if (!c.IsInVehicle)
                                    {
                                        c.StopAnimation();
                                    }
                                }
                            }
                            else if (val == 2)
                            {
                                if ((dynamic)c.GetSharedData("FUNK_CHANNEL") != 0)
                                {
                                    c.SetSharedData("FUNK_TALKING", true);
                                    if ((c.IsInVehicle || player.DeathData.IsDead ? false : player.CanInteractAntiDeath()))
                                    {
                                        player.PlayAnimation(49, "random@arrests", "generic_radio_chatter", 8f);
                                    }
                                }
                            }
                            c.TriggerEvent("updateVoiceState", new object[] { val });
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Logger.Print(string.Concat("[EXCEPTION changeSettings] ", exception.Message));
                Logger.Print(string.Concat("[EXCEPTION changeSettings] ", exception.StackTrace));
            }
        }

        [RemoteEvent("stopAnimation")]
        public void stopAnimation(Client c)
        {
            if (!c.IsInVehicle)
                c.StopAnimation();
        }

        [RemoteEvent("changeFrequenz")]
        public static void changeFrequenz(Client c, string val2)
        {
            try
            {
                int val = 0;
                bool parsed = int.TryParse(val2, out val);
                if (!parsed) return;
                
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                bool encrypted = false;
                
                Faction faction = FactionModule.factionList.FirstOrDefault((Faction faction2) => faction2.Id == val);
                if (faction != null)
                {
                    if (dbPlayer.Faction.Id != faction.Id)
                    {
                        encrypted = true;
                    }
                }

                if (encrypted)
                {
                    dbPlayer.SendNotification("Dieser Funkkanal ist verschlüsselt.", 3000, "red");
                    return;
                }

                if (val > 0 && val <= 10000)
                {
                    c.SetSharedData("FUNK_CHANNEL", val);
                    dbPlayer.SendNotification("Funkkanal " + val + " MHz betreten!", 3000, "green", "FUNK");
                }
                else
                    dbPlayer.SendNotification("Dieser Funk ist nicht existent.", 3000, "red", "FUNK");
            }
            catch(Exception ex)
            {
                Logger.Print("[EXCEPTION changeFrequenz] " + ex.Message);
                Logger.Print("[EXCEPTION changeFrequenz] " + ex.StackTrace);
            }
}
    }
}
