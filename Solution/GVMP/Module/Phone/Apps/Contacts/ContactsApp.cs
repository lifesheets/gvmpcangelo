using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    public class ContactsApp : GVMP.Module.Module<ContactsApp>
    {
        public static List<Contact> getContactList(Client c)
        {
            if (c == null) return new List<Contact>();
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return new List<Contact>();

            List<Contact> contacts = new List<Contact>();
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM phone_contacts WHERE Id = @userid LIMIT 1");
            mySqlQuery.Parameters = new List<MySqlParameter>()
            {
                new MySqlParameter("@userid", dbPlayer.Id)
            };
            MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
            try
            {
                MySqlDataReader reader = mySqlReaderCon.Reader;
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            contacts = NAPI.Util.FromJson<List<Contact>>(reader.GetString("Contacts"));
                        }
                    }
                    else
                    {
                        mySqlQuery.Query = "INSERT INTO phone_contacts (Id) VALUES (@userid)";
                        mySqlQuery.Parameters = new List<MySqlParameter>()
                        {
                            new MySqlParameter("@userid", dbPlayer.Id)
                        };
                        MySqlHandler.ExecuteSync(mySqlQuery);
                    }
                }
                finally
                {
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION getContactList] " + ex.Message);
                Logger.Print("[EXCEPTION getContactList] " + ex.StackTrace);
            }
            finally
            {
                mySqlReaderCon.Connection.Dispose();
            }

            return contacts;
        }

        [RemoteEvent("requestPhoneContacts")]
        public void requestPhoneContacts(Client c)
        {
            try
            {
                if (c == null) return;
                c.TriggerEvent("responsePhoneContacts", NAPI.Util.ToJson(getContactList(c)));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION requestPhoneContacts] " + ex.Message);
                Logger.Print("[EXCEPTION requestPhoneContacts] " + ex.StackTrace);
            }
        }

        [RemoteEvent("addPhoneContact")]
        public void addPhoneContact(Client c, string name, string number2)
        {
            try
            {
                int number = 0;
                bool number3 = int.TryParse(number2, out number);
                if(!number3) return;
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<Contact> contacts = getContactList(c);
                contacts.Add(new Contact
                {
                    Name = name,
                    Number = number
                });
                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE phone_contacts SET Contacts = @val WHERE Id = @userid");
                mySqlQuery.Parameters = new List<MySqlParameter>()
                {
                    new MySqlParameter("@userid", dbPlayer.Id),
                    new MySqlParameter("@val", NAPI.Util.ToJson(contacts))
                };
                MySqlHandler.ExecuteSync(mySqlQuery);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION addPhoneContact] " + ex.Message);
                Logger.Print("[EXCEPTION addPhoneContact] " + ex.StackTrace);
            }
        }

        [RemoteEvent("delPhoneContact")]
        public void delPhoneContact(Client c, string number2)
        {
            try
            {
                int number = 0;
                bool number3 = int.TryParse(number2, out number);
                if(!number3) return;
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<Contact> contacts = getContactList(c);
                Contact removeContact = contacts.FirstOrDefault((Contact con) => con.Number == number);
                if (removeContact == null)
                    return;

                contacts.Remove(removeContact);

                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE phone_contacts SET Contacts = @val WHERE Id = @userid");
                mySqlQuery.Parameters = new List<MySqlParameter>()
                {
                    new MySqlParameter("@userid", dbPlayer.Id),
                    new MySqlParameter("@val", NAPI.Util.ToJson(contacts))
                };
                MySqlHandler.ExecuteSync(mySqlQuery);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION delPhoneContact] " + ex.Message);
                Logger.Print("[EXCEPTION delPhoneContact] " + ex.StackTrace);
            }
        }

        [RemoteEvent("updatePhoneContact")]
        public void updatePhoneContact(Client c, int oldNumber, string newNumber2, string name)
        {
            try
            {
                if (c == null) return;
                int newNumber = 0;
                bool newNumber3 = int.TryParse(newNumber2, out newNumber);
                if(!newNumber3) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<Contact> contacts = getContactList(c);
                Contact changeContact = contacts.FirstOrDefault((Contact con) => con.Number == oldNumber);
                if (changeContact == null)
                    return;

                contacts.Remove(changeContact);
                changeContact.Name = name;
                changeContact.Number = newNumber;
                contacts.Add(changeContact);

                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE phone_contacts SET Contacts = @val WHERE Id = @userid");
                mySqlQuery.Parameters = new List<MySqlParameter>()
                {
                    new MySqlParameter("@userid", dbPlayer.Id),
                    new MySqlParameter("@val", NAPI.Util.ToJson(contacts))
                };
                MySqlHandler.ExecuteSync(mySqlQuery);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION updatePhoneContact] " + ex.Message);
                Logger.Print("[EXCEPTION updatePhoneContact] " + ex.StackTrace);
            }
        }
    }
}
