using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;

namespace AccountingDatabaseBackend.DBEntities
{
	public class Address : ISQLUpdateable
	{
		public string city;
		public string state;
		public string street;
		public int id;

		public Address() : this("", "", "", 0) { }

		public Address(string city, string state, string street, int id)
		{
			this.city = city;
			this.state = state;
			this.street = street;
			this.id = id;
		}

		public static List<Address> CreateRandom(int count)
		{
			var addresses = new Faker<Address>()
				.StrictMode(true)
				.RuleFor(a => a.id, f => f.UniqueIndex)
				.RuleFor(a => a.city, f => f.Address.City())
				.RuleFor(a => a.street, f => f.Address.StreetAddress())
				.RuleFor(a => a.state, f => f.Address.State());

			return addresses.Generate(count);
		}

		public static SchemaEntry GetEntry(string name)
		{
			return new SchemaEntry(name, new SchemaFeild[]
			{
				new SchemaFeild.Basic(nameof(city), "VARCHAR(20)"),
				new SchemaFeild.Basic(nameof(state), "VARCHAR(20)"),
				new SchemaFeild.Basic(nameof(street), "VARCHAR(30)"),
				new SchemaFeild.Basic(nameof(id), "INT"),
				new SchemaFeild.PrimaryKey(nameof(id)),
			});
		}

		public (string, string) GetId()
		{
			return (nameof(id), id.ToString());
		}

		public void OnDeserialize(ref SQLiteDataReader reader)
		{
			city = reader.GetString(0);
			state = reader.GetString(1);
			street = reader.GetString(2);
			id = reader.GetInt32(3);
		}

		public string[] OnSerialize()
		{
			return new[]
			{
				$"'{city}'",
				$"'{state}'",
				$"'{street}'",
				id.ToString()
			};
		}
	}
}
