using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    internal class DeathHandler : Script
    {
        private void handleDeath(DbPlayer dbPlayer)
        {
            dbPlayer.StopScreenEffect("DeathFailOut");
            dbPlayer.SpawnPlayer(dbPlayer.Client.Position);
            dbPlayer.disableAllPlayerActions(true);
            dbPlayer.StartScreenEffect("DeathFailOut", 0, true);
            dbPlayer.PlayAnimation(33, "combat@damage@rb_writhe", "rb_writhe_loop", 8f);
            dbPlayer.SetInvincible(true);
            dbPlayer.SetSharedData("FUNK_CHANNEL", 0);
            dbPlayer.SetSharedData("FUNK_TALKING", false);
        }

        [ServerEvent(Event.PlayerDeath)]
        public void 
            eath(Client c, Client k, uint reason)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                PaintballModel paintballModel = dbPlayer.GetData("PBZone");

                if (paintballModel != null)
                {
                    if (k != null && k.Exists)
                    {
                        DbPlayer dbKiller = k.GetPlayer();
                        if (dbKiller == null || !dbKiller.IsValid(true))
                            return;

                        dbPlayer.SendNotification("Du wurdest von " + dbKiller.Name + " getötet!", 5000, "black");
                        dbKiller.SendNotification("Du hast " + dbPlayer.Name + " getötet! +30.000$", 5000, "black");
                        dbKiller.addMoney(30000);

                        dbKiller.SetHealth(200);
                        dbKiller.SetArmor(100);

                        dbPlayer.disableAllPlayerActions(true);
                        dbPlayer.SpawnPlayer(dbPlayer.Client.Position);
                        dbPlayer.PlayAnimation(33, "combat@damage@rb_writhe", "rb_writhe_loop", 8f);
                        dbPlayer.SetInvincible(true);

                        WebhookSender.SendMessage("Spieler wird geötet",
                            "Der Spieler " + dbPlayer.Name + " wurde von " + dbKiller.Name + " in FFA getötet.",
                            Webhooks.killWebhook, "Kill");

                        NAPI.Task.Run(() => { PaintballModule.PaintballDeath(dbPlayer, dbKiller); }, 5000);

                        return;
                    }

                }

                if (dbPlayer.HasData("IN_GANGWAR"))
                {
                    dbPlayer.StartScreenEffect("DeathFailOut", 0, true);
                    dbPlayer.DeathData = new DeathData
                    {
                        IsDead = true,
                        DeathTime = new DateTime(0)
                    };
                    if (k != null && k.Exists)
                    {
                        DbPlayer dbKiller = k.GetPlayer();
                        if (dbKiller == null || !dbKiller.IsValid(true))
                            return;

                        if (dbKiller.Faction.Id != dbPlayer.Faction.Id)
                            GangwarModule.handleKill(dbKiller);

                        dbPlayer.SendNotification("Du wurdest von " + dbKiller.Name + " getötet!", 5000, "black");
                        dbKiller.SendNotification("Du hast " + dbPlayer.Name + " getötet! +30.000$", 5000, "black");
                        dbKiller.addMoney(30000);

                        WebhookSender.SendMessage("Spieler wird geötet",
                           "Der Spieler " + dbPlayer.Name + " wurde von " + dbKiller.Name + " " + dbKiller.Loadout + " in Gangwar getötet.",
                           Webhooks.killWebhook, "Kill");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du bist gestorben!", 3000, "black");
                    }

                    NAPI.Task.Run(() => handleDeath(dbPlayer), 5000);
                }
                else if (paintballModel == null)
                {
                    dbPlayer.StartScreenEffect("DeathFailOut", 0, true);
                    dbPlayer.DeathData = new DeathData { IsDead = true, DeathTime = DateTime.Now };
                    if (k != null && k.Exists)
                    {
                        DbPlayer dbKiller = k.GetPlayer();
                        if (dbKiller == null || !dbKiller.IsValid(true))
                            return;

                        dbPlayer.SendNotification("Du wurdest von " + dbKiller.Name + " getötet!", 5000, "black");
                        dbKiller.SendNotification("Du hast " + dbPlayer.Name + " getötet! +30.000$", 5000, "black");
                        dbKiller.addMoney(30000);

                        WebhookSender.SendMessage("Spieler wird geötet",
                           "Der Spieler " + dbPlayer.Name + " wurde von " + dbKiller.Name + " getötet.",
                           Webhooks.killWebhook, "Kill");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du bist gestorben!", 3000, "black");
                    }

                    dbPlayer.SetAttribute("Death", 1);

                    NAPI.Task.Run(() => handleDeath(dbPlayer), 5000);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION PlayerDeath] " + ex.Message);
                Logger.Print("[EXCEPTION PlayerDeath] " + ex.StackTrace);
            }
        }
    }
}
