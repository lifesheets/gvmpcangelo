using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GVMP
{
    class WebhookSender : GVMP.Module.Module<WebhookSender>
    {
        public static async void SendMessage(string title, string msg, string webhook, string type)
        {
            try
            {
                DateTime now = DateTime.Now;
                string[] strArray = new string[20];
                strArray[0] = "{\"username\":\"GVMPc\",\"avatar_url\":\"https://cdn.discordapp.com/attachments/908080997470437497/908404099043688478/gvmpccclogo.png\"},\"title\":\"" + type + "\",\"thumbnail\":{\"url\":\"https://cdn.discordapp.com/attachments/908080997470437497/908404099043688478/gvmpccclogo.png\"},\"url\":\"https://discord.gg/gvmpc\",\"description\":\"Es wurde am **";
                int num = now.Day;
                strArray[1] = num.ToString();
                strArray[2] = ".";
                num = now.Month;
                strArray[3] = num.ToString();
                strArray[4] = ".";
                num = now.Year;
                strArray[5] = num.ToString();
                strArray[6] = " | ";
                num = now.Hour;
                strArray[7] = num.ToString();
                strArray[8] = ":";
                num = now.Minute;
                strArray[9] = num.ToString();
                strArray[10] = "** ein " + type + " ausgelöst.\",\"color\":1127128,\"fields\":[{\"name\":\"";
                strArray[11] = title;
                strArray[12] = "\",\"value\":\"";
                strArray[13] = msg;
                strArray[14] = "\",\"inline\":true}],\"footer\":{\"text\":\"GVMPc Crimelife | " + type + " Bot (c) 2021\"}}]}";
                string stringPayload = string.Concat(strArray);
                StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(webhook, (HttpContent)httpContent);
                }
                stringPayload = (string)null;
                httpContent = (StringContent)null;
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION SendMessage] " + ex.Message);
                Logger.Print("[EXCEPTION SendMessage] " + ex.StackTrace);
            }
        }

        public static async void SendMessage423(string title, string msg, string webhook, string type)
        {
            try
            {
                DateTime now = DateTime.Now;
                string[] strArray = new string[20];
                strArray[0] = "{\"username\":\"Atlantical\",\"avatar_url\":\"https://cdn.discordapp.com/attachments/893489347960127498/893495028389928980/standard_5.gif\",\"content\":\"\",\"embeds\":[{\"author\":{\"name\":\"Atlantical Crimelife\",\"url\":\"https://discord.gg/nexuscrimelife\",\"icon_url\":\"https://cdn.discordapp.com/attachments/893489347960127498/893495028389928980/standard_5.gif\"},\"title\":\"" + type + "\",\"thumbnail\":{\"url\":\"https://cdn.discordapp.com/attachments/893489347960127498/893495028389928980/standard_5.gif\"},\"url\":\"https://discord.gg/nexuscrimelife\",\"description\":\"Es wurde am **";
                int num = now.Day;
                strArray[1] = num.ToString();
                strArray[2] = ".";
                num = now.Month;
                strArray[3] = num.ToString();
                strArray[4] = ".";
                num = now.Year;
                strArray[5] = num.ToString();
                strArray[6] = " | ";
                num = now.Hour;
                strArray[7] = num.ToString();
                strArray[8] = ":";
                num = now.Minute;
                strArray[9] = num.ToString();
                strArray[10] = "** ein " + type + " ausgelöst.\",\"color\":1127128,\"fields\":[{\"name\":\"";
                strArray[11] = title;
                strArray[12] = "\",\"value\":\"";
                strArray[13] = msg;
                strArray[14] = "\",\"inline\":true}],\"footer\":{\"text\":\"Atlantical Crimelife | " + type + " Bot (c) 2021\"}}]}";
                string stringPayload = string.Concat(strArray);
                StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(webhook, (HttpContent)httpContent);
                }
                stringPayload = (string)null;
                httpContent = (StringContent)null;
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION SendMessage] " + ex.Message);
                Logger.Print("[EXCEPTION SendMessage] " + ex.StackTrace);
            }
        }

        public static async void SendMessage321(string title, string msg, string webhook, string type)
        {
            try
            {
                DateTime now = DateTime.Now;
                string[] strArray = new string[20];
                strArray[0] = "{\"username\":\"NEXUS\",\"avatar_url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\",\"content\":\"\",\"embeds\":[{\"author\":{\"name\":\"Nexus Crimelife\",\"url\":\"https://discord.gg/nexuscrimelife\",\"icon_url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\"},\"title\":\"" + type + "\",\"thumbnail\":{\"url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\"},\"url\":\"https://discord.gg/nexuscrimelife\",\"description\":\"Es wurde am **";
                int num = now.Day;
                strArray[1] = num.ToString();
                strArray[2] = ".";
                num = now.Month;
                strArray[3] = num.ToString();
                strArray[4] = ".";
                num = now.Year;
                strArray[5] = num.ToString();
                strArray[6] = " | ";
                num = now.Hour;
                strArray[7] = num.ToString();
                strArray[8] = ":";
                num = now.Minute;
                strArray[9] = num.ToString();
                strArray[10] = "** ein " + type + " ausgelöst.\",\"color\":1127128,\"fields\":[{\"name\":\"";
                strArray[11] = title;
                strArray[12] = "\",\"value\":\"";
                strArray[13] = msg;
                strArray[14] = "\",\"inline\":true}],\"footer\":{\"text\":\"Nexus Crimelife | " + type + " Bot (c) 2021\"}}]}";
                string stringPayload = string.Concat(strArray);
                StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(webhook, (HttpContent)httpContent);
                }
                stringPayload = (string)null;
                httpContent = (StringContent)null;
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION SendMessage] " + ex.Message);
                Logger.Print("[EXCEPTION SendMessage] " + ex.StackTrace);
            }
        }

        public static async void SendMessage123(string title, string msg, string webhook, string type)
        {
            try
            {
                DateTime now = DateTime.Now;
                string[] strArray = new string[20];
                strArray[0] = "{\"username\":\"NEXUS\",\"avatar_url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\",\"content\":\"\",\"embeds\":[{\"author\":{\"name\":\"Nexus Crimelife\",\"url\":\"https://discord.gg/nexuscrimelife\",\"icon_url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\"},\"title\":\"" + type + "\",\"thumbnail\":{\"url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\"},\"url\":\"https://discord.gg/nexuscrimelife\",\"description\":\"Es wurde am **";
                int num = now.Day;
                strArray[1] = num.ToString();
                strArray[2] = ".";
                num = now.Month;
                strArray[3] = num.ToString();
                strArray[4] = ".";
                num = now.Year;
                strArray[5] = num.ToString();
                strArray[6] = " | ";
                num = now.Hour;
                strArray[7] = num.ToString();
                strArray[8] = ":";
                num = now.Minute;
                strArray[9] = num.ToString();
                strArray[10] = "** ein " + type + " ausgelöst.\",\"color\":1127128,\"fields\":[{\"name\":\"";
                strArray[11] = title;
                strArray[12] = "\",\"value\":\"";
                strArray[13] = msg;
                strArray[14] = "\",\"inline\":true}],\"footer\":{\"text\":\"Nexus Crimelife | " + type + " Bot (c) 2021\"}}]}";
                string stringPayload = string.Concat(strArray);
                StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(webhook, (HttpContent)httpContent);
                }
                stringPayload = (string)null;
                httpContent = (StringContent)null;
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION SendMessage] " + ex.Message);
                Logger.Print("[EXCEPTION SendMessage] " + ex.StackTrace);
            }
        }

        public static async void SendMessage534(string title, string msg, string webhook, string type)
        {
            try
            {
                DateTime now = DateTime.Now;
                string[] strArray = new string[20];
                strArray[0] = "{\"username\":\"NEXUS\",\"avatar_url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\",\"content\":\"\",\"embeds\":[{\"author\":{\"name\":\"Nexus Crimelife\",\"url\":\"https://discord.gg/nexuscrimelife\",\"icon_url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\"},\"title\":\"" + type + "\",\"thumbnail\":{\"url\":\"https://cdn.discordapp.com/attachments/824761396952432640/866878215456030790/dark-logo_1.png\"},\"url\":\"https://discord.gg/nexuscrimelife\",\"description\":\"Es wurde am **";
                int num = now.Day;
                strArray[1] = num.ToString();
                strArray[2] = ".";
                num = now.Month;
                strArray[3] = num.ToString();
                strArray[4] = ".";
                num = now.Year;
                strArray[5] = num.ToString();
                strArray[6] = " | ";
                num = now.Hour;
                strArray[7] = num.ToString();
                strArray[8] = ":";
                num = now.Minute;
                strArray[9] = num.ToString();
                strArray[10] = "** ein " + type + " ausgelöst.\",\"color\":1127128,\"fields\":[{\"name\":\"";
                strArray[11] = title;
                strArray[12] = "\",\"value\":\"";
                strArray[13] = msg;
                strArray[14] = "\",\"inline\":true}],\"footer\":{\"text\":\"Nexus Crimelife | " + type + " Bot (c) 2021\"}}]}";
                string stringPayload = string.Concat(strArray);
                StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(webhook, (HttpContent)httpContent);
                }
                stringPayload = (string)null;
                httpContent = (StringContent)null;
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION SendMessage] " + ex.Message);
                Logger.Print("[EXCEPTION SendMessage] " + ex.StackTrace);
            }
        }
    }
}
