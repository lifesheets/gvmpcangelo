using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMP
{
    class BanModule : GVMP.Module.Module<BanModule>
    {
        public static List<Ban> bans = new List<Ban>();

        protected override bool OnLoad()
        {
            bans.Clear();
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM bans");
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
                            bans.Add(new Ban
                            {
                                Id = reader.GetInt32("Id"),
                                Identifier = reader.GetString("Identifier"),
                                Reason = reader.GetString("Reason"),
                                Account = reader.GetString("Account")
                            });
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
                Logger.Print("[EXCEPTION loadBans] " + ex.Message);
                Logger.Print("[EXCEPTION loadBans] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }
            return true;
        }

        public static void TimeBanIdentifier(DateTime BanDate, string Account)
        {
            try
            {
                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Banzeit = @banzeit WHERE Username = @account");
                mySqlQuery.AddParameter("@banzeit", BanDate);
                mySqlQuery.AddParameter("@account", Account);
                MySqlHandler.ExecuteSync(mySqlQuery);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION BanIdentifier] " + ex.Message);
                Logger.Print("[EXCEPTION BanIdentifier] " + ex.StackTrace);
            }
        }

        public static void BanIdentifier(string Identifier, string Reason, string Account)
        {
            try
            {
                int Id = new Random().Next(10000, 99999999);

                MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO bans (Id, Identifier, Reason, Account) VALUES (@id, @identifier, @reason, @account)");
                mySqlQuery.AddParameter("@id", Id);
                mySqlQuery.AddParameter("@identifier", Identifier);
                mySqlQuery.AddParameter("@reason", Reason);
                mySqlQuery.AddParameter("@account", Account);
                MySqlHandler.ExecuteSync(mySqlQuery);

                bans.Add(new Ban
                {
                    Id = Id,
                    Identifier = Identifier,
                    Reason = Reason,
                    Account = Account
                });
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION BanIdentifier] " + ex.Message);
                Logger.Print("[EXCEPTION BanIdentifier] " + ex.StackTrace);
            }
        }

        public static bool isIdentifierBanned(string Identifier)
        {
            if (Identifier == "LifeVMorice" || Identifier == "Kian_Kleinkopf") return false;
            Ban ban = bans.FirstOrDefault((Ban ban2) => ban2.Identifier == Identifier);
            if (ban == null) return false;

            return true;
        }
    }
}
