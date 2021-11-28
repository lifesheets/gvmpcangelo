using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GVMP
{
	public static class VehicleAttributes
	{
		public static void SetAttribute(this DbVehicle dbVehicle, string attribute, object val)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET " + attribute + " = @val WHERE Id = @id");
			if (dbVehicle.Fraktion != null)
			{
				mySqlQuery = new MySqlQuery("UPDATE fraktion_vehicles SET " + attribute + " = @val WHERE FactionId = @factionid AND Model = @model");
				mySqlQuery.AddParameter("@factionid", dbVehicle.Fraktion.Id);
				mySqlQuery.AddParameter("@model", dbVehicle.Model.ToLower());
			}
			mySqlQuery.AddParameter("@val", val);
			mySqlQuery.AddParameter("@id", dbVehicle.Id);
			MySqlHandler.ExecuteSync(mySqlQuery);
		}

		public static dynamic GetAttributeInt(this DbVehicle dbVehicle, string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id");
			if (dbVehicle.Fraktion != null)
			{
				mySqlQuery = new MySqlQuery("SELECT * FROM fraktion_vehicles WHERE FactionId = @factionid AND Model = @model");
				mySqlQuery.AddParameter("@factionid", dbVehicle.Fraktion.Id);
				mySqlQuery.AddParameter("@model", dbVehicle.Model.ToLower());
			}
			mySqlQuery.AddParameter("@id", dbVehicle.Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			dynamic result = query.Reader.GetInt32(attribute);
			query.Connection.Dispose();
			return result;
		}

		public static dynamic GetAttributeint(this DbVehicle dbVehicle, string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id");
			if (dbVehicle.Fraktion != null)
			{
				mySqlQuery = new MySqlQuery("SELECT * FROM fraktion_vehicles WHERE FactionId = @factionid AND Model = @model");
				mySqlQuery.AddParameter("@factionid", dbVehicle.Fraktion.Id);
				mySqlQuery.AddParameter("@model", dbVehicle.Model.ToLower());
			}
			mySqlQuery.AddParameter("@id", dbVehicle.Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			dynamic result = query.Reader.GetInt32(attribute);
			query.Connection.Dispose();
			return result;
		}

		public static dynamic GetAttributeString(this DbVehicle dbVehicle, string attribute)
		{
			MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id");
			if (dbVehicle.Fraktion != null)
			{
				mySqlQuery = new MySqlQuery("SELECT * FROM fraktion_vehicles WHERE FactionId = @factionid AND Model = @model");
				mySqlQuery.AddParameter("@factionid", dbVehicle.Fraktion.Id);
				mySqlQuery.AddParameter("@model", dbVehicle.Model.ToLower());
			}
			mySqlQuery.AddParameter("@id", dbVehicle.Id);
			MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
			(query.Reader).Read();
			dynamic @string = query.Reader.GetString(attribute);
			query.Connection.Dispose();
			return @string;
		}
	}
}
