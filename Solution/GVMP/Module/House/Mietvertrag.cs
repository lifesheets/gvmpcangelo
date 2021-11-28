using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class Mietvertrag : Script
    {
        [RemoteEvent("")]
        public void sendMietvertrag(Client c, string text)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            if (text == dbPlayer.Name) return;

            House house = HouseModule.houses.FirstOrDefault((House house2) => house2.OwnerId == dbPlayer.Id);
            if (house == null) return;

            DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(text);
            if (dbPlayer2 != null && dbPlayer.IsValid(true))
            {
                House house2 = HouseModule.houses.FirstOrDefault((House house2) => house2.OwnerId == dbPlayer2.Id || house2.TenantsIds.Contains(dbPlayer2.Id));
                WebhookSender.SendMessage("TEXTINPUTBOX", "" + dbPlayer.Name + " + " + text + " - Für Entwicklungs. - MIETVERTRAG", Webhooks.shoplogs, "Shop");
                if (house2 == null)
                {
                    dbPlayer2.OpenConfirmation(new ConfirmationObject
                    {
                        Title = "Mietvertrag",
                        Message = "Hiermit schließen Sie einen Mietvertrag mit dem Vermieter: " + dbPlayer.Name,
                        Callback = "acceptMietvertrag",
                        Arg1 = dbPlayer.Id.ToString(),
                        Arg2 = house.Id.ToString()
                    });
                }
                else
                {
                    dbPlayer.SendNotification("Der Spieler besitzt bereits ein Haus!", 3000, "red");
                }
            }
            else
            {
                dbPlayer.SendNotification("Spieler nicht gefunden!");
            }
        }

        [RemoteEvent("acceptMietvertrag")]
        public void acceptMietvertrag(Client c, string ownerid2, string houseid2)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            int OwnerId = 0;
            int HouseId = 0;
            bool OwnerId2 = int.TryParse(ownerid2, out OwnerId);
            bool HouseId2 = int.TryParse(houseid2, out HouseId);

            if (!OwnerId2) return;
            if (!HouseId2) return;
            
            House house = HouseModule.houses.FirstOrDefault((House house2) => house2.Id == HouseId);
            if (house == null) return;

            DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(OwnerId);
            if (dbPlayer2 == null || !dbPlayer.IsValid(true)) return;

            HouseModule.houses.Remove(house);
            house.TenantsIds.Add(dbPlayer.Id);
            house.TenantPrices.Add(dbPlayer.Id, 0);
            HouseModule.houses.Add(house);

            MySqlQuery mySqlQuery = new MySqlQuery("UPDATE houses SET TenantPrices = @val WHERE Id = @id");
            mySqlQuery.AddParameter("@id", house.Id);
            mySqlQuery.AddParameter("@val", NAPI.Util.ToJson(house.TenantPrices));
            MySqlHandler.ExecuteSync(mySqlQuery);

            mySqlQuery.Parameters.Clear();
            mySqlQuery = new MySqlQuery("UPDATE houses SET TenantsId = @val WHERE Id = @id");
            mySqlQuery.AddParameter("@id", house.Id);
            mySqlQuery.AddParameter("@val", NAPI.Util.ToJson(house.TenantsIds));
            MySqlHandler.ExecuteSync(mySqlQuery);

            dbPlayer2.SendNotification("Dein Mietvertrag wurde angenommen!", 3000, "green");
            dbPlayer.SendNotification("Du hast den Mietvertrag angenommen!", 3000, "green");


        }
    }
}
