using System.Data.Common;
using GVMP;
using MySql.Data.MySqlClient;

namespace GVMP
{
	public class Business
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int Money { get; set; } = 0;


		public void removeMoney(int amount)
		{
			BusinessModule.businesses.Remove(this);
			Money -= amount;
			BusinessModule.businesses.Add(this);
			MySqlQuery mySqlQuery = new MySqlQuery("UPDATE businesses SET Money = @money WHERE Id = @id");
			mySqlQuery.AddParameter("@money", Money);
			mySqlQuery.AddParameter("@id", Id);
			MySqlHandler.ExecuteSync(mySqlQuery);
		}

		public void addMoney(int amount)
		{
			BusinessModule.businesses.Remove(this);
			Money += amount;
			BusinessModule.businesses.Add(this);
			MySqlQuery mySqlQuery = new MySqlQuery("UPDATE businesses SET Money = @money WHERE Id = @id");
			mySqlQuery.AddParameter("@money", Money);
			mySqlQuery.AddParameter("@id", Id);
			MySqlHandler.ExecuteSync(mySqlQuery);
		}

		public string Motd()
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM businesses WHERE Id = @id");
			mySqlQuery.AddParameter("@id", Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			MySqlDataReader reader = query.Reader;
			if (reader.HasRows)
			{
				if (reader.Read())
				{
					return reader.GetString("Motd");
				}
				return "-";
			}
			return "-";
		}
	}
}
