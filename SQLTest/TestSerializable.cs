using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest
{
	internal class TestEmployee : ISQLSerializeable, ISQLUpdateable
	{
		public string FirstName;
		public string LastName;
		public int Id;

		public TestEmployee(string firstName, string lastName, int id)
		{
			FirstName = firstName;
			LastName = lastName;
			Id = id;
		}

		public TestEmployee() : this("invalid", "invalid", 0) { }

		public override string ToString()
		{
			return $"{FirstName}, {LastName}: = {Id}";
		}

		public static SchemaEntry GetEntry(string name)
		{
			return new SchemaEntry(name, new[]
			{
				new SchemaFeild("first_name", "VARCHAR(20)"),
				new SchemaFeild("last_name", "VARCHAR(20)"),
				new SchemaFeild("id", "INT")
			});
		}

		public void OnDeserialize(ref SQLiteDataReader reader)
		{
			FirstName = reader.GetString(0);
			LastName = reader.GetString(1);
			Id = reader.GetInt32(2);
		}

		public string[] OnSerialize()
		{
			return new[]
			{
				$"'{FirstName}'",
				$"'{LastName}'",
				$"{Id}"
			};
		}

		public (string, string) GetId()
		{
			return ("id", Id.ToString());
		}
	}
}
