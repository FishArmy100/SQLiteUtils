using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions.UnitedStates;
using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;

namespace AccountingDatabaseBackend.DBEntities
{
	public class Employee : ISQLUpdateable
	{
		public int id;
		public string first_name;
		public string last_name;
		public string ssn;
		public float payrate;
		public string fileing_status;
		public int num_dependants;
		public int address_id;

		public Employee()
		{
			first_name = string.Empty;
			last_name = string.Empty;
			ssn = string.Empty;
			fileing_status = string.Empty;
		}

		public static List<(Employee, Address)> CreateRandom(int count)
		{
			var addresses = Address.CreateRandom(count);

			var employees = new Faker<Employee>()
				.StrictMode(true)
				.RuleFor(e => e.id, f => f.IndexGlobal)
				.RuleFor(e => e.first_name, f => f.Person.FirstName)
				.RuleFor(e => e.last_name, f => f.Person.LastName)
				.RuleFor(e => e.ssn, f => f.Person.Ssn())
				.RuleFor(e => e.payrate, f => f.Random.Float(10.5f, 10000.0f))
				.RuleFor(e => e.fileing_status, f => f.Random.Bool() ? "single" : "married")
				.RuleFor(e => e.num_dependants, f => f.Random.Int(0, 10))
				.RuleFor(e => e.address_id, f => addresses[f.IndexFaker].id);

			return employees.Generate(count).Zip(addresses).ToList();
		}

		public Employee(int id, string first_name, string last_name, string ssn, float payrate, string fileing_status, int num_dependants, int address_id)
		{
			this.id = id;
			this.first_name = first_name;
			this.last_name = last_name;
			this.ssn = ssn;
			this.payrate = payrate;
			this.fileing_status = fileing_status;
			this.num_dependants = num_dependants;
			this.address_id = address_id;
		}

		public static SchemaEntry GetEntry(string name)
		{
			return new SchemaEntry(name, new SchemaFeild[]
			{
				new SchemaFeild.Basic(nameof(id), "INT", true),
				new SchemaFeild.Basic(nameof(first_name), "VARCHAR(20)"),
				new SchemaFeild.Basic(nameof(last_name), "VARCHAR(20)"),
				new SchemaFeild.Basic(nameof(ssn), "VARCHAR(11)"),
				new SchemaFeild.Basic(nameof(payrate), "FLOAT"),
				new SchemaFeild.Basic(nameof(fileing_status), "VARCHAR(20)"),
				new SchemaFeild.Basic(nameof(num_dependants), "INT"),
				new SchemaFeild.Basic(nameof(address_id), "INT"),

				new SchemaFeild.PrimaryKey(nameof(id)),
				new SchemaFeild.ForeignKey(nameof(address_id), DBTableNames.ADDRESS_TABLE_NAME, nameof(Address.id))
			});
		}

		public (string, string) GetId()
		{
			return (nameof(id), id.ToString());
		}

		public void OnDeserialize(ref SQLiteDataReader reader)
		{
			id = reader.GetInt32(0);
			first_name = reader.GetString(1);
			last_name = reader.GetString(2);
			ssn = reader.GetString(3);
			payrate = reader.GetFloat(4);
			fileing_status = reader.GetString(5);
			num_dependants = reader.GetInt32(6);
			address_id = reader.GetInt32(7);
		}

		public string[] OnSerialize()
		{
			return new string[]
			{
				id.ToString(),
				$"'{first_name}'",
				$"'{last_name}'",
				$"'{ssn}'",
				payrate.ToString(),
				$"'{fileing_status}'",
				num_dependants.ToString(),
				address_id.ToString(),
			};
		}
	}
}
