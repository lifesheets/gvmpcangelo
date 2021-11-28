using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace GVMP
{
    public static class Login
    {

        public static void OpenLoginWindow(this Client c, string Name)
        {
            LoginWindow loginWindow = new LoginWindow
            {
                Name = Name,
                Auth = "gvmpcbykiss"
            };

            c.TriggerEvent("openWindow", nameof(Login), NAPI.Util.ToJson(loginWindow));
            c.TriggerEvent("componentReady", nameof(Login));
        }

        public static void LoginStatus(this Client c, string Message)
        {
            c.TriggerEvent("componentServerEvent", nameof(Login), "status", Message);
        }
    }
}
