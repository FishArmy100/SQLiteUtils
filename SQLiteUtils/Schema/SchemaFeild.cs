using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Schema
{
	public class SchemaFeild
	{
		public readonly string Name;
		public readonly string SQLType;

		public SchemaFeild(string name, string sqlType)
		{
			Name = name;
			SQLType = sqlType;
		}
	}
}
