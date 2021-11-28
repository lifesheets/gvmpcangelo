using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class MySqlHandler
    {
        public static async void ExecuteSync(MySqlQuery query)
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            con.Open();
            MySqlCommand mySqlCommand = con.CreateCommand();
            try
            {
                mySqlCommand.CommandText = query.Query;
                foreach(MySqlParameter item in query.Parameters)
                {
                    /*if (item.Obj is string)
                    {
                        mySqlCommand.CommandText.Replace(item.Name, $"'{item.Obj}'");
                    }
                    else*/ mySqlCommand.Parameters.AddWithValue(item.Name, item.Obj);
                }
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION ExecuteSync] " + NAPI.Util.ToJson(query));
                Logger.Print("[EXCEPTION ExecuteSync] " + ex.Message);
                Logger.Print("[EXCEPTION ExecuteSync] " + ex.StackTrace);
                Logger.Print("[EXCEPTION ExecuteSync] " + mySqlCommand.CommandText);
            }
            finally
            {
                con.Dispose();
            }
        }

        public static MySqlResult GetQuery(MySqlQuery query)
        {
            MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            con.Open();
            MySqlCommand mySqlCommand = con.CreateCommand();
            try
            {
                mySqlCommand.CommandText = query.Query;
                foreach (MySqlParameter item in query.Parameters)
                {
                    mySqlCommand.Parameters.AddWithValue(item.Name, item.Obj);
                }

                MySqlDataReader mySqlReader = mySqlCommand.ExecuteReader();
                return new MySqlResult(mySqlReader, con);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Query] " + NAPI.Util.ToJson(query));
                Logger.Print("[EXCEPTION Query] " + ex.Message);
                Logger.Print("[EXCEPTION Query] " + ex.StackTrace);
                Logger.Print("[EXCEPTION ExecuteSync] " + mySqlCommand.CommandText);
            }
            finally
            {
                NAPI.Task.Run(() =>
                {
                    con.Dispose();
                }, 1000);
            }
            return null;
        }
    }
}
