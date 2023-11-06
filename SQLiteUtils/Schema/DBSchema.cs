using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Schema
{
    public class DBSchema
    {
        public readonly string Name;
        public readonly SchemaEntry[] Entries;

		public DBSchema(string name, SchemaEntry[] entries)
		{
			Name = name;
			Entries = entries;
		}
	}
}
