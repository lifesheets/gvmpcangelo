using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GVMP
{
    class MessengerApp : GVMP.Module.Module<MessengerApp>
    {
        public static List<Conversation> getConversationList(Client c)
        {
            if (c == null) return new List<Conversation>();
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return new List<Conversation>();

            List<Conversation> conversations = new List<Conversation>();
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM phone_conversations WHERE Player_1 = @userid OR Player_2 = @userid";
                cmd.Parameters.AddWithValue("@userid", dbPlayer.Id);
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            conversations.Add(new Conversation(reader));
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
                Logger.Print("[EXCEPTION getConversationList] " + ex.Message);
                Logger.Print("[EXCEPTION getConversationList] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }

            return conversations;
        }

        public static string getContactName(Client c, int number)
        {
            if (c == null) return "Error";
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return "Error";

            if (number == dbPlayer.Id)
            {
                return "Ich";
            }
            List<Contact> contacts = ContactsApp.getContactList(c);
            Contact contact = contacts.FirstOrDefault((Contact contact2) => contact2.Number == number);

            if(contact == null)
            {
                return number.ToString();
            }

            return contact.Name;
        }

        [RemoteEvent("requestKonversations")]
        public void requestKonversations(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<ClientKonversation> clientKonversations = new List<ClientKonversation>();

                foreach (Conversation conversation in getConversationList(c))
                {
                    ClientKonversation clientKonversation = new ClientKonversation();
                    clientKonversation.KonversationId = conversation.Id;
                    clientKonversation.KonversationPartnerName = getContactName(c,
                        conversation.Player2 == dbPlayer.Id ? conversation.Player1 : conversation.Player2);
                    clientKonversation.KonversationPartnerNumber = conversation.Player2 == dbPlayer.Id
                        ? conversation.Player1
                        : conversation.Player2;
                    CultureInfo culture = new CultureInfo("de-DE");

                    List<ClientKonversationMessage> clientKonversationMessages = new List<ClientKonversationMessage>();

                    using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = con.CreateCommand();
                        cmd.CommandText =
                            "SELECT * FROM phone_conversations_messages WHERE Phone_Conversation_Id = @conversationid";
                        cmd.Parameters.AddWithValue("@conversationid", clientKonversation.KonversationId);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ClientKonversationMessage clientKonversationMessage =
                                        new ClientKonversationMessage();
                                    clientKonversationMessage.Id = reader.GetInt32("Id");
                                    clientKonversationMessage.Message = reader.GetString("Message");
                                    clientKonversationMessage.MessageSenderName =
                                        getContactName(c, reader.GetInt32("Sender_Id"));
                                    clientKonversationMessage.Receiver = reader.GetInt32("Sender_Id") != dbPlayer.Id;
                                    clientKonversationMessage.KonversationMessageUpdatedTime =
                                        reader.GetDateTime("Timestamp").ToString("d", culture);
                                    clientKonversationMessages.Add(clientKonversationMessage);
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
                        Logger.Print("[EXCEPTION requestKonversations] " + ex.Message);
                        Logger.Print("[EXCEPTION requestKonversations] " + ex.StackTrace);
                    }
                    finally
                    {
                        con.Dispose();
                    }

                    if (clientKonversationMessages.Count > 0)
                    {
                        clientKonversation.KonversationUpdatedTime =
                            clientKonversationMessages[clientKonversationMessages.Count - 1]
                                .KonversationMessageUpdatedTime;
                    }

                    clientKonversation.KonversationMessages = clientKonversationMessages;
                    clientKonversations.Add(clientKonversation);
                }

                c.TriggerEvent("componentServerEvent", "MessengerListApp", "responseKonversations",
                    NAPI.Util.ToJson(clientKonversations));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION requestKonversations] " + ex.Message);
                Logger.Print("[EXCEPTION requestKonversations] " + ex.StackTrace);
            }
        }

        [RemoteEvent("deletePhoneChat")]
        public void deletePhoneChat(Client c, int id)
        {
            try
            {
                if (c == null) return;
                MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM phone_conversations WHERE Id = @conversationid");
                mySqlQuery.AddParameter("@conversationid", id);
                MySqlHandler.ExecuteSync(mySqlQuery);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION deletePhoneChat] " + ex.Message);
                Logger.Print("[EXCEPTION deletePhoneChat] " + ex.StackTrace);
            }
        }

        [RemoteEvent("sendPhoneMessage")]
        public void sendPhoneMessage(Client c, int number, string message)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            if (!dbPlayer.CanInteractAntiFlood(2)) return;
            
            if (number == dbPlayer.Id) return;

            try
            {
                MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM phone_conversations WHERE (Player_1 = @userid AND Player_2 = @partnerid) OR (Player_1 = @partnerid AND Player_2 = @userid)");
                mySqlQuery.AddParameter("@userid", dbPlayer.Id);
                mySqlQuery.AddParameter("@partnerid", number);
                MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
                MySqlDataReader reader = mySqlResult.Reader;

                if (!reader.HasRows)
                {
                    mySqlQuery.Parameters.Clear();
                    mySqlQuery = new MySqlQuery("INSERT INTO phone_conversations (Player_1, Player_2) VALUES (@userid, @player2)");
                    mySqlQuery.AddParameter("@userid", dbPlayer.Id);
                    mySqlQuery.AddParameter("@player2", number);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                    
                    mySqlResult.Reader.Dispose();
                    mySqlResult.Connection.Dispose();
                    
                    mySqlQuery.Parameters.Clear();
                    mySqlQuery = new MySqlQuery("SELECT * FROM phone_conversations WHERE (Player_1 = @userid AND Player_2 = @partnerid) OR (Player_1 = @partnerid AND Player_2 = @userid)");
                    mySqlQuery.AddParameter("@userid", dbPlayer.Id);
                    mySqlQuery.AddParameter("@partnerid", number);
                    mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
                    reader = mySqlResult.Reader;
                }

                int msgid = new Random().Next(10000, 99999999);

                try {
                    while (reader.Read())
                    {

                        mySqlQuery.Parameters.Clear();
                        mySqlQuery = new MySqlQuery("INSERT INTO phone_conversations_messages (Id, Phone_Conversation_Id, Message, Sender_Id) VALUES (@id, @conversationid, @msg, @userid)");
                        mySqlQuery.AddParameter("@userid", dbPlayer.Id);
                        mySqlQuery.AddParameter("@conversationid", reader.GetInt32("id"));
                        mySqlQuery.AddParameter("@msg", message);
                        mySqlQuery.AddParameter("@id", msgid);
                        MySqlHandler.ExecuteSync(mySqlQuery);

                        ClientKonversationMessage clientKonversationMessage = new ClientKonversationMessage();
                        clientKonversationMessage.Id = msgid;
                        clientKonversationMessage.KonversationMessageUpdatedTime = DateTime.Now.ToString().Split(" ")[1];
                        clientKonversationMessage.Message = message;
                        clientKonversationMessage.Receiver = false;
                        clientKonversationMessage.MessageSenderName = "Ich";
                        c.TriggerEvent("componentServerEvent", "MessengerOverviewApp", "updateChat", NAPI.Util.ToJson(clientKonversationMessage));

                        int partner = reader.GetInt32("Player_1") == dbPlayer.Id ? reader.GetInt32("Player_2") : reader.GetInt32("Player_1");

                        DbPlayer target = PlayerHandler.GetPlayer(partner);
                        if (target != null && target.IsValid(true) && target.Client.Exists && !SettingsApp.isFlugmodus(target.Client))
                        {
                            target.SendNotification("Neue SMS von: " + getContactName(target.Client, dbPlayer.Id), 3000, "black");

                            ClientKonversationMessage clientKonversationMessage2 = new ClientKonversationMessage();
                            clientKonversationMessage2.Id = msgid;
                            clientKonversationMessage2.KonversationMessageUpdatedTime = DateTime.Now.ToString().Split(" ")[1];
                            clientKonversationMessage2.Message = message;
                            clientKonversationMessage2.Receiver = true;
                            clientKonversationMessage2.MessageSenderName = getContactName(target.Client, dbPlayer.Id);
                            target.Client.TriggerEvent("componentServerEvent", "MessengerOverviewApp", "updateChat", NAPI.Util.ToJson(clientKonversationMessage2));
                            target.Client.TriggerEvent("playSMSRingtone");
                        }
                        else
                        {
                            dbPlayer.SendNotification("Der Empfänger ist derzeit nicht erreichbar. Die SMS wurde trotzdem versendet.", 3000, "red", "");
                        }
                    }
                }
                finally
                {
                    reader.Dispose();
                    mySqlResult.Connection.Dispose();
                }

            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION sendPhoneMessage] " + ex.Message);
                Logger.Print("[EXCEPTION sendPhoneMessage] " + ex.StackTrace);
            }
        }

        public class ClientKonversation
        {
            [JsonProperty(PropertyName = "messagesId")]
            public int KonversationId { get; set; }

            [JsonProperty(PropertyName = "messageSender")]
            public string KonversationPartnerName { get; set; }

            [JsonProperty(PropertyName = "messageSenderNumber")]
            public int KonversationPartnerNumber { get; set; }

            [JsonProperty(PropertyName = "lastMessage")]
            public string KonversationUpdatedTime { get; set; }

            [JsonProperty(PropertyName = "messages")]
            public List<ClientKonversationMessage> KonversationMessages { get; set; }
        }

        public class ClientKonversationMessage
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "sender")]
            public string MessageSenderName { get; set; }

            [JsonProperty(PropertyName = "date")]
            public string KonversationMessageUpdatedTime { get; set; }

            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }

            [JsonProperty(PropertyName = "receiver")]
            public bool Receiver { get; set; }
        }
    }
}
