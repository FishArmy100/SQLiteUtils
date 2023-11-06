using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Schema
{
    public class SchemaEntry
    {
        public readonly string Name;
        public readonly SchemaFeild[] Feilds;

		public SchemaEntry(string name, SchemaFeild[] feilds)
		{
			Name = name;
			Feilds = feilds;
		}
	}
}
