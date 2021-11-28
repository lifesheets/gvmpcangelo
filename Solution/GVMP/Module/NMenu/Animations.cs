using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class Animations : GVMP.Module.Module<Animations>
    {
        public static List<Animation> animations = new List<Animation>()
        {
            new Animation
            {
                Slot = 0,
                Icon = "Abbrechen.png",
                Select = "exit",
                Name = "Abbrechen",
                Flag = 0
            }
        };

        protected override bool OnLoad()
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM animations";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            animations.Add(new Animation
                            {
                                Slot = reader.GetInt32("Slot"),
                                Icon = reader.GetString("Icon"),
                                Name = reader.GetString("Name"),
                                Select = reader.GetString("Selectable"),
                                Flag = reader.GetInt32("Flag")
                            });
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
                Logger.Print("[EXCEPTION loadAnimations] " + ex.Message);
                Logger.Print("[EXCEPTION loadAnimations] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }
            return true;
        }
    }
}
