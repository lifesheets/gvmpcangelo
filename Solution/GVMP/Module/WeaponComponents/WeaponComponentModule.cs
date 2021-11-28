using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class NXWeaponComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WeaponHash WeaponHash { get; set; }
        public WeaponComponent WeaponComponent { get; set; }
        public int Price { get; set; }

        public NXWeaponComponent(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            WeaponComponent = (WeaponComponent)NAPI.Util.GetHashKey(reader.GetString("Component"));
            Price = reader.GetInt32("Price");
            Name = reader.GetString("Name");
            WeaponHash = (WeaponHash)NAPI.Util.GetHashKey(reader.GetString("Weapon"));
            Logger.Print(Name + " - Weapon: " + NAPI.Util.GetHashKey(reader.GetString("Weapon")).ToString() + " - Component: " + NAPI.Util.GetHashKey(reader.GetString("Component")).ToString());
        }
    }

    public class WeaponComponentModule : GVMP.Module.Module<WeaponComponentModule>
    {
        public static List<NXWeaponComponent> nXWeaponComponents = new List<NXWeaponComponent>();

        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM weapon_components");
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
                            NXWeaponComponent nXWeaponComponent = new NXWeaponComponent(reader);
                            nXWeaponComponents.Add(nXWeaponComponent);
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
                Logger.Print("[EXCEPTION nXWeaponComponent] " + ex.Message);
                Logger.Print("[EXCEPTION nXWeaponComponent] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            return true;
        }

        [RemoteEvent]
        public void log(Client c, string log)
        {
            Logger.Print(c.Name + ": " + log);
        }

    }
}
