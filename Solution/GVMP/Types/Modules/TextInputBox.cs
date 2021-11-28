using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public static class TextInputBox
    {
        public static void OpenTextInputBox(this DbPlayer dbPlayer, TextInputBoxObject textInputBoxObject)
        {
            object variable = new { textBoxObject = textInputBoxObject };
            Client client = dbPlayer.Client;
            client.TriggerEvent("openWindow", new object[] { "TextInputBox", NAPI.Util.ToJson(variable) });
            client.TriggerEvent("componentReady", new object[] { "TextInputBox" });
        }

        public static void OpenTextInputBox(this Client c, TextInputBoxObject textInputBoxObject)
        {
            object variable = new { textBoxObject = textInputBoxObject };
            c.TriggerEvent("openWindow", new object[] { "TextInputBox", NAPI.Util.ToJson(variable) });
            c.TriggerEvent("componentReady", new object[] { "TextInputBox" });
        }
    }
}
