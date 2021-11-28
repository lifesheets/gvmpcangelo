using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GVMP
{
	public static class LaboratoryAttributes
	{
		public static void SetAttribute(this Laboratory laboratory, string attribute, object val)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("UPDATE laboratorys SET " + attribute + " = @val WHERE Id = @id");
			mySqlQuery.AddParameter("@val", val);
			mySqlQuery.AddParameter("@id", laboratory.Id);
			MySqlHandler.ExecuteSync(mySqlQuery);
		}

		public static dynamic GetAttributeInt(this Laboratory laboratory, string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM laboratorys WHERE Id = @id");
			mySqlQuery.AddParameter("@id", laboratory.Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			object result = query.Reader.GetInt32(attribute);
			query.Connection.Dispose();
			return result;
		}

		public static dynamic GetAttributeint(this Laboratory laboratory, string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM laboratorys WHERE Id = @id");
			mySqlQuery.AddParameter("@id", laboratory.Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			object result = query.Reader.GetInt32(attribute);
			query.Connection.Dispose();
			return result;
		}

		public static dynamic GetAttributeString(this Laboratory laboratory, string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM laboratorys WHERE Id = @id");
			mySqlQuery.AddParameter("@id", laboratory.Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			object @string = query.Reader.GetString(attribute);
			query.Connection.Dispose();
			return @string;
		}
	}
}
