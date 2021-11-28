using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GVMP
{
    public class Laboratory
    {
		public int Id { get; set; }

		public int FactionId { get; set; }

		public Vector3 Entrance { get; set; }

		public bool Status { get; set; }

		public void SetAttribute(string attribute, object val)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("UPDATE laboratorys SET " + attribute + " = @val WHERE Id = @id");
			mySqlQuery.AddParameter("@val", val);
			mySqlQuery.AddParameter("@id", Id);
			MySqlHandler.ExecuteSync(mySqlQuery);
		}

		public dynamic GetAttributeInt(string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM laboratorys WHERE Id = @id");
			mySqlQuery.AddParameter("@id", Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			object result = query.Reader.GetInt32(attribute);
			query.Connection.Dispose();
			return result;
		}

		public dynamic GetAttributeint(string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM laboratorys WHERE Id = @id");
			mySqlQuery.AddParameter("@id", Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			object result = query.Reader.GetInt32(attribute);
			query.Connection.Dispose();
			return result;
		}

		public dynamic GetAttributeString(string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM laboratorys WHERE Id = @id");
			mySqlQuery.AddParameter("@id", Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			object @string = query.Reader.GetString(attribute);
			query.Connection.Dispose();
			return @string;
		}

		public Laboratory() { }

	}
}
