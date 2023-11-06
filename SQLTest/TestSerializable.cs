using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest
{
	class TestEmployee : ISQLSerializeable
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

		public static SchemaEntry GetEntry(string name)
		{
			return new SchemaEntry(name, new[]
			{
				new SchemaFeild("first_name", "VARCHAR(20)"),
				new SchemaFeild("last_name", "VARCHAR(20)"),
				new SchemaFeild("id", "INT")
			});
		}

		public void OnDeserialize(object[] data)
		{
			throw new NotImplementedException();
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
	}
}
