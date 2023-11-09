using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;

namespace AccountingDatabaseBackend.DBEntities
{
	internal class Employee : ISQLUpdateable
	{
		public int id;
		public string first_name;
		public string last_name;
		public string ssn;
		public float payrate;
		public string fileing_status;
		public int num_dependants;
		public int address_id;

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
			throw new NotImplementedException();
		}

		public void OnDeserialize(ref SQLiteDataReader reader)
		{
			throw new NotImplementedException();
		}

		public string[] OnSerialize()
		{
			throw new NotImplementedException();
		}
	}
}
